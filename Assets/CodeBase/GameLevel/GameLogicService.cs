using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

namespace CodeBase.GameLevel
{
    public class GameLogicService
    {
        private readonly ElementsGenerator _elementsGenerator;

        public BoardCell[,] CurrentCells;

        public GameLogicService(ElementsGenerator elementsGenerator)
        {
            _elementsGenerator = elementsGenerator;
        }

        public async UniTask InitGameBoard(Transform container)
        {
            //CurrentCells = await _elementsGenerator.FillElements(container);
        }

        public async UniTask MoveBlock(string id, Vector2 direction)
        {
            (int, int) coors = _elementsGenerator.BlockCoors[id];

            int targetY = coors.Item2 + (int) direction.x;
            int targetX = coors.Item1 - (int) direction.y;

            if (IsInBounds(targetX, targetY)) {
                SwapBlocks(CurrentCells[coors.Item1, coors.Item2], CurrentCells[targetX, targetY]);
                await NormalizeGrid();
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < CurrentCells.GetLength(0) && y >= 0 && y < CurrentCells.GetLength(1);
        }

        private void SwapBlocks(BoardCell cell1, BoardCell cell2)
        {
            int x1 = cell1.X;
            int y1 = cell1.Y;
            int x2 = cell2.X;
            int y2 = cell2.Y;

            BoardCell cell1test = CurrentCells[x1, y1];
            BoardCell cell2test = CurrentCells[x2, y2];

            CurrentCells[x1, y1] = cell2test;
            CurrentCells[x2, y2] = cell1test;

            CurrentCells[x1, y1].X = x1;
            CurrentCells[x1, y1].Y = y1;
            CurrentCells[x2, y2].X = x2;
            CurrentCells[x2, y2].Y = y2;

            if (CurrentCells[x1, y1].ElementBlock != null) {
                _elementsGenerator.BlockCoors[CurrentCells[x1, y1].ElementBlock.Id] = (x1, y1);
            }

            if (CurrentCells[x2, y2].ElementBlock != null) {
                _elementsGenerator.BlockCoors[CurrentCells[x2, y2].ElementBlock.Id] = (x2, y2);
            }

            if (cell1.ElementBlock != null) {
                cell1.ElementBlock.transform.DOLocalMove(_elementsGenerator.Positions[x2, y2], .1f).SetEase(Ease.OutQuad);
            }

            if (cell2.ElementBlock != null) {
                cell2.ElementBlock.transform.DOLocalMove(_elementsGenerator.Positions[x1, y1], .1f).SetEase(Ease.OutQuad);
            }
        }

        private async UniTask NormalizeGrid()
        {
            bool hasFallen = true;

            while (hasFallen) {
                hasFallen = false;

                for (int j = 0; j < CurrentCells.GetLength(1); j++) {
                    for (int i = CurrentCells.GetLength(0) - 1; i >= 0; i--) {
                        if (CurrentCells[i, j].IsEmpty()) {
                            int upperBlockI = i - 1;
                            if (upperBlockI < 0) {
                                continue;
                            }
                            if (CurrentCells[upperBlockI, j].IsEmpty()) {
                                continue;
                            }
                            SwapBlocks(CurrentCells[i, j], CurrentCells[upperBlockI, j]);
                            hasFallen = true; // Устанавливаем флаг, что произошли изменения
                            await UniTask.WaitForSeconds(.5f);
                        }
                    }
                }
            }

        }

        private void FindMatches(BoardCell[,] board)
        {
            var destroyMatrix = new MatchInfo[board.GetLength(0), board.GetLength(1)];

            int targetTypeInt = -1;

            for (int i = 0; i < board.GetLength(0); i++) {
                targetTypeInt = -1;
                var match = new List<(int, int)>();
                for (int j = 0; j < board.GetLength(1); j++) {
                    if (board[i, j].IsEmpty()) {
                        continue;
                    }
                    if (targetTypeInt == -1) {
                        targetTypeInt = (int) board[i, j].ElementBlock.ElementType;
                        match.Add((i, j));
                        continue;
                    }

                    if (targetTypeInt == (int) board[i, j].ElementBlock.ElementType) {
                        match.Add((i, j));
                        continue;
                    }

                    if (match.Count >= 3) {
                        foreach (var tuple in match) {
                            destroyMatrix[tuple.Item1, tuple.Item2] = new MatchInfo(false, true, (ElementType) targetTypeInt);
                        }
                    }

                    targetTypeInt = -1;
                    match = new List<(int, int)>();
                }
            }

            for (int j = 0; j < board.GetLength(1); j++) {
                targetTypeInt = -1;
                var match = new List<(int, int)>();
                for (int i = 0; i < board.GetLength(0); i++) {
                    if (board[i, j].IsEmpty()) {
                        continue;
                    }
                    if (targetTypeInt == -1) {
                        targetTypeInt = (int) board[i, j].ElementBlock.ElementType;
                        match.Add((i, j));
                        continue;
                    }

                    if (targetTypeInt == (int) board[i, j].ElementBlock.ElementType) {
                        match.Add((i, j));
                        continue;
                    }

                    if (match.Count >= 3) {
                        foreach (var tuple in match) {
                            destroyMatrix[tuple.Item1, tuple.Item2] = new MatchInfo(false, true, (ElementType) targetTypeInt);
                        }
                    }

                    targetTypeInt = -1;
                    match = new List<(int, int)>();
                }
            }

            
        }

        // Метод для поиска матчей в указанном направлении
        private List<BoardCell> FindMatches(int startX, int startY, int deltaX, int deltaY)
        {
            List<BoardCell> matches = new List<BoardCell>();
            ElementType targetType = CurrentCells[startX, startY].ElementBlock.ElementType;

            for (int i = startX, j = startY;
                 IsInBounds(i, j) && CurrentCells[i, j].ElementBlock != null && CurrentCells[i, j].ElementBlock.ElementType == targetType;
                 i += deltaX, j += deltaY) {
                matches.Add(CurrentCells[i, j]);
            }

            return matches;
        }
    }
}
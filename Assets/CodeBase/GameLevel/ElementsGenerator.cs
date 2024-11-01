using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GameLevel
{
    public class ElementsGenerator
    {
        private readonly GameFactory _gameFactory;
        private const float HorizontalStep = 2f;
        private const float VerticalStep = 1.925f;

        public BoardCellType[,] StartBoard = new BoardCellType[2, 5] {
                { BoardCellType.Water, BoardCellType.Empty, BoardCellType.Fire, BoardCellType.Empty, BoardCellType.Empty },
                { BoardCellType.Water, BoardCellType.Empty, BoardCellType.Water, BoardCellType.Fire, BoardCellType.Fire }
        };

        public Vector3[,] Positions = new Vector3[2, 5];
        public Dictionary<string, (int, int)> BlockCoors = new Dictionary<string, (int, int)>();

        public ElementsGenerator(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public async UniTask<BoardCell[,]> FillElements(Transform container)
        {
            var cells = new BoardCell[StartBoard.GetLength(0), StartBoard.GetLength(1)];
            float leftStartPoint = CalculateStartPoint(StartBoard.GetLength(1));

            int verticalLevel = 0;
            for (int i = StartBoard.GetLength(0) - 1; i >= 0; i--) {
                for (int j = 0; j < StartBoard.GetLength(1); j++) {
                    BoardCellType boardCellType = StartBoard[i, j];
                    Vector3 globalPos = CalculatePosition(container, leftStartPoint, i,j, verticalLevel);
                    await CreateElement(container, boardCellType, cells, i, j, globalPos);
                }
                verticalLevel++;
            }

            return cells;
        }

        private float CalculateStartPoint(int blockLineCount)
        {
            bool isEven = IntUtils.IsEven(blockLineCount);
            float leftStartPoint;
            int halfCount = blockLineCount / 2;
            if (isEven) {
                leftStartPoint = -HorizontalStep / 2 - (halfCount - 1) * HorizontalStep;
            } else {
                leftStartPoint = -(halfCount) * HorizontalStep;
            }
            return leftStartPoint;
        }

        private Vector3 CalculatePosition(Transform container, float leftStartPoint, int i, int j, int verticalLevel)
        {
            float xPos = leftStartPoint + j * HorizontalStep;
            float yPos = verticalLevel * VerticalStep;
            var localPos = new Vector3(xPos, yPos, -verticalLevel * 10 - j);
            Positions[i, j] = localPos;
            Vector3 globalPos = container.TransformPoint(localPos);
            return globalPos;
        }

        private async UniTask CreateElement(Transform container, BoardCellType boardCellType, BoardCell[,] cells, int i, int j, Vector3 globalPos)
        {
            string newId = Guid.NewGuid().ToString();
            switch (boardCellType) {
                case BoardCellType.Empty:
                    cells[i, j] = new BoardCell(newId, i, j, BoardCellType.Empty, null);
                    break;
                case BoardCellType.Fire:
                    GameObject fire = await _gameFactory.CreateFireElement(globalPos, container);
                    cells[i, j] = new BoardCell(newId, i, j, BoardCellType.Fire, fire.GetComponent<ElementBlock>());

                    break;
                case BoardCellType.Water:
                    GameObject water = await _gameFactory.CreateWaterElement(globalPos, container);
                    cells[i, j] = new BoardCell(newId, i, j, BoardCellType.Water, water.GetComponent<ElementBlock>());
                    break;
            }

            BlockCoors.Add(newId, (i, j));
        }
        
        
    }
}
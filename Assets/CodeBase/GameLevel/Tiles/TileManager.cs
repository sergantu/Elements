using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector2Int gridSize;

    private TileBase[,] tiles;

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        tiles = new TileBase[gridSize.x, gridSize.y];

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            int x = pos.x;
            int y = pos.y;

            if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cellPosition))
                {
                    tiles[x, y] = tilemap.GetTile(cellPosition);
                }
            }
        }

        UpdateTilePositions();
    }

    private void UpdateTilePositions()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, tiles[x, y]);
            }
        }
    }

    public bool IsTileAt(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize.x &&
               position.y >= 0 && position.y < gridSize.y &&
               tiles[position.x, position.y] != null;
    }

    public async UniTask TrySwipe(Vector2Int from, Vector2Int to)
    {
        if (!IsValidMove(from, to)) return;

        await SwapTiles(from, to);
        await NormalizeTiles();
        await CheckMatches();
    }

    private bool IsValidMove(Vector2Int from, Vector2Int to)
    {
        if (to.x < 0 || to.x >= gridSize.x || to.y < 0 || to.y >= gridSize.y)
            return false; // За границы поля не допускаем

        if (Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y) != 1)
            return false; // Проверка на соседние ячейки

        if (from.y < to.y && !IsTileAt(new Vector2Int(from.x, from.y + 1)))
            return false; // Запретить движение вверх, если сверху пустая ячейка

        return true;
    }

    private async UniTask SwapTiles(Vector2Int from, Vector2Int to)
    {
        Vector3Int fromPosition = new Vector3Int(from.x, from.y, 0);
        Vector3Int toPosition = new Vector3Int(to.x, to.y, 0);

 
        
        TileBase temp = tiles[from.x, from.y];
        tiles[from.x, from.y] = tiles[to.x, to.y];
        tiles[to.x, to.y] = temp;

        UpdateTilePositions();
    }
    


    private async UniTask NormalizeTiles()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (tiles[x, y] == null)
                {
                    for (int k = y + 1; k < gridSize.y; k++)
                    {
                        if (tiles[x, k] != null)
                        {
                            // Сдвинуть тайл вниз
                            tiles[x, y] = tiles[x, k];
                            tiles[x, k] = null;

                            // Обновляем тайлы на Tilemap
                            tilemap.SetTile(new Vector3Int(x, y, 0), tiles[x, y]);
                            tilemap.SetTile(new Vector3Int(x, k, 0), null);

                            // Устанавливаем Z-координату для тайлов
                            tilemap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                            tilemap.SetTileFlags(new Vector3Int(x, k, 0), TileFlags.None);

                            break; // Смотрим только один раз для каждой ячейки
                        }
                    }
                }
            }
        }

        // Обновляем позиции всех тайлов для корректного отображения
        UpdateTileZOrder();
    }

    private void UpdateTileZOrder()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (tiles[x, y] != null)
                {
                    float zOrder = x*10 + y * 0.1f; // Умножаем на 0.1 для более мелкой регулировки
                    tilemap.SetTransformMatrix(new Vector3Int(x, y, 0), Matrix4x4.Translate(new Vector3(0, 0, zOrder))); // Установка порядка Z
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (tilemap == null || tiles == null) return;

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(worldPos, tilemap.cellSize);
            UnityEditor.Handles.Label(worldPos, $"({pos.x}, {pos.y}, {pos.z})");
        }
    }


    private async UniTask CheckMatches()
    {
        List<Vector2Int> matchedTiles;

        do
        {
            matchedTiles = new List<Vector2Int>();

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (tiles[x, y] == null) continue;

                    // Проверка по горизонтали
                    if (x < gridSize.x - 2 && tiles[x, y] == tiles[x + 1, y] && tiles[x, y] == tiles[x + 2, y])
                    {
                        matchedTiles.Add(new Vector2Int(x, y));
                        matchedTiles.Add(new Vector2Int(x + 1, y));
                        matchedTiles.Add(new Vector2Int(x + 2, y));
                    }

                    // Проверка по вертикали
                    if (y < gridSize.y - 2 && tiles[x, y] == tiles[x, y + 1] && tiles[x, y] == tiles[x, y + 2])
                    {
                        matchedTiles.Add(new Vector2Int(x, y));
                        matchedTiles.Add(new Vector2Int(x, y + 1));
                        matchedTiles.Add(new Vector2Int(x, y + 2));
                    }
                }
            }

            foreach (var pos in matchedTiles)
            {
                tiles[pos.x, pos.y] = null;
                tilemap.SetTile((Vector3Int)pos, null);
            }

            if (matchedTiles.Count > 0)
            {
                await NormalizeTiles(); // Повторная нормализация после удаления совпадений
            }

        } while (matchedTiles.Count > 0); // Повторять, пока есть совпадения
    }
}

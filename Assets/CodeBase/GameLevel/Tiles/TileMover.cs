using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMover : MonoBehaviour
{
    public static IEnumerator AnimateTileMovement(Tilemap tilemap, Vector3Int from, Vector3Int to, TileBase tile, float duration)
    {
        // Создаем временный объект для визуального представления
        GameObject tileObject = new GameObject("TileVisual");
        SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
        renderer.sprite = ((Tile)tile).sprite;

        Vector3 startPos = tilemap.CellToWorld(from) + tilemap.tileAnchor;
        Vector3 targetPos = tilemap.CellToWorld(to) + tilemap.tileAnchor;

        tileObject.transform.position = startPos;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            tileObject.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tileObject.transform.position = targetPos;

        // Завершаем анимацию, устанавливая тайл на конечную позицию и удаляя объект
        tilemap.SetTile(to, tile);
        Destroy(tileObject);
    }
}
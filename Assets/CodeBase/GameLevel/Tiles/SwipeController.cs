using Cysharp.Threading.Tasks;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public TileManager tileManager; // Ссылка на TileManager
    private Vector2Int startSwipePosition;
    private bool isSwiping;

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            DetectSwipeMobile();
        }
        else
        {
            DetectSwipePC();
        }
    }

    private void DetectSwipeMobile()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            Vector3Int gridPosition = tileManager.tilemap.WorldToCell(worldPosition);
            Vector2Int tilePosition = (Vector2Int)gridPosition;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (tileManager.IsTileAt(tilePosition)) // Проверяем наличие тайла в начальной точке
                    {
                        startSwipePosition = tilePosition;
                        isSwiping = true;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        Vector2Int direction = GetSwipeDirection(tilePosition);
                        if (direction != Vector2Int.zero)
                        {
                            Vector2Int targetPosition = startSwipePosition + direction;
                            tileManager.TrySwipe(startSwipePosition, targetPosition).Forget();
                            isSwiping = false;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    isSwiping = false;
                    break;
            }
        }
    }

    private void DetectSwipePC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tileManager.tilemap.WorldToCell(worldPosition);
            Vector2Int tilePosition = (Vector2Int)gridPosition;

            if (tileManager.IsTileAt(tilePosition))
            {
                startSwipePosition = tilePosition;
                isSwiping = true;
            }
        }
        else if (Input.GetMouseButton(0) && isSwiping)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tileManager.tilemap.WorldToCell(worldPosition);
            Vector2Int tilePosition = (Vector2Int)gridPosition;

            Vector2Int direction = GetSwipeDirection(tilePosition);
            if (direction != Vector2Int.zero)
            {
                Vector2Int targetPosition = startSwipePosition + direction;
                tileManager.TrySwipe(startSwipePosition, targetPosition);
                isSwiping = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }
    }

    private Vector2Int GetSwipeDirection(Vector2Int endSwipePosition)
    {
        Vector2Int direction = endSwipePosition - startSwipePosition;

        // Оставляем только горизонтальные и вниз движения, не допускаем вверх
        if (direction == Vector2Int.right) return Vector2Int.right;
        if (direction == Vector2Int.left) return Vector2Int.left;
        if (direction == Vector2Int.down) return Vector2Int.down;

        return Vector2Int.zero; // Неверное направление
    }
} 
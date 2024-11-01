using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CodeBase.GameLevel
{
    public class MoveBlockService : ITickable
    {
        private readonly GameLogicService _gameLogicService;
        private UnityEngine.Camera mainCamera; // Объявите переменную для камеры
        public LayerMask blockLayer; // Параметр для выбора слоя блоков

        private ElementBlock currentBlockHandler;
        private Vector2 swipeStartPos;
        private float swipeThreshold = 0.5f; // Порог для определения свайпа
        private bool isDragging = false; // Добавляем переменную для отслеживания состояния перетаскивания

        // Конструктор для инъекции камеры
        public MoveBlockService(GameLogicService gameLogicService)
        {
            _gameLogicService = gameLogicService;
            // Получаем камеру по тегу
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
            blockLayer = LayerMask.GetMask("Block"); // Устанавливаем маску по умолчанию
        }

        public void Tick() // Метод Tick, который будет вызываться каждый кадр
        {
           /* if (Input.GetMouseButtonDown(0)) {
                HandleMouseClick();
            } else if (Input.GetMouseButton(0) && isDragging) // Проверяем, удерживается ли кнопка мыши и активен ли перетаскивание
            {
                HandleMouseDrag();
            } else if (Input.GetMouseButtonUp(0)) // Сбрасываем текущее значение при отпускании кнопки
            {
                isDragging = false; // Сброс состояния перетаскивания
                currentBlockHandler = null;
            }*/
        }

        private void HandleMouseClick()
        {
            HandleClick(Input.mousePosition);
        }

        private void HandleMouseDrag()
        {
            // Проверяем, что текущий блок выбран
            if (currentBlockHandler != null) {
                HandleSwipe(Input.mousePosition).Forget();
            }
        }

        private void HandleClick(Vector2 position)
        {
            // Преобразуем экранные координаты в мировые
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(position);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, blockLayer); // Используем LayerMask

            if (hit.collider != null) {
                var blockHandler = hit.collider.GetComponent<ElementBlock>();
                if (blockHandler != null) {
                    currentBlockHandler = blockHandler;
                    swipeStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    isDragging = true; // Устанавливаем состояние перетаскивания
                }
            }
        }

        private async UniTask HandleSwipe(Vector2 currentPosition)
        {
            /*Vector2 currentSwipePos = mainCamera.ScreenToWorldPoint(currentPosition);
            Vector2 swipeDirection = currentSwipePos - swipeStartPos;

            // Проверяем, достаточно ли свайп для движения
            if (isDragging && swipeDirection.magnitude >= swipeThreshold) {
                Vector2 direction = Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y)
                                            ? (swipeDirection.x > 0 ? Vector2.right : Vector2.left)
                                            : (swipeDirection.y > 0 ? Vector2.up : Vector2.down);

                var elementBlock = currentBlockHandler.GetComponent<ElementBlock>(); 
                _gameLogicService.MoveBlock(elementBlock.Id, direction);
                isDragging = false; // Сбрасываем состояние перетаскивания после перемещения
            }*/
        }
    }
}
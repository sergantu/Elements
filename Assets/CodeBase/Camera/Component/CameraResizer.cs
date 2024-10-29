using UnityEngine;

namespace CodeBase.Camera.Component
{
    public class CameraResizer : MonoBehaviour
    {
        public UnityEngine.Camera mainCamera;

        private void Start()
        {
            float sceneWidth = 4.9f;
            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            mainCamera.orthographicSize = desiredHalfHeight;
        }
    }
}
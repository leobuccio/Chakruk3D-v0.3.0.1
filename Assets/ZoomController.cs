using System.Collections;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public float zoomSpeed = 5.0f;
    public float minFOV = 20.0f;
    public float returnToOriginalDelay = 5.0f;
    public float fovTransitionTime = 5.0f; // Tiempo de transición en segundos

    private Camera mainCamera;
    private float initialFOV;
    private float originalFOV;
    private float timeSinceZoom = 0f;
    
    private float elapsedTransitionTime = 0.0f;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        initialFOV = mainCamera.fieldOfView;
        originalFOV = initialFOV;
    }

    private void Update()
    {
        timeSinceZoom += Time.deltaTime;

        // Validación para PC o Editor
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            ZoomWithMouse(scrollInput);

            if (scrollInput != 0f)
            {
                timeSinceZoom = 0f;
            }
        }
        // Validación para Android
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
                float currentMagnitude = (touch0.position - touch1.position).magnitude;

                float zoomInput = currentMagnitude - prevMagnitude;
                ZoomWithTouch(zoomInput);

                timeSinceZoom = 0f;
            }
        }

        // Verificar si debe volver al FOV original
        if (timeSinceZoom >= returnToOriginalDelay)
        {
            ReturnToOriginalZoom();
        }
    }

    private void ZoomWithMouse(float zoomInput)
    {
        float newFOV = mainCamera.fieldOfView - zoomInput * zoomSpeed * Time.deltaTime;
        newFOV = Mathf.Clamp(newFOV, minFOV, initialFOV);
        mainCamera.fieldOfView = newFOV;
    }

    private void ZoomWithTouch(float zoomInput)
    {
        float newFOV = mainCamera.fieldOfView - zoomInput * zoomSpeed * Time.deltaTime;
        newFOV = Mathf.Clamp(newFOV, minFOV, initialFOV);
        mainCamera.fieldOfView = newFOV;
    }

    private void ReturnToOriginalZoom()
    {
        if (elapsedTransitionTime < fovTransitionTime)
        {
            elapsedTransitionTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTransitionTime / fovTransitionTime);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFOV, t);
        }
        else
        {
            mainCamera.fieldOfView = originalFOV;
            elapsedTransitionTime = 0.0f; // Reiniciar el tiempo de transición
        }
    }
}

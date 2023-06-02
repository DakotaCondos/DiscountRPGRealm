using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 5f; // Base camera panning speed
    public float zoomSpeed = 500f; // Base camera zoom speed
    public float panMultiplierScaler = .7f; // Pan multiplier scaler for sensitivity adjustment

    public Transform topBound;
    public Transform bottomBound;
    public Transform leftBound;
    public Transform rightBound;
    public Transform frontBound;
    public Transform backBound;

    public float minOrthographicSize = 2f; // Minimum orthographic size
    public float maxOrthographicSize = 10f; // Maximum orthographic size

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // Get input for X and Y movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Get input for zoom
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // Pan the camera
        float panMultiplier = mainCamera.orthographicSize * panMultiplierScaler;
        Vector3 panMovement = new Vector3(moveX, moveY, 0f) * panSpeed * panMultiplier * Time.deltaTime;
        Vector3 newPosition = transform.position + panMovement;

        // Apply bounds to the new position
        newPosition.x = Mathf.Clamp(newPosition.x, leftBound.position.x, rightBound.position.x);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBound.position.y, topBound.position.y);
        newPosition.z = Mathf.Clamp(newPosition.z, backBound.position.z, frontBound.position.z);

        transform.position = newPosition;

        // Zoom the camera
        float AdditionalZoom = (zoomInput == 0) ? 0 : zoomInput * (5 * mainCamera.orthographicSize / maxOrthographicSize);
        float newOrthographicSize = mainCamera.orthographicSize - (zoomInput + AdditionalZoom) * zoomSpeed;
        newOrthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);

        mainCamera.orthographicSize = newOrthographicSize;
    }
}

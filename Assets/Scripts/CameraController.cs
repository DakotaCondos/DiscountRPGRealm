using System;
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
    private GameObject focusObject = null;
    private float defaultZ = 0;

    public GameObject mainGameUI;

    [SerializeField] private Vector3 _lastFramePos = Vector3.zero;
    [SerializeField] private Vector3 _currentFramePos = Vector3.zero;
    [SerializeField] private Vector3 _badFramePos = new(42, 42, 42);

    public static CameraController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        defaultZ = transform.position.z;
    }

    void Update()
    {
        if (focusObject != null && focusObject.activeInHierarchy)
        {

            Vector3 focusPosition = focusObject.transform.position;
            focusPosition.z = defaultZ;
            transform.position = focusPosition;
            CheckJitter();

            return;
        }

        // Check if viewing the gameboard
        if (!mainGameUI.activeInHierarchy) { return; }

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

        CheckJitter();
    }

    private void CheckJitter()
    {
        float jitterLimit = 100;
        _currentFramePos = transform.position;
        float frameOffset = Vector3.Distance(_lastFramePos, _currentFramePos);
        if (frameOffset > jitterLimit)
        {
            ConsolePrinter.PrintToConsole($"Jitter distance:{frameOffset} CurrentFrame: {_currentFramePos} LastFrame: {_lastFramePos}", Color.cyan);
            _badFramePos = _currentFramePos;
        }
        _lastFramePos = _currentFramePos;
    }

    public void SetFocusObject(GameObject gameObject)
    {
        focusObject = gameObject;
        print(gameObject.name);
    }

    public void ClearFocusObject(GameObject gameobjectToLookAt = null)
    {
        focusObject = null;
        if (gameobjectToLookAt != null)
        {

            Vector3 targetPos = gameobjectToLookAt.transform.position;
            transform.position = new(targetPos.x, targetPos.y, transform.position.z);
        }
    }

}

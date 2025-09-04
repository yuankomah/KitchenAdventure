using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CinemachineOrbitalFollow orbital;
    private Vector2 scrollDelta;
    private float targetZoom;
    private float currentZoom;

    public static CameraController Instance { get; set; }

    // Use this for initialization
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetZoom = currentZoom = orbital.Radius;
    }

    public void Handle_MouseScrolled(InputAction.CallbackContext context)
    {
        scrollDelta = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (scrollDelta.y != 0)
        {
            print("Scrolled");
            if (orbital != null)
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minDistance, maxDistance);
                scrollDelta = Vector2.zero;
            }
        }

        float bumperDelta = GameInput.Instance.ReadGamePadZoomValue();
        if (bumperDelta != 0)
        {
            targetZoom = Mathf.Clamp(orbital.Radius - bumperDelta * zoomSpeed, minDistance, maxDistance);
        }
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        orbital.Radius = currentZoom;
    }
}

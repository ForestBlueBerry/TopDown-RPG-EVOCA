using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class CineMachineController : MonoBehaviour
{
    private CinemachineCamera cam;
    private float speedscroll = 1f;
    private float minSize = 1f;
    private float maxSize = 12f;
    private float smoothing = 3f;
    private float targetzoom;

    private float baseZoom;

    private void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        baseZoom = cam.Lens.OrthographicSize;
        targetzoom = cam.Lens.OrthographicSize;

    }
    void Update()
    {
        Vector2 scroll = Mouse.current.scroll.ReadValue();

        if(scroll.y != 0)
        {
            targetzoom -= scroll.y * speedscroll;
            targetzoom = Mathf.Clamp(targetzoom, minSize, maxSize);
            cam.Lens.OrthographicSize = Mathf.Lerp(cam.Lens.OrthographicSize, targetzoom, Time.deltaTime * smoothing);
        }
    }

    private void OnDestroy()
    {
        cam.Lens.OrthographicSize = baseZoom;
    }
}

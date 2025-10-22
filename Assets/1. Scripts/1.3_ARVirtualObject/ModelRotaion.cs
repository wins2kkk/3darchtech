using UnityEngine;
using UnityEngine.EventSystems; // Import thư viện để kiểm tra UI
using System.Collections;
using System.Runtime.CompilerServices;

public class ModelRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Tốc độ xoay model
    private Vector2 deltaTouch; // Chênh lệch vị trí chạm
    private Coroutine returnCoroutine; // Coroutine để trả về góc quay gốc
    public float returnDelay = 1f; // Thời gian trễ trước khi quay về góc ban đầu
    public float returnSpeed = 2f; // Tốc độ quay về góc ban đầu

    private Transform lastTransformHit;
    private Transform currentTransformHit;

    private Camera mainCamera;

    [SerializeField] private Quaternion originalRotation; // Lưu trạng thái góc quay gốc
    [SerializeField] private LayerMask _arGenesisLayerMask;
    [SerializeField] private Transform Canvas3d;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            // if (IsPointerOverUI())
            //     return;
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 150f, _arGenesisLayerMask))
                {
                    if (hit.collider != null)
                    {
                        currentTransformHit = hit.transform;
                        RotateModel(touch);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (currentTransformHit != null)
                {
                    RotateModel(touch);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (currentTransformHit != null)
                {
                    lastTransformHit = currentTransformHit;
                    returnCoroutine = StartCoroutine(ReturnToOriginalRotation());
                }
                currentTransformHit = null;
            }
        }
    }

    private void RotateModel(Touch touch)
    {
        Debug.Log(deltaTouch);
        deltaTouch = touch.deltaPosition;

        if (Mathf.Abs(deltaTouch.x) > Mathf.Abs(deltaTouch.y))
        {
            deltaTouch.y = 0;
        }
        else
        {
            deltaTouch.x = 0;
        }

        float rotationX = deltaTouch.y * rotationSpeed * Time.deltaTime;
        float rotationY = -deltaTouch.x * rotationSpeed * Time.deltaTime;

        // currentTransformHit.Rotate(0, rotationY, 0, Space.World);
        // currentTransformHit.GetChild(0).Rotate(rotationX, 0, 0, Space.Self);

        // currentTransformHit.Rotate(Vector3.right, rotationX, Space.World);
        // currentTransformHit.Rotate(Vector3.up, rotationY, Space.World);

        currentTransformHit.Rotate(Canvas3d.right, rotationX, Space.World);
        currentTransformHit.Rotate(Canvas3d.up, rotationY, Space.World);


        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    private IEnumerator ReturnToOriginalRotation()
    {
        yield return new WaitForSeconds(returnDelay);

        while (Quaternion.Angle(lastTransformHit.localRotation, originalRotation) > 0.1f)
        {
            lastTransformHit.localRotation = Quaternion.Slerp(lastTransformHit.localRotation, originalRotation, returnSpeed * Time.deltaTime);
            yield return null;
        }

        lastTransformHit.localRotation = originalRotation;

        returnCoroutine = null;
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}

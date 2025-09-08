using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("타겟설정")]
    public Transform targret;

    [Header("카메라 거리 설정")]
    public float distance = 8.0f;
    public float height = 5.0f;

    [Header("마우스 설정")]
    public float mouseSensitivity = 2.0f;
    public float minVerticalAngle = -30.0f;
    public float maxVerticalAngle = 60.0f;

    [Header("부드러움 설정")]
    public float positonSmoothTime = 2.0f;
    public float rotationSmoothTime = 2.0f;

    private float horizontalAngle = 0f;
    private float verticalAngle = 0f;

    private Vector3 currentVelocity;
    private Vector3 currentPosition;
    private Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        if(targret == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null) 
                targret = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { ToggleCursor(); }
    }

    private void LateUpdate()
    {
        if( targret == null ) return;

        HandleMouseInput();
        UpdateCameraSmooth();
    }

    void HandleMouseInput()
    {
        if(Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalAngle += mouseX;
        verticalAngle -= mouseY;

        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle , maxVerticalAngle);

    }
    void UpdateCameraSmooth()
    {
        Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        Vector3 rotatedOffset = rotation * new Vector3(0, height, -distance);
        Vector3 targetPosition = targret.position + rotatedOffset;

        Vector3 lookTarget = targret.position + Vector3.up * height;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - targetPosition);

        currentPosition = Vector3.SmoothDamp(currentPosition,targetPosition  , ref currentVelocity, positonSmoothTime);

        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime / rotationSmoothTime);

        transform.rotation = currentRotation;
        transform.rotation = currentRotation;


    }
    void ToggleCursor()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("공격 설정")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("컴포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    private float currenSpeed;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float verial = Input.GetAxis("Vertical");

        if(horizontal != 0 || verial != 0)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * verial + cameraRight * horizontal;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currenSpeed = runSpeed;
            }
            else
            {
                currenSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currenSpeed * Time.deltaTime);

            Quaternion targetRotaion = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotaion, rotationSpeed * Time.deltaTime);

            

        }
        else
        {
            currenSpeed = 0;
        }
    }
    void UpdateAnimator()
    {
        float animatorSpeed = Mathf.Clamp01(currenSpeed / runSpeed);
        animator.SetFloat("speed", animatorSpeed);
    }
}

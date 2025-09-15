using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("점프 설정")]
    public float jumpHeight = 2f;
    public float gravity = -9.8f;
    public float landingDuration = 0.3f;


    [Header("공격 설정")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("컴포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //현재 상태
    private float currenSpeed;
    private bool isAttacking = false;
    private bool isLanding = false;
    public float landingTimer;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();

    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if(!isGrounded && wasGrounded)         //땅에서 떨어졌을때
        {
            Debug.Log("떨어지기 시작");
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            // 착지모션 트리거 및 착지 상태
            if(!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landingTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("착지");
            }
        }

    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;     //랜딩 시간만큼 못 움직임
            if(landingTimer <= 0)
            {
                isLanding = false;     //착지 완료
            }
        }
    }

   void HandleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0)
            {
                isAttacking = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)   //공격중이 아닐떄
        {
            isAttacking = true;
            attackTimer = attackDuration;

            if(animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if(animator != null)
            {
                animator.SetTrigger("jumpTrigger");

            }
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }


    void HandleMovement()
    {
        if((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currenSpeed = 0;
            return;
        }

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
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;            //캐릭터의 Y축 속도가 음수로 넘어가면 떨어지고 있다고 판단
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);

    }
}

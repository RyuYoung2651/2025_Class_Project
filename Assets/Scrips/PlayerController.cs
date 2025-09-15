using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�̵� ����")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("���� ����")]
    public float jumpHeight = 2f;
    public float gravity = -9.8f;
    public float landingDuration = 0.3f;


    [Header("���� ����")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("������Ʈ")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //���� ����
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

        if(!isGrounded && wasGrounded)         //������ ����������
        {
            Debug.Log("�������� ����");
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            // ������� Ʈ���� �� ���� ����
            if(!wasGrounded && animator != null)
            {
                //animator.SetTrigger("landingTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("����");
            }
        }

    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;     //���� �ð���ŭ �� ������
            if(landingTimer <= 0)
            {
                isLanding = false;     //���� �Ϸ�
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
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)   //�������� �ƴҋ�
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

        bool isFalling = !isGrounded && velocity.y < -0.1f;            //ĳ������ Y�� �ӵ��� ������ �Ѿ�� �������� �ִٰ� �Ǵ�
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);

    }
}

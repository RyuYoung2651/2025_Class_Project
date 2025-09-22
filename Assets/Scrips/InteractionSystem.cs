using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionSystem : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactionRange = 2.0f;            //상호작용 범위
    public LayerMask interactionLayerMask = 1;         //상호작용할 레이어
    public KeyCode interactionKey = KeyCode.E;          //상호작용 키

    [Header("UI 설정")]
    public Text interactionText;           //상호작용 UI 텍스트
    public GameObject interactionUI;       // 상호작용 UI 패널

    private Transform playerTransform;
    private InteractableObject currentInteractiable;   //감지된 오브젝트를 담는 클래스



    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        HideInteractionUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
        HandleInteractionInput();
    }

    void CheckForInteractables()
    {
        Vector3 checkPosition = playerTransform.position + playerTransform.forward * (interactionRange * 0.5f);

        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, interactionRange, interactionLayerMask);

        InteractableObject closestInteractable = null;
        float closestDistacne = float.MaxValue;

        //가장 가까운 상호작용 오브젝트 찾기
        foreach (Collider collider in hitColliders)
        {
            InteractableObject interactable= collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);

                //플레이어가 바로보는 방향에 있는지 확인
                Vector3 directionToObject = (collider.transform.position - playerTransform.position).normalized;
                float angle= Vector3.Angle(playerTransform.forward, directionToObject);

                if(angle < 90f && distance < closestDistacne)     //앞쪽 90도 범위 내
                {
                    closestDistacne = distance;
                    closestInteractable = interactable;
                }


            }
        }

        if(currentInteractiable != currentInteractiable)
        {
            if(currentInteractiable != null)
            {
                currentInteractiable.OnPlayerExit();
            }
            currentInteractiable = closestInteractable;

            if(currentInteractiable != null)
            {
                currentInteractiable.OnPlayerEnter();
                ShowInteractionUI(currentInteractiable.GetInteractionText());
            }
            else
            {
                HideInteractionUI();
            }


        }

    }




    void HandleInteractionInput()
    {
        if (currentInteractiable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractiable.Interact();
        }
    }

    void ShowInteractionUI(string text)
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }

        if(interactionText != null)
        {
            interactionText.text = text;
        }
    }

    void HideInteractionUI()
    {
        if(!interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

}

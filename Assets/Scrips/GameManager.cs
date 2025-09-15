using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("���ӻ���")]
    public int playerScore = 0;
    public int itemCollected = 0;

    [Header("UI����")]
    public Text scoreText;
    public Text itemCountText;
    public Text gameStatusText;

    public static GameManager instance;  //�̱��� ����

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem()
    {
        itemCollected++;
        Debug.Log($"������ ����! (�� : {itemCollected} ��");
    }

    void UpdateUI()
    {
        if(scoreText != null)
        {
            scoreText.text = "���� : " + playerScore;
        }
        if(itemCountText != null)
        {
            itemCountText.text = "������ : " + itemCollected + "��";
        }
    }
}

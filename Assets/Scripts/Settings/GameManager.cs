using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject StartText;
    [SerializeField] private GameObject CancelText;



    private static GameManager instance = null;

    public delegate void CancleStage();
    public static CancleStage canslestage;
    
    List<Enemy> EnemyCount;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /*
     ���ӸŴ������� �ʿ��� ��
    1. �÷��̾� ����
    2. ���� ���� _ ����, ����
    3. �������� ����
     */

    public void GameReStart() { }

    public void GameFail() { }



}

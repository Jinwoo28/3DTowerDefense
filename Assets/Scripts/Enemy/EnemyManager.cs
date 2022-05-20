using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class StageInfo
{
    public float SpawnTime = 0;
    public int EnemyCount;
}
public class EnemyManager : MonoBehaviour
{

    public delegate void StageClear();
    public static StageClear stageclear;

    [SerializeField] private MultipleSpeed speedSet = null;

    private float EnemyCoinRate = 0;

    [SerializeField] private GameObject unituiParent = null;

    //�������� ����
    [SerializeField] private StageInfo[] stageinfo = null;

    //���� �װų� �������� �������� ��, ���ȹ���̳� ������ ���Ҹ� ���� �÷��̾� ����
    [SerializeField] private PlayerState playerstate = null;

    //enemy�� ü�¹ٿ� ���ݽ� �������� ���� UI������ ���� canvas
    [SerializeField] private Transform canvas = null;

    [SerializeField] private GameObject hpbar = null;
    [SerializeField] private GameObject damagenum = null;

    private int StageNum = 0;
    public int GetStageNum => StageNum;

    //���������� ���������� �Ǵ�
    private bool gameongoing = false;

    //�� ������ �������� ����
    private bool SpawnFinish = false;

    private Vector3[] waypoint;
    private Vector3 SpawnPos;

    [SerializeField] private GameObject ClearPanal = null;
    [SerializeField] private TextMeshProUGUI PlusCoin1 = null;
    [SerializeField] private GameObject FailPanal = null;
    [SerializeField] private TextMeshProUGUI PlusCoin2 = null;
    //private bool StageClear = false;

    [SerializeField] private Transform water = null;

    //��ȯ�Ǵ� ������ ������ ���� List
    List<Enemy> EnemyCount = null;

     int EnemyRemainCount = 0;

    [SerializeField] private WeatherSetting weather =  null;

    private EnemyPooling Pooling = null;

    public int Getmaxstage => stageinfo.Length;
    public int Getcurrentstage => StageNum + 1;

    private void Start()
    {
        Pooling = this.GetComponent<EnemyPooling>();


        //if (UserInformation.userDataStatic.skillSet[2].skillUnLock)
        //{
        //    EnemyCoinRate = (UserInformation.userDataStatic.skillSet[2].damage/100);
        //}

        EnemyCount = new List<Enemy>();
        MultipleSpeed.speedup += SpeedUP;
        ClearPanal.SetActive(false);
        FailPanal.SetActive(false);
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

 


    //���� ���� �� �� enemy�� ��Ʈ�� ���� ��ġ�� �޾Ƽ� ���� ����
    public void gameStartCourtain(Vector3[] _waypoint, Vector3 _SpawnPos)
    {
        Debug.Log("���� ����");
        waypoint = _waypoint;
        SpawnPos = _SpawnPos;
        StartCoroutine("GameStart");
    }

    int X = 0;

    IEnumerator GameStart()
    {
        GameManager.buttonOff();

        weather.GameStart();

       gameongoing = true;

        //���� ���� ����
        int count = stageinfo[StageNum ].EnemyCount;
        int stagenum = StageNum;
        
        //�� ����
        for (int i = 0; i < count; i++)
        {
            X++;
            int enemynum = 0;
            if (StageNum == 0)
            {
                enemynum = 0;
            }
            else if (StageNum == 1)
            {
                enemynum = Random.Range(StageNum - 1, StageNum + 1);
            }
            else if (StageNum == 2)
            {
                enemynum = Random.Range(StageNum - 2, StageNum + 1);
            }
            else
            {
                enemynum = Random.Range(StageNum - 3, StageNum + 1);
            }

            var enemy = Pooling.GetEnemy(enemynum, SpawnPos);
            enemy.SetUpEnemy(this,waypoint,canvas,hpbar,damagenum, water);
            enemy.SetPooling(Pooling, enemynum);
            enemy.StartMove();

            //��ȯ�Ǵ� enemy�� list�� �߰�
            EnemyCount.Add(enemy.GetComponent<Enemy>());
            EnemyRemainCount++;

            yield return new WaitForSeconds(stageinfo[StageNum].SpawnTime);
        }
        SpawnFinish = true;


        while (true)
        {
            if (SpawnFinish && EnemyRemainCount == 0)
            {
                StageNum++;

                stageclear();

                if (StageNum >= stageinfo.Length)
                {
                    ClearPanal.SetActive(true);

                    int sumCoin = 0;

                    speedSet.StopGame();




                    UserInformation.userDataStatic.userCoin += (stagenum+1 * 20);
                    sumCoin += stagenum+1 * 20;


                    PlusCoin1.text = "ȹ������ : " +sumCoin;

                    //�� ������ ���� ��� ���
                }

                gameongoing = false;

                break;
            }
            yield return null;
        }
    }

    public bool GetGameOnGoing => gameongoing;


    //������ ���� ü���� �� �Ǽ� ���� ��
    public void EnemyDie(Enemy enemy,int coin)
    {
        playerstate.PlayerCoinUp(coin + Mathf.CeilToInt(coin * EnemyCoinRate));
        EnemyCount.Remove(enemy);
        EnemyRemainCount--;
    }

    //������ ���� �������� �������� ��
    public void EnemyArriveDestination(Enemy enemy)
    {
        playerstate.PlayerLifeDown();
        EnemyCount.Remove(enemy);
        EnemyRemainCount--;

        if (playerstate.GetPlayerLife <= 0)
        {
            speedSet.StopGame();
            FailPanal.SetActive(true);

            UserInformation.userDataStatic.userCoin += (StageNum * 50);
            PlusCoin2.text = "ȹ������ : " + StageNum * 50;
            Debug.Log(UserInformation.userDataStatic.userCoin);
        }
    }

    public bool GameOnGoing
    {
        get
        {
            return gameongoing;
        }
    }




}

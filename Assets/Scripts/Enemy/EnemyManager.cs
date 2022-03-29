using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class StageInfo
{
    public float SpawnTime = 0;
    public int EnemyCount;
    public GameObject[] enemykind;
}
public class EnemyManager : MonoBehaviour
{
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
    [SerializeField] private GameObject FailPanal = null;
    private bool StageClear = false;

    [SerializeField] private Transform water = null;

    //��ȯ�Ǵ� ������ ������ ���� List
    List<Enemy> EnemyCount = null;

    [SerializeField] private WeatherSetting weather =  null;

    public int Getmaxstage => stageinfo.Length;
    public int Getcurrentstage => StageNum + 1;

    private void Start()
    {
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
        waypoint = _waypoint;
        SpawnPos = _SpawnPos;
        StartCoroutine("GameStart");
    }

    IEnumerator GameStart()
    {
        weather.UpSeaLevel();

       gameongoing = true;

        //���� ���� ����
        int count = stageinfo[StageNum ].EnemyCount;
        int stagenum = StageNum;
        //�� ����
        GameObject[] EnemyList = stageinfo[StageNum].enemykind;

        int enemykind = EnemyList.Length;

        for (int i = 0; i < count; i++)
        {
            int enemynum = Random.Range(0, enemykind);
            GameObject enemy = Instantiate(EnemyList[enemynum], SpawnPos, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetUpEnemy(this,waypoint,canvas,hpbar,damagenum, water);

            //��ȯ�Ǵ� enemy�� list�� �߰�
            EnemyCount.Add(enemy.GetComponent<Enemy>());


                yield return new WaitForSeconds(stageinfo[StageNum].SpawnTime);
            
        }
        SpawnFinish = true;


        while (true)
        {
            if (SpawnFinish && EnemyCount.Count == 0)
            {
                StageNum++;
                if (StageNum >= stageinfo.Length)
                {
                    ClearPanal.SetActive(true);
                }

                gameongoing = false;
                weather.WeatherSettings();

                break;
            }
            yield return null;
        }
    }

    public bool GetGameOnGoing => gameongoing;


    //������ ���� ü���� �� �Ǽ� ���� ��
    public void EnemyDie(Enemy enemy,int coin)
    {
        playerstate.PlayerCoinUp(coin);
        EnemyCount.Remove(enemy);
    }

    //������ ���� �������� �������� ��
    public void EnemyArriveDestination(Enemy enemy)
    {
        playerstate.PlayerLifeDown();
        EnemyCount.Remove(enemy);

        if (playerstate.PlayerLife <= 0)
        {
            FailPanal.SetActive(true);
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

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
    public GameObject[] enemykind;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] starImage = null;

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

    [SerializeField] private WeatherSetting weather =  null;

    private EnemyPooling Pooling = null;

    public int Getmaxstage => stageinfo.Length;
    public int Getcurrentstage => StageNum + 1;

    private void Start()
    {
        Pooling = this.GetComponent<EnemyPooling>();

        if (UserInformation.userDataStatic.skillSet[2].skillUnLock)
        {
            EnemyCoinRate = (UserInformation.userDataStatic.skillSet[2].damage/100);
        }

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

        Debug.Log(count);

        for (int i = 0; i < count; i++)
        {
            //int enemynum = Random.Range(0, enemykind);
            //GameObject enemy = Instantiate(EnemyList[enemynum], SpawnPos, Quaternion.identity);
            //enemy.GetComponent<Enemy>().SetUpEnemy(this,waypoint,canvas,hpbar,damagenum, water);

            var enemy = Pooling.GetEnemy(1, SpawnPos);
            enemy.SetUpEnemy(this,waypoint,canvas,hpbar,damagenum, water);
            enemy.SetPooling(Pooling, 1);

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

                //�������� Ŭ����
                if (StageNum >= stageinfo.Length)
                {
                    ClearPanal.SetActive(true);

                    int sumCoin = 0;

                    UserInformation.userDataStatic.userCoin += (stagenum+1 * 20);
                    sumCoin += stagenum+1 * 20;


                    //������ ���� �� ���

                    Debug.Log(UserInformation.userDataStatic.stageClear[0].Star1);


                    if (!UserInformation.userDataStatic.stageClear[0].Star1)
                    {
                        UserInformation.userDataStatic.stageClear[0].Star1 = true;
                        UserInformation.userDataStatic.userCoin += 200;
                        sumCoin += 200;
                    }

                    if (!UserInformation.userDataStatic.stageClear[0].Star2)
                    {
                        if (playerstate.GetPlayerLife >= 15)
                        {
                            starImage[0].SetActive(true);
                            UserInformation.userDataStatic.stageClear[0].Star2 = true;
                            UserInformation.userDataStatic.userCoin += 200;
                            sumCoin += 200;
                        }
                    }
                    else if (UserInformation.userDataStatic.stageClear[0].Star2)
                    {
                        starImage[0].SetActive(true);
                    }

                    if (!UserInformation.userDataStatic.stageClear[0].Star3)
                    {
                        if (playerstate.GetPlayerLife >= 25)
                        {
                            starImage[1].SetActive(true);
                            UserInformation.userDataStatic.stageClear[0].Star3 = true;
                            UserInformation.userDataStatic.userCoin += 200;
                            sumCoin += 200;
                        }
                    }
                    else if (UserInformation.userDataStatic.stageClear[0].Star3)
                    {
                        starImage[1].SetActive(true);
                    }

                    PlusCoin1.text = "ȹ������ : " +sumCoin;

                    //�� ������ ���� ��� ���


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
        playerstate.PlayerCoinUp(coin + Mathf.CeilToInt(coin * EnemyCoinRate));
        EnemyCount.Remove(enemy);
    }

    //������ ���� �������� �������� ��
    public void EnemyArriveDestination(Enemy enemy)
    {
        playerstate.PlayerLifeDown();
        EnemyCount.Remove(enemy);

        if (playerstate.GetPlayerLife <= 0)
        {
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

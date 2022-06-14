using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ÿ���� ���׷��̵� ����

public class UpgradeValue
{
    //Ÿ�� ���׷��̵� ���
    public int priceUprate = 0;
    public int upgradeprice = 0;
    public float UpdamageValue = 0;
    public float UpcriticalValue = 0;
}


//Ÿ���� ����

public class TowerInfo
{
    //Ÿ���� ����
    public int towerprice = 0;
    public string towername = null;
    public float towerdamage = 0;
    public float towercritical = 0;
    public float towerrange = 0;
    public float atkdelay = 0;
}

public class Tower : MonoBehaviour
{
    public enum TowerType
    {
        Unknown,
        offense,
        support
    }

    private void Awake()
    {
       TowerSetUp(TowerDataSetUp.GetData(TowerCode));

    }

    private void TowerSetUp(TowerData towerdata)
    {
        towerinfo.atkdelay = towerdata.Delay;
        towerinfo.towerrange = towerdata.Range;
        towerinfo.towercritical = towerdata.Critical;
        towerinfo.towerdamage = towerdata.Damage;
        towerinfo.towername = towerdata.Name;
        towerinfo.towerprice = towerdata.TowerPrice;

        upgradevalue.UpdamageValue = towerdata.UpgradeAtk;
        upgradevalue.UpcriticalValue = towerdata.UpgradeCri;
        upgradevalue.upgradeprice = towerdata.UpgradePrice;
        upgradevalue.priceUprate = towerdata.UpgradePrice;
    }

    

    [SerializeField] private int TowerCode;
    public int GetTowerCode => TowerCode;

    protected AudioSource AS = null;

    //��ȭ �� ���� Ÿ��
    [SerializeField] private GameObject uppertower = null;

    //Ÿ���� ���׷��̵� ��ġ
    UpgradeValue upgradevalue = new UpgradeValue();

    //Ÿ���� ��ġ
    protected TowerInfo towerinfo = new TowerInfo();

    //tower prefab���� �Ѱ��� ���� Ui
    public GameObject[] buildstate = null;

    //�̸����� Ÿ�� ������
    [SerializeField] private GameObject towerpreview = null;

    //�̸����� Ÿ�� ����
    //private GameObject preview = null;

    private bool TowerCanWork = true;

    //�� �������� ���ư� ����
    //y�� ȸ��
    [SerializeField] protected Transform towerBody;
    //x�� ȸ��
    [SerializeField] protected Transform towerTurret;

    [SerializeField] protected Transform shootPos = null;

    //������ �˻��� ���̾�
    [SerializeField] private LayerMask layer;

    //�÷��̾� coin���� ������ playstate
    private PlayerState playerstate = null;

    //��ź�� �߻� �� ���ݼӵ� ���� ���� �� ����� ����
    protected float atkspeed = 0;

    //Ÿ���� ���׷��̵� ����
    private int towerlevel = 0;
    
    //Ÿ���� ��ü �ܰ� 1,2,3�ܰ�
    [SerializeField] private int towerstep = 1;


    //������ ȸ���� �ӵ�
    protected float rotationspeed = 720;

    //�� Ÿ�� Transform
    protected Transform FinalTarget = null;

    //���� Ÿ�� �Ʒ��� �ִ� Ÿ���� ���
    private Node node = null;

    //Ÿ���� ������� ����
    private bool towericed = false;
    public bool GetIced { get => towericed; set => towericed = value; }

    //Ÿ���� ���ݹ���
    private ShowTowerInfo showtowerinfo = null;

    private bulletpolling objectPooling = null;

    protected Camera cam = null;

    [SerializeField] 
    protected GameObject AtkParticle = null;

    protected virtual void Start()
    {
        AS = this.GetComponent<AudioSource>();
        atkspeed = towerinfo.atkdelay;
        StartCoroutine("AutoSearch");
        node.GetOnTower = true;
        MultipleSpeed.speedup += SpeedUP;
        cam = Camera.main;
        AS.volume = PlayerPrefs.GetFloat("ESound");
        SoundSettings.effectsound += SoundChange;

        upgradevalue.upgradeprice = (int)(upgradevalue.upgradeprice*SkillSettings.PassiveValue("UpTowerDown"));
        towerinfo.towerprice = (int)(towerinfo.towerprice * SkillSettings.PassiveValue("SetTowerDown"));

        sellprice += (int)(towerinfo.towerprice * SkillSettings.PassiveValue("SellTowerUp"));
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    private void SoundChange(float x)
    {
        if (AS != null)
        {
            AS.volume = x;
        }
    }

    protected virtual void Update()
    {
       // AS.volume = SoundSettings.currentsound;
        if (TowerCanWork&&!towericed)
        {
            if (FinalTarget != null)
            {
                RotateTurret();
            }
            else
            {
                AS.Stop();
            }
        }
    }

    public void SetUp(PlayerState _playerstate)
    {
        playerstate = _playerstate;
    }

    
    //Ÿ�� �ڵ� Ž�� �Լ�
    IEnumerator AutoSearch()
    {
        while (true)
        {
            if (FinalTarget != null)
            {
                Vector3 GetDistance = FinalTarget.position - this.transform.position;
                if (Vector3.Magnitude(GetDistance)>towerinfo.towerrange)
                {
                    FinalTarget = null;
                }
            }

            //OverlapSphere : ��ü �ֺ��� Collider�� ����
            //������ collider�� �迭�� ������ ����
            Collider[] E_collider = Physics.OverlapSphere(this.transform.position, towerinfo.towerrange, layer);

            //���� ª�� �Ÿ��� ������Ʈ ��ġ�� ���� ����
            Transform ShortestTarget = null;

            if (FinalTarget == null)
            {
                if (E_collider.Length > 0)
                {
                    float S_ShortestTarget = Mathf.Infinity;
                    // �Ÿ���꿡 ����� ���� ����.

                    foreach (Collider EC in E_collider)
                    {
                        float CalDistance = Vector3.SqrMagnitude(EC.transform.position - this.transform.position);
                        // �ͷ��� ����� collider���� �Ÿ��� ���� ��������
                        // Vector3.Distance�� Vector3.magnitude�� �Ÿ��񱳸� �� �� ������ �� ���� Root�� ���� ���� �Ÿ��� ����ϱ� ������ ������ �� ����.
                        //SqrMagnitude�� �����Ÿ�*�����Ÿ��� Root�� ������ �ʴ� �Լ��� �ܼ� �Ÿ����� ���� �̰��� ���� �� ���� �ӵ��� ������.

                        if (CalDistance < S_ShortestTarget)
                        {
                            S_ShortestTarget = CalDistance;
                            ShortestTarget = EC.transform;
                        }
                    }
                    FinalTarget = ShortestTarget;
                    //���� �Ÿ��� ª�� ����� ���� Ÿ������ ����.
                }
            }
            yield return null;
        }
    }



    //���ݹ���� �ٸ��� ���� �� �ִ� �Լ�

    //Y�� ȸ�� �Լ�
    //x�� ȸ�� �Լ�

    //X�� Y�� ���� ȸ�� �Լ�


    //protected void RotateXY()
    //{
    //    Vector3 relativePos = FinalTarget.position - transform.position;
    //    //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
    //    Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

    //    //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
    //    Vector3 TowerDir = Quaternion.RotateTowards(towerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

    //    //������ rotation���� Vector3���·� ������ �� ���
    //    towerBody.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);

    //    if (Quaternion.Angle(towerBody.rotation, rotationtotarget) < 3.0f)
    //    {
    //        atkspeed -= Time.deltaTime;
    //        if (atkspeed <= 0)
    //        {
    //            atkspeed = towerinfo.atkdelay;
    //            int critical = Random.Range(1, 101);

    //            Debug.Log("Attack");
    //            //var BT = objectPooling.GetObject(bulletpos.position);

    //            //if (critical > towerinfo.towercritical)
    //            //    BT.SetBulletTest(FinalTarget, towerinfo.towerdamage, objectPooling);

    //            //else if (critical < towerinfo.towercritical)
    //            //    BT.SetBulletTest(FinalTarget, towerinfo.towerdamage * 2, objectPooling);
    //        }
    //    }
    //}

    protected bool Atking = false;

    protected virtual void RotateTurret()
    {
        
        Vector3 relativePos = FinalTarget.position - transform.position;
        //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
        Vector3 TowerDir = Quaternion.RotateTowards(towerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;
        Vector3 TowerDir2 = Quaternion.RotateTowards(towerTurret.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        //������ rotation���� Vector3���·� ������ �� ���
        towerBody.rotation = Quaternion.Euler(0, TowerDir.y, 0);
        towerTurret.rotation = Quaternion.Euler(TowerDir2.x + (FinalTarget.localScale.y / 2), TowerDir2.y, 0);
        
        
        if (Quaternion.Angle(towerTurret.rotation, rotationtotarget) < 1.0f)
        {
            Atking = true;
            atkspeed -= Time.deltaTime;
            if (atkspeed <= 0)
            {
                atkspeed = towerinfo.atkdelay;
                int critical = Random.Range(1, 101);
                Attack();
            }
        }
        else
        {
            Atking = false;
        }


    }


    protected virtual void Attack() 
    {

    }



    public void TowerUpgrade()
    {
        if (playerstate.GetSetPlayerCoin >= (int)(upgradevalue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown")))
        {
            sellprice += (int)(upgradevalue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));

            towerlevel++;
            towerinfo.towerdamage += upgradevalue.UpdamageValue;
            if (towerinfo.towercritical + upgradevalue.UpcriticalValue >= 100)
            {
                towerinfo.towercritical = 100;
            }
            else
            {
                towerinfo.towercritical += upgradevalue.UpcriticalValue;
            }

            playerstate.GetSetPlayerCoin = (int)(upgradevalue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));
            upgradevalue.upgradeprice += (int)(upgradevalue.priceUprate * SkillSettings.PassiveValue("UpTowerDown"));
        }

    }

    public void SellTower()
    {
        GameManager.buttonOff();
        if (!towericed)
        {
            playerstate.GetSetPlayerCoin = (-sellprice);
            node.GetOnTower = false;
            Destroy(this.gameObject);
        }
    }

    private int sellprice = 0;
    public int GetSetSellPrice { get => sellprice; set => sellprice = value; }

    public void TowerMove()
    {
        GameManager.buttonOff();
        if (!towericed)
        {
            TowerCanWork = true;

            GameObject preview = Instantiate(towerpreview, this.transform.position, Quaternion.identity);
            showtowerinfo.SetTowerinfoOff();
            //preview.GetComponent<TowerPreview>().SetBuildState = buildstate;
            //preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);

            preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, towerinfo.towerrange);

            preview.GetComponent<TowerPreview>().TowerMoveSetUp(this.gameObject);

            ActiveOff();
        }
    }

    public ShowTowerInfo SetShowTower
    {
        set
        {
            showtowerinfo = value;
        }
    }

    public void ActiveOff()
    {
 
        this.gameObject.SetActive(false);
        node.GetOnTower = false;
    }

    public void ActiveOn()
    {
        showtowerinfo.ShowInfo(this);
        showtowerinfo.ShowRange(this.gameObject.transform, towerinfo.towerrange);
        this.gameObject.SetActive(true);
        node.GetOnTower = true;
        StartCoroutine("AutoSearch");
    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradevalue.UpdamageValue);
        towerinfo.towercritical += (lev * (float)upgradevalue.UpcriticalValue);
        upgradevalue.upgradeprice += towerlevel * upgradevalue.priceUprate;
       
    }

    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        buildedtower.GetComponent<Tower>().SetState(_tower.GetTowerLevel, towerlevel);
        buildedtower.GetComponent<Tower>().towerinfo.towername = towerinfo.towername;

        buildedtower.GetComponent<Tower>().GetSetSellPrice += 
            ((sellprice - towerinfo.towerprice) + (_tower.sellprice-_tower.towerinfo.towerprice));



        showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
        showtowerinfo.ClickTower();

        Destroy(this.gameObject);
    }

    public void TowerUpSkill()
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        buildedtower.GetComponent<Tower>().SetState(1, towerlevel);
        buildedtower.GetComponent<Tower>().towerinfo.towername = towerinfo.towername;
        showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
        showtowerinfo.ClickTower();

        Destroy(this.gameObject);
    }


    //�����䰡 Ÿ������ �Ѱ��� ����
    //Ÿ���� ���� Ÿ������ �Ѱ��� ����
    public void TowerSetUp(Node _node, ShowTowerInfo _showtowerinfo,GameObject[] _buildstate,PlayerState _playerstate)
    {
        node = _node;
        showtowerinfo = _showtowerinfo;
        buildstate = _buildstate;
        playerstate = _playerstate;
    }
    public GameObject[] GetBuildState
    {
        set
        {
            buildstate = value;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            other.GetComponent<Node>().GetOnTower = true;
        }
    }

    #region TowerProperties

    public bool SetTowerCanWork
    {
        get => TowerCanWork;
        set => TowerCanWork = value;
    }
    public Node SetNode
    {
        get
        {
            return node;
        }
        set
        {
            node = value;
        }
    }
         
    
    //Ÿ�� �̸�
    public string Getname => towerinfo.towername;
    //Ÿ�� �ܰ�
    public int GetStep { get => towerstep; }
    //Ÿ������
    public int GetTowerLevel => towerlevel;
    //Ÿ�� ������
    public float GetDamage => towerinfo.towerdamage;
    //ũ��Ƽ�� Ȯ��
    public float GetCritical => towerinfo.towercritical;
    //Ÿ�� ���� �ӵ�
    public float GetSpeed => atkspeed;
    //Ÿ�� ���ݹ���
    public float GetRange => towerinfo.towerrange;
    //Ÿ������
    public int Gettowerprice => towerinfo.towerprice;

    public int Gettowerupgradeprice => (int)(upgradevalue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));

    public float GetTowerUPDamage => upgradevalue.UpdamageValue;
    public float GetTowerUpCri => upgradevalue.UpcriticalValue;

    #endregion
}

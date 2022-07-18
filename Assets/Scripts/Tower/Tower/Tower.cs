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
    public int towerstep = 0;
    public float towerdamage = 0;
    public float towercritical = 0;
    public float towerrange = 0;
    public float atkdelay = 0;
    public int CanAtk = 0;
}

[RequireComponent(typeof(AudioSource))]

public class Tower : MonoBehaviour
{
    [SerializeField] GameObject SM;

    [SerializeField] private int TowerCode = 0;

    protected bool TowerCanWork = true;

    protected AudioSource AS = null;

    //������ �˻��� ���̾�
    protected int SearchLayer;

    protected virtual void Awake()
    {
        this.transform.rotation = Quaternion.Euler(0, 180,0);
       TowerSetUp(TowerDataSetUp.GetData(TowerCode));
    }

    private void TowerSetUp(TowerDataFrame towerdata)
    {
        towerinfo.atkdelay = towerdata.delay;
        towerinfo.towerrange = towerdata.range;
        towerinfo.towercritical = towerdata.critical;
        towerinfo.towerdamage = towerdata.damage;
        towerinfo.towername = towerdata.name;
        towerinfo.towerprice = towerdata.towerPrice;
        towerinfo.towerstep = towerdata.towerStep;
        towerinfo.CanAtk = towerdata.atkType;

        towerstep = towerdata.towerStep;

        upgradevalue.UpdamageValue = towerdata.upgradAtk;
        upgradevalue.UpcriticalValue = towerdata.upgradCri;
        upgradevalue.upgradeprice = towerdata.upgradPrice;
        upgradevalue.priceUprate = 0;
    }

    

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



    //�� �������� ���ư� ����
    //y�� ȸ��
    [SerializeField] protected Transform towerBody;
    //x�� ȸ��
    [SerializeField] protected Transform towerTurret;

    [SerializeField] protected Transform shootPos = null;

    //�÷��̾� coin���� ������ playstate
    private PlayerState playerstate = null;

    //��ź�� �߻� �� ���ݼӵ� ���� ���� �� ����� ����
    protected float atkspeed = 0;

    //Ÿ���� ���׷��̵� ����
    private int towerlevel = 0;
    
    //Ÿ���� ��ü �ܰ� 1,2,3�ܰ�
    private int towerstep = 1;

    //������ ȸ���� �ӵ�
    protected float rotationspeed = 720;

    //�� Ÿ�� Transform
    protected Transform FinalTarget = null;

    //���� Ÿ�� �Ʒ��� �ִ� Ÿ���� ���
    private Node node = null;

    public bool GetCanWork { get => TowerCanWork; set => TowerCanWork = value; }

    //Ÿ���� ���ݹ���
    private ShowTowerInfo showtowerinfo = null;

    private bulletpolling objectPooling = null;

    protected Camera cam = null;

    [SerializeField] 
    protected GameObject AtkParticle = null;

    protected virtual void Start()
    {
        AS = this.GetComponent<AudioSource>();
        Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(1);
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

    private void OnDestroy()
    {
        SoundSettings.effectsound -= SoundChange;
    }

    private void SoundChange(float x)
    {
        if (AS != null)
        {
            AS.volume = x;
        }
    }

    protected bool SoundStop = true;

    public void SetUp(PlayerState _playerstate)
    {
        playerstate = _playerstate;
    }

    
    //Ÿ�� �ڵ� Ž�� �Լ�
    protected virtual IEnumerator AutoSearch()
    {
        yield return null;
    }

    protected bool Atking = false;

    public void TowerUpgrade()
    {
        int upgradeprice = (int)(upgradevalue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));

        if (playerstate.GetSetPlayerCoin >= upgradeprice)
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(2);
            sellprice += upgradeprice;

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
        else
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(0);
        }

    }

    public void SellTower()
    {
        GameManager.buttonOff();

            playerstate.GetSetPlayerCoin = (-sellprice);
            node.GetOnTower = false;
            Destroy(this.gameObject);
        Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(3);
    }

    private int sellprice = 0;
    public int GetSetSellPrice { get => sellprice; set => sellprice = value; }

    public void TowerMove()
    {
        GameManager.buttonOff();
        if (TowerCanWork)
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(2);
            TowerCanWork = true;

            GameObject preview = Instantiate(towerpreview, this.transform.position, Quaternion.identity);
            showtowerinfo.SetTowerinfoOff();
            //preview.GetComponent<TowerPreview>().SetBuildState = buildstate;
            //preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);

            preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, towerinfo.towerrange);

            preview.GetComponent<TowerPreview>().TowerMoveSetUp(this.gameObject);

            ActiveOff();
        }
        else
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(1);
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
    public int GetTowerCode => TowerCode;

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeValue
{
    //Ÿ�� ���׷��̵� ���
    public int priceincrease = 0;
    public int upgradeprice = 0;
    public float damagevalue = 0;
    public float upcriticalvalue = 0;
}

[System.Serializable]
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
    //��ȭ �� ���� Ÿ��
    [SerializeField] private GameObject uppertower = null;

    //Ÿ���� ���׷��̵� ��ġ
    [SerializeField] UpgradeValue upgradevalue = null;
    //Ÿ���� ��ġ
    [SerializeField] protected TowerInfo towerinfo = null;

    //tower prefab���� �Ѱ��� ���� ui
    public GameObject[] buildstate = null;
    //�̸����� Ÿ�� ������
    [SerializeField] private GameObject towerpreview = null;

    //�̸����� Ÿ�� ����
    //private GameObject preview = null;

    //�� �������� ���ư� ����
    //y�� ȸ��
    [SerializeField] protected Transform towerBody;
    //x�� ȸ��
    [SerializeField] protected Transform towerTurret;

    [SerializeField] protected Transform shootPos = null;

    //���� �˻��� ���̾�
    [SerializeField] private LayerMask enemylayer;

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

    //Ÿ���� ���ݹ���
    private ShowTowerInfo showtowerinfo = null;

    private ObjectPooling objectPooling = null;

    protected Camera cam = null;

    [SerializeField] protected GameObject AtkParticle = null;
    protected virtual void Start()
    {

        atkspeed = towerinfo.atkdelay;
        StartCoroutine(AutoSearch());
        //node.GetOnTower = true;
        MultipleSpeed.speedup += SpeedUP;
        cam = Camera.main;
    }

private void SpeedUP(int x)
{
    Time.timeScale = x;
}

    protected virtual void Update()
    {


        if (FinalTarget == null)
        {

        }
        else
        {
            RotateTurret();
        }

        //if (towermove)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        towermove = false;
        //        Base.SetActive(true);
        //        towercollider.enabled = true;
        //        Destroy(preview);
        //        showtowerinfo.ShowRange(this.transform,towerinfo.towerrange);

        //    }
        //}


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
            Collider[] E_collider = Physics.OverlapSphere(this.transform.position, towerinfo.towerrange, enemylayer);

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
        towerTurret.rotation = Quaternion.Euler(TowerDir2.x+(FinalTarget.localScale.y/2), TowerDir2.y, 0);

        if (Quaternion.Angle(towerTurret.rotation, rotationtotarget) < 1.0f)
        {

            atkspeed -= Time.deltaTime;
            if (atkspeed <= 0)
            {
                atkspeed = towerinfo.atkdelay;
                int critical = Random.Range(1, 101);

                Attack();
            }
        }


    }

  

    protected virtual void Attack() { }



    public void TowerUpgrade()
    {
        
        if (playerstate.GetSetPlayerCoin >= upgradevalue.upgradeprice)
        {
            towerlevel++;
            towerinfo.towerdamage += upgradevalue.damagevalue;
            if (towerinfo.towercritical + upgradevalue.upcriticalvalue >= 100)
            {
                towerinfo.towercritical = 100;
            }
            else
            {
                towerinfo.towercritical += upgradevalue.upcriticalvalue;
            }
            playerstate.GetSetPlayerCoin = upgradevalue.upgradeprice;
            upgradevalue.upgradeprice += upgradevalue.priceincrease;
        }
    }

    public void SellTower()
    {
        playerstate.GetSetPlayerCoin = -(int)((towerinfo.towerprice + upgradevalue.upgradeprice * towerstep) * 0.7f);
        node.GetOnTower = false;
        Destroy(this.gameObject);
    }



    //public void Combine()
    //{
    //    StartCoroutine("TowerCombination");
    //}

    //IEnumerator TowerCombination()
    //{
    //    Base.SetActive(false);
    //    towercollider.enabled = false;
    //    GameObject preview = Instantiate(towerpreview);
    //    preview.GetComponent<TowerPreview>().SetUp(this.gameObject);

    //    while (true)
    //    {

    //        if (preview.GetComponent<TowerPreview>().GetCanCombine && preview.GetComponent<TowerPreview>().AlreadyTower)
    //        {
    //            Debug.Log(preview.GetComponent<TowerPreview>().GetCanCombine);
    //            Debug.Log(preview.GetComponent<TowerPreview>().AlreadyTower);
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                GameObject UpperTower = Instantiate(uppertower, preview.transform.position, Quaternion.identity);
    //                if (preview.GetComponent<TowerPreview>().GetTower != null)
    //                {
    //                    Tower tower = preview.GetComponent<TowerPreview>().GetTower;
    //                    Destroy(tower.gameObject);
    //                    UpperTower.GetComponent<Tower>().SetState(towerlevel, tower.GetTowerLevel);
    //                    UpperTower.GetComponent<Tower>().SetUp(playerstate);
    //                }

    //                Destroy(preview);
    //                Destroy(this.gameObject);
    //            }
    //        }

    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            Destroy(preview);
    //            Base.SetActive(true);
    //            towercollider.enabled = true;
    //            break;
    //        }

    //        yield return null;
    //    }

    //}

    public void TowerMove()
    {
        Debug.Log(towerpreview);
        GameObject preview = Instantiate(towerpreview,this.transform.position,Quaternion.identity);

        //preview.GetComponent<TowerPreview>().SetBuildState = buildstate;
        //preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);

        preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, towerinfo.towerrange);

        preview.GetComponent<TowerPreview>().TowerMoveSetUp(this.gameObject);

        ActiveOff();
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

        this.gameObject.SetActive(true);
        node.GetOnTower = true;
    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradevalue.damagevalue);
        towerinfo.towercritical += (lev * (float)upgradevalue.upcriticalvalue);
    }

    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        buildedtower.GetComponent<Tower>().SetState(_tower.GetTowerLevel, towerlevel);
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
    public int GetStep {
        get
        {
         return   towerstep;
        }
    }
    
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

    public int Gettowerupgradeprice => upgradevalue.upgradeprice;

}

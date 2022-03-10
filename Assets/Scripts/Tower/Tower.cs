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
    public float towerspeed = 0;
}



public class Tower : MonoBehaviour
{
    //��ȭ�� ���� Ÿ��
    [SerializeField] private GameObject uppertower = null;
    //�̸����� Ÿ��
    [SerializeField] private GameObject towerpreview = null;
    //Ÿ�� �̸����� ������
    private GameObject preview = null;

    //��ü�� �� ��� ����� ������Ʈ�� collider
    [SerializeField] private GameObject Base = null;
    private BoxCollider towercollider = null;

    //�� �������� ���ư� ����
    [SerializeField] private Transform TowerBody;

    //�Ѿ��� �߻��� ��ġ�� �Ѿ� ������
    [SerializeField] private Transform bulletpos;
    [SerializeField] private GameObject bullet;

    //���� �˻��� ���̾�
    [SerializeField] private LayerMask enemylayer;

    private PlayerState playerstate = null;

    [SerializeField] UpgradeValue upgradevalue = null;
    [SerializeField] TowerInfo towerinfo = null;

    //��ź�� �߻� �� ���ݼӵ� ���� ���� �� ����� ����
    private float atkspeed = 0;

    //Ÿ���� ���׷��̵� ����
    private int towerlevel = 1;
    
    //Ÿ���� ��ü �ܰ� 1,2,3�ܰ�
    private int towerstep = 1;

    //������ ȸ���� �ӵ�
    private float rotationspeed = 1080;

    //�� Ÿ�� Transform
    private Transform FinalTarget = null;

    private bool towermove = false;

    private Node node = null;

    public ShowTowerInfo showtowerinfo = null;
   

    void Start()
    {
        towercollider = this.GetComponent<BoxCollider>();
        atkspeed = towerinfo.towerspeed;
        StartCoroutine("AutoSearch");
        node.GetComponent<Node>().GetOnTower = true;

        Base.SetActive(true);
        towercollider.enabled = true;
        MultipleSpeed.speedup += SpeedUP;
    }

    

private void SpeedUP(int x)
{
    Time.timeScale = x;
}
public void SetActiveOn()
    {

        Base.SetActive(true);
        towercollider.enabled = true;
    }

    private void Update()
    {
        if (FinalTarget == null)
        {
 
        }
        else
        {
             RotateToTarget();
        }

        if (towermove)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                towermove = false;
                Base.SetActive(true);
                towercollider.enabled = true;
                Destroy(preview);
                showtowerinfo.ShowRange(this.transform,towerinfo.towerrange);
                Debug.Log("2222");
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

    

    private void RotateToTarget()
    {
        Vector3 relativePos = FinalTarget.position - transform.position;
        //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        
        //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
        Vector3 TowerDir = Quaternion.RotateTowards(TowerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        Debug.DrawLine(this.transform.position, FinalTarget.position, Color.red);


        //������ rotation���� Vector3���·� ������ �� ���
        TowerBody.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);




        if (Quaternion.Angle(TowerBody.rotation, rotationtotarget) < 3.0f)
        {
            atkspeed -= Time.deltaTime;
            if (atkspeed <= 0)
            {
                atkspeed = towerinfo.towerspeed;
                int critical = Random.Range(1, 101);
                
                GameObject BT = Instantiate(bullet, bulletpos.position, Quaternion.identity);
                
                if (critical > towerinfo.towercritical) 
                BT.GetComponent<BulletTest>().SetBulletTest(FinalTarget, towerinfo.towerdamage);

                else if(critical<towerinfo.towercritical)
                    BT.GetComponent<BulletTest>().SetBulletTest(FinalTarget, towerinfo.towerdamage*2);
            }
        }

    }

    public void TowerUpgrade()
    {
        
        if (playerstate.GetSetPlayerCoin >= upgradevalue.upgradeprice)
        {
            towerlevel++;
            towerinfo.towerdamage += upgradevalue.damagevalue;
            towerinfo.towercritical += upgradevalue.upcriticalvalue;
            playerstate.GetSetPlayerCoin = upgradevalue.upgradeprice;
            upgradevalue.upgradeprice += upgradevalue.priceincrease;
        }
    }

    public void SellTower()
    {
        playerstate.GetSetPlayerCoin = -(int)((towerinfo.towerprice + upgradevalue.upgradeprice * towerstep) * 0.7f);
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

        towermove = true;
        Base.SetActive(false);
        towercollider.enabled = false;
        preview = Instantiate(towerpreview,this.transform.position,Quaternion.identity);
        preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);
        preview.GetComponent<TowerPreview>().SetUp(this.gameObject);
        preview.GetComponent<TowerPreview>().SetOriginTower=this.gameObject;

    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        Debug.Log(lev);
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradevalue.damagevalue);
        towerinfo.towercritical += (lev * (float)upgradevalue.upcriticalvalue);
    }

    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower =Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().SetNode = node;
        buildedtower.GetComponent<Tower>().SetState(_tower.GetTowerLevel, towerlevel);
        buildedtower.GetComponent<Tower>().GetStep = 1;
        Debug.Log(_tower.GetTowerLevel + " : " + towerlevel);
        buildedtower.GetComponent<Tower>().SetUp(playerstate);
        buildedtower.GetComponent<Tower>().showtowerinfo = showtowerinfo;

        showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
        showtowerinfo.SetTowerinfo();

        Destroy(this.gameObject);
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

    public void TowerDestroy()
    {
        Destroy(this.gameObject);
    }
        
    
    //Ÿ�� �̸�
    public string Getname => towerinfo.towername;
    //Ÿ�� �ܰ�
    public int GetStep {
        get
        {
         return   towerstep;
        }
        set
        {
            towerstep += 1;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeValue
{   
    //Ÿ�� ���׷��̵� ���
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
    private float rotationspeed = 360;

    //�� Ÿ�� Transform
    private Transform FinalTarget = null;

   

    void Start()
    {
        towercollider = this.GetComponent<BoxCollider>();
        atkspeed = towerinfo.towerspeed;
        StartCoroutine("AutoSearch");
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
                GameObject BT = Instantiate(bullet, bulletpos.position, Quaternion.identity);
                BT.GetComponent<BulletTest>().SetTarget=FinalTarget.position;

                int critical = Random.Range(1, 101);
                if (critical < towerinfo.towercritical) 
                BT.GetComponent<BulletTest>().SetDamage = towerinfo.towerdamage*2;

                else
                BT.GetComponent<BulletTest>().SetDamage = towerinfo.towerdamage;
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
        }
    }

    public void SellTower()
    {
        playerstate.GetSetPlayerCoin = -(int)((towerinfo.towerprice + upgradevalue.upgradeprice * towerstep) * 0.7f);
        Destroy(this.gameObject);
    }

    public void Combine()
    {
        StartCoroutine("TowerCombination");
    }

    IEnumerator TowerCombination()
    {
        Base.SetActive(false);
        towercollider.enabled = false;
        GameObject preview = Instantiate(towerpreview);
        preview.GetComponent<TowerPreview>().SetUp(towerinfo.towername,towerstep);

        while (true)
        {
            preview.GetComponent<TowerPreview>().Ontile();

            if (preview.GetComponent<TowerPreview>().GetCanCombine&& preview.GetComponent<TowerPreview>().AlreadyTower)
            {
                Debug.Log(preview.GetComponent<TowerPreview>().GetCanCombine);
                Debug.Log(preview.GetComponent<TowerPreview>().AlreadyTower);
                if (Input.GetMouseButtonDown(0))
                { 
                    GameObject UpperTower = Instantiate(uppertower, preview.transform.position, Quaternion.identity);
                    if (preview.GetComponent<TowerPreview>().GetTower != null)
                    {
                        Tower tower = preview.GetComponent<TowerPreview>().GetTower;
                        Destroy(tower.gameObject);
                        UpperTower.GetComponent<Tower>().SetState(towerlevel, tower.GetTowerLevel);
                        UpperTower.GetComponent<Tower>().SetUp(playerstate);
                    }
                    Destroy(preview);
                    Destroy(this.gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(preview);
                Base.SetActive(true);
                break;
            }




            yield return null;
        }
        
    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradevalue.damagevalue);
        towerinfo.towercritical += (lev * (float)upgradevalue.upcriticalvalue);
    }

    public string Getname => towerinfo.towername;
    public int gettowerstep => towerstep;
    public float getatkdelay => atkspeed;
    public float getatkdamage => towerinfo.towerdamage;
    public float getatkrange => towerinfo.towerrange;
    public float getatkcritical => towerinfo.towercritical;
    public int Gettowerprice => towerinfo.towerprice;
    public int GetTowerLevel => towerlevel;

}

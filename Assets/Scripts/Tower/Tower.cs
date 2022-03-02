using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeValue
{
    public float updamagevalue = 0;
    public float updamagecri = 0;
    public float upspeed = 0;
    public float uprange = 0;
}

public class Tower : MonoBehaviour
{

    [SerializeField] private Transform TowerBody;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletpos;

    [SerializeField] private LayerMask enemylayer;

    [SerializeField] private float AtkDelay = 0;
    private float atkdelay = 0;
    private float atkrange = 10;
    private float atkdamage = 0;
    private float criticalrate = 0;

    [SerializeField] private UpgradeValue upgradevalue = null;

    private PlayerState playerstate = null;
    private float towerprice = 0;

    private float rotationspeed = 270;

    private Transform FinalTarget = null;

    public float getatkdelay => atkdelay;
    public float getatkdamage => atkdamage;
    public float getatkrange => atkrange;
    public float getatkcritical => criticalrate;

    void Start()
    {
        
        atkdelay = AtkDelay;
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
            //OverlapSphere : ��ü �ֺ��� Collider�� ����
            //������ collider�� �迭�� ������ ����
            Collider[] E_collider = Physics.OverlapSphere(this.transform.position, atkrange, enemylayer);

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
            atkdelay -= Time.deltaTime;
            if (atkdelay <= 0)
            {
                atkdelay = AtkDelay;
                GameObject BT = Instantiate(bullet, bulletpos.position, Quaternion.identity);
                BT.GetComponent<BulletTest>().SetTarget=FinalTarget.position;
            }
        }

    }

    public void TowerUpgrade()
    {
        atkdamage += upgradevalue.updamagevalue;
        atkdelay += upgradevalue.upspeed;
        atkrange +=upgradevalue.uprange;
        criticalrate +=upgradevalue.updamagecri;
    }

    public void SellTower()
    {

    }
}

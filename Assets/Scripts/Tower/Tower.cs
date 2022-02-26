using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private Transform TowerBody;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletpos;

    [SerializeField] private LayerMask enemylayer;

    [SerializeField] private float AtkDelay = 0;
    private float atkdelay = 0;
    private float AtkRange = 10;
    private float AtkDamage = 0;

    private float rotationspeed = 180;

    private Transform FinalTarget = null;

    void Start()
    {
        atkdelay = AtkDelay;
        InvokeRepeating("AutoSearch",0,0.5f);
    }

    private void Update()
    {
        if (FinalTarget == null)
        {
            Debug.Log("�� ����");
        }
        else
        {
             RotateToTarget();
            Debug.Log("�� �߰�");
        }
    }

    
    //Ÿ�� �ڵ� Ž�� �Լ�
    public void AutoSearch()
    {
        Debug.Log("Ž�� ����");

            //OverlapSphere : ��ü �ֺ��� Collider�� ����
            //������ collider�� �迭�� ������ ����
            Collider[] E_collider = Physics.OverlapSphere(this.transform.position, AtkRange, enemylayer);

            //���� ª�� �Ÿ��� ������Ʈ ��ġ�� ���� ����
            Transform ShortestTarget = null;

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
}

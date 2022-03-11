using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaBullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
    private float Range = 3.0f;
    private int Count = 3;

    HashSet<Transform> enemylist = new HashSet<Transform>();

    private void Start()
    {
        bullspeed = 10.0f;
    }

    protected override void AtkCharactor()
    {
        target.GetComponent<Enemy>().EnemyAttacked(damage);

        Collider Origin = target.GetComponent<Collider>();

        Count--;
        damage--;
        Range -= 1.0f;
        if (Count <= 0)
        {
            Destroy(this.gameObject);
        }

        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, Range, enemylayer);
        Transform ShortestTarget = null;

        if (E_collider.Length > 1)
        {
            float S_ShortestTarget = Mathf.Infinity;
            // �Ÿ���꿡 ����� ���� ����.

            foreach (Collider EC in E_collider)
            {
                if(EC == Origin)
                {
                    Debug.Log("���ư�");
                    continue;
                }

                else
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
            }
            
            //���� �Ÿ��� ª�� ����� ���� Ÿ������ ����.
            
            SetBulletTest(ShortestTarget, damage);
        }
        else
        {
            Debug.Log("�ı�");
            Destroy(this.gameObject);
        }
    }
    
}

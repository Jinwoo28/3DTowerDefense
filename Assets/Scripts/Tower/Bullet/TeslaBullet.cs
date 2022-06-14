using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeslaBullet : MonoBehaviour
{
    private TeslaEffect TE = null;

    [SerializeField] private LayerMask enemylayer;
    private float Range = 0.6f;

    private Transform target = null;

    private TeslaTower tesla = null;

    private float Damage = 0;
    private int Count = 3;
    private int OriginCount = 0;
    private int MaxCount = 3;
    private int MaxOriginCount = 0;

    private bool isgoing = false;

    List<Enemy> enemylist = new List<Enemy>();

    private void Awake()
    {
        TE = this.GetComponent<TeslaEffect>();
    }

    public void InitSetUp(int _Count, TeslaTower _teslaTower,int _MaxCount)
    {
        Count = _Count;
        OriginCount = Count;
        tesla = _teslaTower;
        MaxCount = _MaxCount;
        MaxOriginCount = MaxCount;
    }

    public void SetUp(float _Damage, Transform _target)
    {
        target = _target;
        Damage = _Damage;
        isgoing = true;
        StartCoroutine(Trigger(_Damage));
    }

    /*
     * 1. ���ư� Ÿ�� �ޱ�
     * 2. ���ư���
     * 3. ���������� �������� �ְ� �ٽ� Ÿ��ã�� ����
     * 3-1 �̹� ������ ���� �ֳ� Ÿ���� ����
     * 3-2 ����Ƚ���� ���Ҵ��� Ȯ��
     * 4. ã������ 1������ �ݺ�
     * 5. ������ ���ư���
     
     */



    private void Update()
    {
        //if (isgoing)
        //{
        //    Vector3 Dir = target.position - transform.position;

        //    this.transform.position += Dir.normalized * Time.deltaTime*20;

        //    if (Vector3.Distance(target.position, this.transform.position) < 0.1f)
        //    {
        //        AtkCharactor(Damage);
        //    }
        //}
    }


    IEnumerator Trigger(float damage)
    {
        TE.SetPos(this.transform.position, target.position);
        yield return new WaitForSeconds(0.15f);
        AtkCharactor(damage);
    }
    
    public void AtkCharactor(float damage)
    {
        this.transform.position = target.transform.position;
        if (target.GetComponent<Enemy>().GetShock())
        {
            Debug.Log(target.GetComponent<Enemy>().GetShock());
            if (MaxCount > 1)
            {
                Count++;
                MaxCount--;
            }
        }

        //������ �ֱ�
        target.GetComponent<Enemy>().ElectricDamage(Damage);
        //ī���� ����, ������ ����
        Count--;
        
        Damage--;

        if(Damage < 1)
        {
            Damage = 1;
        }

        if (Range > 0.3f)
        {
            Range -= 0.1f;
        }

        Debug.Log(enemylist);
        //�����ߴ� ���� ����Ʈ ����
        enemylist.Add(target.GetComponent<Enemy>());

        //���� ��ġ�� �������� ���� �˻�
        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, Range, enemylayer);
        Debug.Log(E_collider.Length);

        Transform ShortestTarget = null;

        if (Count == 0)
        {
            ReturnBullet();
        }

        //�̹� ���� �� + ���ο� ���� �� ���� �̻��� ��
        if (E_collider.Length > 1)
        {
            float S_ShortestTarget = Mathf.Infinity;
            // �Ÿ���꿡 ����� ���� ����.

            foreach (Collider EC in E_collider)
            {
                //�˻� ����� �̹� ������ ���̶�� �ǳʶٱ�
                if(enemylist.Contains(EC.GetComponent<Enemy>()))
                {
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

            //�α��� ���� �̹� ������ ���ۿ� ���� ��� ����
            if(ShortestTarget == null)
            {
                ReturnBullet();
            }
            //����� �ִٸ� �� ����� target���� �����Լ� ����
            else
            {
                SetUp(Damage, ShortestTarget);
            }
            
            //���� �Ÿ��� ª�� ����� ���� Ÿ������ ����.
            
            //SetBulletTest(ShortestTarget, damage);
        }
        //�α��� ����� ���ٸ� ����
        else
        {
            ReturnBullet();
        }
    }

    public void ReturnBullet()
    {
        this.transform.position = tesla.GetShootPos().position;
        TE.SetisTrigger = false;
        target = null;
        Range = 2.0f;
        enemylist.Clear();
        Count = OriginCount;
        MaxCount = MaxOriginCount;
        isgoing = false;
        //tesla.ReturnBullet(this);        
    }

    public void ResetInfo()
    {

    }
    
}

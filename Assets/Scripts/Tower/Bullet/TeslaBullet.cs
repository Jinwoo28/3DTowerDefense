using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeslaBullet : MonoBehaviour
{
    [SerializeField] private LayerMask enemylayer;
    private float Range = 0.5f;

    private Transform target = null;

    private TeslaTower tesla = null;

    private float Damage = 0;
    private int Count = 3;
    private int OriginCount = 0;
    private int MaxCount = 3;
    private int MaxOriginCount = 0;

    List<Enemy> enemylist = new List<Enemy>();

    [SerializeField]
    private GameObject BoltPrefab = null;

    Queue<TeslaEffect> boltqueue = new Queue<TeslaEffect>();

    //�ʱ�ȭ
    public void InitSetUp(int _Count, TeslaTower _teslaTower,int _MaxCount)
    {
        Count = _Count;
        OriginCount = Count;
        tesla = _teslaTower;
        MaxCount = _MaxCount;
        MaxOriginCount = MaxCount;
        MakeQueue();
    }

    //���� ����Ʈ ������Ʈ ����
    private void MakeQueue()
    {
        for(int i = 0; i < MaxCount+Count; i++)
        {
            var bolt = Instantiate(BoltPrefab, this.transform).GetComponent<TeslaEffect>();
            bolt.gameObject.transform.position = tesla.GetShootPos().position;
            bolt.GetTB = this;
            bolt.gameObject.SetActive(false);
            boltqueue.Enqueue(bolt);
        }
    }

    //ť���� ������Ʈ ��������
    private TeslaEffect DequeueBolt()
    {
        var obj = boltqueue.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    //ť�� ������Ʈ ����ֱ�
    public void ReturnEffect(TeslaEffect effect)
    {
        boltqueue.Enqueue(effect);
        effect.gameObject.SetActive(false);
        effect.gameObject.transform.position = tesla.GetShootPos().position;
    }


    //������ ������ �� ó�� ���۵� �Լ�
    public void SetUp(float _Damage, Transform _target,Transform Startpos)
    {        
        //����Ʈ�� �ϳ� �����ͼ� ����
        var obj = DequeueBolt();

        obj.SetPos(Startpos, _target);

        //List�� Ȱ��ȭ ����Ʈ �־�α�
        ActiveEffect.Add(obj);

        target = _target;
        Damage = _Damage;

        StartCoroutine("Trigger");
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


    private List<TeslaEffect> ActiveEffect = new List<TeslaEffect>();

    IEnumerator Trigger()
    {

        yield return new WaitForSeconds(0.15f);
        AtkCharactor(Damage);
    }
    
    public void AtkCharactor(float damage)
    {



        if (target.GetComponent<Enemy>().GetShock())
        {
            if (MaxCount > 1)
            {
                Count++;
                MaxCount--;
            }
        }

        //������ �ֱ�
       
        //ī���� ����, ������ ����
        Count--;
        
       

        if (!target.GetComponent<Enemy>().GetWet&&Range > 0.2f)
        {
            Range -= 0.1f;
        }

        //�����ߴ� ���� ����Ʈ ����
        enemylist.Add(target.GetComponent<Enemy>());

        //���� ��ġ�� �������� ���� �˻�
        Collider[] E_collider = Physics.OverlapSphere(target.position, Range, enemylayer);


        Transform ShortestTarget = null;

        target.GetComponent<Enemy>().ElectricDamage(Damage);

        Damage--;

        if (Damage < 1)
        {
            Damage = 1;
        }

        if (Count == 0)
        {
            ReturnBullet();
            return;
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
                return;
            }
            //����� �ִٸ� �� ����� target���� �����Լ� ����
            else
            {
                SetUp(Damage, ShortestTarget,target);
            }
            
            //���� �Ÿ��� ª�� ����� ���� Ÿ������ ����.

        }
        //�α��� ����� ���ٸ� ����
        else
        {
            ReturnBullet();
            return;
        }
    }

    public void ReturnBullet()
    {
        ActiveEffect.Clear();
        this.transform.position = tesla.GetShootPos().position;
        target = null;
        Range = 2.0f;
        enemylist.Clear();
        Count = OriginCount;
        MaxCount = MaxOriginCount;
        //tesla.ReturnBullet(this);        
    }
    
}

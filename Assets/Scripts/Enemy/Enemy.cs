using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using UnityEngine.UI;

   public class  UnitState
{
    public float unitspeed = 0;
    public int unitcoin = 0;
    public float unithp = 0;
    public int unitamour = 0;
    public int avoidancerate = 0;
}



public class Enemy : MonoBehaviour
{

    [SerializeField] private int EnemyCode;

    //���� �̵� ���
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    protected UnitState unitstate = new UnitState();

    private GameObject hpbar = null;
    private GameObject damagenum = null;
    protected GameObject hpbarprefab = null;

    private Camera cam = null;
    protected enum currentstate { nomal, posion, fire, ice, }
    protected currentstate CS;

    protected enum EnemyType { creture, machine, unconfineobject }
    protected EnemyType enemytype;

    private Transform canvas = null;

    private Transform Water = null;
    protected bool underTheSea = false;
    public bool GetWet => underTheSea;
    private bool speedInit = false;

    private float unitspeed = 0;
    public float GetUnitSpeed { get => unitspeed; }
    private float Timer = 0;

    bool jump = false;

    private EnemyPooling EP = null;
    private int enemyNum = 0;
    float Hp = 0;


    private bool electricShock = false;
    public bool GetShock() => electricShock;

    public bool Fired = false;
    public bool Iced = false;
    public bool blood = false;


    public void SetPooling(EnemyPooling _pooling, int _num)
    {
        EP = _pooling;
        enemyNum = _num;
    }

    public void ResetHp()
    {
        unitstate.unithp = Hp;
    }

    private void Awake()
    {
        SetState(EnemyStateSetUp.GetData(EnemyCode));
    }

    public void SetState(EnemyState stat)
    {
        unitstate.avoidancerate = stat.avoidance;
        unitstate.unitamour = stat.Amour;
        unitstate.unitspeed = stat.Speed;
        unitstate.unitcoin = stat.coin;
        unitstate.unithp = stat.Hp;
    }

    protected virtual void Start()
    {
        Hp = unitstate.unithp;
        MultipleSpeed.speedup += SpeedUP;
        TimeScale = Time.timeScale;
    }

    private float TimeScale = 1;
    private void SpeedUP(int x)
    {
        OriginTimeScale = x;

        TimeScale = x;
    }


    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint, Transform _canvas, GameObject _hpbar, GameObject _damagenum, Transform _water)
    {
        Water = _water;
        hpbar = _hpbar;
        damagenum = _damagenum;
        Waypoint = _waypoint;
        EM = _enemymanager;
        canvas = _canvas;
    }

    public void StartMove()
    {

        unitspeed = unitstate.unitspeed;

        if (hpbarprefab == null)
        {
            hpbarprefab = Instantiate(hpbar);
            hpbarprefab.transform.SetParent(canvas);
            hpbarprefab.GetComponent<EnemyHpbar>().SetUpEnemy(this, this.transform);
            hpbar.GetComponentInChildren<RectTransform>().localScale = new Vector3(1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1);
        }

        StartCoroutine("MoveUnit");
    }

    public IEnumerator MoveUnit()
    {
        int waypointindex = 0;

        Vector3 MoveToPoint = Waypoint[waypointindex];

        Vector3 currentPos = this.transform.position;


        while (waypointindex != Waypoint.Length - 1)
        {

            if (this.transform.position.y < Water.transform.position.y)
            {
                underTheSea = true;
                speedInit = false;
            }
            else
            {
                if (!speedInit) unitspeed *= 1.15f;
                speedInit = true;
                underTheSea = false;
            }

            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                if (Vector3.Magnitude(MoveToPoint - this.transform.position) < 0.05f)
                {
                    currentPos = Waypoint[waypointindex];
                    waypointindex++;

                }

                //��������
                if (Waypoint[waypointindex].y > currentPos.y)
                {
                    if (!jump)
                    {
                        if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                        else unitspeed *= 0.8f;
                        StartCoroutine(MoveToNext(currentPos, Waypoint[waypointindex]));
                    }
                }

                //��������
                else if (Waypoint[waypointindex].y < currentPos.y)
                {

                    if (!jump)
                    {
                        if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                        else unitspeed *= 1.2f;
                        StartCoroutine(MoveToNext(currentPos, Waypoint[waypointindex]));
                    }
                }

                //����
                else
                {

                    float X = unitspeed - unitstate.unitspeed;

                    //��������
                    if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                    else
                    {

                        if (X < -0.05f) unitspeed += Time.unscaledDeltaTime * TimeScale;
                        else if (X > 0.05f) unitspeed -= Time.unscaledDeltaTime * TimeScale;
                        else unitspeed = unitstate.unitspeed;
                    }
                    //���� �������� �̵�
                    this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.unscaledDeltaTime * TimeScale);
                }

                //Debug.Log("���� ���ǵ� : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.unscaledDeltaTime * TimeScale).eulerAngles;

                //������ rotation���� Vector3���·� ������ �� ���
                this.transform.rotation = Quaternion.Euler(0, TowerDir.y, 0);

                yield return null;
            }
            else
            {

                yield break;
            }
        }
        EM.EnemyArriveDestination(this);
        var prefab = hpbarprefab;
        hpbarprefab = null;
        Destroy(prefab);
        EP.ReturnEnemy(this, enemyNum);
    }

    //������ �̵�
    private Vector3 parabola(Vector3 _start, Vector3 _end, float _height, float _power, float _time)
    {
        //y���� �Ŀ����� ���̰��� time.deltatime�� ���Ѵ�.
        //time�� 1���� ���� ���� ���, 1���� Ŭ ����(����)�̱� ������ ������ �̵��� ����
        float heightvalue = -_power * _height * _time * _time + _power * _height * _time;

        //Mathf.sin
        //���� ������ ��ǥ�� ���������� ��ǥ ������
        //x��� z���� ������ ������ ������Ʈ

        Vector3 pos = Vector3.Lerp(_start, _end, _time);

        if (heightvalue <= 0)
        {
            heightvalue = 0;
        }

        //Debug.Log(heightvalue+ "���� Parabola");

        //Debug.Log(heightvalue + pos.y + "���� Y");
        return new Vector3(pos.x, heightvalue + pos.y, pos.z);
    }



    private IEnumerator MoveToNext(Vector3 _current, Vector3 _next)
    {
        jump = true;
        Timer = 0;
        while (Vector3.Magnitude(_next - this.transform.position) > 0.05f)
        {
            Timer += Time.unscaledDeltaTime * TimeScale;

            Vector3 MovePos = parabola(_current, _next, 1.5f, 1, Timer * unitspeed);

            this.transform.position = MovePos;
            yield return null;
        }
        jump = false;
    }

    public void ElectricDamage(float _damage)
    {

        if (hpbarprefab != null)
        {
            hpbarprefab.GetComponent<EnemyHpbar>().StateChange(enemyState.Electric);
            StopCoroutine("ElectricShock");
            StartCoroutine("ElectricShock");

            float Damage = underTheSea ? _damage * 2 : _damage;

            EnemyAttacked(Damage);
            electricShock = true;
        }
    }

    IEnumerator ElectricShock()
    {
        yield return new WaitForSeconds(1.0f);
        hpbarprefab.GetComponent<EnemyHpbar>().ReturnIcon(enemyState.Electric);
        electricShock = false;
    }

    public virtual void EnemyAttacked(float _damage)
    {
        float realdamage = 0;

        realdamage = _damage - unitstate.unitamour;

        int X = Random.Range(1, 101);
        if (X < unitstate.avoidancerate)
        {
            realdamage = 0;
        }

        realDamage(realdamage);
    }

    public void realDamage(float _damage)
    {
        ShowDamage(_damage);
        if (_damage >= unitstate.unithp)
        {
            EnemyDie();
        }
        else
        {
            unitstate.unithp -= _damage;
        }
    }


    float originSpeed;


    private float OriginTimeScale = 1;

    private bool alreadyslow = false;

    public void SlowDown()
    {
        if (!alreadyslow)
        {
            alreadyslow = true;

            if (!underTheSea)
            {
                TimeScale *= 0.5f;
                Debug.Log("SLow");
            }
            else
            {
                Debug.Log("Stop");
                TimeScale = 0;
                Iced = true;
            }
        }

        if (underTheSea)
        {
            TimeScale = 0;
            Iced = true;
        }
    }

    public void returnSpeed()
    {
        ReturnTimeScale();
        Iced = false;
        alreadyslow = false;
    }

    public void ReturnTimeScale()
    {
        TimeScale = OriginTimeScale;
    }

    public void SpeedChange(float n)
    {
        TimeScale *= n; 
    }


    public IEnumerator IcedEnemy()
    {
        unitspeed = 0;

        yield return new WaitForSeconds(1.0f);
        unitspeed = originSpeed;

    }



    

    public void EnemyDie()
    {
        if(GetComponentInChildren<Bullet>() != null)
        {
            GetComponentInChildren<Bullet>().ArrowReturnBullet();
        }

        jump = false;

        Destroy(hpbarprefab);
        EM.EnemyDie(this, unitstate.unitcoin);
        EP.ReturnEnemy(this, enemyNum);
    }

    

    public void ShowDamage(float _damage)
    {
        cam = Camera.main;
    
        GameObject damagecount = Instantiate(damagenum, cam.WorldToScreenPoint(this.transform.position),
            Quaternion.identity);
        damagecount.transform.SetParent(canvas);
        damagecount.GetComponent<HpNum>().SetUp(this.transform.position.x, this.transform.position.y, this.transform.position.z, _damage);
 
    }

    virtual protected void UnitCharacteristic() { }

    virtual protected void UnitSkill() { }

    public float GetHp
    {
        get
        {
            return unitstate.unithp;
        }

        set
        {
            unitstate.unithp += value;
        }
    }

    public float SetOriginHp
    {
        set
        {
            unitstate.unithp = value;
        }
    }

}

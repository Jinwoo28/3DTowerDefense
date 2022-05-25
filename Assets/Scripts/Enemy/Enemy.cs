using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using UnityEngine.UI;

   [System.Serializable]
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
    //���� �̵� ���
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    [SerializeField] protected UnitState unitstate = null;

    private GameObject hpbar = null;
    private GameObject damagenum = null;
    private GameObject hpbarprefab = null;
    
    private Camera cam = null;
    protected enum currentstate { nomal, posion, fire, ice, }
    protected currentstate CS;

    protected enum EnemyType { creture, machine, unconfineobject }
    protected EnemyType enemytype;

    private Transform canvas = null;

    private Transform Water = null;
    private bool underTheSea = false;
    private bool speedInit = false;

    private float unitspeed = 0;
    public float GetUnitSpeed { get => unitspeed; }
    private float Timer = 0;

    bool jump = false;

    private EnemyPooling EP = null;
    private int enemyNum = 0;
    float Hp = 0;

    public void SetPooling(EnemyPooling  _pooling, int _num)
    {
        EP = _pooling;
        enemyNum = _num;
    }

    public void ResetHp()
    {
        unitstate.unithp = Hp;

    }

    protected virtual void Start()
    {
        Hp = unitstate.unithp;
        MultipleSpeed.speedup += SpeedUP;
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }



    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint,Transform _canvas,GameObject _hpbar, GameObject _damagenum,Transform _water)
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
            hpbarprefab.GetComponent<EnemyHpbar>().SetUpEnemy(this,this.transform);
            hpbar.GetComponentInChildren<RectTransform>().localScale = new Vector3(1+ 0.03f * (1+unitstate.unithp*0.1f), 1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1);
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
                if(!speedInit) unitspeed *= 1.15f;
                speedInit = true;
                underTheSea = false;
            }

            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                if (Vector3.Magnitude(MoveToPoint-this.transform.position)<0.05f)
                {
                    currentPos = Waypoint[waypointindex];
                    waypointindex++;
                    
                }

                //��������
                if (Waypoint[waypointindex].y > currentPos.y)
                {
                    if (!jump)
                    {
                        if(underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
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
                        if (X < -0.05f) unitspeed += Time.deltaTime;
                        else if (X > 0.05f) unitspeed -= Time.deltaTime;
                        else unitspeed = unitstate.unitspeed;
                    }
                    //���� �������� �̵�
                    this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.deltaTime);
                }

                //Debug.Log("���� ���ǵ� : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.deltaTime).eulerAngles;

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
        Destroy(hpbarprefab);
        EP.ReturnEnemy(this, enemyNum);
    }

    //������ �̵�
    private Vector3 parabola(Vector3 _start, Vector3 _end, float _height, float _power,float _time)
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
            Timer += Time.deltaTime;
           
            Vector3 MovePos =  parabola(_current, _next, 1.5f, 1, Timer*unitspeed);

            this.transform.position = MovePos;
            yield return null;
        }
        jump = false;
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

        ShowDamage(realdamage);
        if (realdamage >= unitstate.unithp)
        {
            EnemyDie();
        }
        else
        {
            unitstate.unithp -= realdamage;
        }
    }

    public void firedamage(float _damage)
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

    public void EnemyDie()
    {
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

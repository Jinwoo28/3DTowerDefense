using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

   
public class Enemy : MonoBehaviour
{
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    protected int UnitCoin = 10;

    [SerializeField] private float unitSpeed = 0;
    [SerializeField] private float unitHp = 0;
    [SerializeField] private float Amour = 0;

    protected enum currentstate { nomal, posion, fire, ice, }
    protected currentstate CS;

    protected enum EnemyType { creture, machine, unconfineobject }
    protected EnemyType enemytype;

    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint)
    {
        Waypoint = _waypoint;
        EM = _enemymanager;
    }

   protected void StartMove()
    {
        StartCoroutine("MoveUnit");
    }

   public IEnumerator MoveUnit()
    {
        int waypointindex = 0;

        Vector3 MoveToPoint = Waypoint[waypointindex];

        while (waypointindex != Waypoint.Length - 1)
        {

            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                if (transform.position == MoveToPoint) 
                {
                    waypointindex++;
                }

                this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitSpeed * Time.deltaTime);

                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);


                //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 270 * Time.deltaTime).eulerAngles;

                //������ rotation���� Vector3���·� ������ �� ���
                this.transform.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);
                
                yield return null;
            }
            else
            {
                yield break;
            }
        }
        EM.EnemyArriveDestination(this);
        Destroy(this.gameObject);
    }

    public void EnemyAttack(float _damage)
    {
        float realdamage = _damage - Amour;
        if (realdamage > unitHp)
        {
            EnemyDie();
        }
        else
        {
            unitHp -= realdamage;
        }
    }

    public void EnemyDie()
    {
        EM.EnemyDie(this, UnitCoin);
        Destroy(this.gameObject);
    }

    IEnumerator DotDamage(float _damage,currentstate damagetype)
    {
        int damagecount = 5;
        while (damagecount >= 0)
        {
            if (_damage < unitHp)
            {
                damagecount--;
                unitHp -= _damage;
                CS = damagetype;
            }
            else
            {
                EnemyDie();
            }
            yield return new WaitForSeconds(0.5f);
        }
        CS = currentstate.nomal;
    }

    virtual protected void UnitCharacteristic() { }

    virtual protected void UnitSkill() { }



}

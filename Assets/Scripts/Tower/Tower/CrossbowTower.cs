using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTower : AtkTower
{
    private bulletpolling op = null;
    protected override void Start()
    {
        op = this.GetComponent<bulletpolling>();
        base.Start();
    }


    protected override void Attack() 
    {

            var obj = op.GetObject(shootPos.position);
            obj.SetUp(FinalTarget, towerinfo.towerdamage, op, 20f);

            obj.SetParent = shootPos;
            obj.MortarSetDestination(FinalTarget.position, shootPos.transform.position);
            obj.SetArrowDir(shootPos.forward);
            obj.ResetBullet();
            
        
            AS.Play();
    }

    IEnumerator Level3Attack()
    {
        for(int i = 0; i < 2; i++)
        {
            Vector3 shootpos = shootPos.position;
            shootpos.y = shootPos.position.y + i * -0.4f;

            Debug.Log(shootpos);
            var obj = op.GetObject(shootpos);
            obj.ResetBullet();
            obj.SetUp(FinalTarget, towerinfo.towerdamage, op, 10f-i*2);
            //obj.MortarSetDestination(FinalTarget.position + new Vector3(0, FinalTarget.transform.localPosition.y / 2, 0), shootPos.transform.position);
            obj.SetArrowDir(shootPos.forward);
            obj.SetParent = this.gameObject.transform;
            yield return new WaitForSeconds(0.5f);
        }
    }

}

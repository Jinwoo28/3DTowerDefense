using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : SkillParent
{
    List<Enemy> enemylist = new List<Enemy>();
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 180*Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy_Creture>() != null)
            {
            enemylist.Add(other.GetComponent<Enemy>());
                other.GetComponent<Enemy_Creture>().FireAttacked(SkillSettings.ActiveSkillSearch("freezing").Value, 5);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().SlowDown();
            }
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < enemylist.Count; i++)
        {
            if (enemylist[i].isActiveAndEnabled)
            {
                enemylist[i].returnSpeed();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy>() != null)
            {
            enemylist.Remove(other.GetComponent<Enemy>());
                other.GetComponent<Enemy>().returnSpeed();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSettings : MonoBehaviour
{
    public enum skillType
    {
        Active,
        Passive
    }

    //��ų Ÿ��
    public skillType skilltype;
    

    //��ų �̸�
    public string skillName;

    public int skillprice;

    //��ų�� ���� ����
    public bool skillUnLock;

    public int skillcooltime;
    public int cooltimeDecrease;

    public int damage;
    public int damageIncrease;

    public int skillLevel;
    public int MaxSkillLevel;

    public int upgradeprice;
    public int OriginUpgradePrice;

    public void SkillLevelUp()
    {
        skillLevel++;
        upgradeprice = skillLevel * OriginUpgradePrice;
        skillcooltime -= cooltimeDecrease;
        damage += damageIncrease;
    }

}

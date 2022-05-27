using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class skill
{
    public GameObject buyButton;
    public GameObject upgradeButton;
}

public class ShowSkillinfo : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI coin = null;
    [SerializeField] private GameObject[] SkillInfo = null;
    [SerializeField] private skill[] skill = null;

    [SerializeField] private TextMeshProUGUI[] showstate = null;

    [SerializeField] private UserInformation userinfo = null;

    [SerializeField] private GameObject pricePanel = null;
    [SerializeField] private TextMeshProUGUI price = null;


    private int ShowNum = 0;
    private int PriceValue = 0;

    private void Update()
    {
        coin.text = "���� : " + UserInformation.userDataStatic.userCoin;

 
    }


    private void ActiveSkillState(int num)
    {
        if (UserInformation.userDataStatic.skillSet[num].GetType == global::SkillInfo.skillType.Active)
        {
            if (!UserInformation.userDataStatic.skillSet[num].SetUnLock)
            {
                showstate[num].text = $"���� ���� : ���Ծȵ�\n" +
                        $"���� ��Ÿ�� : --��\n" +
                        $"[���� ���� ��Ÿ��] : --��\n\n" +
                        $"���� ������ : --��\n" +
                        $"[���� ���� ������] : --";
            }
            else
            {
                showstate[num].text = /*$"���� ���� : {UserInformation.userDataStatic.skillSet[num].skillLevel}\n" +*/
                        $"���� ��Ÿ�� : {UserInformation.userDataStatic.skillSet[num].GetCoolTime}��\n" +
                        $"[���� ���� ��Ÿ��] : {UserInformation.userDataStatic.skillSet[num].GetCoolTime - UserInformation.userDataStatic.skillSet[num].GetCoolTime}��\n\n" +
                        $"���� ������ : {UserInformation.userDataStatic.skillSet[num].GetDamage}\n" +
                        $"[���� ���� ������] : {UserInformation.userDataStatic.skillSet[num].GetDamage + UserInformation.userDataStatic.skillSet[num].GetDamage}";
            }
        }

        else if(UserInformation.userDataStatic.skillSet[num].GetType == global::SkillInfo.skillType.Passive)
        {
            if (!UserInformation.userDataStatic.skillSet[num].SetUnLock)
            {
                showstate[num].text = $"���� ���� : ���Ծȵ�\n" +
                        $"���� �߰� �ۼ�Ʈ : --%\n" +
                        $"[���� ���� �ۼ�Ʈ] : --%\n\n";
            }
            else
            {
                showstate[num].text = /*$"���� ���� : {UserInformation.userDataStatic.skillSet[num].skillLevel}\n" +*/
                        $"���� �߰� �ۼ�Ʈ : {UserInformation.userDataStatic.skillSet[num].GetDamage}%\n" +
                        $"[���� ���� ������] : {UserInformation.userDataStatic.skillSet[num].GetDamage + UserInformation.userDataStatic.skillSet[num].GetDamage}%\n\n";
            }
        }

    }


    public void ShowSkillInfo(int num)
    {
        ShowNum = num;
        for (int i = 0; i < SkillInfo.Length; i++)
        {
            SkillInfo[i].SetActive(false);
        }

        SkillInfo[num].SetActive(true);

        ActiveSkillState(num);

        if (UserInformation.userDataStatic.skillSet[num].SetUnLock)
        {
            skill[num].buyButton.SetActive(false);
            if (UserInformation.userDataStatic.skillSet[num].GetLevel < 5)
            {
                skill[num].upgradeButton.SetActive(true);
            }
            else
            {
                skill[num].upgradeButton.SetActive(false);
                OffShowPrice();
            }
            PriceValue = UserInformation.userDataStatic.skillSet[num].GetPrice;
            Debug.Log(PriceValue);
        }
        else
        {
            
            skill[num].buyButton.SetActive(true);
            skill[num].upgradeButton.SetActive(false);
            PriceValue = UserInformation.userDataStatic.skillSet[num].GetPrice;
        }
    }

    public void UnLockSkill(string _skillname)
    {
        UserInformation.userDataStatic.UnLockSkill(_skillname);
        ShowSkillInfo(ShowNum);
    }

    public void LevelUpSkill(string _skillname)
    {
        UserInformation.userDataStatic.SkillLevelUp(_skillname);
        OffShowPrice();
        ShowSkillInfo(ShowNum);
        OnShowPrice();
    }


    public void OnShowPrice()
    {
        pricePanel.SetActive(true);
        price.text = PriceValue.ToString() + "��";
    }

    public void OffShowPrice()
    {
        pricePanel.SetActive(false);
    }







}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;


[System.Serializable]
public class UserData
{
    public int userCoin = 0;

    public List<SkillInfo> skillSet;

    public List<SkillSet> PassiveSkill;

    public List<SkillSet> ActiveSkill;

    public void UnLockSkill(string skillname)
    {
        foreach (var skill in skillSet)
        {
            if (skill.GetName == skillname)
            {
                if (skill.SetUnLock == false)
                {
                    if (skill.GetPrice <= userCoin)
                    {
                        skill.SetUnLock = true;
                        userCoin -= skill.GetPrice;
                    }
                }
            }
        }
    }

    public void SkillLevelUp(string skillname)
    {
        foreach (var skill in skillSet)
        {
            if (skill.GetName == skillname)
            {
                if (skill.SetUnLock == true)
                {
                    if (skill.GetPrice <= userCoin)
                    {
                        userCoin -= skill.GetPrice;

                        skill.SkillLevelUp();

                    }
                }
            }
        }
    }
}

[System.Serializable]
public class SkillSet
{
    public string BundleName;
    public List<SkillInfo> skillInfoList;
}

//Dictionary�� �ø��������ȭ ��ų �� ���� ������ ���� class�� ���� list�� ��� ����
[System.Serializable]
public class SkillInfo
{
    public enum skillType
    {
        Active,
        Passive
    }

    //��ų Ÿ��
    [SerializeField]
    private skillType skilltype;

    public skillType GetType => skilltype;

    //��ų �̸�
    [SerializeField]
    private string skillName;

    public string GetName => skillName;

    [SerializeField]
    private int skillprice;
    public int GetPrice => skillprice;

    //��ų�� ���� ����
    [SerializeField]
    private bool skillUnLock;

    public bool SetUnLock { get => skillUnLock; set => skillUnLock = value; }

    [SerializeField]
    private string preskill;

    [SerializeField]
    private int skillcooltime;
    public int GetCoolTime => skillcooltime;

    [SerializeField]
    private int cooltimeDecrease;

    [SerializeField]
    private int damage;
    public int GetDamage => damage;

    [SerializeField]
    private int damageIncrease;

    [SerializeField]
    private int skillLevel;
    public int GetLevel => skillLevel;

    [SerializeField]
    private int MaxSkillLevel;
    public int GetMaxLevel => MaxSkillLevel;

    [SerializeField]
    private int upgradeprice;
    [SerializeField]
    private int OriginUpgradePrice;

    public void SkillLevelUp()
    {
        skillLevel++;
        upgradeprice = skillLevel * OriginUpgradePrice;
        skillcooltime -= cooltimeDecrease;
        damage += damageIncrease;
    }
}

public class UserInformation : MonoBehaviour
{

    //inspectorâ���� ���� ������ �� �ֵ��� ���� ����
    public UserData userData;



    //userData�� �������� ����ϱ� ���� static���� ����� ����, �ҷ����� �� �� userData�� �ְ� ����
    //������ �����͸� ������ static����
    public static UserData userDataStatic = new UserData();

    private static bool SetData = false;

    private SkillSettings skill = null;

    private void Start()
    {
        skill = this.GetComponent<SkillSettings>();

        if (!SetData)
        {
            LoadUserInfo();
            SetData = true;
            userDataStatic = userData;
          //Debug.Log(userDataStatic.skillSet[0].skillUnLock);
        }

        userData.userCoin = userDataStatic.userCoin;

    }

    private void OnDestroy()
    {
        userData.skillSet = userDataStatic.skillSet;
        SaveUserInfo();
    }

    //�����ڸ� private���� ���� ���ο� ��ü ��������
    private UserInformation() { }


    public void UnlockSkill(string _skillname)
    {
        userData.UnLockSkill(_skillname);
        
    }

    public void SwitchInfoForward()
    {
        userData = userDataStatic;
    }

    public void SwitchInfoReverse()
    {
        userDataStatic = userData;
    }
    


    //���� ������ ����
    public void SaveUserInfo()
    {
        //json�� ���ӵ� string, byte�� ����
        //userdata�� ���̽����� �����ϱ� ���� string���� ��ȯ
        string jdata = JsonConvert.SerializeObject(userData,Formatting.Indented);

        ////��ȣȭ �ϱ� ���� byte���·� ���� ��ȯ
        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);

        ////��ȯ�� byte�� �ٽ� string���·� ��ȯ
        //string format = System.Convert.ToBase64String(bytes);

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.txt");

        //�׷��� ������ string������ ����
        File.WriteAllText(path, jdata);

        //File.WriteAllText(Application.streamingAssetsPath + "/Jinwoo", jdata);


        Debug.Log("����");
    }


    //���� ������ �ҷ�����
    public void LoadUserInfo()
    {

        string path = Path.Combine(Application.streamingAssetsPath, "PlayerData.txt");
        string jdata = File.ReadAllText(path);

    //byte[] bytes = System.Convert.FromBase64String(jdata);
    //string reformat = System.Text.Encoding.UTF8.GetString(bytes);

//    https://forum.unity.com/threads/json-file-not-found-after-building.959523/

        userData = JsonConvert.DeserializeObject<UserData>(jdata);
        Debug.Log("�ҷ�����");
    }

 


}

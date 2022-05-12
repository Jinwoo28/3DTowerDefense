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

    public List<SkillSet> skillSet;

    public void UnLockSkill(string skillname)
    {
        foreach (var skill in skillSet)
        {
            if (skill.skillName == skillname)
            {
                if (skill.skillUnLock == false)
                {
                    if (skill.skillprice <= userCoin)
                    {
                        skill.skillUnLock = true;
                        userCoin -= skill.skillprice;
                    }
                }
            }
        }
    }

    public void SkillLevelUp(string skillname)
    {
        foreach (var skill in skillSet)
        {
            if (skill.skillName == skillname)
            {
                if (skill.skillUnLock == true)
                {
                    if (skill.upgradeprice <= userCoin)
                    {
                        userCoin -= skill.upgradeprice;

                        skill.SkillLevelUp();

                    }
                }
            }
        }
    }


}




//Dictionary�� �ø��������ȭ ��ų �� ���� ������ ���� class�� ���� list�� ��� ����
[System.Serializable]
public class SkillSet
{
    public enum skillType
    {
        Active,
        Passive
    }

    public skillType skilltype;

    public string skillName;

    public int skillprice;
    
    public bool skillUnLock;
    
    public int skillcooltime;
    public int cooltimeDecrease;

    public int damage;
    public int damageIncrease;

    public int skillLevel;
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

public class UserInformation : MonoBehaviour
{

    //inspectorâ���� ���� ������ �� �ֵ��� ���� ����
    public UserData userData;



    //userData�� �������� ����ϱ� ���� static���� ����� ����, �ҷ����� �� �� userData�� �ְ� ����
    //������ �����͸� ������ static����
    public static UserData userDataStatic = new UserData();

    private static bool SetData = false;

    private void Start()
    {


        if (!SetData)
        {
            LoadUserInfo();
            SetData = true;
            userDataStatic = userData;

          //  Debug.Log(userDataStatic.skillSet[0].skillUnLock);
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

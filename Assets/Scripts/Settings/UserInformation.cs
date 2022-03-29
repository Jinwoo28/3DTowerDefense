using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class UserData
{
    public int userCoin = 0;
    public bool[] stageClear;

    public List<SkillSet> skillSet;
}


//Dictionary�� �ø��������ȭ ��ų �� ���� ������ ���� class�� ���� list�� ��� ����
[System.Serializable]
public class SkillSet
{
    public string skillName;
    public bool skellUnLock;
    public int skillupgrade;
}

public class UserInformation : MonoBehaviour
{
    public UserData userData = new UserData();
    private Dictionary<string, bool> skillset;


    private void Start()
    {

    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            GetCoin(10);
        }
    }


    void StageClear(int stagenum)
    {
        userData.stageClear[stagenum] = true;
    }

    void GetCoin(int num)
    {
        userData.userCoin += num;
        Debug.Log(userData.userCoin);
    }

    public void UnLockSkill(string skillname)
    {
        foreach(var skill in userData.skillSet)
        {
            if (skill.skillName == skillname)
            {
                skill.skellUnLock = true;
            }
        }
    }




    public void SaveUserInfo()
    {
        string jdata = JsonConvert.SerializeObject(userData);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        string format = System.Convert.ToBase64String(bytes);

        File.WriteAllText(Application.dataPath + "/Jinwoo.json", format);

        Debug.Log("����");


    }

    public void LoadUserInfo()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/Jinwoo.json");

        byte[] bytes = System.Convert.FromBase64String(jdata);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes);

        userData = JsonConvert.DeserializeObject<UserData>(reformat);
        Debug.Log("�ҷ�����");

    }

}

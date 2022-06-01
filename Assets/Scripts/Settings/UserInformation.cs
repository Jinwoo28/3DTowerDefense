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

    public List<PassiveSkillSet> PassiveSkill;
    
    public List<ActiveSkillSet> ActiveSkill;
    
}

public class UserInformation : MonoBehaviour
{
    //inspectorâ���� ���� ������ �� �ֵ��� ���� ����
    public UserData userData;

    //userData�� �������� ����ϱ� ���� static���� ����� ����, �ҷ����� �� �� userData�� �ְ� ����
    //������ �����͸� ������ static����
    //public static UserData userDataStatic = new UserData();

    public static int getMoney = 0;


    private static bool SetData = false;

    private SkillSettings skill = null;

    private void Start()
    {
        skill = this.GetComponent<SkillSettings>();

       // PSkillSetUp();

        if (!SetData)
        {
            LoadUserInfo();
            SetData = true;
            skill.SkillSetUp(userData.PassiveSkill, userData.ActiveSkill);
     //       userDataStatic = userData;
        }

        userData.userCoin += getMoney;
        getMoney = 0;



       // userDataStatic.PassiveSkill.Add(P1skillBundle);
     //   userData.userCoin = userDataStatic.userCoin;
    }

    public void SavePSkill(List<PassiveSkillSet> passiveSkillSets)
    {
        userData.PassiveSkill = passiveSkillSets;
    }

    public void SaveASkill(List<ActiveSkillSet> activeSkillSets)
    {
        userData.ActiveSkill = activeSkillSets;
    }

    private void OnDestroy()
    {
     //   userData = userDataStatic;
        SaveUserInfo();
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

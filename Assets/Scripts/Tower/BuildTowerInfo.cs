using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildTowerInfo : MonoBehaviour
{
    [SerializeField] private GameObject buildtowerInfo = null;
    [SerializeField] private TextMeshProUGUI infoText = null;
    
    public void ShowBuildTower_Gatling()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"�����" +
            "==========\n" +
            $"����Ÿ���� ������� �������� ������ ���� �ӵ��� �����Ѵ�.";
    }

    public void ShowBuildTower_Mortar()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"�ڰ���" +
            "==========\n" +
            $"���������� ���ư��� ��ź�� �߻�.\n"+
            "���� ���� �������� ������ ������ �������� �ش�.";
    }

    public void ShowBuildTower_CrossBow()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"����" +
            "==========\n" +
            $"���� Ÿ�� ȭ���� �߻�.\n" +
            $"�ؼ��� ���ݷ°� �ؼ��� ���ݼӵ��� ����.";
    }

    public void ShowBuildTower_Tesla()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"������" +
            "==========\n" +
            $"���� Ÿ���� ������� ���� ���� �����ϸ�\n" +
            $"���� ���� ������ �Ѵ�.";
    }

    public void ShowBuildTower_Laser()
    {
        buildtowerInfo.SetActive(true);
        infoText.text = $"������" +
            "==========\n" +
             $"���� Ÿ���� ������� ���� ���� �����ϸ�\n" +
            $"���� ���� ������ �Ѵ�.";
    }

    public void ShowBuildTowerOff()
    {
        buildtowerInfo.SetActive(false);
    }
}

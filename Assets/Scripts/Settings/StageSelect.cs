using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetStageStar
{
    public int stageNum;
    public GameObject[] Star;
}

public class StageSelect : MonoBehaviour
{
    [SerializeField] private SetStageStar[] setStageStar = null;

    private void Start()
    {
        SetResolution();
        ShowStar();
    }

    private void ShowStar()
    {
        int index = 0;
        foreach(var star in UserInformation.userDataStatic.stageClear)
        {
            if (star.Star1)
            {
                setStageStar[index].Star[0].SetActive(true);
            }
            if (star.Star2)
            {
                setStageStar[index].Star[1].SetActive(true);
            }
            if (star.Star3)
            {
                setStageStar[index].Star[2].SetActive(true);
            }
            index++;
        }
    }

    public void SelectStage(string _stagename)
    {
        LoadSceneControler.LoadScene(_stagename);
    }

    public void SetResolution()
    {
        int Index = PlayerPrefs.GetInt("ResolutionIndex");
        int full = PlayerPrefs.GetInt("isFullScreen");

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(ResolutionSetting.staticResolutions[Index].width, ResolutionSetting.staticResolutions[Index].height, PlayerPrefs.GetInt("isFullScreen")==1?true:false);

        //����� �ػ󵵿� ������ �ػ󵵰� ���� ���� ��� ī�޶��� viewport Rect�� �����Ͽ� �ϱ׷��� ���� ���
        if ((float)ResolutionSetting.staticResolutions[Index].width / ResolutionSetting.staticResolutions[Index].height < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)ResolutionSetting.staticResolutions[Index].width / ResolutionSetting.staticResolutions[Index].height)
                / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight)
                / (float)ResolutionSetting.staticResolutions[Index].width / ResolutionSetting.staticResolutions[Index].height; // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

  
}



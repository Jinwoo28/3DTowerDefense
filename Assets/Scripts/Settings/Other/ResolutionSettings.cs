using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSettings : MonoBehaviour
{
    private void Start()
    {
        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        //Screen.SetResolution(ResolutionSetting.staticResolutions[ResolutionIndex].width, ResolutionSetting.staticResolutions[ResolutionIndex].height, IsFullscreen());

        //Debug.Log("�ػ� ����");

        ////����� �ػ󵵿� ������ �ػ󵵰� ���� ���� ��� ī�޶��� viewport Rect�� �����Ͽ� �ϱ׷��� ���� ���
        //if ((float)ResolutionSetting.staticResolutions[ResolutionIndex].width / ResolutionSetting.staticResolutions[ResolutionIndex].height < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        //{
        //    float newWidth = ((float)ResolutionSetting.staticResolutions[ResolutionIndex].width / ResolutionSetting.staticResolutions[ResolutionIndex].height)
        //        / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
        //    Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        //}
        //else // ������ �ػ� �� �� ū ���
        //{
        //    float newHeight = ((float)deviceWidth / deviceHeight)
        //        / (float)ResolutionSetting.staticResolutions[ResolutionIndex].width / ResolutionSetting.staticResolutions[ResolutionIndex].height; // ���ο� ����
        //    Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        //}
    }
    public int ResolutionIndex
    {
        get => PlayerPrefs.GetInt("ResolutionIndex");
    }

    //��üȭ��, â��� ����
    public bool IsFullscreen()
    {
        if (PlayerPrefs.GetInt("isFullScreen") == 1)
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}

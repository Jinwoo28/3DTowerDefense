using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSetting : MonoBehaviour
{

    //�ػ󵵸� �� dropdown
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    //��üȭ�� âȭ�� ����
    [SerializeField] private TMP_Dropdown fullscreenDropdown = null;

    //������ �ִ� �ػ� �迭
    private List<Resolution> resolutions = new List<Resolution>();

    private int resolutionNum = 0;
    private int fullscreenNum = 0;

    FullScreenMode screenMode;

    private void Start()
    {
        ResolutionInit();
    }
    private void ResolutionInit()
    {
        resolutions.AddRange(Screen.resolutions);
        resolutions.Reverse();


        resolutionDropdown.options.Clear();

        fullscreenDropdown.options.Clear();

        
        TMP_Dropdown.OptionData fulloption = new TMP_Dropdown.OptionData();
        fulloption.text = "Full Screen";

        TMP_Dropdown.OptionData windowoption = new TMP_Dropdown.OptionData();
        windowoption.text = "Window Screen";

        fullscreenDropdown.options.Add(fulloption);
        fullscreenDropdown.options.Add(windowoption);

        int optionNum = 0;

        foreach(Resolution res in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = res.width + "X" + res.height + "Y" + res.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if(res.width == Screen.width && res.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
                optionNum++;
            }


        }

        resolutionDropdown.RefreshShownValue();
        
    }

    //�ػ� �ٲٴ� ��ӹڽ�
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    //â���� ��üȭ�� ��ӹڽ�
    public void DropboxOptionFullscreen(int x)
    {
        //��üȭ��
        if (x == 0) screenMode = FullScreenMode.FullScreenWindow;
        //âȭ��
        else if (x == 1) screenMode = FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}

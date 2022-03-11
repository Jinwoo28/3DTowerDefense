using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private SceneControl sceneManager = null;
    [SerializeField] private GameObject settingPanal = null;
    private MultipleSpeed multipleSpeed = null;

    private bool settingpanelcheck = false;

    private void Start()
    {
        settingPanal.SetActive(false);
        multipleSpeed = this.GetComponent<MultipleSpeed>();
    }

    private void Update()
    {




        if (settingpanelcheck)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SettingPanalOff();
            }
        }

    }
    public void SettingPanalOn()
    {
        settingpanelcheck = true;
        settingPanal.SetActive(true);
        multipleSpeed.OnClickStart(0);
    }

    public void SettingPanalOff()
    {
        settingpanelcheck = false;
        settingPanal.SetActive(false);
        multipleSpeed.OnClickStart(1);
    }



}

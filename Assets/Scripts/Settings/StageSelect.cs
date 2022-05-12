using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StageSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelChoice = null;

    [SerializeField] private TextMeshProUGUI easyMoney = null;
    [SerializeField] private TextMeshProUGUI namalMoney = null;
    [SerializeField] private TextMeshProUGUI hardMoney = null;

    private string stageShape = null;

    public void StageShape(string _shape)
    {
        stageShape = _shape;
    }

    public void SelectStage(int level)
    {
        GameManager.SetStageShape = stageShape;
        GameManager.SetGameLevel = level;
        LoadSceneControler.LoadScene("Stage2");
    }

    public void OnLevelChoice()
    {
        easyMoney.text = "���� : 100��";
        namalMoney.text = "���� : 175��";
        hardMoney.text = "���� : 400��";

        levelChoice.SetActive(true);
    }

    public void OffLevelChoice()
    {
        levelChoice.SetActive(false);
        stageShape = null;
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowData : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI datatext;

    [SerializeField] private GameObject EnemyPanel;
    [SerializeField] private Image EnemyPanelUi;
    [SerializeField] private GameObject TowerPanel;
    [SerializeField] private Image TowerPanelUi;

    private void Start()
    {
        ShowEnemyPanel();
    }

    public void ShowEnemyData(int enemycode) 
    {
        EnemyState stat = EnemyStateSetUp.GetData(enemycode);
        string type = stat.enemytype == 0 ? "����" : "����";
        datatext.text = $"�̸� : {stat.Name}\n ���� : ���� \n ü�� : {stat.Hp}\t���ǵ� : {stat.Speed}\t�Ƹ� : {stat.Amour}\n" +
            $"���� : ${stat.coin}\tȸ���� : {stat.avoidance}%\n����Ÿ�� : {type}\n����Ư¡ : 3�ʸ��� {stat.feature}�� ü���� ȸ��";
    }

    public void ShowTowerData(int towercode)
    {
        TowerData towerdata = TowerDataSetUp.GetData(towercode);
        string AtkType;
        if(towerdata.CanAtk == 1)
        {
            AtkType = "����";
        }
        else if(towerdata.CanAtk == 2)
        {
            AtkType = "����";
        }
        else
        {
            AtkType = "����, ����";
        }

        datatext.text = $"�̸� : {towerdata.Name}\t ���ݴ�� : {AtkType}\n" +
            $"���� : {towerdata.TowerStep}\t���� : {towerdata.TowerPrice}\t������ : {towerdata.Damage}\n" +
            $"���ݼӵ� : {towerdata.Delay}\t���ݹ��� : {towerdata.Range}\tũ��Ƽ�� : {towerdata.Critical * 100}%\n" +
            $"Ÿ�� Ư¡ : {towerdata.DetailInformation}\n\n" +
            $"���׷��̵� ��ġ\n" +
            $"���ݷ� +{towerdata.UpgradeAtk}\tũ��Ƽ�� +{towerdata.UpgradeCri*100}%\t���׷��̵� ���� : {towerdata.UpgradePrice}\n";
    }

    public void ShowEnemyPanel()
    {
        datatext.text = "����� �����ϼ���.";
        EnemyPanel.SetActive(true);
        TowerPanel.SetActive(false);
        EnemyPanelUi.color = Color.white;
        TowerPanelUi.color = Color.gray;
    }
    public void ShowTowerPanel()
    {
        datatext.text = "����� �����ϼ���.";
        EnemyPanel.SetActive(false);
        TowerPanel.SetActive(true);
        EnemyPanelUi.color = Color.gray;
        TowerPanelUi.color = Color.white;
    }

}

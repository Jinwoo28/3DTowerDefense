using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI informationText;
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
        EnemyDataFrame stat = EnemyDataSetUp.GetData(enemycode);
        string type = stat.enemytype == 0 ? "����" : "����";
        informationText.text = $"�̸� : {stat.name}\n ���� : ���� \n ü�� : {stat.hp}\t���ǵ� : {stat.speed}\t�Ƹ� : {stat.amour}\n" +
            $"���� : ${stat.coin}\tȸ���� : {stat.avoidance}%\n����Ÿ�� : {type}\n����Ư¡ : 3�ʸ��� {stat.feature}�� ü���� ȸ��";
    }

    public void ShowTowerData(int towercode)
    {
        TowerDataFrame towerdata = TowerDataSetUp.GetData(towercode);
        string AtkType;
        if(towerdata.atkType == 1)
        {
            AtkType = "����";
        }
        else if(towerdata.atkType == 2)
        {
            AtkType = "����";
        }
        else
        {
            AtkType = "����, ����";
        }

        informationText.text = $"�̸� : {towerdata.name}\t ���ݴ�� : {AtkType}\n" +
            $"���� : {towerdata.towerStep}\t���� : {towerdata.towerPrice}\t������ : {towerdata.damage}\n" +
            $"���ݼӵ� : {towerdata.delay}\t���ݹ��� : {towerdata.range}\tũ��Ƽ�� : {towerdata.critical * 100}%\n" +
            $"Ÿ�� Ư¡ : {towerdata.detailInformation}\n\n" +
            $"���׷��̵� ��ġ\n" +
            $"���ݷ� +{towerdata.upgradAtk}\tũ��Ƽ�� +{towerdata.upgradCri*100}%\t���׷��̵� ���� : {towerdata.upgradPrice}\n";
    }

    public void ShowEnemyPanel()
    {
        informationText.text = "����� �����ϼ���.";
        EnemyPanel.SetActive(true);
        TowerPanel.SetActive(false);
        EnemyPanelUi.color = Color.white;
        TowerPanelUi.color = Color.gray;
    }
    public void ShowTowerPanel()
    {
        informationText.text = "����� �����ϼ���.";
        EnemyPanel.SetActive(false);
        TowerPanel.SetActive(true);
        EnemyPanelUi.color = Color.gray;
        TowerPanelUi.color = Color.white;
    }

}

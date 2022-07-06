using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NumChange
{
    public string StringReturnNum(float num)
    {
        if((num*10)%10 == 0)
        {
            return num.ToString();
        }
        else if((num * 100) % 10 == 0)
        {
            return num.ToString("F1");
        }
        else
        {
            return num.ToString("F2");
        }
    }

    public string HunReturnNum(float num)
    {
        

        float hunNum = num * 100;
        return hunNum.ToString("F1");
        //if (hunNum % 1 == 0)    //10~1�ۼ�Ʈ
        //{
        //    return 1111.ToString("F0");
        //}
        //else if((hunNum*10)%1==0)   //0.1�ۼ�Ʈ
        //{
        //    return 22222.ToString("F1");
        //}
        //else //�Ҽ�������
        //{
        //    return 00000.ToString();
        //}

        //if (hunNum % 10 == 0)   //����
        //{
        //    return hunNum.ToString("F0");
        //}
        //else if ((num * 10) % 10 == 0)//�Ҽ��� �� �ڸ�
        //{
        //    return hunNum.ToString("F1");
        //}
        //else    //�Ҽ��� 2�ڸ�
        //{
        //    return hunNum.ToString("F2");
        //}
    }
}

[System.Serializable]
public class towerTextinfo
{
    public string towername;
    public int towercode;
}

public class ShowTowerInfo : MonoBehaviour
{
    private Tower tower;

    [SerializeField] private GameObject towerinfopanel = null;

    private DetectObject detectob = null;

    [SerializeField] private Button upgradebutton;

    [SerializeField] private Image towerimage = null;
    [SerializeField] private TextMeshProUGUI towername = null;
    [SerializeField] private TextMeshProUGUI towerlevel = null;
    [SerializeField] private TextMeshProUGUI atkdamage = null;
    [SerializeField] private TextMeshProUGUI atkcritical = null;
    [SerializeField] private TextMeshProUGUI atkspeed = null;
    [SerializeField] private TextMeshProUGUI upgradeprice = null;

    [SerializeField] private towerTextinfo[] textinfo;



    //���� ���� ǥ�ÿ� ����� Sprite�̹���
    [SerializeField] private GameObject rangePrefab = null;

    [SerializeField] private EnemyManager enemymanager = null;

    private GameObject RangeParent = null;

    private int towerupgradeprice = 0;

    private GameObject[] rangesprite = null;

    private Transform towertransform = null;

    [SerializeField] private GameObject SellPanel = null;
    [SerializeField] private TextMeshProUGUI SellPrice = null;

    private bool ShowUpgradeInfo = false;

    NumChange NC = new NumChange();
    public void ShowUPInfo()
    {
        ShowUpgradeInfo = !ShowUpgradeInfo;
    }

    public Transform GetTowerTransform
    {
        set
        {
            towertransform = value;
        }
    }

    private void Start()
    {
        RangeParent = new GameObject("Range");
        detectob = this.GetComponent<DetectObject>();
        rangesprite = new GameObject[72];
        towerinfopanel.SetActive(false);
        for (int i = 0; i < 72; i++)
        {
            rangesprite[i] = Instantiate(rangePrefab, RangeParent.transform);
            rangesprite[i].SetActive(false);
        }

        //for(int i = 0; i< textinfo.Length; i++)
        //{
        //    textinfo[i].showinfo.text = textinfo[i].towername + "\n" + TowerDataSetUp.GetData(textinfo[i].towercode).TowerPrice * SkillSettings.PassiveValue("SetTowerDown");

        //}

    }

    private bool CanUpgrade = true;

    private void Update()
    {
        
        ClickTower();

        if (tower != null)
        {
            towername.text = $"{tower.Getname}";
            atkspeed.text =  "���� "+tower.GetStep.ToString();
            towerimage.sprite = Resources.Load<Sprite>("Image/Tower/" + (TowerDataSetUp.GetData(tower.GetTowerCode).Name)+TowerDataSetUp.GetData(tower.GetTowerCode).TowerStep);


            if (!ShowUpgradeInfo)
            {
                towerlevel.text = "���׷��̵� : " + tower.GetTowerLevel.ToString();
                towerlevel.color = Color.black;
                atkdamage.text = "������ : " + NC.StringReturnNum(tower.GetDamage);
                atkdamage.color = Color.black;
                atkcritical.fontSize = 60.5f;
                atkcritical.text = "ũ��Ƽ�� : " + NC.HunReturnNum(tower.GetCritical) + "%";
                atkcritical.color = Color.black;
            }
            else
            {
                if (upgradebutton.interactable)
                {
                    towerlevel.text = "���׷��̵� : " + tower.GetTowerLevel.ToString() + " -> " + (tower.GetTowerLevel + 1).ToString();
                    towerlevel.color = Color.red;
                    atkdamage.text = "������ : " + NC.StringReturnNum(tower.GetDamage) + " -> " + NC.StringReturnNum(tower.GetDamage + tower.GetTowerUPDamage);
                    atkdamage.color = Color.red;
                    atkcritical.fontSize = 50;
                    atkcritical.text = "ũ��Ƽ�� : " + NC.HunReturnNum(tower.GetCritical) + "%" + " -> " + NC.HunReturnNum(tower.GetCritical + tower.GetTowerUpCri) + "%";
                    atkcritical.color = Color.red;
                }
                else
                {
                    towerlevel.text = "���׷��̵� : " + tower.GetTowerLevel.ToString();
                    towerlevel.color = Color.black;
                    atkdamage.text = "������ : " + NC.StringReturnNum(tower.GetDamage);
                    atkdamage.color = Color.black;
                    atkcritical.fontSize = 60.5f;
                    atkcritical.text = "ũ��Ƽ�� : " + NC.HunReturnNum(tower.GetCritical) + "%";
                    atkcritical.color = Color.black;
                }
            }


            switch (tower.GetStep)
            {
                case 1:
                    if (tower.GetTowerLevel >= 3)
                    {
                        upgradebutton.interactable = false;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "���׷��̵� �Ұ�";
                    }
                    else
                    {
                        upgradebutton.interactable = true;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "���׷��̵� : " + tower.Gettowerupgradeprice.ToString();

                    }
                    break;

                case 2:
                    if (tower.GetTowerLevel >= 5)
                    {
                        upgradebutton.interactable = false;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "���׷��̵� �Ұ�";
                    }
                    else
                    {
                        upgradebutton.interactable = true;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "���׷��̵� : " + tower.Gettowerupgradeprice.ToString();
                    }
                    break;
                case 3:
                    if (tower.GetTowerLevel >= 7)
                    {
                        upgradebutton.interactable = false;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "���׷��̵� �Ұ�";
                    }
                    else
                    {
                        upgradebutton.interactable = true;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "���׷��̵� : " + tower.Gettowerupgradeprice.ToString();
                    }
                    break;

            }
        }
    }

    

    public void ClickTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //towertransform = detectob.ReturnTransform();
                //towertransform = DetectObject.GetNodeInfo();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {

                    if (hit.collider.CompareTag("Tower"))
                    {

                        tower = hit.collider.GetComponent<Tower>();
                        ShowInfo(tower);

                        if (tower != null)
                        {
                            SetTowerinfo();
                        ShowRange(tower.gameObject.transform, tower.GetRange);
                        }
                    }
                    else
                    {
                        towerinfopanel.SetActive(false);
                        RangeOff();
                    }
                }
                
            }
        }
    }

    public void SetTowerinfo()
    {
        towerinfopanel.SetActive(true);
    }
    public void SetTowerinfoOff()
    {
        towerinfopanel.SetActive(false);
    }


    public void ShowInfo(Tower _tower)
    {
        tower = _tower;
    }

    public void ShowRange(Transform _transform,float _range)
    {
        Transform towerpos = _transform;
        int rotation = 0;
        float range = _range;
        Vector3 DD = towerpos.forward * range;

        for(int i = 0; i < 72; i++)
        {
            towerpos.rotation = Quaternion.Euler(0, rotation, 0);
            
            RaycastHit hit;
            if (Physics.Raycast(towerpos.position + towerpos.forward* range + new Vector3(0, 10, 0), Vector3.down, out hit))
            {
                Debug.DrawLine(towerpos.position + towerpos.forward * range + new Vector3(0, 10, 0), towerpos.position + towerpos.forward + new Vector3(0,-10,0));
                if (hit.collider.CompareTag("Tile"))
                {
                    rangesprite[i].transform.position = hit.point;
                    rangesprite[i].transform.rotation = Quaternion.Euler(90, rotation, 0);
                }
                else
                {
                    rangesprite[i].transform.position = towerpos.position + towerpos.forward* range;
                    rangesprite[i].transform.rotation = Quaternion.Euler(90, rotation, 0);
                }
            }
            rangesprite[i].SetActive(true);
            rotation += 5;
        }

    }

    public void RangeOff()
    {

        for (int i = 0; i < 72; i++)
        {

            rangesprite[i].SetActive(false);
        }
    }
    public void OnClickSellTower()
    {
        GameManager.buttonOff(); RangeOff();
        tower.SellTower();
        towerinfopanel.SetActive(false);
        OffSellPrice();

    }

    public void OnClickUpgradeTower()
    {
        GameManager.buttonOff();
        tower.TowerUpgrade();
    }

    public void OnClickTowerMove()
    {
        GameManager.buttonOff();
        if (!enemymanager.GetGameOnGoing)
        {
            if (tower != null)
            {
                tower.TowerMove();
                RangeOff();
            }
        }
    }

    public void ShowSellPrice()
    {
        SellPrice.text = "�Ǹ� �ݾ� : " + (tower.GetSetSellPrice).ToString();
        SellPanel.SetActive(true);
    }

    public void OffSellPrice()
    {
        SellPanel.SetActive(false);
    }


}

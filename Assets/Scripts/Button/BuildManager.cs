using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


//��ž�� Ÿ���� ����ȭ ���� ���̶�Ű â���� ����
[System.Serializable]
public class BuildTower
{
    public GameObject preview = null;
    public GameObject builditem = null;
}


public class BuildManager : MonoBehaviour
{
    [SerializeField] private SoundManager SM;
    [SerializeField] private GameObject[] buildstate = null;

    //string towername = null;


    [SerializeField] private ShowTowerInfo showtowerinfo = null;
    [SerializeField] private BuildTower[] buildtower = null;
    [SerializeField] private PlayerState playerstate= null;
    int playercoin = 0;

    //Ÿ�� �̸����� ������
    private GameObject preview = null;
    //Ÿ�� ������
    private GameObject craft = null;

    //Ÿ������ �ٸ� ��ž�� �ִ��� Ȯ��
    //private bool alreadyontile = false;

    //���� Ÿ�� ���� ������ ��ž�� ��ġ���ִ���
    //private bool ontile = false;

    private static List<FireGunTower> firetowerlist = new List<FireGunTower>();
    private static List<TeslaTower> teslatowerlist = new List<TeslaTower>();

    private static bool iswet = false;

    [SerializeField] private GameObject BuildInfoPanel = null;
    [SerializeField] private TextMeshProUGUI[] info = null;
    [SerializeField] private Image towerimage = null;
    private int TowerCode = 0;
    private int PrefabsNum = 0;
    private bool BTowerPanel = false;
    private bool BMouseOnPanel = false;

    public void TOnPanel()
    {
        BMouseOnPanel = true;
    }
    public void FOnPanel()
    {
        BMouseOnPanel = false;
    }

    public void ClickBtnCode(int Code)
    {
        TowerCode = Code;
        BTowerPanel = true;
    }

    public void OnMouseBuildTowerPanel(int TowerCode)
    {
        BuildInfoPanel.SetActive(true);

        info[0].text = TowerDataSetUp.GetData(TowerCode).name;
        info[1].text = TowerDataSetUp.GetData(TowerCode).towerInfo;
        info[2].text = "���ݷ� : " + TowerDataSetUp.GetData(TowerCode).damage;
        info[3].text = "���ݼӵ� : "+TowerDataSetUp.GetData(TowerCode).delay;
        info[4].text = "��� : " + TowerDataSetUp.GetData(TowerCode).towerPrice * SkillSettings.PassiveValue("SetTowerDown");
        towerimage.sprite = Resources.Load<Sprite>("Image/Tower/" + (TowerDataSetUp.GetData(TowerCode).name+ TowerDataSetUp.GetData(TowerCode).towerStep));
    }
    public void ExitMouseBUildTowerPanel()
    {
        BuildInfoPanel.SetActive(false);
    }

    public void ClickBuild()
    {
        SlotClick(PrefabsNum);
        OffTowerPanel();
    }

    private void OffTowerPanel()
    {
        BTowerPanel = false;
        FOnPanel();
        BuildInfoPanel.SetActive(false);
    }

    public static void ActiveTower(FireGunTower firetower)
    {
        firetowerlist.Add(firetower);
        firetower.ChangeWet(iswet);
    }
    public static void ReturnActiveTower(FireGunTower firetower)
    {
        firetowerlist.Remove(firetower);
    }

    public static void ActiveTowerTesla(TeslaTower tesla)
    {
        teslatowerlist.Add(tesla);
    }
    public static void ReturnActiveTowerTesla(TeslaTower tesla)
    {
        teslatowerlist.Remove(tesla);
    }


    public static void Rained(bool wet)
    {
        iswet = wet;
        for (int i = 0; i < firetowerlist.Count; i++)
        {
            firetowerlist[i].ChangeWet(wet);
        }
        for (int i = 0; i < teslatowerlist.Count; i++)
        {
            teslatowerlist[i].ChangeWet(wet);
        }
    }


    private void OnDestroy()
    {
        iswet = false;
        firetowerlist.Clear();
    }


    //������ ��ž�� Ȱ��ȭ �Ǿ��ִ���
    private bool towerpreviewActive = false;
    
    //private bool canbuild = false;


    Vector3 mousepos = Vector3.zero;

    public GameObject Testtile = null;

    private void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            SlotClick(0);
        }

        if (BTowerPanel)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OffTowerPanel();
            }
        }

        if (!BMouseOnPanel)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OffTowerPanel();
            }
        }

        playercoin = playerstate.GetSetPlayerCoin;

        // if (preview == null) towerpreviewActive = false;

        if (towerpreviewActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelTower();
            }
        }

    }

    public void CancelTower()
    {
        towerpreviewActive = false;
        playerstate.GetSetPlayerCoin = -towerprice;
        preview.GetComponent<TowerPreview>().DestroyThis();
    }


    //��ž ����
    //Ÿ���� ���� ��, �̹� ��ž�� �ִ� ��, ���� �ִ� ���� ��ž ���� �Ұ�

    //�˻��׸�
    //1. Ÿ���� �ִ� ���ΰ�.
    //2. �������� ��ġ�� ���� ��ž�� �ִ°�
    //3. ��ã�⸦ ���� walkable�� true�� �ٲ� ���ΰ�.

    public void SlotClick(int _slotnum)
    {

        GameManager.buttonOff();

        if (towerpreviewActive)
        {
            playerstate.GetSetPlayerCoin = -towerprice;
            preview.GetComponent<TowerPreview>().DestroyThis();
            towerpreviewActive = false;
        }

        showtowerinfo.SetTowerinfoOff();

        float price = TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).towerPrice * SkillSettings.PassiveValue("SetTowerDown");
         towerprice = (int)price;
       // Debug.Log(TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).TowerPrice * SkillSettings.PassiveValue("SetTowerDown"));
        
            if (playercoin >= towerprice)
            {
                if (!towerpreviewActive)
                {
                SM.TurnOnSound(0);
                    towerpreviewActive = true;

                    preview = Instantiate(buildtower[_slotnum].preview, Vector3.zero, Quaternion.identity);

                    preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).range);
                    preview.GetComponent<TowerPreview>().FirstSetUp(buildtower[_slotnum].builditem,this);
                    playerstate.GetSetPlayerCoin = towerprice;
                }
            }
            else
            {
            SM.TurnOnSound(6);
            playerstate.ShowNotEnoughMoneyCor();
            }
    }




    public bool GettowerpreviewActive
    {
        set
        {
            towerpreviewActive = value;
        }
    }

    int towerprice;
    int upgradeprice;

    

    //private void TowerPos()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //    {
    //        mousepos = hit.point;
    //    }

    //    if (preview.GetComponent<TowerPreview>().CanBuildable())
    //    {
            
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            GameObject buildtower = Instantiate(craft, preview.transform.position,Quaternion.identity);
    //            Node node = preview.GetComponent<TowerPreview>().GetTowerNode;
    //            node.GetOnTower = true;
    //            buildtower.GetComponent<Tower>().SetUp(playerstate);
    //            buildtower.GetComponent<Tower>().SetNode=node;
    //            Destroy(preview);
    //            towerpreviewActive = false;

    //            showtowerinfo.ShowRange(buildtower.transform, buildtower.GetComponent<Tower>().GetRange);
    //            showtowerinfo.ClickTower();
                
    //        }
    //    }  
        
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        towerpreviewActive = false;
    //        playerstate.GetSetPlayerCoin = -towerprice;
    //        showtowerinfo.RangeOff();
    //        Destroy(preview);
    //    }
    //}

}

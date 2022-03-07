using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��ž�� Ÿ���� ����ȭ ���� ���̶�Ű â���� ����
[System.Serializable]
public class BuildTower
{
    public GameObject preview = null;
    public GameObject builditem = null;
}


public class BuildManager : MonoBehaviour
{
    string towername = null;

    [SerializeField] private ShowTowerInfo showtowerinfo = null;
    [SerializeField] private BuildTower[] buildtower = null;
    [SerializeField] private PlayerState playerstate= null;
    int playercoin = 0;

    //Ÿ�� �̸����� ������
    private GameObject preview = null;
    //Ÿ�� ������
    private GameObject craft = null;

    //Ÿ������ �ٸ� ��ž�� �ִ��� Ȯ��
    private bool alreadyontile = false;

    //���� Ÿ�� ���� ������ ��ž�� ��ġ���ִ���
    private bool ontile = false;

    //������ ��ž�� Ȱ��ȭ �Ǿ��ִ���
    private bool towerpreviewActive = false;
    
    
    private bool canbuild = false;


    Vector3 mousepos = Vector3.zero;

    public GameObject Testtile = null;

    private MapManager mapmanager = null;
    private bool addtileactive = false;

    private void Start()
    {
        mapmanager = this.GetComponent<MapManager>();
    }

    private void Update()
    {
        addtileactive = mapmanager.GetSetAddTile;

        playercoin = playerstate.GetSetPlayerCoin;

        if (preview == null) towerpreviewActive = false;

        if (towerpreviewActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                towerpreviewActive = false;
                playerstate.GetSetPlayerCoin = -towerprice;
                showtowerinfo.RangeOff();
                Destroy(preview);
            }
        }

    }


    //��ž ����
    //Ÿ���� ���� ��, �̹� ��ž�� �ִ� ��, ���� �ִ� ���� ��ž ���� �Ұ�

    //�˻��׸�
    //1. Ÿ���� �ִ� ���ΰ�.
    //2. �������� ��ġ�� ���� ��ž�� �ִ°�
    //3. ��ã�⸦ ���� walkable�� true�� �ٲ� ���ΰ�.

    public void SlotClick(int _slotnum)
    {
        
         towerprice = buildtower[_slotnum].builditem.GetComponent<Tower>().Gettowerprice;

            if (playercoin >= towerprice)
            {
                if (!towerpreviewActive)
                {
                     towerpreviewActive = true;
                    mapmanager.GetSetTileChange = false;

                    preview = Instantiate(buildtower[_slotnum].preview, Vector3.zero, Quaternion.identity);

                    preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, buildtower[_slotnum].builditem.GetComponent<Tower>().GetRange);
                    preview.GetComponent<TowerPreview>().SetUp(buildtower[_slotnum].builditem);
                preview.GetComponent<TowerPreview>().playerstate = playerstate;
                    //                    craft = buildtower[_slotnum].builditem;

                    playerstate.GetSetPlayerCoin = towerprice;
                }
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

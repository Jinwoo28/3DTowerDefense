using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��ž�� Ÿ���� ����ȭ ���� ���̶�Ű â���� ����
[System.Serializable]
public class BuildTower
{
    public string name;
    public GameObject preview = null;
    public GameObject builditem = null;
    public int buildcoin;
}


public class BuildManager : MonoBehaviour
{
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

    public delegate void MapButtonOff();
    public static event MapButtonOff buttonoff;

    Vector3 mousepos = Vector3.zero;

    public GameObject Testtile = null;

    private void Awake()
    {

    }

    private void Update()
    {
        playercoin = playerstate.PlayerCoin;

        if (towerpreviewActive)
        {
            TowerPos();
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
        buttonoff();
        if (playercoin >= buildtower[_slotnum].buildcoin)
        {
            if (!towerpreviewActive)
            {
                towerpreviewActive = true;
                preview = Instantiate(buildtower[_slotnum].preview, Vector3.zero, Quaternion.identity);
                craft = buildtower[_slotnum].builditem;

                playerstate.PlayerCoin = buildtower[_slotnum].buildcoin;
                towerprice = buildtower[_slotnum].buildcoin;
            }
        }
    }

    int towerprice;


    private void TowerPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            mousepos = hit.point;
        }

        if (preview.GetComponent<TowerPreview>().CanBuildable())
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject buildtower = Instantiate(craft, preview.transform.position,Quaternion.identity);
                Node node = preview.GetComponent<TowerPreview>().GetTowerNode;
                node.GetOnTower = true;
                buildtower.GetComponent<Tower>().SetUp(playerstate);
                Destroy(preview);
                towerpreviewActive = false;
                
            }
        }  
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            towerpreviewActive = false;
            playerstate.PlayerCoin = -towerprice;
            Destroy(preview);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPreview : MonoBehaviour
{

    private GameObject[] buildstate = null;
    //0 = ���׷��̵�
    //1 = �̵�����
    //2 = �̵��Ұ�

    //�ش� Ÿ�Ͽ� �̹� Ÿ���� ���� ���
    private bool alreadytower = false;

    //Ÿ�� ������ �˻�
    private bool ontile = false;

    //�̵��ϴ� ������� Ȯ��
    private bool checkOnroute = false;
    
    //build�� �� �ִ��� ���� ����
    private bool canbuildable = false;

    //��ü���� ���� ����
    private bool CanCombination = false;
    
    [SerializeField] private LayerMask layermask;

    //������ Ÿ��
    private GameObject buildTower = null;

    //�����䰡 ���� Ÿ���� �ܰ�
    private int towerstep = 0;

    public string towername = null;

    public PlayerState playerstate = null;

    private void UiStateChange(int _i)
    {

            for (int i = 0; i < 3; i++)
            {
                if (i == _i)
                {
                    buildstate[i].SetActive(true);
                }
                else
                {
                    buildstate[i].SetActive(false);
                }
            }
        
    }

     private void UiStateOff()
    {
        for (int i = 0; i < 3; i++)
        {
            buildstate[i].SetActive(false);
        }
    }

    private Node towernode;

    private Tower tower = null;
    private ShowTowerInfo showtowerinfo = null;
    private BuildManager buildmanager = null;
    private float range = 0;

    //�̵��� �� ���� Ÿ���� ����
    private GameObject Origintower = null;

    public delegate void MakeRouteOff();
    public static MakeRouteOff makerouteoff;

    private void Start()
    {
        makerouteoff();
    }

    private void Update()
    {
        Camera cam = Camera.main;
        Vector3 thisPos = this.transform.position;
        thisPos.y += 1.0f;
        for (int i = 0; i < 3; i++)
        {
            buildstate[i].transform.position = cam.WorldToScreenPoint(thisPos);
        }

        if (Origintower != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Origintower.GetComponent<Tower>().ActiveOn();
                Destroy(this.gameObject);
            }
        }

        Debug.Log("��ü ���� : "+CanCombination + " -- "+"Ÿ������ : " + alreadytower);
    }

    IEnumerator BuildTower()
    {
        while (true)
        {
           // Debug.Log(showtowerinfo);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    int X = hit.collider.GetComponent<Node>().gridX;
                    int Z = hit.collider.GetComponent<Node>().gridY;
                    float Y = hit.collider.transform.localScale.y;
                    this.transform.position = new Vector3(X, (Y / 2), Z);

                    showtowerinfo.ShowRange(this.gameObject.transform, range);

                    //Ÿ�� ���� �ִ���
                    ontile = true;
                    //�̹� Ÿ���� �ִ���
                    alreadytower = hit.collider.GetComponent<Node>().GetOnTower;
                    //�̵� �������
                    checkOnroute = hit.collider.GetComponent<Node>().Getwalkable;

                    towernode = hit.collider.GetComponent<Node>();
                }
                else
                {
                    ontile = false;
                }
            }

            //���� ��ġ�� ��ġ���� ���ο� ���� preview ���� uiǥ��
            if ((alreadytower && !CanCombination) || (CanCombination && towerstep == 3) || (CanCombination && tower.GetStep == 3)|| checkOnroute)
            {
               // Debug.Log((alreadytower && !CanCombination) + " : " + (CanCombination && towerstep == 3) + " : " + (CanCombination && tower.GetStep == 3));
                UiStateChange(2);
            }
            else if((alreadytower && CanCombination && towerstep != 3 && tower.GetStep != 3))
            {
              //  Debug.Log(alreadytower + " : " + CanCombination + " : " + (towerstep != 3 && tower.GetStep != 3));
                UiStateChange(0);
            }
            else
            {
                UiStateChange(1);
            }


            //Ÿ�� ��ü
            if (ontile && !checkOnroute)
            {
                if (CanCombination && alreadytower)
                {
                    if (tower != null)
                    {
                        //if (tower.GetStep != 3)
                        //{
                            if (Input.GetMouseButtonDown(0))
                            {
                                if (Origintower != null)
                                {
                                    tower.TowerStepUp(Origintower.GetComponent<Tower>());
                                    Destroy(Origintower);
                                }
                                else if(buildTower !=null)
                                { 
                                    tower.TowerStepUp(buildTower.GetComponent<Tower>());
                                
                                }

                                if (buildmanager != null)
                                    buildmanager.GettowerpreviewActive = false;

                                tower.GetBuildState = buildstate;
                                
                                Destroy(this.gameObject);
                                UiStateOff();
                            }
                        //}
                    }
                }

                //�̵��� �� Ȥ�� ���� ���� ��
                else if(!alreadytower)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //�̵����� ���� ���°��� �˻�
                        //�̵��� ��� ��ġ�� ����
                        //��ġ�� ��� instantiate

                        //��ġ����
                        //������ Ÿ���� �̵��� ��
                        if (Origintower != null)
                        {
                           
                            //Origintower.GetComponent<Tower>().SetNode.GetComponent<Node>().GetOnTower = false;
                            //Origintower.transform.position = this.transform.position;
                            //Origintower.GetComponent<Tower>().SetNode.GetComponent<Node>().GetOnTower = false;
                            //Origintower.GetComponent<Tower>().SetNode.GetOnTower = true;
                            //obj.GetComponent<Tower>().ActiveOn();
                            Origintower.transform.position = this.transform.position;
                            Origintower.GetComponent<Tower>().SetNode = towernode;
                            Origintower.GetComponent<Tower>().ActiveOn();
                            showtowerinfo.ShowInfo(Origintower.GetComponent<Tower>());
                            //GameObject obj = Instantiate(Origintower, this.transform.position, Quaternion.identity);
                            //obj.GetComponent<Tower>().TowerSetUp(towernode, showtowerinfo, buildstate, playerstate);
                            //obj.GetComponent<Tower>().ActiveOn();
                            //obj.GetComponent<Tower>().SetNode = towernode;
                            //obj.GetComponent<Tower>().SetShowTower = showtowerinfo;
                            //showtowerinfo.ShowInfo(obj.GetComponent<Tower>());
                            //showtowerinfo.ShowRange(obj.transform, Origintower.GetComponent<Tower>().GetRange);
                            //                   Origintower.GetComponent<Tower>().TowerDestroy();
                            Destroy(this.gameObject);
                        }

                        //���ο� Ÿ�� �Ǽ�
                        //����Ŵ������� ������ �����䰡 Ÿ�Ͽ� ������ ��
                        else
                        {
                            //Debug.Log(Origintower);
                            //Debug.Log("ssss");
                            GameObject buildedtower = Instantiate(buildTower, this.transform.position, Quaternion.identity);
                            buildedtower.GetComponent<Tower>().SetNode = towernode;
                            buildedtower.GetComponent<Tower>().SetNode.GetOnTower = true;
                            buildedtower.GetComponent<Tower>().SetShowTower = showtowerinfo;
                            buildedtower.GetComponent<Tower>().SetUp(playerstate);
                            buildedtower.GetComponent<Tower>().buildstate = buildstate;
                            showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
                            showtowerinfo.ShowRange(buildedtower.transform, buildedtower.GetComponent<Tower>().GetRange);

                        }
                        if (buildmanager != null)
                            buildmanager.GettowerpreviewActive = false;
                        showtowerinfo.SetTowerinfo();
                        UiStateOff();
                        Destroy(this.gameObject);

                    }

                }

            }
            yield return null;
        }
    }

    public void RangeOff()
    {
        showtowerinfo.RangeOff();
    }

    //�� Ÿ�� / ��X => Ÿ�� ����
    //Ÿ���� ���� ��� ���� �̸�, ���� step�̸� �ٷ� ��ȭ

    //Ÿ�� ��, �� X, Ÿ�� X => Ÿ�� ����
    //Ÿ�� ��, �� X, Ÿ�� O -- ���� �̸������ ���� �̸� �ܰ� => ��ȭŸ������


    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log(towerstep);
        if (other.CompareTag("Tower"))
        {
     //       Debug.Log("asdasdfasdfasdfsdfsdfaasdfasdfsdfasdfasfasdf");
            if(other.GetComponent<Tower>().Getname == towername&& other.GetComponent<Tower>().GetStep==towerstep && other.GetComponent<Tower>().GetStep !=3&&towerstep!=3)
            {
                Debug.Log(other.GetComponent<Tower>().Getname == towername + "1");
                Debug.Log((other.GetComponent<Tower>().GetStep == towerstep) + "2"); 
                Debug.Log((other.GetComponent<Tower>().GetStep != 3 && towerstep != 3)+"3");
                CanCombination = true;
               // Debug.Log(CanCombination + " : ��ü ����");
                tower = other.GetComponent<Tower>();
            }
            else
            {
         //       Debug.Log("4564548564856456456456456456456456");
                CanCombination = false;
                tower = null;
            }

         //alreadytower = true;
        }
        //else
        //{
        //    Debug.Log("4564548564856456456456456456456456 : 456456456456456");
        //    CanCombination = false;
        //    //alreadytower = false;
        //}
     //   Debug.Log(CanCombination + " : ��ü ����222");
    }

    //����Ŵ������� �����並 ������ �� �ʱ�ȭ�Լ�
    public void FirstSetUp(GameObject _buildtower,BuildManager _buildmanager)
    {
        buildmanager = _buildmanager;
        buildTower = _buildtower;
        towername = _buildtower.GetComponent<Tower>().Getname;
        towerstep = _buildtower.GetComponent<Tower>().GetStep;
        StartCoroutine("BuildTower");
    }

    //Ÿ������ �̵���ư�� ������ �� �ʱ�ȭ�Լ�
    public void TowerMoveSetUp(GameObject _OriginTower)
    {
        Origintower = _OriginTower;
        towername = _OriginTower.GetComponent<Tower>().Getname;
        towerstep = _OriginTower.GetComponent<Tower>().GetStep;
        StartCoroutine("BuildTower");
    }

    //preview�� ui���� ���� �Լ�
    public void TowerPreviewSetUp(ShowTowerInfo _showtowerinfo, GameObject[] _buildstate, PlayerState _playerstate,float _range)
    {
        range = _range;
        showtowerinfo = _showtowerinfo;
        buildstate = _buildstate;
        playerstate = _playerstate;
    }


}

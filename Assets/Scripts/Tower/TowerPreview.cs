using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    public string towername = null;

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

    private GameObject buildTower = null;

    private int towerstep = 0;

    public PlayerState playerstate = null;

    public void SetUp(PlayerState _playerstate)
    {
        playerstate = _playerstate;
    }

    public bool GetCanCombine => CanCombination;

    private Node towernode;

    private Tower tower = null;
    private ShowTowerInfo showtowerinfo = null;
    private BuildManager buildmanager = null;
    private float range = 0;

    private GameObject Origintower = null;

    public BuildManager Setbuildmanager
    {
        set
        {
            buildmanager = value;
        }
    }

    public GameObject SetOriginTower
    {
        set
        {
            Origintower = value;
        }
    }

    public delegate void MakeRouteOff();
    public static MakeRouteOff makerouteoff;

    private void Start()
    {
        makerouteoff();
    }

    private void Update()
    {
        Debug.Log("��ü ���� : "+CanCombination + " -- "+"Ÿ������ : " + alreadytower);
    }

    IEnumerator BuildTower()
    {
        while (true)
        {
           



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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                showtowerinfo.RangeOff();
            }



            if (ontile && !checkOnroute)
            {
                if (CanCombination && alreadytower)
                {
                    if (tower != null)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (tower.GetStep != 3)
                            {
                                if (Origintower != null)
                                {
                                    //���⼭ ����
                                    Origintower.GetComponent<Tower>().SetNode.GetComponent<Node>().GetOnTower = false;
                                    //tower.TowerStepUp(Origintower.GetComponent<Tower>());
                                    Destroy(Origintower);
                                }
                                tower.TowerStepUp(buildTower.GetComponent<Tower>());
                                if (buildmanager != null)
                                    buildmanager.GettowerpreviewActive = false;
                                Destroy(this.gameObject);
                            }
                        }
                    }
                }
                else if(!alreadytower)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //�̵����� ���� ���°��� �˻�
                        //�̵��� ��� ��ġ�� ����
                        //��ġ�� ��� instantiate

                        if (Origintower != null)
                        {
                            Origintower.GetComponent<Tower>().SetNode.GetComponent<Node>().GetOnTower = false;
                            Origintower.transform.position = this.transform.position;
                            Origintower.GetComponent<Tower>().SetActiveOn();
                            Origintower.GetComponent<Tower>().SetNode = towernode;
                            Origintower.GetComponent<Tower>().SetNode.GetOnTower = true;
                            showtowerinfo.ShowInfo(Origintower.GetComponent<Tower>());
                            showtowerinfo.ShowRange(Origintower.transform, Origintower.GetComponent<Tower>().GetRange);

                        }

                        else
                        {
                            GameObject buildedtower = Instantiate(buildTower, this.transform.position, Quaternion.identity);
                            buildedtower.GetComponent<Tower>().SetNode = towernode;
                            //towernode.GetOnTower = true;
                            buildedtower.GetComponent<Tower>().SetNode.GetOnTower = true;
                           // Debug.Log("��ġ");
                            buildedtower.GetComponent<Tower>().showtowerinfo = showtowerinfo;
                            buildedtower.GetComponent<Tower>().SetUp(playerstate);
                            showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
                            showtowerinfo.ShowRange(buildedtower.transform, buildedtower.GetComponent<Tower>().GetRange);

                        }
                        if (buildmanager != null)
                            buildmanager.GettowerpreviewActive = false;
                        showtowerinfo.SetTowerinfo();
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

    //private void OnDestroy()
    //{
    //    showtowerinfo.RangeOff();
    //}

    //public bool CanBuildable()
    //{
    //    showtowerinfo.ShowRange(this.gameObject.transform,range);
    //    if (ontile&&!checkOnroute)
    //    {
    //        if (CanCombination)
    //        {

    //        }
    //        canbuildable = true;
    //    }
    //    else canbuildable = false;

    //        return canbuildable;
    //}

    //�� Ÿ�� / ��X => Ÿ�� ����
    //Ÿ���� ���� ��� ���� �̸�, ���� step�̸� �ٷ� ��ȭ

    //Ÿ�� ��, �� X, Ÿ�� X => Ÿ�� ����
    //Ÿ�� ��, �� X, Ÿ�� O -- ���� �̸������ ���� �̸� �ܰ� => ��ȭŸ������

    //public void Ontile()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray,out hit, Mathf.Infinity, layermask))
    //    {
    //        if (hit.collider.CompareTag("Tile"))
    //        {
    //            int X = hit.collider.GetComponent<Node>().gridX;
    //            int Z = hit.collider.GetComponent<Node>().gridY;
    //            float Y = hit.collider.transform.localScale.y;
    //            this.transform.position = new Vector3(X, (Y/2), Z);

    //            //Ÿ�� ���� �ִ���
    //            ontile = true;
    //            //�̹� Ÿ���� �ִ���
    //            alreadytower = hit.collider.GetComponent<Node>().GetOnTower;
    //            //�̵� �������
    //            checkOnroute = hit.collider.GetComponent<Node>().Getwalkable;

    //            towernode = hit.collider.GetComponent<Node>();
    //        }
    //        else 
    //        { 
    //            ontile = false;
    //            this.transform.position = new Vector3((int)hit.point.x, 1, (int)hit.point.z);
    //        }
    //    }
    //}



    //public void CombinePreview()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //    {
    //        if (hit.collider.CompareTag("Tile"))
    //        {

    //            int X = hit.collider.GetComponent<Node>().gridX;
    //            int Z = hit.collider.GetComponent<Node>().gridY;
    //            float Y = hit.collider.transform.localScale.y;

    //            this.transform.position = new Vector3(X, (Y / 2), Z);

    //        }
    //        else
    //        {

    //            this.transform.position = new Vector3((int)hit.point.x, 1, (int)hit.point.z);
    //        }
    //    }
    //}


    public void SetUp(GameObject _buildtower)
    {
        buildTower = _buildtower;
        towername = _buildtower.GetComponent<Tower>().Getname;
        towerstep = _buildtower.GetComponent<Tower>().GetStep;
        StartCoroutine("BuildTower");
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log(towerstep);
        if (other.CompareTag("Tower"))
        {
            if(other.GetComponent<Tower>().Getname == towername&& other.GetComponent<Tower>().GetStep==towerstep && other.GetComponent<Tower>().GetStep !=3&&towerstep!=3)
            { 
                CanCombination = true;
                tower = other.GetComponent<Tower>();
            }
            else
            {
                
                CanCombination = false;
                tower = null;
            }

//         alreadytower = true;
        }
        else
        {
            CanCombination = false;
        }
    }



    //private void OnDestroy()
    //{
    //    showtowerinfo.RangeOff();
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Tower"))
    //    {
    //        alreadytower = false;
    //        tower = null;
    //    }
    //}

   // public bool AlreadyTower => alreadytower;
    public Tower GetTower => tower;

    public Node GetTowerNode
    {
        get
        {
            return towernode;
        }
    }

    public void SetShowTowerInfo(ShowTowerInfo _showtowerinfo, float _range)
    {
        showtowerinfo = _showtowerinfo;
        range = _range;
    }

}

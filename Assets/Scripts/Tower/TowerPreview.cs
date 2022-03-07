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

    private float range = 0;

    private GameObject Origintower = null;


    public GameObject SetOriginTower
    {
        set
        {
            Origintower = value;
        }
    }

    private void Update()
    {
        
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


            showtowerinfo.ShowRange(this.gameObject.transform, range);

            if (ontile && !checkOnroute)
            {
                if (CanCombination && alreadytower)
                {
                    if (tower != null)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("��ü");
                            if (Origintower != null)
                            {
                                Origintower.GetComponent<Tower>().SetNode.GetComponent<Node>().GetOnTower = false;
                                Destroy(Origintower);
                            }
                            tower.TowerStepUp(tower);
                            
                            Destroy(this.gameObject);
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                       GameObject buildedtower = Instantiate(buildTower, this.transform.position, Quaternion.identity);
                        buildedtower.GetComponent<Tower>().SetNode = towernode;
                        Debug.Log("��ġ");
                        buildedtower.GetComponent<Tower>().showtowerinfo = showtowerinfo;
                        buildedtower.GetComponent<Tower>().SetUp(playerstate);
                        if (Origintower != null)
                        {
                            Origintower.GetComponent<Tower>().SetNode.GetComponent<Node>().GetOnTower = false;
                            Destroy(Origintower);
                        }
                        Destroy(this.gameObject);
                    }
                }

            }
            yield return null;
        }
    }

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
        if (other.CompareTag("Tower"))
        {
            if(other.GetComponent<Tower>().Getname == towername&& other.GetComponent<Tower>().GetStep==towerstep)
            { 
                CanCombination = true;
                tower = other.GetComponent<Tower>();
            }
            else
            {
                CanCombination = false;
                tower = null;
            }

            alreadytower = true;
            
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            alreadytower = false;
            tower = null;
        }
    }

    public bool AlreadyTower => alreadytower;
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

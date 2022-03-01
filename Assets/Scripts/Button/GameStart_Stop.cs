using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart_Stop : MonoBehaviour
{
    private MapMake mapmake = null;
    private Node[,] grid = null;
    private int gridX=0;
    private int gridY=0;

    //��ã�⿡�� ���۰� �� ���
    private Node StartNode;
    private Node EndNode;

    private List<Node> waypointnode = null;

    [SerializeField] private EnemyManager EM = null;

    private bool gameisgoing = false;
    private void Start()
    {
        waypointnode = new List<Node>();

        mapmake = this.GetComponent<MapMake>();
        gridX = mapmake.GetgridX;
        gridY = mapmake.GetgridY;
        grid = mapmake.GetGrid;
        StartNode = mapmake.GetStartNode;
        EndNode = mapmake.GetEndNode;
    }

    private void Update()
    {
        //������ ������ �ʵ忡 ���� ���� ���� ������ ����
        gameisgoing = EM.GameOnGoing;
    }

    IEnumerator GameStartCheck()
    {
        while (true)
        {
            gameisgoing = EM.GameOnGoing;
            if (!gameisgoing) break;
            yield return null;
        }

        

        for (int i = 1; i < waypointnode.Count-1; i++)
        {
            waypointnode[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        waypointnode.Clear();
    }

    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //��ã�� �Լ�
    public void FindPath()
    {
        if (!gameisgoing)
        {
            gameisgoing = true;
            bool findpath = false;

            List<Node> OpenList = new List<Node>();

            //closedList�� ������ ������ ����� ���� ������ HashSet���� ����
            //HastSet�� ������ ������ ������� �ߺ����θ� üũ, �ߺ��� ��� false�� ���� �ʴ´�.
            HashSet<Node> ClosedList = new HashSet<Node>();

            OpenList.Add(StartNode);

            //openList�� ��尡 ���� �� ���� �ݺ�
            //openList�� ����ٴ� ���� ��� ��带 �˻��ߴٴ� ��
            while (OpenList.Count > 0)
            {

                //���� ���� OpenList[0] ��, ���� ������
                Node currentNode = OpenList[0];
                for (int i = 1; i < OpenList.Count; i++)
                {
                    //i�� 1���� �����ϴ� ������ startNode�� �̹� OpenList�� ���ֱ� ����.
                    //openList�� �ִ� ������ �Ÿ� ��� �� ���� ���� ����� ���� node�� currentnode�� ����
                    if (currentNode.GetfCost < OpenList[i].GetfCost || currentNode.GetfCost == OpenList[i].GetfCost && currentNode.GethCost < OpenList[i].GethCost)
                    {

                        currentNode = OpenList[i];
                    }
                }

                //currentNode�� �˻��� ���� Node�̱� ������ closedList�� �߰�
                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);

                //if (currentNode != StartNode)
                //    currentNode.OriginColor();

                //���� ����� �̿���带 ã�Ƽ� OpenList�� �߰�
                foreach (Node neighbournode in GetNeighbours(currentNode))
                {

                    //�̿� ��尡 closedlist�� �ְų�(�̹� �˻��� Node) �̵��Ұ��� ����
                    if (!neighbournode.Getwalkable || ClosedList.Contains(neighbournode))
                    {

                        continue;
                    }


                    //�̿� ����� cost���
                    int newMovementCost = currentNode.GetgCost + GetDistanceCost(currentNode, neighbournode);
                    if (newMovementCost < neighbournode.GetgCost || !OpenList.Contains(neighbournode))
                    {

                        neighbournode.GetgCost = newMovementCost;
                        neighbournode.GethCost = GetDistanceCost(neighbournode, EndNode);
                        neighbournode.parent = currentNode;


                        if (!OpenList.Contains(neighbournode))
                        {

                            OpenList.Add(neighbournode);
                        }
                    }

                }

                if (currentNode == EndNode)
                {
                    
                    findpath = true;
                    Debug.Log("��ã�� ����");
                    break;
                }


                // https://kiyongpro.github.io/algorithm/AStarPathFinding/

            }

            if (findpath)
            {

                Vector3[] ddd = WayPoint(StartNode, EndNode);

                EM.gameStartCourtain(ddd, ddd[0]);
                StartCoroutine("GameStartCheck");
            }

            else
            {
                Debug.Log("��ã�� ����");
                gameisgoing = false;
            }

        }
    }

    //�� ã�� �Ϸ��� �ش� Ÿ���� ��ġ�� ��ȯ
    private Vector3[] WayPoint(Node Startnode, Node Endnode)
    {
        Node currentNode = Endnode;
        List<Vector3> waypoint = new List<Vector3>();

        Vector3 tilePos = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth/2, currentNode.ThisPos.z);

        //�� ��带 �߰�
        waypoint.Add(tilePos);

        waypointnode.Add(currentNode);

        while (currentNode != Startnode)
        {
            Vector3 tilePos2 = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);
            currentNode = currentNode.parent;

            waypoint.Add(tilePos2);
            waypointnode.Add(currentNode);
        }

        Vector3 tilePos3 = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z); 
        waypoint.Add(tilePos3);

        waypointnode.Reverse();

        waypoint.Reverse();
        Vector3[] waypointary = waypoint.ToArray();

        for(int i = 1; i < waypointnode.Count-1; i++)
        {
            Debug.Log($"gridX : {waypointnode[i].gridX}, gridY : {waypointnode[i].gridY}, ���� : {waypointnode.Count}");

            waypointnode[i].OriginColor();
            if (waypointnode[i].gridX < waypointnode[i + 1].gridX)
            {//������
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(1);
            }
            else if (waypointnode[i].gridX > waypointnode[i + 1].gridX)
            {//����
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(2);
            }
            else if (waypointnode[i].gridY < waypointnode[i + 1].gridY)
            {//����
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(3);
            }
            else if (waypointnode[i].gridY > waypointnode[i + 1].gridY)
            {//�Ʒ���
                waypointnode[i].GetComponentInChildren<ShowRoute>().ShowArrow(4);
            }
        }

       
        return waypointary;
    }

    //��尣�� �Ÿ����
    int GetDistanceCost(Node A, Node B)
    {

        int disX = Mathf.Abs(A.gridX - B.gridX);
        int disY = Mathf.Abs(A.gridY - B.gridY);

        //if (disX > disY)
        //    return disY * 14 + (disX - disY) * 10;
        //return disX * 14 + (disY - disX) * 10;

        return disX + disY;

    }

    //node�� �̿����
    //�� ���ӿ��� �밢�� �̵��� �����Ƿ� ��,��,��,�Ʒ��� ���� �̿��� ���
    public List<Node> GetNeighbours(Node node)
    {
        
        List<Node> neighbours = new List<Node>();

        //���� ��带 �������� x,y�ε������� ��,�Ʒ�,����,�������� ����� 2���� int�迭 
        int[,] temp = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int i = 0; i < 4; i++)
        {
            
            int checkX = node.gridX + temp[i, 0]; //1,-1,0,0
            int checkY = node.gridY + temp[i, 1]; //0,0,1,-1

            if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY)
            {
                neighbours.Add(grid[checkY, checkX]);
            }

        }

        return neighbours;
    }







}

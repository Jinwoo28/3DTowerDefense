using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorimage = null;
    [SerializeField]private PlayerState playerstate = null;

    //tile ������
    [SerializeField] private GameObject Tile;

    //��ü Ÿ�� ������ũ��
    //¦�� ������ �Է��� ��
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    //�ʱ� Ÿ�� ����� ������ Ÿ�� ����
    [SerializeField] private int initialGridX;
    [SerializeField] private int initialGridY;

    //������ ���� Ÿ�� ��
    [SerializeField] private Material[] Tilecolor = null;

    //Ÿ�ϵ��� �θ� �� �� ���� ������Ʈ
    private GameObject parentgrid;

    //Node�� 2���� �迭�� ����� index�� �ο�
    private Node[,] grid;

    //��ã�⿡�� ���۰� �� ���
    private Node StartNode;
    private Node EndNode;

    /// ////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////

    //Ÿ���� �� ����� ��ư Ȱ��ȭ bool ��_ true�� ���� ���� �������
    private bool TileCanChange = false;

    private HashSet<Node> overlapcheck = null;

    /// ////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////
   
    //AddTile������
    [SerializeField] private GameObject AddTilePrefab;

    //�߰��� ��Ʈ���� Ÿ�� �ѹ�
    private int AddtileNum = 0;
    private int addtileprice = 0;

    public int GetAddTileNum => AddtileNum;
    public int GetAddtilePrice => addtileprice;

    private bool AddTileActive = false;
    private bool canaddtile = false;


    /// ////////////////////////////////////////////////
    /// ////////////////////////////////////////////////
    private bool isgameing = false;
    [SerializeField] private GameObject NotFound = null;

    private List<Node> waypointnode = null;
    private List<Node> activeNode = new List<Node>();
    public List<Node> GetActiveList => activeNode;

    public EnemyManager EM = null;

    private void Start()
    {
        waypointnode = new List<Node>();
        overlapcheck = new HashSet<Node>();
        TowerPreview.makerouteoff += makerouteoff;
    }

    private void makerouteoff()
    {
        TileCanChange = false;
    }

    private void Awake()
    {
   
        Mapmake();
        MakeHeight();

        AddtileNum = Random.Range(0, 7);
    }

    private void MakeHeight()
    {
        int Count = Random.Range(9, 10);
        int[,] temp = { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
        for (int i = 0; i < Count; i++)
        {
            int RanNum = Random.Range(2, 5);
            int Near = Random.Range(0, 3);

            int Xnum = Random.Range(0, gridX);
            int Ynum = Random.Range(0, gridY);
            grid[Xnum, Ynum].UpDownTile(grid[Xnum, Ynum].neighbournode, RanNum);
            if (Near >= 1)
            {
                while (Near >= 1)
                {
                    Near = Random.Range(0, 3);
                    int RanNum2 = Random.Range(0, 4);
                    if (Xnum + temp[RanNum2, 0] < gridX && Xnum + temp[RanNum2, 0] >= 0 && Ynum + temp[RanNum2, 1] < gridY && Ynum + temp[RanNum2, 1] >= 0)
                    {
                        grid[Xnum + temp[RanNum2, 0], Ynum + temp[RanNum2, 1]].UpDownTile(grid[Xnum + temp[RanNum2, 0], Ynum + temp[RanNum2, 1]].neighbournode, RanNum);

                    }
                   
                }
            }

        }
    }


    private void Update()
    {
        isgameing = EM.GameOnGoing;

        if (TileCanChange)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Node node = ReturnNode();

                    if (node != null)
                    {
                       StartCoroutine(NodeWalkableChange(node));
                    }
                }
            }

        if (TileCanChange)
            Cursor.SetCursor(cursorimage, Vector2.zero, CursorMode.ForceSoftware);
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }




    private void Mapmake()
    {
        //���� ���۽� Ÿ�ϸ��� ����� �Լ�

        //Node�� ���� �������迭 grid�� ũ�� ���� (���簢��)
        grid = new Node[gridX, gridY];

        if (parentgrid != null) Destroy(parentgrid);
        parentgrid = new GameObject("ParentGrid");

        //�߽����� �������� �翷,���Ʒ��� Ÿ���� ��ġ�ϱ� ���� ��ü ������ ���� ���
        int halfgridx = gridX / 2;
        int halfgridy = gridY / 2;

        for (int i = 0; i < gridY; i++)
        {
            for (int j = 0; j < gridX; j++)
            {
                GameObject tile = Instantiate(Tile, new Vector3(j, 0, i), Quaternion.identity);
                tile.transform.parent = parentgrid.transform;
                //������ Ÿ���� ParentGrid�� �ڽ����� �־ ����



                //������ ���ÿ� �ε��� ��ȣ �ο�
                //grid[i, j] = new Node(tile,false, i, j, 0,false);

                grid[i, j] = tile.GetComponent<Node>();
                   // tile.AddComponent<Node>();
                tile.GetComponent<Node>().Setnode(tile, false, j, i, false);
                tile.GetComponent<Node>().SetColor(Tilecolor);

                //Monobehaviour�� ��ӹ��� ��ũ��Ʈ�� newŰ���带 ����� �� ����.
                //����ϱ� ���ؼ��� ��� ���ӿ�����Ʈ�� component�� �پ��־�� ��.

                //�ذ��� 1. ��ũ��Ʈ�� ������Ʈ�� ���δ�.
                //�ذ��� 2. monobegaviour�� ������� �ʴ´ٸ� ���ش�.

                //https://etst.tistory.com/32


                tile.gameObject.SetActive(false);
                //������ ���ÿ� ��� ��Ȱ��ȭ
            }
        }

            /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////
        

            for (int Y = 0; Y < gridY; Y++)
            {
                for (int X = 0; X < gridX; X++)
                {
                    List<Node> node2 = new List<Node>();
                    int[,] temp = { { -1, 1 }, { 0, 1 }, { 1, 1 }, { -1, 0 }, { 1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 } };
                    for (int k = 0; k < 8; k++)
                    {
                        int checkX = X + temp[k, 0];
                        int checkY = Y + temp[k, 1];

                        if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY)
                        {
                            node2.Add(grid[checkY, checkX]);
                        }
                    }

                    grid[Y, X].GetNeighbournode(node2);
                }
            }


        /////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////

        int widthcount = initialGridX;

        int ycount = 0;
        int xcount = initialGridX - 1;



        //����� Ÿ�� Ȱ��ȭ
        for (int i = halfgridy - initialGridY; i < halfgridy + initialGridY; i++)
        {
            for (int j = halfgridx - widthcount; j < halfgridx + widthcount; j++)
            {
                grid[i, j].SetActiveTile(true);
                grid[i, j].GetSetActive = true;
                activeNode.Add(grid[i, j]);
            }

            //���ǹ��� �̿��� ����Ÿ���� ���� ����
            //ycount�� Y�� ������ �߰������� �߰�, ���� �ٽ� ����
            ycount++;

            if (ycount < initialGridY - xcount)
            {
                widthcount++;
            }

            //X�� ���� ������ŭ ��� Y�� ���� ����(����� �������)
            else if (ycount >= initialGridY - xcount && ycount <= initialGridY + xcount) { }
            else
            {
                widthcount--;
            }
        }


        //start�� end Node ���� ����
        //1. �������� start��� ����
        //2. �������� endNode�� �ε����� ����
        //endNode�� Y�� ��尪�� start�� ���ٸ�
        //endNode�� X�� ��尪�� startNode �� -1,0,1�� �ɼ� ����
        //endNode�� Y�� ��尪�� start�� �ٸ��ٸ�
        //endNode�� X�� ��尪�� startNode�� 0�� �� �� ����.

        //Ư���� : Y�࿡ �ش�Ǿ��ִ� x�� Ÿ���� ������ �˾ƾ� �Ѵ�.
        //https://kinanadel.blogspot.com/2018/09/c.html
        //    int RandomNum;

        //startNode ���� �ε�����
        int SYnum = Random.Range(halfgridy - initialGridY, halfgridy + initialGridY);
        int SXnum = Random.Range(0, gridX);

        while (!grid[SYnum, SXnum].CheckActiveTF)
        {
            SXnum = Random.Range(0, gridX);
        }
        grid[SYnum, SXnum].SetStartNode();
        StartNode = grid[SYnum, SXnum];
        StartNode.Getwalkable = true;

        //endNode ���� �ε��� ��
        int EYnum = Random.Range(halfgridy - initialGridY, halfgridy + initialGridY);
        int EXnum = Random.Range(0, gridX);

        if (EYnum == SYnum)
        {
           // Debug.Log("11");
            //Y���� ���� ��

            //Ÿ���� �־�� �ϰ�,
            //start x�ε����� -1,0,+1�� �ƴϾ�� �Ѵ�.

            while (!grid[EYnum, EXnum].CheckActiveTF || EXnum == SXnum || EXnum == (SXnum - 1) || EXnum == (SXnum + 1))
            {
                EXnum = Random.Range(0, gridX);
            }
            grid[EYnum, EXnum].SetEndNode();
            EndNode = grid[EYnum, EXnum];
            EndNode.Getwalkable = true;

        }
        else if (EYnum != SYnum)
        {
           // Debug.Log("22");
            //Y���� �ٸ� ��
            while (!grid[EYnum, EXnum].CheckActiveTF || EXnum == SXnum)
            {
                EXnum = Random.Range(0, gridX);
            }
            grid[EYnum, EXnum].SetEndNode();
            EndNode = grid[EYnum, EXnum];
            EndNode.Getwalkable = true;
        }

        grid[SYnum, SXnum].ChangeColor(Color.red);
        grid[EYnum, EXnum].ChangeColor(Color.blue);

    }

    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //Ÿ���߰� �Լ�
    public void OnClickMapAdd()
    {
        StartCoroutine("MapTileAdd");
    }

    IEnumerator MapTileAdd()
    {
        if (playerstate.GetSetPlayerCoin >= addtileprice)
        {
            //�߰�Ÿ�� ���ݸ�ŭ �÷��̾� ���� ����
            playerstate.GetSetPlayerCoin = addtileprice;

            //Ÿ���� ���� ����
            int tiledir = 0;
            
            //AddTile = true;
            Vector3 mousepos = Vector3.zero;
            Vector3 tilepos = Vector3.zero;

            GameObject[] AddtileX = new GameObject[4];

            //Ÿ�� 4�� ����
            for (int i = 0; i < 4; i++)
            {
                AddtileX[i] = Instantiate(AddTilePrefab, mousepos, Quaternion.identity);
            }
                AddTileActive = true;

            while (AddTileActive)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //���콺 ��ġ�� Ÿ�� ����

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // if(!hit.collider.CompareTag("Tile")&&hit.point.x>=0&& hit.point.x<gridX)
                    mousepos = hit.point;
                }

                if (AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[0].z < gridY
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[1].z < gridY
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[2].z < gridY
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].x >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].x < gridX
                    && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].z >= 0 && AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[3].z < gridY
                    )
                {
                    //4�� Ÿ���� ��ġ
                    for (int i = 0; i < 4; i++)
                    {
                        int X = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].x;
                        int Y = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i].z;
                       
                        AddtileX[i].transform.position = AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[i];

                        Vector3 intmousepos = Vector3.zero;
                        intmousepos = grid[Y, X].GetComponent<Node>().transform.localScale;

                        AddtileX[i].transform.localScale = intmousepos;


                        //4�� Ÿ���� ũ��� ��
                        if (AddtileX[0].GetComponent<Preview>().CanBuildable && AddtileX[1].GetComponent<Preview>().CanBuildable && AddtileX[2].GetComponent<Preview>().CanBuildable && AddtileX[3].GetComponent<Preview>().CanBuildable)
                        {
                            canaddtile = true;
                            switch ((int)(intmousepos.y*2)-1)
                            {
                                case 1:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[0].color;
                                    break;
                                case 2:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[1].color;
                                    break;
                                case 3:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[2].color;
                                    break;
                                case 4:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[3].color;
                                    break;
                                case 5:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[4].color;
                                    break;
                                case 6:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[5].color;
                                    break;
                                case 7:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[6].color;
                                    break;
                                case 8:
                                    AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[7].color;
                                    break;
                            }
                        }
                        else
                        {
                            AddtileX[i].GetComponentInChildren<MeshRenderer>().material.color = Tilecolor[8].color;
                            canaddtile = false;
                        }
                    }
                }

                if (AddTileActive)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        tiledir++;
                        if (tiledir >= 4) tiledir = 0;
                    }
                }

                if (canaddtile)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            //AddTile = false;
                            AddTileActive = false;

                            for (int j = 0; j < 4; j++)
                            {
                                int X = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].x;
                                int Y = (int)AddTilePos(AddtileNum, tiledir, (int)mousepos.x, (int)mousepos.z)[j].z;
                                Destroy(AddtileX[j]);
                                grid[Y, X].GetComponent<Node>().SetActiveTile(true);
                                grid[Y, X].GetSetActive = true;
                                AddTileActive = false;
                                activeNode.Add(grid[Y,X]);
                            }
                            addtileprice += 50;
                            AddtileNum = Random.Range(0, 7);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //AddTile = false;
                    AddTileActive = false;
                    for (int i = 0; i < 4; i++)
                    {
                        Destroy(AddtileX[i]);
                    }
                    playerstate.GetSetPlayerCoin = -addtileprice;
                }
                yield return null;
            }
        }
    }

    //��Ʈ���� ����� ����ġ ��ȯ
    private Vector3[] AddTilePos(int tileshapenum, int _tileDir, int Xnum, int Ynum)
    {
        Vector3[] tilelist = new Vector3[11];
        //����� Ÿ��, ����� ȸ��Ƚ��, Ÿ���� ��ġ ����

        Vector3[] returntilepos = new Vector3[4];

        /*
         
        ��� Ÿ�ϻ����� �ʿ��� index��ȣ
        0�� ���콺 ����
        X 1 2  3
        4 5 0  6
        X 7 8  9
        X X 10 X
         */

        //Ÿ�� ��� / ��������Ƚ��
        // O(0) I(1) S(2) Z(2) L(4) J(4) T(4)

        //Ÿ�ϸ�� O     I          S           Z           L           J           T
        //�� �� �� ��       �� �� �� ��    �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��
        //�� �� �� ��       �� �� �� ��    �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��
        //�� �� �� ��       �� �� �� ��    �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��
        //�� �� �� ��       �� �� �� ��    �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��     �� �� �� ��

        tilelist[0] = new Vector3(Xnum, 0, Ynum);
        tilelist[1] = new Vector3(Xnum - 1, 0, Ynum + 1);
        tilelist[2] = new Vector3(Xnum, 0, Ynum + 1);
        tilelist[3] = new Vector3(Xnum + 1, 0, Ynum + 1);
        tilelist[4] = new Vector3(Xnum - 2, 0, Ynum);
        tilelist[5] = new Vector3(Xnum - 1, 0, Ynum);
        tilelist[6] = new Vector3(Xnum + 1, 0, Ynum);
        tilelist[7] = new Vector3(Xnum - 1, 0, Ynum - 1);
        tilelist[8] = new Vector3(Xnum, 0, Ynum - 1);
        tilelist[9] = new Vector3(Xnum + 1, 0, Ynum - 1);
        tilelist[10] = new Vector3(Xnum, 0, Ynum - 2);

        switch (tileshapenum)
        {
            case 0:
                returntilepos[0] = tilelist[0];
                returntilepos[1] = tilelist[1];
                returntilepos[2] = tilelist[2];
                returntilepos[3] = tilelist[5];
                break;
            case 1:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[10];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returntilepos[0] = tilelist[4];
                    returntilepos[1] = tilelist[5];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[6];
                }
                break;
            case 2:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returntilepos[0] = tilelist[6];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[7];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[6];
                    returntilepos[3] = tilelist[9];
                }
                break;
            case 3:
                if (_tileDir == 0 || _tileDir == 2)
                {
                    returntilepos[0] = tilelist[5];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[9];
                }
                else if (_tileDir == 1 || _tileDir == 3)
                {
                    returntilepos[0] = tilelist[3];
                    returntilepos[1] = tilelist[6];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                break;
            case 4:
                if (_tileDir == 0)
                {
                    returntilepos[0] = tilelist[6];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[5];
                    returntilepos[3] = tilelist[7];
                }
                else if (_tileDir == 1)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[9];
                }
                else if (_tileDir == 2)
                {
                    returntilepos[0] = tilelist[3];
                    returntilepos[1] = tilelist[6];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[5];
                }
                else if (_tileDir == 3)
                {
                    returntilepos[0] = tilelist[1];
                    returntilepos[1] = tilelist[2];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                break;

            case 5:
                if (_tileDir == 0)
                {
                    returntilepos[0] = tilelist[5];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[6];
                    returntilepos[3] = tilelist[9];
                }
                else if (_tileDir == 1)
                {
                    returntilepos[0] = tilelist[3];
                    returntilepos[1] = tilelist[2];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                else if (_tileDir == 2)
                {
                    returntilepos[0] = tilelist[1];
                    returntilepos[1] = tilelist[5];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[6];
                }
                else if (_tileDir == 3)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[7];
                }
                break;
            /*

��� Ÿ�ϻ����� �ʿ��� index��ȣ
0�� ���콺 ����
X 1 2  3
4 5 0  6
X 7 8  9
X X 10 X
*/
            case 6:
                if (_tileDir == 0)
                {
                    returntilepos[0] = tilelist[5];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[6];
                    returntilepos[3] = tilelist[8];
                }
                else if (_tileDir == 1)
                {
                    returntilepos[0] = tilelist[6];
                    returntilepos[1] = tilelist[2];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[8];
                }
                else if (_tileDir == 2)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[5];
                    returntilepos[2] = tilelist[0];
                    returntilepos[3] = tilelist[6];
                }
                else if (_tileDir == 3)
                {
                    returntilepos[0] = tilelist[2];
                    returntilepos[1] = tilelist[0];
                    returntilepos[2] = tilelist[8];
                    returntilepos[3] = tilelist[5];
                }
                break;
        }

        /*if (AddTilePos2(returntilepos))*/
        return returntilepos;
        //else return null;


    }


    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //�� ����� �Լ�
    //Ÿ�� Ŭ���� �ش� ����� ���� walkable �ٲٱ�
    IEnumerator NodeWalkableChange(Node _changenode)
    {
        bool walkable = !_changenode.Getwalkable;

        while (Input.GetMouseButton(0))
        {
            Node changenode = ReturnNode();

            if (changenode != null)
            {
                ReturnNode().ChangeWalkableColor(walkable);

                if (walkable)
                {
                    overlapcheck.Add(changenode);
                }
            }
            yield return null;
        }
    }
    //��ư���� �游��� Ȱ��ȭ, ��Ȱ��ȭ ��ų ��ư�Լ�
    public void OnClickWalkableChange()
    {
        if (!isgameing)
        {
            if (!AddTileActive)
                TileCanChange = !TileCanChange;
        }
    }




    public void RouteReset()
    {
        if (!isgameing)
        {
            foreach (Node i in overlapcheck)
            {
                i.ChangeWalkableColor(false);
            }
        }
        
    }




    ///////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////
    //��ã�� �Լ�
    public void FindPath()
    {
        if (!isgameing)
        {
            isgameing = true;
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
                TileCanChange = false;
                Vector3[] waypoint = WayPoint(StartNode, EndNode);
                EM.gameStartCourtain(waypoint, waypoint[0]);
                StartCoroutine("GameStartCheck");
            }

            else
            {
                StopCoroutine("ShowNotFoundRoute");
                StartCoroutine("ShowNotFoundRoute");
                NotFound.SetActive(true);
                Debug.Log("��ã�� ����");
                isgameing = false;
            }

        }
    }

    private IEnumerator ShowNotFoundRoute()
    {
        yield return new WaitForSeconds(1.5f);
        NotFound.SetActive(false);
    }

    IEnumerator GameStartCheck()
    {
        while (true)
        {
            isgameing = EM.GameOnGoing;
            if (!isgameing)
            {
                addtileprice = 0;
                break;
            }
            yield return null;
        }

        for (int i = 0; i < waypointnode.Count - 1; i++)
        {
            waypointnode[i].GetComponentInChildren<ShowRoute>().ReturnArrow();
        }
        waypointnode.Clear();
    }


    //�� ã�� �Ϸ��� �ش� Ÿ���� ��ġ�� ��ȯ
    private Vector3[] WayPoint(Node Startnode, Node Endnode)
    {
        Node currentNode = Endnode;
        List<Vector3> waypoint = new List<Vector3>();

        Vector3 tilePos = new Vector3(currentNode.ThisPos.x, currentNode.GetYDepth / 2, currentNode.ThisPos.z);

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

        for (int i = 0; i < waypointnode.Count - 1; i++)
        {
          
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



    //���콺 ��ġ�� ��� ��ȯ
    public Node ReturnNode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.GetComponent<Node>();
        }
        return null;
    }


    //////////////////////////////////////////////////////////
    ///    //////////////////////////////////////////////////////////
    ///    
    public int GetgridX
    {
        get
        {
            return gridX;
        }
    }

    public int GetgridY
    {
        get
        {
            return gridY;
        }
    }

    public Material[] GetMaterials
    {
        get
        {
            return Tilecolor;
        }
    }

    public Node[,] GetGrid
    {
        get
        {
           return grid;
        }
    }

    public Node GetStartNode
    {
        get
        {
            return StartNode;
        }
    }
    public Node GetEndNode
    {
        get
        {
            return EndNode;
        }
    }

    public bool GetSetTileChange
    {
        set
        {
            TileCanChange = value;
        }
    }

    public bool GetSetAddTile
    {
        get
        {
            return AddTileActive;
        }
    }
}


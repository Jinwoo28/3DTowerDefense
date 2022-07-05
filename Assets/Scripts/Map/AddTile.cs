using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddTile : MonoBehaviour
{
    private GameObject[] AddtileX = null;

    //��ü Ÿ�� ������ũ��
    //¦�� ������ �Է��� ��
    private int gridX;
    private int gridY;

    private bool AddTileActive = false;
    private bool canaddtile = false;

    private int addtileprice = 10;

    [SerializeField] private PlayerState playerstate = null;

    //AddTile������
    [SerializeField] private GameObject AddTilePrefab;

    //�߰��� ��Ʈ���� Ÿ�� �ѹ�
    private int AddtileNum = 0;

    //Node�� 2���� �迭�� ����� index�� �ο�
    private Node[,] grid;

    //������ ���� Ÿ�� ��
    private Material[] Tilecolor = null;

    private List<Node> activeNode = null;
    public List<Node> GetActiveNode => activeNode;

    public void Settings(int _gridX, int _gridY, Node[,] _grid, Material[] _tilecolor, List<Node> _activeNode)
    {
        gridX = _gridX;
        gridY = _gridY;
        grid = _grid;
        Tilecolor = _tilecolor;
        activeNode = _activeNode;
    }

    private void Start()
    {
        AddtileNum = Random.Range(0, 7);
        GameManager.buttonOff+= CanCelMakeTile;
        EnemyManager.stageclear+= TileChange;
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

    public void OnClickAddTile()
    {
        GameManager.buttonOff();
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

            /*GameObject[]*/
            AddtileX = new GameObject[4];

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
                            switch ((int)(intmousepos.y * 2) - 1)
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
                                activeNode.Add(grid[Y, X]);
                            }
                            addtileprice += 10;
                            AddtileNum = Random.Range(0, 7);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CanCelMakeTile();
                }
                yield return null;
            }
        }
        else
        {
            playerstate.ShowNotEnoughMoneyCor();
        }
    }

    public void CanCelMakeTile()
    {
        if (AddTileActive)
        {
            AddTileActive = false;
            for (int i = 0; i < 4; i++)
            {
                Destroy(AddtileX[i]);
            }
            playerstate.GetSetPlayerCoin = -addtileprice;
        }
    }

    public void TileChange()
    {
        AddtileNum = Random.Range(0, 7);
    }

    public int GetAddTileNum => AddtileNum;
    public int GetAddtilePrice => addtileprice;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    //Node�� ������ ���ӿ�����Ʈ
    private GameObject tile;

    //��ã�⿡�� �̵� �������� ����
    private bool walkable = false;

    //Node�� �ε��� ��ȣ
    public int gridX;
    public int gridY;

    private float YDepth;

    //��ã���� ���۰� ��
    public bool start = false;
    public bool end = false;

    //���� Ÿ������ Ÿ���� �ִ��� ����
    private bool ontower = false;

    //�̵����
    private int gCost;
    //������������ ���ο� �������� �̵����

    private int hCost;
    //���ο� ������ ������ ������ �̵����

    public Node parent;
    //ã�� ���� ������ �ö� �� ����� �θ� ���

    private bool ActiveCheck = false;

    //Ÿ���� ���� ������ Material�迭
    private Material[] tilecolor;

    //���� Ÿ���� walkable���� �ƴ����� ���� �޸��� ��
    private Color walkablecolorT;
    private Color walkablecolorF;



    public void SetColor(Material[] color)
    {
        tilecolor = color;
    }

    /// ////////////////////////////////////
    /// ////////////////////////////////////

    private bool alreadymove = true;
    private bool DDDD = true;
    public List<Node> neighbournode;

    

    //������ ��� ����ؼ� ��� ���� �ʱ�ȭ ������
    public void Setnode(GameObject Tile, bool _walkable, int _gridX, int _gridY, bool onTower)
    {
        ontower = onTower;
        tile = Tile;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }


    private void Awake()
    {
        alreadymove = true;
        DDDD = true;
    }

    private void Start()
    {
        walkablecolorT = Color.red;
        Invoke("ColorChange2", 1.0f);
    }

    public bool GetAlreadymove
    {
        get
        {
            return alreadymove;
        }
    }

    public void OriginColor()
    {
        this.GetComponentInChildren<MeshRenderer>().material.color = walkablecolorF;
    }

    public bool GetDDDD
    {
        get
        {
            return DDDD;
        }
    }

    //Ÿ���� �α� ���̸� ���ϱ� ���� Ÿ�ϸ��� ���� ����� ������ ������ �Լ�
    public void GetNeighbournode(List<Node> _neighbournode)
    {
        neighbournode = _neighbournode;
    }

    float ydepth = 0;



    public void UpDownTile(List<Node> _neighbournode, float _Ydepth)
    {
        alreadymove = false;
        DDDD = false;

        Vector3 X = this.transform.localScale;
        ydepth = _Ydepth;

        if (_Ydepth >= X.y)
        {
            X.y = ydepth;
        }
        this.transform.localScale = X;
        float Xxx = X.y - 0.5f;


        for (int i = 0; i < _neighbournode.Count; i++)
        {
            Vector3 neighbourpos = _neighbournode[i].transform.localScale;
            float Y = _neighbournode[i].transform.localScale.y;

            int Xddd = (int)(X.y - Y);
            if (_neighbournode[i].alreadymove && Xxx > 0 && X.y > neighbourpos.y)
            {

                _neighbournode[i].alreadymove = false;
                neighbourpos.y = Xxx;
                _neighbournode[i].transform.localScale = neighbourpos;


            }

            else if (!_neighbournode[i].alreadymove && Xddd > 0.5f)
            {


                neighbourpos.y = Xxx;
                _neighbournode[i].transform.localScale = neighbourpos;

            }
        }

        for (int i = 0; i < _neighbournode.Count; i++)
        {
            Vector3 neighbourpos = _neighbournode[i].transform.localScale;
            if (ydepth > 0.5f && X.y > neighbourpos.y)
            {
                _neighbournode[i].UpDownTile(_neighbournode[i].neighbournode, Xxx);
            }
        }
    }


    public Vector3 ThisPos
    {
        get
        {
            Vector3 thispos = tile.transform.position;
            thispos.y += 1.0f;
            return thispos;
        }
    }



    public void SetActiveTile(bool X)
    {
        tile.SetActive(X);
        ActiveCheck = X;
    }

    public float GetYDepth{
        get
        {
            return ydepth;
        }
    }


    public void ChangeWalkableColor(bool _walkable)
    {
        if (!ontower)
        {
            if (!start && !end)
            {
                //�̹� �ٲ� ���� �ִ� Ÿ������ Ȯ��
                walkable = _walkable;
                if (walkable)
                {
                    ChangeColor(walkablecolorT);
                    color = walkablecolorT;
                }

                else if (!walkable)
                {
                    ChangeColor(walkablecolorF);
                    color = walkablecolorF;
                }

            }
        }
    }

    private Color color = Color.yellow;

    public Color GetColor
    {
        get
        {
            return color;
        }
    }

    public void ChangeColor(Color col)
    {
        this.GetComponentInChildren<MeshRenderer>().material.color = col;
    }

    public void ColorChange2()
    {
        Vector3 thisscale = this.transform.localScale;
        int x = (int)thisscale.y;

        if (!start && !end)
        {
            switch (x)
            {
                case 1:
                    ChangeColor(tilecolor[0].color);
                  //  Debug.Log("111");
                    color = tilecolor[0].color;
                    walkablecolorF = tilecolor[0].color;
  //                  walkablecolorT = tilecolor[7].color;
                    break;
                case 2:
                    ChangeColor(tilecolor[1].color);
                    color = tilecolor[1].color;
                    walkablecolorF = tilecolor[1].color;
   //                 walkablecolorT = tilecolor[8].color;
                    break;
                case 3:
                    ChangeColor(tilecolor[2].color);
                    color = tilecolor[2].color;
                    walkablecolorF = tilecolor[2].color;
   //                 walkablecolorT = tilecolor[9].color;
                    break;
                case 4:
                    ChangeColor(tilecolor[3].color);
                    color = tilecolor[3].color;
                    walkablecolorF = tilecolor[3].color;
  //                  walkablecolorT = tilecolor[10].color;
                    break;
                case 5:
                    ChangeColor(tilecolor[4].color);
                    color = tilecolor[4].color;
                    walkablecolorF = tilecolor[4].color;
    //                walkablecolorT = tilecolor[11].color;
                    break;
                case 6:
                    ChangeColor(tilecolor[5].color);
                    color = tilecolor[5].color;
                    walkablecolorF = tilecolor[5].color;
     //               walkablecolorT = tilecolor[12].color;
                    break;
                case 7:
                    ChangeColor(tilecolor[6].color);
                    color = tilecolor[6].color;
                    walkablecolorF = tilecolor[6].color;
   //                 walkablecolorT = tilecolor[13].color;
                    break;
            }
        }
    }

    public void SetStartNode()
    {
        start = true;
    }

    public void SetEndNode()
    {
        end = true;
    }


    public int GetX
    {
        get
        {
            return gridX;
        }
    }

    public int GetY
    {
        get
        {
            return gridY;
        }
    }

    public void ChangeTF(bool tf)
    {
        this.gameObject.SetActive(tf);
    }

    public bool CheckActiveTF
    {
        get
        {
            return ActiveCheck;
        }
    }

    public int GetgCost
    {
        get
        {
            return gCost;
        }
        set
        {
            gCost = value;
        }
    }

    public int GethCost
    {
        get
        {
            return hCost;
        }
        set
        {
            hCost = value;
        }
    }

    public int GetfCost
    {
        get
        {
            return gCost + hCost;
        }
   
    }

    public bool Getwalkable
    {
        get
        {
            return walkable;
        }
        set
        {
            walkable = value;
        }
    }

    public bool GetOnTower
    {
        get
        {
            return ontower;
        }
        set
        {
            ontower = value;
        }
    }

    public bool GetStartEnd
    {
        get
        {
            return (!end && !start);
        }
    }






}

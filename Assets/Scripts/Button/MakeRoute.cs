using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoute : MonoBehaviour
{
    private MapMake mapmake = null;
    private Node[,] grid = null;
    private bool TileChange = false;
    private bool AddTileActive = false;

    private List<Node> savenode = null;
    private HashSet<Node> overlapcheck = null;

    void Start()
    {
        mapmake = this.GetComponent<MapMake>();
        grid = mapmake.GetGrid;
        savenode = new List<Node>();
        overlapcheck = new HashSet<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (TileChange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Node node = ReturnNode();

                if (node != null)
                {
                    StartCoroutine("NodeWalkableChange", node);
                }
            }
        }
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
            //while�� �ȿ��� �̷��� �ڷ�ƾ ���� ������ �Ǹ�
            //���� ���� ������ ����Ƽ�� �������
            yield return null;
        }
    }

    //��ư���� ��ã�⸦ Ȱ��ȭ, ��Ȱ��ȭ ��ų ��ư�Լ�
    public void WalkableChangeButton()
    {
        if (!AddTileActive)
            TileChange = !TileChange;
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

    public void RouteReset()
    {
        Debug.Log("1111");

         foreach(Node i in overlapcheck)
        {
            i.ChangeWalkableColor(false);
        }
    }

    public void ReturnColor()
    {
        foreach (Node i in overlapcheck)
        {
            i.ChangeWalkableColor(true);
        }
    }
}

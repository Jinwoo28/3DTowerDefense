using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSetting : MonoBehaviour
{
    [SerializeField] private AddTile mapmanager = null;

    //�޾ƿ� Ȱ��ȭ ����Ʈ
    private List<Node> tilelist = new List<Node>();

    //��밡���� node����Ʈ
    private List<Node> checkednodelist = new List<Node>();

    //������ ���� ����Ʈ
    [SerializeField] private GameObject[] MakeTreeObj = null;
    private List<TreeSc> treelist = new List<TreeSc>();

    private List<Tree> treelistN = new List<Tree>();

    [SerializeField] private GameObject water = null;
    [SerializeField] private GameObject waterTrigger = null;

    [SerializeField] private ParticleSystem Rain = null;
    private bool rained = false;

    private int rainRate = 10;

    //private bool treechanged = false;

    // ���������� �Ѿ�� �������� ���� ���� 0~3��
    // ������ �����Ǹ� ����Ʈ�� ����
    // ����� �������� ���� ++ => ���̰� ¦���̸� ���� ������ ��ȭ
    
    // �� ���� �������� ������ 2��, �ؼ��� ���

    // ��� ������ node����Ʈ �ʿ�

    // ���������� ��ȭ�ϴ��� �� �� �־�� ��

    private void Start()
    {
        EnemyManager.stageclear += StageClear; 
    }

    private void OnDestroy()
    {
        EnemyManager.stageclear -= StageClear;
    }

    //�����غ� ȭ�鿡 �۵��� �޼���
    public void StageClear()
    {
        tilelist = mapmanager.GetActiveNode;

        //�������� ���� ����
        //���� ����
        for (int i = 0; i < treelistN.Count; i++)
        {
            if (!rained)
            {
                treelistN[i].EvolveTree(1);
            }
            else
            {
                //�񰡿��� �������� �� ���� �ڶ�
                treelistN[i].EvolveTree(2);
            }
        }

        InsTree();

        //���� Ȯ�� ����
        rainRate += 2;
        if (rainRate > 100)
        {
            rainRate = 100;
        }

        RainTF();
    }

    //���ӽ��� ȭ�鿡 �۵��� �޼���

    //���� ����
    private void InsTree()
    {
        int Count = Random.Range(0, 3);

        for(int i = 0; i < Count; i++)
        {
            CheckEmptyNode();
            int num = Random.Range(0, checkednodelist.Count);
            int treeNum = Random.Range(0, MakeTreeObj.Length);

            var newTree = Instantiate(MakeTreeObj[treeNum], new Vector3(checkednodelist[num].gridX, checkednodelist[num].GetYDepth / 2, checkednodelist[num].gridY), Quaternion.identity);
            newTree.GetComponent<Tree>().SetNode = checkednodelist[num];

            checkednodelist[num].OnBranch();

            treelistN.Add(newTree.GetComponent<Tree>());
            newTree.GetComponent<Tree>().SetWs = this;
        }

    }

    public void RemoveTree(Tree tree)
    {
        treelistN.Remove(tree);
    }

    public void RainTF()
    {
        int rain = Random.Range(1, 101);

        if (rain <= rainRate)
        {
            rained = true;
            Rain.Play();
            BuildManager.Rained(true);
        }
        else
        {
            rained = false;
            Rain.Stop();
            BuildManager.Rained(false);
        }
    }

    //��밡���� node Ȯ��
    private void CheckEmptyNode()
    {
        checkednodelist.Clear();
        for(int i = 0; i < tilelist.Count; i++)
        {
            if (GameManager.SetGameLevel == 1)
            {
                if (!tilelist[i].GetOnTower && !tilelist[i].Getwalkable)
                {
                    checkednodelist.Add(tilelist[i]);
                }
            }
            else
            {
                if (!tilelist[i].GetOnTower)
                {
                    checkednodelist.Add(tilelist[i]);
                }
            }
        }
    }

    public void GameStart()
    {
        if (rained) UpSeaLevel();
        else if (!rained) DryWater();
    }

    public void UpSeaLevel()
    {
        if (rained)
        {
            water.transform.position += new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale += new Vector3(0, 0.5f, 0);
        }
    }

    private void DryWater()
    {
        if(water.transform.position.y > 0.625f)
        {
            water.transform.position -= new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale -= new Vector3(0, 0.5f, 0);
        }
    }

}

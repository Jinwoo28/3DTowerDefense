using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
   



    //��ã�� ����
    //gCost = ���� ������ �ش� �������� ���
    //hcost = �޸���ƽ�� �̿��� ������������ ���
    //fCost = gCost + hCost;

    //���� ���ڹ��̿��� ��ã�⸦ �� �� ��Ÿ��� ���ǿ� ����
    //������ �̵� ����� 10, �밢���� �̵� ����� 14�� ���

    //openList = Ž���� ��ٸ��� �����
    //closedList = �̹� Ž���� ���� ����

    //currentNode == endNode�̰ų�
    //OpenList�� �������� �ݺ�
    //currentNode�� OpenList�� �ִ� ����� fCost�� ���� ���� ���
    //currentNode�� ������ OpenList���� currentNode�� �����ϰ� closeList�� �ֱ�

    //foreach������ current �ֺ� ���(neighbour Node)���� ��� �˻�.
    //�� �� �̵��� �� ����(walkable ==false)Node�� closedList�� ���ԵǾ� �ִ� Node�� ����

    //neighbour Node�� parentNode�� current Node

    //���� neighbour Node�� openList�� ������� �ʴٸ� �߰�

    //startNode�� openList

}

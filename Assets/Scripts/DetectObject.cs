using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectObject : MonoBehaviour
{    
    private Transform returntransform = null;

    public Transform ReturnTransform(LayerMask layerMask)
    {
        //���콺��ġ�� UI�� �ƴϾ��� ����
        //using���� EventSystem�� �־�� ��� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                Debug.DrawLine(new Vector3(15,10,15), hit.point);
                returntransform = hit.collider.transform;
            }

            return returntransform;
        }
        return null;
    }

    Vector3 pos = Vector3.zero;

    public Vector3 ReturnTransform2(LayerMask layerMask)
    {
        //���콺��ġ�� UI�� �ƴϾ��� ����
        //using���� EventSystem�� �־�� ��� ����
        
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                pos = hit.point;
                return hit.point;
            }  
        }
        return pos;
    }







    public static Node GetNodeInfo()
    {
        Node mousepointnode = GetNodeInfo();

        if (mousepointnode != null)
        {
            return mousepointnode;
        }
        else
        {
            return null;
        }

    }

    public Transform GetObjectInfo()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                returntransform = hit.collider.transform;
            }

            return returntransform;
        }
        return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectObject : MonoBehaviour
{
    private Transform returntransform = null;

    public Transform ReturnTransform()
    {
        //���콺��ġ�� UI�� �ƴϾ��� ����
        //using���� EventSystem�� �־�� ��� ����
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

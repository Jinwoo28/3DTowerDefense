using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private void Update()
    {
        CamMove();
    }
   

    private void CamMove()
    {
        Vector2 mouseDela2 = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        if (Input.GetMouseButton(1))
        {
            //���콺�� �̵��� ��ǥ ����
            Vector2 mouseDelat = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            Vector3 CamAngle = this.transform.rotation.eulerAngles; //ī�޶��� �θ� ������Ʈ�� ȸ������ ������ ����
           

            //���콺�� Y�� ȸ���� 3D������Ʈ�� X�� ȸ��
            float X = CamAngle.x -= mouseDelat.y;
            
            //ī�޶� ������ ����
            X = Mathf.Clamp(X, 1f, 90f);
            Debug.Log(X);

            this.transform.rotation = Quaternion.Euler(X, CamAngle.y + mouseDelat.x, CamAngle.z).normalized;
        }

        //ī�޶� �̵�
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveZ = Input.GetAxisRaw("Vertical");

        this.transform.position += this.transform.forward*MoveZ* Time.deltaTime*2.0f;
        this.transform.position += this.transform.right*MoveX*Time.deltaTime*2.0f;

        float Y = Input.GetAxisRaw("Mouse ScrollWheel");
        Debug.Log(Y);
        this.transform.position -= new Vector3(0, Y, 0).normalized * Time.deltaTime*5.0f;

    }

}

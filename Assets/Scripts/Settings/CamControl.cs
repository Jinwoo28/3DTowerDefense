using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private bool canCamMove = true;
    float xrotation = 0;

    private void Update()
    {
        if (canCamMove)
        {
            CamMove();
            xrotation = this.transform.rotation.x;
        }
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

            xrotation = X;

            this.transform.rotation = Quaternion.Euler(X, CamAngle.y + mouseDelat.x, CamAngle.z).normalized;
        }

        else if (Input.GetMouseButton(2))
        {

            Vector3 thispos = this.transform.position;
            thispos.x -= mouseDela2.x * 10.0f * Time.deltaTime;
            thispos.z -= mouseDela2.y * 10.0f * Time.deltaTime;
            if (xrotation > 50)
            {
                this.transform.position -= this.transform.forward * mouseDela2.y * Time.deltaTime * 12.0f;
            }
            else if (xrotation <= 50)
            {
                this.transform.position -= this.transform.up * mouseDela2.y * Time.deltaTime * 12.0f;
            }


            this.transform.position -= this.transform.right * mouseDela2.x * Time.deltaTime * 12.0f;

        }

        //ī�޶� �̵�
        float zoominout = Input.GetAxisRaw("Mouse ScrollWheel") * 10f;
        this.transform.position += this.transform.forward * zoominout * Time.deltaTime * 20f;

    }

    public void CamMoveOn()
    {
        canCamMove = true;
    }
    public void CamMoveOff()
    {
        canCamMove = false;
    }

}

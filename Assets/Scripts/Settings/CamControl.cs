using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    // ���콺�� UI���� �ִ��� üũ
    // UI���� ������ ���콺�� ī�޶� �̵� �Ұ�
    private bool canCamMove = true;

    //�̵� �ӵ�
    private float MoveSpeed = 5.0f;
  
    //���� ī�޶� ���� ����
    private Camera camera;

    private Vector3 OriginPos;
    private Vector3 OriginAngle;

    //���콺�� �̵��� �� �ش� �κ��� �β�
    private float XThicness = Screen.height / 25;
    private float YThicness = Screen.height / 17;

    private void Start()
    {
        camera = Camera.main;

        OriginPos = this.transform.position;
        OriginAngle = this.transform.rotation.eulerAngles;

    }

    public void ReSetCamPos()
    {
        this.transform.position = OriginPos;
        this.transform.rotation = Quaternion.Euler(OriginAngle);
        rotation = 0;
        sumzoom = 0;
    }

    //ī�޶� �̵�

    //1. WASDŰ
    //2. ���콺�� ȭ�鿡 ���� ������ �̵�
    //3. ī�޶� �ٶ󺸴� ������ ��� Ȯ��
    
    // ī�޶� ȸ���� Y�����θ� ȸ��

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Time.timeScale += 1;
        //    Debug.Log(Time.timeScale);
        //}

        if (canCamMove)
        {
            //Ű���� WASD�� �̵�
            CamMoveByKeyBoard();
            ZoomCamera();
            RotateCamera();
            //ī�޶� ȸ������ ���� ���콺�� �̵� �Ұ�
            if (Input.GetMouseButton(1))
            {
                RotateCamera();
            }
            else
            {
                CamMoveByMouse();
            }
        }
    }


    float rotation = 0;
    private void RotateCamera()
    {
        //���콺�� �̵��� ��ǥ ����
        float mouseDelat = Input.GetAxisRaw("Mouse X");
        float thisAngle = this.transform.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(0, thisAngle + mouseDelat, 0).normalized;

        if (Input.GetKey(KeyCode.E)) 
        {
            rotation -= Time.deltaTime*10;
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            rotation -= Time.deltaTime*10;
        }
        this.transform.rotation = Quaternion.Euler(0, rotation, 0).normalized;
    }


    private float sumzoom = 0;

    private void ZoomCamera()
    {
        //ī�޶� Ȯ�� �̵�
        float zoominout = Input.GetAxisRaw("Mouse ScrollWheel") * 10f;

        //�� ���� ����
        sumzoom += zoominout;

        //�� ���� ������ �������� �ΰ� Ȯ���� ���� ����
        sumzoom = Mathf.Clamp(sumzoom, -10, 20);

        if(zoominout >= 0 && sumzoom <20)
        {
            camera.transform.position += camera.transform.forward * zoominout * Time.deltaTime*20;
        }
        else if (zoominout <= 0 && sumzoom>-10)
        {
            camera.transform.position += camera.transform.forward * zoominout * Time.deltaTime*20;
        }
    }

    private void CamMoveByMouse()
    {
        if (Input.mousePosition.y >= Screen.height - YThicness)
        {
            this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= YThicness)
        {
            this.transform.position -= this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - XThicness)
        {
            this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= XThicness)
        {
            this.transform.position -= this.transform.right * MoveSpeed * Time.deltaTime;
        }

        //Mathf�� ������Ʈ�� ��ġ�� ����
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, 30), this.transform.position.y, Mathf.Clamp(this.transform.position.z, 0, 30));
    }

    private void CamMoveByKeyBoard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += this.transform.forward*MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= this.transform.right * MoveSpeed * Time.deltaTime;
        }

        //Mathf�� ������Ʈ�� ��ġ�� ����
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, 30), this.transform.position.y, Mathf.Clamp(this.transform.position.z, 0, 30));
    }

    //UI���� ���콺�� �ִ��� üũ
    public void CamMoveOn()
    {
        canCamMove = true;
    }
    public void CamMoveOff()
    {
        canCamMove = false;
    }

}

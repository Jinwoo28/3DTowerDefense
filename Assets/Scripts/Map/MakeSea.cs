using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSea : MonoBehaviour
{
    [SerializeField] private GameObject Sea = null;

    private void Awake()
    {
        for(int i = 0; i < 6; i++)  //���� 
        {
            for(int j = 0;j < 6; j++)   //����
            {
                for (int k = 0; k < 2; k++) //�� �� �ȿ� ����
                {
                    for (int l = 1; l < 3; l++) //�� �� �ȿ� ����
                    {
                        //-12.5   -> -7.5
                        //-12.5   -> -7.5
                        var sea =Instantiate(Sea, new Vector3((-17.5f + l * 5)+(j*10), this.transform.position.y, (-12.5f + k * 5)+(i*10)), Quaternion.identity,this.transform);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

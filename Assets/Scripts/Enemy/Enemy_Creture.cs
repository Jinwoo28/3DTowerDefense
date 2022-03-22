using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Creture : Enemy
{
    private float timevalue = 0;
    private float originHP = 0;
    [SerializeField] private int HpUpValue = 0;

    protected virtual void Start()
    {
        StartMove();
        originHP = GetHp;
    }

    // Update is called once per frame
    void Update()
    {
        UnitCharacteristic();
    }

    protected override void UnitCharacteristic()
    {
        timevalue += Time.deltaTime;

        if (timevalue >= 3.0f)
        {
            timevalue = 0;
            if (GetHp < originHP)
            {
                GetHp = HpUpValue;
            }
        }
    }
}
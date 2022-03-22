using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSetting : MonoBehaviour
{
    private enum weather
    {
        spring,
        summer,
        fall,
        winter
    }

    [SerializeField] private EnemyManager enemyManager = null;
    private int stageNum = 1;

    [SerializeField] private MapManager mapmanager = null;

    //�޾ƿ� Ȱ��ȭ ����Ʈ
    private List<Node> tilelist = new List<Node>();

    //��밡���� node����Ʈ
    private List<Node> checkednodelist = new List<Node>();

    private weather weaTher = weather.spring;

    [SerializeField] private GameObject[] SpringItem = null;

    [SerializeField] private GameObject water = null;
    [SerializeField] private GameObject waterTrigger = null;

    [SerializeField] private GameObject[] fallItem = null;
    [SerializeField] private GameObject[] fallFruit = null;

    [SerializeField] private GameObject[] WinterItem = null;
    [SerializeField] private GameObject[] WinterObstacle = null;

    [SerializeField] private ParticleSystem Rain = null;
    private bool rained = false;
    [SerializeField] private ParticleSystem Snow = null;


    private void Update()
    {
        tilelist = mapmanager.GetActiveList;
        WeatherChange();
    }


    private void WeatherChange()
    {
        if (stageNum <= 5)
        {
            weaTher = weather.spring;
        }
        else if (stageNum <= 10)
        {
            weaTher = weather.summer;
        }
        else if (stageNum <= 15)
        {
            weaTher = weather.fall;
        }
        else if (stageNum <= 20)
        {
            weaTher = weather.winter;
        }
    }

    public void WeatherSettings()
    {
        stageNum++;

        if (!rained)
        {
            DryWater();
        }

        switch ((int)weaTher)
        {
            case 0:
                SpringAct();
                break;
            case 1:
                SummerAct();
                break;
            case 2:
                FallAct();
                break;
            case 3:
                WinterAct();
                break;
        }
    }

    private void SpringAct() 
    {
        //�������� �� ���� ������
        int i = Random.Range(0, 4);
        for(int j = 0; j < i; j++)
        {
            CheckEmptyNode();
            //������ ����� index
            int num = Random.Range(0, checkednodelist.Count);
            //������ ����
            int InsTree = Random.Range(0, SpringItem.Length);

            GameObject Tree = Instantiate(SpringItem[InsTree], new Vector3(checkednodelist[num].gridX, checkednodelist[num].GetYDepth/2, checkednodelist[num].gridY), Quaternion.identity);
            checkednodelist[num].GetOnTower = true;

        }
    }
    private void SummerAct() 
    {

        int rainprobabilty = Random.Range(0, 2);
        if(rainprobabilty < 1)
        {
            Rain.Play();
            rained = true;
        }
        else
        {
            Rain.Stop();
            rained = false;
        }
    }
    private void FallAct() { }
    private void WinterAct() { }

    private void CheckEmptyNode()
    {
        checkednodelist = new List<Node>();
        for(int i = 0; i < tilelist.Count; i++)
        {
            if (!tilelist[i].GetOnTower && !tilelist[i].Getwalkable)
            {
                checkednodelist.Add(tilelist[i]);
            }
            else
            {
                checkednodelist.Remove(tilelist[i]);
            }
        }


    }

    private void RainDown()
    {
        Rain.Play();
    }

    public void UpSeaLevel()
    {
        if (rained)
        {
            water.transform.position += new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale += new Vector3(0, 0.5f, 0);
        }

        if(weaTher != weather.summer) rained = false;
    }

    private void RainStop()
    {
        Rain.Stop();
    }

    private void DryWater()
    {
        if(water.transform.position.y > 0.625f)
        {
            water.transform.position -= new Vector3(0, 0.25f, 0);
            waterTrigger.transform.localScale -= new Vector3(0, 0.5f, 0);
        }
    }

    private void SnowDown()
    {
        Snow.Play();
    }

    private void StopSnow()
    {
        Snow.Stop();
    }



}

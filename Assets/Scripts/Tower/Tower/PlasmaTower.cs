using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaTower : Tower
{
    [SerializeField] private ParticleSystem Charging;
    [SerializeField] private ParticleSystem Boom;
    [SerializeField] private GameObject PlasmaLaser;

    private bool isShoot = false;

    protected override void Start()
    {
        base.Start();
        SoundStop = false;
    }

    protected override void Update()
    {
        //�߻����� �ƴ� ��
        if (!isShoot)
        {
            base.Update();
            //if (FinalTarget != null)
            //{
            //    Charging.Play();
            //}
            //else
            //{
            //    Charging.Stop();
            //}
        }
    }

    protected override void Attack()
    {
        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        Charging.Play();
        yield return new WaitForSeconds(0.5f);

        AS.Play();
        Boom.Play();
        isShoot = true;
        PlasmaLaser.GetComponent<Plasmabullet>().SetDamage(towerinfo.towerdamage);
        PlasmaLaser.SetActive(true);
     

        yield return new WaitForSeconds(0.3f);
        PlasmaLaser.GetComponent<Plasmabullet>().ReturnScale();
        isShoot = false;
        PlasmaLaser.SetActive(false);
        Charging.Stop();
    }




}

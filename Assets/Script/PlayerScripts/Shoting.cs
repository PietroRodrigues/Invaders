using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotting
{
    ShottingSettings shottingSettings;
    Chronometry chronometry = new Chronometry();
    bool primeroTiro = true;

    public Shotting(ShottingSettings settings)
    {
        shottingSettings = settings;
        shottingSettings.particleCanon = shottingSettings.cannon.Find("ParticleCanon").transform;
        shottingSettings.cxBalas = shottingSettings.cannon.Find("CxBalas");

    }

    public void CanonShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset)
    {             
        if(attack){
            
            shottingSettings.speedBody = speed;

            if(primeroTiro){
                AnimationStart();
                FireBullet();
                primeroTiro = false;
            }
        }
        
        if(primeroTiro == false){
            if(chronometry.ChronometryPorMiles(600)){
                chronometry.Reset();
                primeroTiro = true;
            }
        }

        bulletReturn(pos, maxDistanceReset);

    }

    void FireBullet()
    {
        if(shottingSettings.cxBalas.childCount == 0){
            GameObject bala = GameObject.Instantiate(shottingSettings.bullet);
            bala.name = shottingSettings.cxBalas.root.name + " Bullet";
            bala.GetComponent<Bullet>().gumOrigen = shottingSettings.cxBalas;
            bala.GetComponent<Bullet>().BulletOrigen();            
            shottingSettings.BoxBullet.Add(bala);            
        }
      

        if(shottingSettings.cxBalas.childCount != 0){                
            
            GameObject bullet = shottingSettings.cxBalas.GetChild(0).gameObject;
            
            if (!bullet.activeSelf)
            {                  
                bullet.GetComponent<Bullet>().Disparate(shottingSettings.speedBody);
              
            }
        }

    }

    void bulletReturn(Vector3 pos, float maxDistanceReset)
    {
        for (int i = 0; i < shottingSettings.BoxBullet.Count; i++)
        {            
            GameObject bullet = shottingSettings.BoxBullet[i];

            if (bullet.activeSelf)
            {
                if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
                {   
                    bullet.GetComponent<Bullet>().BulletOrigen();
                    i = shottingSettings.BoxBullet.Capacity;
                }
            }
            
        }
    }

    void AnimationStart()
    {       
        ParticleSystem p = shottingSettings.particleCanon.GetComponent<ParticleSystem>();
        var main = p.main;
        main.loop = false;
        p.Play();                    
    }

}

[System.Serializable]
public struct ShottingSettings
{   
    public Transform cannon;
    [HideInInspector] public List<GameObject> BoxBullet;
    [HideInInspector]public Transform particleCanon;
    [HideInInspector]public Transform cxBalas;
    public GameObject bullet;
    [HideInInspector] public float speedBody;
}
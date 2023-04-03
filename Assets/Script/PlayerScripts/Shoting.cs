using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoting
{
    ShotingSettings shotingSettings;
    Cronometro cronometro = new Cronometro();
    bool primeiroTiro = true;

    public Shoting(ShotingSettings settings)
    {
        shotingSettings = settings;
        shotingSettings.particleCanon = shotingSettings.canho.Find("ParticleCanon").transform;
        shotingSettings.cxBalas = shotingSettings.canho.Find("CxBalas");

    }

    public void CanonShoting(bool atack, float speed, Vector3 pos, float maxDistanceReset)
    {             
        if(atack){
            
            shotingSettings.speedBody = speed;

            if(primeiroTiro){
                AnimationStart();
                FireBullet();
                primeiroTiro = false;
            }
        }
        
        if(primeiroTiro == false){
            if(cronometro.CronometroPorMileseg(400)){
                cronometro.Reset();
                primeiroTiro = true;
            }
        }

        bulletReturn(pos, maxDistanceReset);

    }

    void FireBullet()
    {
        if(shotingSettings.cxBalas.childCount == 0){
            GameObject bala = GameObject.Instantiate(shotingSettings.bullet);
            bala.name = shotingSettings.cxBalas.root.name + " Bullet";
            bala.GetComponent<Bullet>().gumOringem = shotingSettings.cxBalas;
            bala.GetComponent<Bullet>().BulletOrigen();            
            shotingSettings.BoxBullet.Add(bala);            
        }
      

        if(shotingSettings.cxBalas.childCount != 0){                
            
            GameObject bullet = shotingSettings.cxBalas.GetChild(0).gameObject;
            
            if (!bullet.activeSelf)
            {                  
                bullet.GetComponent<Bullet>().Disparar(shotingSettings.speedBody);
              
            }
        }

    }

    void bulletReturn(Vector3 pos, float maxDistanceReset)
    {
        for (int i = 0; i < shotingSettings.BoxBullet.Count; i++)
        {            
            GameObject bullet = shotingSettings.BoxBullet[i];

            if (bullet.activeSelf)
            {
                if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
                {   
                    bullet.GetComponent<Bullet>().BulletOrigen();
                    i = shotingSettings.BoxBullet.Capacity;
                }
            }
            
        }
    }

    void AnimationStart()
    {        
        foreach (Transform particleSysten in shotingSettings.particleCanon.transform)
        {
            ParticleSystem p = particleSysten.GetComponent<ParticleSystem>();
            
            if(p.isStopped){
                
                var main = p.main;
                main.loop = true;
                p.Play();
                
            }
        }         
    }

    void AnimationStop()
    {
        foreach (Transform particleSysten in shotingSettings.particleCanon.transform)
        {
            ParticleSystem p = particleSysten.GetComponent<ParticleSystem>();
            var main = p.main;
            main.loop = false;
            p.Stop();
                            
        }      
    }
}

[System.Serializable]
public struct ShotingSettings
{   
    public Transform canho;
    [HideInInspector] public List<GameObject> BoxBullet;
    [HideInInspector]public Transform particleCanon;
    [HideInInspector]public Transform cxBalas;
    public GameObject bullet;
    [HideInInspector] public float speedBody;
}
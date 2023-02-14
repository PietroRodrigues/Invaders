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

        shotingSettings.particleCanon = new Transform[shotingSettings.canhoes.Length];

        shotingSettings.BoxBullet = new GameObject[shotingSettings.canhoes.Length, shotingSettings.bulletsInBox];

        for (int i = 0; i < shotingSettings.canhoes.Length; i++)
        {
            shotingSettings.particleCanon[i] = shotingSettings.canhoes[i].Find("ParticleCanon").transform;
            Transform cxBalas = shotingSettings.canhoes[i].Find("CxBalas");          

            for (int y = 0; y < shotingSettings.bulletsInBox; y++)
            {
                GameObject bala = GameObject.Instantiate(shotingSettings.bullet);
                bala.name = (y+1).ToString();
                bala.GetComponent<MisselBasico>().gumOringem = cxBalas;
                bala.GetComponent<MisselBasico>().BulletOrigen();
                shotingSettings.BoxBullet[i, y] = bala;               
            }
        }
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
        for (int i = 0; i < shotingSettings.canhoes.Length; i++)
        {
            if(shotingSettings.canhoes[i].GetChild(0).childCount != 0){                
                GameObject bullet = shotingSettings.canhoes[i].GetChild(0).transform.GetChild(0).gameObject;
                
                if (!bullet.activeSelf)
                {                  
                    bullet.GetComponent<MisselBasico>().Disparar(shotingSettings.speedBody);
                }
            }
        }      
    }

    public void bulletReturn(Vector3 pos, float maxDistanceReset)
    {
        for (int i = 0; i < shotingSettings.BoxBullet.GetLength(0); i++)
        {
            for (int y = 0; y < shotingSettings.BoxBullet.GetLength(1); y++)
            {
                GameObject bullet = shotingSettings.BoxBullet[i, y];
                if (bullet.activeSelf)
                {
                    if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
                    {   
                        bullet.GetComponent<MisselBasico>().BulletOrigen();
                        y = shotingSettings.bulletsInBox;
                    }
                }
            }
        }
    }

    void AnimationStart()
    {
        for (int i = 0; i < shotingSettings.particleCanon.Length; i++)
        {
            foreach (Transform particleSysten in shotingSettings.particleCanon[i].transform)
            {
                ParticleSystem p = particleSysten.GetComponent<ParticleSystem>();
                
                if(p.isStopped){
                    
                    var main = p.main;
                    main.loop = true;
                    p.Play();
                   
                }
            }                
            
        }
      
    }

    void AnimationStop()
    {
        for (int i = 0; i < shotingSettings.particleCanon.Length; i++)
        {
            foreach (Transform particleSysten in shotingSettings.particleCanon[i].transform)
            {
                ParticleSystem p = particleSysten.GetComponent<ParticleSystem>();
                var main = p.main;
                main.loop = false;
                p.Stop();
                               
            }
            
        }
      
    }

}

[System.Serializable]
public struct ShotingSettings
{
    public Transform[] canhoes;
    [HideInInspector] public GameObject[,] BoxBullet;
    [HideInInspector] public Transform[] particleCanon;
    public int bulletsInBox;
    public GameObject bullet;
    [HideInInspector] public float speedBody;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMecanics
{   
    EnemyParamets parameters;

    float lastUsedTime = -Mathf.Infinity;
    
    public EnemyMecanics(EnemyParamets parameters){
        this.parameters = parameters;
    }

    public void Attack(){

        ShotBullet(parameters.rb.transform.position,20);
        
    }

    void ShotBullet(Vector3 pos, float maxDistanceReset){

        if(parameters.cxBalas.childCount == 0){
            GameObject bala = GameObject.Instantiate(parameters.bullet);
            bala.name = parameters.cxBalas.root.name + " Bullet";
            bala.GetComponent<Bullet>().gumOrigen = parameters.cxBalas;
            bala.GetComponent<Bullet>().BulletOrigen();
            parameters.BoxBullet.Add(bala);            
        }

        if(parameters.cxBalas.childCount != 0){                
            
            GameObject bullet = parameters.cxBalas.GetChild(0).gameObject;
            
            if (!bullet.activeSelf)
            {                  
                bullet.GetComponent<Bullet>().Disparate(parameters.speedBody); 
                parameters.attack = false;
                lastUsedTime = Time.time;
              
            }
        }

        bulletReturn(pos, maxDistanceReset);

    }

    void bulletReturn(Vector3 pos, float maxDistanceReset)
    {
        for (int i = 0; i < parameters.BoxBullet.Count; i++)
        {            
            GameObject bullet = parameters.BoxBullet[i];

            if (bullet.activeSelf)
            {
                if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
                {   
                    bullet.GetComponent<Bullet>().BulletOrigen();
                    i = parameters.BoxBullet.Capacity;
                }
            }
            
        }
    }
}
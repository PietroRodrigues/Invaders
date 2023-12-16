using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBossNucleo : MonoBehaviour
{
    Boss bossStatos;

    int hitLifeForNucleo = 3;

    void Start()
    {
        bossStatos = transform.root.GetComponent<Boss>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(!other.collider.isTrigger){
            if(other.gameObject.GetComponent<Bullet>() != null){
                if(other.gameObject.GetComponent<Bullet>().DanoProjetil >= 20){
                    hitLifeForNucleo--;
                    
                    if(hitLifeForNucleo <= 0){
                        bossStatos.statos.hp -= 10;
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}

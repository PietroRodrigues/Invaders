using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alvos : MonoBehaviour
{

    [SerializeField] MoveObstaculos pai;


    private void OnCollisionEnter(Collision other)
    {
        if(!other.collider.isTrigger){
            if(other.collider.CompareTag("ProjetilPlayer")){
                
                pai.defeated++;              

                if(pai.defeated >= pai.objectTargets.Count){
                    pai.lava.SetActive(false);
                    pai.plataforma.SetActive(true);
                }
                
                this.gameObject.SetActive(false);
            }
        }
    }
}

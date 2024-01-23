using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayserTrigger : MonoBehaviour
{
    Chronometry chronometry = new Chronometry();
    
    void OnTriggerStay(Collider other)
   {
        if(!other.isTrigger && other.transform.root.GetComponent<Player>() != null){
            Player player = other.transform.root.GetComponent<Player>();

            if(chronometry.CronometryPorMiles(200)){
            
            if(player.inventario.shield > 0)
                player.inventario.shield -= 5;
            else
                player.hp -= 5;
            
            chronometry.Reset();

            }
        }
   }
}

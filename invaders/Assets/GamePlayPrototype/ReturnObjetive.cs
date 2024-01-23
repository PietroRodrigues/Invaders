using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObjetive : MonoBehaviour
{
    [SerializeField] bool isBoss;
    [SerializeField] Transform trapBoss;
    [SerializeField] int danoInPlayer = 100;
    
    Player player;

    bool used = false;

    private void OnTriggerEnter(Collider other)
    {   
        if(other.transform.root.CompareTag("Player") && !other.isTrigger){
            if(player == null)
                player = other.transform.root.GetComponent<Player>();
            
            if(player.gameObject.Equals(other.transform.root.gameObject))
                used = false;

            if(!used){
                
                    player.inventario.shield = 0;
                    player.hp -= danoInPlayer;
                    
                if(isBoss){
                    player.transform.position = trapBoss.position;
                }

                used = true;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObjetive : MonoBehaviour
{
    [SerializeField] Transform inicioPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.CompareTag("Player")){
            Transform player = other.transform.root;
            player.transform.position = inicioPos.position;
        }  
    }
}

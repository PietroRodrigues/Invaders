using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
   {
        if(!other.isTrigger && other.transform.root.GetComponent<Player>() != null){
            other.transform.root.GetComponent<Player>().checkPoint = this.transform;
        }
   }
}

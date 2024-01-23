using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
   [SerializeField] GameObject porta;

   void Start()
   {
        if(porta != null)
            porta.SetActive(false);
   }

   void OnTriggerEnter(Collider other)
   {
        if(!other.isTrigger && other.transform.root.GetComponent<Player>() != null){
            other.transform.root.GetComponent<Player>().checkPoint = this.transform;
            if(porta != null){
                porta.SetActive(true);
            }
        }
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        MecanicDrop();
    }

    private void Update() {
        
        VFXDrop();
        
    }

    private void FixedUpdate() {
        FisicDrop();
    }

    void FisicDrop()
    {
            Collider[] hitCollider = Physics.OverlapSphere(new Vector3(transform.position.x,transform.position.y -0.5f,transform.position.z),0.5f);

            int hits = 0;

            for (int i = 0; i < hitCollider.Length; i++)
            {
                if(!hitCollider[i].isTrigger)
                    hits++;            
            }
            
            if( hits > 0){
                rb.Sleep();
            }else{
                rb.WakeUp();
            }

    }

   void VFXDrop()
   {

   }

   public virtual void MecanicDrop()
   {

   }

}

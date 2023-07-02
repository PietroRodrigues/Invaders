using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public enum DropsTipos {Shild , Life, Drones, Missiles, Special}

    public DropsTipos dropsTipos;

    Chronometry chronometry = new Chronometry();

    Rigidbody rb;
    public int stageTime = 60;
    [HideInInspector] public bool getDrop;
    

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        
        VFXDrop(getDrop,chronometry.CronometroPorSeg(stageTime));
        
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

   void VFXDrop(bool getDrop, bool sumir)
   {
        if(sumir){
            //roda particula de sumir o drop
        }else if(getDrop){
            //roda Particula de Pegar Drop
        }else{
            //roda particula de Idle
        }

   }

   public virtual void MecanicDrop(Collider other)
   {

        getDrop = true;
        Destroy(gameObject,1);

   }

}

using System;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public class Explosive
{    
    [HideInInspector] public GameObject meuObjeto;
    [SerializeField] public GameObject particle;
    [SerializeField] public float triggerForce = 0.5f;
    [SerializeField] public float explosionRadius = 5;
    [SerializeField] public float explosionForce = 500;

    public void Collision(Collision collision){
        if(collision.relativeVelocity.magnitude >= triggerForce){
            var surroundingObjects =  Physics.OverlapSphere(meuObjeto.transform.position, explosionForce);

            foreach (var obj in surroundingObjects)
            {   
                var rb = obj.GetComponent<Rigidbody>();
                if(rb == null) continue;
                
                if(rb.gameObject.GetComponent<Bullet>() == null)
                    rb.AddExplosionForce(explosionForce,meuObjeto.transform.position,explosionRadius);
            }

            if (particle != null)
            {
                GameObject vfx = UnityEngine.Object.Instantiate(particle);
                vfx.transform.position = meuObjeto.transform.position;
                vfx.GetComponent<VisualEffect>().Play();
            }

        }
    }

}

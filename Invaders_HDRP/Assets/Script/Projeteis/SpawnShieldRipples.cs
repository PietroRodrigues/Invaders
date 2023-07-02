using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnShieldRipples : MonoBehaviour
{ 
    [SerializeField] GameObject shieldRipples;

    private VisualEffect shieldRipplesVFX;

    private void OnCollisionEnter(Collision other)
    {   
        if(other.gameObject.GetComponent<Bullet>() != null){
            var ripples = Instantiate(shieldRipples, transform) as GameObject;
            shieldRipplesVFX = ripples.GetComponent<VisualEffect>();
            shieldRipplesVFX.SetVector3("SphereCenter",transform.position);
            Destroy(ripples,2);
        }
    }

    
}

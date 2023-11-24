using UnityEngine;

[System.Serializable]
public class Breakable
{
    ComponentsEstrutures components;
    [SerializeField] private float breakforce = 2;
    [SerializeField] private float collisionMultiplier = 100;
    [SerializeField] private float explosionRadius = 0.4f;
    [SerializeField] private bool broken;

    public Breakable(ComponentsEstrutures components){
        this.components = components;
    }

    public void Conlision(Collision collision, IEstrutura intetace, Collision other) {
        
        if(broken) return;
        
        if(collision.relativeVelocity.magnitude >= breakforce){
                
            if (components.hp > 0)
            {
                if(other.gameObject.GetComponent<Bullet>() == null)
                    components.hp -= 1;
                else
                    components.hp -= other.gameObject.GetComponent<Bullet>().DanoProjetil;

                if (components.hp < 0)
                    components.hp = 0;

                broken = intetace.AplicaModification(components.hp,components.hpMax);

                if(broken){

                    components.EsturaQuebrada.SetActive(true);
                    components.EsturaInteira.SetActive (false);
                    components.boxCollider.enabled = false;
                                        
                    var rbs = components.EsturaQuebrada.GetComponentsInChildren<Rigidbody>();
                    foreach (var rb in rbs)
                    {
                        rb.AddExplosionForce(collision.relativeVelocity.magnitude * collisionMultiplier,collision.contacts[0].point,explosionRadius);                        
                    }                    
                }
            }        
        }
    }
}

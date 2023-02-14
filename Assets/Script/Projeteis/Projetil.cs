using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{   
    public Transform gumOringem;
    public GameObject particulaColider;
    [SerializeField] float speed;
    [SerializeField] float DanoProjetil;
    Rigidbody rb;
    float speedBody;

    void Awake(){
        rb =  GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter(Collision other)
    {
        if(!other.collider.isTrigger){
            AplicaDano(other,DanoProjetil);
            //InstatiateParticle(other);
            BulletOrigen();
        }
    }

   void AplicaDano(Collision other, float DanoAplicado){

        GameObject ObjGame = other.collider.gameObject;

        if(ObjGame.tag == "Player"){
            
            float hp = ObjGame.GetComponent<Enemy>().hp;
            float Shild = ObjGame.GetComponent<Enemy>().shild;

            if(Shild > 0){
                Shild -= DanoAplicado * 2;
                if(Shild  < 0)
                Shild = 0;

            }else if(hp > 0){
                hp -= DanoAplicado;
                if(hp < 0)
                hp = 0;
            }

        }else if (ObjGame.tag == "Enemy"){

            float hp = ObjGame.GetComponent<Enemy>().hp;
            float Shild = ObjGame.GetComponent<Enemy>().shild;

            if(Shild > 0){
                Shild -= DanoAplicado * 2;
                if(Shild  < 0)
                Shild = 0;

            }else if(hp > 0){
                hp -= DanoAplicado;
                if(hp < 0)
                hp = 0;
            }
        }

    }

    void InstatiateParticle(Collision other)
    {
        GameObject particleObj = Instantiate(particulaColider);
        particleObj.transform.position = other.contacts[0].point;
        particleObj.transform.rotation = transform.rotation;
    }

    public void BulletOrigen()
    {   
        this.gameObject.SetActive(false);
        this.transform.SetParent(gumOringem);    
        transform.localPosition = new Vector3(0,0,0);
        transform.localRotation = Quaternion.Euler(0,0,0);
        
        if(rb != null)
            rb.velocity = Vector3.zero;
    }

    void OnEnable() {
        rb.velocity = this.transform.forward * (speedBody + speed);
        transform.SetParent(null);
    }

    public void Disparar(float speedBody){
        this.speedBody = speedBody;
        this.gameObject.SetActive(true); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{   
    public float distanceRadar;
    public GameObject meuDono;
    public Transform gumOrigen;
    public GameObject particulaColider;
    [SerializeField] float speed;
    [SerializeField] float speedRot;
    [SerializeField] float DanoProjetil;
    Rigidbody rb;
    float speedBody;
    
    bool jaColidio = false;

    [SerializeField] SpawnerEnemys spawnerEnemys;
    [SerializeField] GameObject alvoTermico;
    [SerializeField] List<Enemy> allEnemy;

    private void Update() {
        SetAlvoTermico(distanceRadar);

        if(rb != null)
            rb.velocity = this.transform.forward * (speedBody + speed);
        
        if(alvoTermico != null){
            
            Vector3 direction = alvoTermico.transform.position - rb.transform.position;
            Quaternion projetilRotation = Quaternion.LookRotation(direction);
            rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation,projetilRotation,speedRot * Time.deltaTime);

        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        if(!other.collider.isTrigger && !jaColidio){
            //this.GetComponentInChildren<Collider>().enabled = false;
            AplicaDano(other,DanoProjetil);
            InstatiateParticle(other);
            BulletOrigen();

        }
    }

    void AplicaDano(Collision other, float DanoAplicado){

        GameObject ObjGame = other.collider.gameObject.transform.root.gameObject;

        if(!jaColidio && ObjGame.GetComponent<Player>() != null){
            
            Player player = ObjGame.GetComponent<Player>();

            if(player.shild > 0){
                player.shild -= DanoAplicado;
                if(player.shild  < 0)
                player.shild = 0;

            }else if(player.hp > 0){
                player.hp -= DanoAplicado;
                if(player.hp < 0)
                player.hp = 0;
            }

            jaColidio = true;

        }else if (!jaColidio && ObjGame.GetComponent<Enemy>() != null){

            Enemy enemy = ObjGame.GetComponent<Enemy>();

            if(enemy.shild > 0){
                enemy.shild -= DanoAplicado;
                if(enemy.shild  < 0)
                enemy.shild = 0;

            }else if(enemy.hp > 0){
                enemy.hp -= DanoAplicado;
                if(enemy.hp < 0)
                enemy.hp = 0;
            }

            jaColidio = true;
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
        allEnemy.Clear();
        if(rb != null)
            rb.velocity = Vector3.zero;
        this.transform.SetParent(gumOrigen);
        meuDono = transform.root.gameObject;
        transform.localPosition = new Vector3(0,0,0);
        transform.localRotation = Quaternion.Euler(0,0,0);
        this.gameObject.SetActive(false);
    
    }

    void EnableBullet() {
        
        if( rb == null){
            rb =  GetComponent<Rigidbody>();
        }
        
        if( rb != null){
            transform.SetParent(null);
            gameObject.SetActive(true);
            jaColidio = false;
            //this.GetComponentInChildren<Collider>().enabled = true;
        }
    }

    void SetAlvoTermico(float DistanceRadar){
        if(alvoTermico == null){
            if(meuDono.GetComponent<Enemy>() != null){
                alvoTermico = meuDono.GetComponent<Enemy>().proprerts.target;
            }else if(meuDono.GetComponent<Player>() != null){
                foreach (Enemy enemy in allEnemy)
                {
                    if(Vector3.Distance(enemy.transform.position,transform.position) <= DistanceRadar){
                        alvoTermico = enemy.gameObject;
                    }                    
                }                
            }
        }else{
            if(Vector3.Distance(alvoTermico.transform.position,transform.position) > DistanceRadar){
                alvoTermico = null;
            }                    
        }

    }

    void AlvosPossiveis() {  
      if(meuDono.GetComponent<Player>() != null){
         if(spawnerEnemys == null)
            spawnerEnemys = FindObjectOfType<SpawnerEnemys>(); 

         allEnemy.Clear();
         
         foreach (GameObject enemy in spawnerEnemys.enemyesInScene)
         {
            if(enemy.activeSelf)
                allEnemy.Add(enemy.GetComponent<Enemy>());      
         }
         
      }
    }

    private void OnDisable() {
        allEnemy.Clear();
        alvoTermico = null;
    }

    public void Disparate(float speedBody){
      this.speedBody = speedBody;
      AlvosPossiveis();
      EnableBullet();
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projetil : MonoBehaviour
{
   public bool isProjetilPlayer;

   public float distanceRadar;
   public GameObject meuEmisor;
   public Transform gumOrigen;
   [SerializeField] Explosive explosive = new Explosive();
   [HideInInspector] public object especie;
   [HideInInspector] public List<VisualEffect> myParticulesColider = new();
   [SerializeField] float speed;
   [SerializeField] float speedRot;
   [SerializeField] float DanoProjetil;
   [SerializeField] float spread;
   Rigidbody rb;
   float speedBody;


   bool jaColidio = false;
   SpawnerEnemys spawnerEnemys;

   [SerializeField] GameObject alvoTermico;
   [SerializeField] List<Enemy> allEnemy;

   TrailRenderer trailRenderer;

   private void Awake()
   {
      explosive.meuObjeto = this.gameObject;
   }

   private void Update()
   {
      SetAlvoTermico(distanceRadar);

      if (rb != null)
         rb.velocity = this.transform.forward * (speedBody + speed);

      if (alvoTermico != null)
      {
         Vector3 direction = alvoTermico.transform.position - rb.transform.position;
         Quaternion projetilRotation = Quaternion.LookRotation(direction);
         rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, projetilRotation, speedRot * Time.deltaTime);

      }
   }

   void OnCollisionEnter(Collision other)
   {
      if (!other.collider.isTrigger && !jaColidio)
      {
         AplicaDano(other, DanoProjetil);
         explosive.Collision(other);
         BulletOrigen();
      }
   }

   void AplicaDano(Collision other, float DanoAplicado)
   {
      GameObject ObjGame = other.collider.gameObject;

      if (other.collider.gameObject.GetComponent<Bullet>() == null)
      {

         if (!jaColidio && !isProjetilPlayer)
         {
            if (ObjGame.GetComponentInParent<Player>() != null)
            {
               Player player = ObjGame.GetComponentInParent<Player>();

               if (player.inventario.shield > 0)
               {
                  player.inventario.shield -= DanoAplicado;
                  player.Ripples(rb.transform.position);

                  if (player.inventario.shield < 0)
                     player.inventario.shield = 0;

               }
               else if (player.hp > 0)
               {
                  player.hp -= DanoAplicado;
                  if (player.hp < 0)
                     player.hp = 0;
               }

               jaColidio = true;
            }

         }
         else if (!jaColidio && isProjetilPlayer)
         {
            if (ObjGame.GetComponentInParent<Enemy>() != null)
            {
               Enemy enemy = ObjGame.GetComponentInParent<Enemy>();

               if (enemy.shild > 0)
               {
                  enemy.shild -= DanoAplicado;
                  enemy.Ripples(rb.transform.position);

                  if (enemy.shild < 0)
                     enemy.shild = 0;

               }
               else if (enemy.hp > 0)
               {
                  enemy.hp -= DanoAplicado;
                  if (enemy.hp < 0)
                     enemy.hp = 0;
               }

               jaColidio = true;

            }

         }
      }
   }

   public void BulletOrigen()
   {
      allEnemy.Clear();
      if (rb != null)
         rb.velocity = Vector3.zero;
      this.transform.SetParent(gumOrigen);
      meuEmisor = transform.root.gameObject;
      transform.localPosition = new Vector3(0, 0, 0);
      Vector3 direction = gumOrigen.forward;
      direction.x += Random.Range(-spread, spread);
      direction.y += Random.Range(-spread, spread);
      Quaternion rot = Quaternion.identity;
      rot.SetLookRotation(direction);
      transform.rotation = rot;
      this.gameObject.SetActive(false);

      if (this.transform.GetComponent<TrailRenderer>() != null)
      {
         trailRenderer = GetComponent<TrailRenderer>();
         trailRenderer.Clear();
      }

   }

   void EnableBullet()
   {
      if (rb == null)
      {
         rb = GetComponent<Rigidbody>();
      }

      if (rb != null)
      {
         transform.SetParent(null);
         gameObject.SetActive(true);
         jaColidio = false;
      }
   }

   void SetAlvoTermico(float DistanceRadar)
   {
      if (alvoTermico == null)
      {
         if (meuEmisor.GetComponent<Enemy>() != null)
         {
            alvoTermico = meuEmisor.GetComponent<Enemy>().proprerts.target;
         }
         else if (meuEmisor.GetComponent<Player>() != null)
         {
            foreach (Enemy enemy in allEnemy)
            {
               if (Vector3.Distance(enemy.transform.position, transform.position) <= DistanceRadar)
               {
                  alvoTermico = enemy.gameObject;
               }
            }
         }
      }
      else
      {
         if (Vector3.Distance(alvoTermico.transform.position, transform.position) > DistanceRadar)
         {
            alvoTermico = null;
         }
      }
   }

   void AlvosPossiveis()
   {
      if (meuEmisor.GetComponent<Player>() != null)
      {
         if (spawnerEnemys == null)
            spawnerEnemys = FindObjectOfType<SpawnerEnemys>();

         allEnemy.Clear();

         foreach (GameObject enemy in spawnerEnemys.enemyesInScene)
         {
            if (enemy.activeSelf)
               allEnemy.Add(enemy.GetComponent<Enemy>());
         }

      }
   }

   private void OnDisable()
   {
      allEnemy.Clear();
      alvoTermico = null;
   }

   public void Disparate(float speedBody)
   {
      this.speedBody = speedBody;
      AlvosPossiveis();
      EnableBullet();
   }

}

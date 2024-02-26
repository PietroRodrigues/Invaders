using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy : StatosEnemyes
{
   [SerializeField] int scorePoint;

   [SerializeField] public EnemyParamets proprerts;
   [SerializeField] GameObject particleExplosion;
   [SerializeField] Transform shildParticle;

   [HideInInspector] public EnemyMoviment moviment;
   [HideInInspector] public EnemyMecanics mecanics;
   

   private void Awake()
   {
      proprerts.rb = GetComponent<Rigidbody>();
      mecanics = new EnemyMecanics();
      moviment = new EnemyMoviment();
      ResetEnemy();
   }

   private void OnEnable()
   {
      ResetEnemy();
   }

   void Update()
   {
      AutoDestruir();
      mecanics.InAlerte(proprerts);
   }

   void FixedUpdate()
   {
      moviment.NewRotation(proprerts);
      moviment.MovimentaEnemy(proprerts);
   }

   void AutoDestruir()
   {

      if (hp <= 0)
      {
         this.gameObject.SetActive(false);
         proprerts.target.GetComponent<Player>().hud.score += scorePoint;
         GameObject particleObj = Instantiate(particleExplosion);
         particleObj.transform.position = transform.position;
         particleObj.transform.rotation = transform.rotation;

      }

   }

   void ResetEnemy()
   {
      hp = hpMax;
      shild = ShildMax;
      proprerts.posDestination = Vector3.zero;
      proprerts.rb.velocity = Vector3.zero;
      proprerts.rb.angularVelocity = Vector3.zero;
   }

   public void Ripples(Vector3 posBullet){
      
      VisualEffect ripples = shildParticle.Find("ShieldRipples").GetComponent<VisualEffect>();

      Vector3 direction = posBullet - ripples.transform.position;

      Quaternion ripplesRotation = Quaternion.LookRotation(direction,Vector3.up);
      Vector3 ripplesEulerAngle = ripplesRotation.eulerAngles;

      ripples.transform.rotation = Quaternion.Euler(ripplesEulerAngle);

      ripples.Play();
            
   }

}

[System.Serializable]
public struct EnemyParamets
{
   public float speedRotation;
   public float speed;
   public Vector3 posDestination;
   public Transform waveTransform;

   [Range(0.01f, 1f)] public float raioDistance;
   public float checkRadius;
   public Player target;
   [HideInInspector] public Rigidbody rb;

   [HideInInspector] public List<GameObject> BoxBullet;
   public Transform particleCanon;
   public Transform cxBalas;
   public GameObject bullet;
   [HideInInspector] public float speedBody;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Statos
{
   [SerializeField] Transform miniMapIco;
   [SerializeField] public EnemyParamets proprerts;

   EnemyMoviment moviment;
   EnemyMecanics mecanics;
   Chronometry cronometro = new Chronometry();

   Vector3 posObjetive;

   float speedStart;

   int porcentAttck;

   private void Awake()
   {
      hp = hpMax;
      shild = ShildMax;
      proprerts.rb = GetComponent<Rigidbody>();
      mecanics = new EnemyMecanics(proprerts);
      moviment = new EnemyMoviment();
      speedStart = proprerts.speed;     
   }

   private void OnEnable() {
      porcentAttck = Random.Range(0,100);
      proprerts.target = FindObjectOfType<Player>().gameObject;
   }

   void Update()
   {
      AutoDestruir();

      miniMapIco.gameObject.SetActive(true);
      Vector3 baseV3 = transform.eulerAngles;
      baseV3.x = 90;
      baseV3.z = 0;
      miniMapIco.rotation  = Quaternion.Euler(baseV3);
      miniMapIco.transform.position = new Vector3(miniMapIco.transform.position.x,20,miniMapIco.transform.position.z);

      Color cor;
      
      if(proprerts.target != null){
         if(Vector3.Distance(transform.position,proprerts.target.transform.position) >= proprerts.distanceShoting + 2){
            if(cronometro.CronometroPorSeg(proprerts.cooldownTime)){
               if(porcentAttck <= proprerts.attakPorcent){
                  proprerts.attack = true;
               }else{
                  cronometro.Reset();
                  porcentAttck = Random.Range(0,100);
               }
            }
         }
      }else{
         cronometro.Reset();
         porcentAttck = Random.Range(0,100);
         proprerts.attack = false;
      }

      if(proprerts.target != null && proprerts.attack){
         proprerts.speed = speedStart * (speedStart/2);
         posObjetive = proprerts.target.transform.position;

         if(Vector3.Distance(transform.position,proprerts.target.transform.position) <= proprerts.distanceShoting){
            mecanics.Attack();
            cronometro.Reset();
            porcentAttck = Random.Range(0,100);
            proprerts.attack = false;
         }
         
         cor = Color.red;
      }else{
         proprerts.speed = speedStart;
         posObjetive = moviment.GeraPosObjetivo(proprerts);
         cor = Color.green;
      }

      Debug.DrawLine(transform.position,posObjetive,cor);
   }

   private void FixedUpdate() 
   {     
      moviment.MovimentaEnemy(proprerts);
      moviment.NewRotation(proprerts,posObjetive);      
   }

   void AutoDestruir(){

      if(hp <= 0){
         this.gameObject.SetActive(false);
      }

   }

   public void ResetEnemy(){
      hp = hpMax;
      shild = ShildMax;
      proprerts.rb.velocity = Vector3.zero;
      proprerts.rb.angularVelocity = Vector3.zero;
   }

}




[System.Serializable]
public struct EnemyParamets
{
   public int cooldownTime;
   public int distanceShoting;
   public int attakPorcent;
   public float speed;
   public float speedRotation;
   public float AlturaMax;
   public float AlturaMin;
   public float eixoX;
   public float eixoZ;

   [Range(5,50)]public float raioDistance;
   public float checkRadius;
   [HideInInspector] public Rigidbody rb;
   public GameObject target;
   [HideInInspector] public bool attack;

   [HideInInspector] public List<GameObject> BoxBullet;
   public Transform particleCanon;
   public Transform cxBalas;
   public GameObject bullet;
   [HideInInspector] public float speedBody;
}

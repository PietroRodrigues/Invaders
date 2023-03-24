using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Statos
{
   public GameObject target;
   [SerializeField] EnemyParamets proprerts;

   EnemyMoviment moviment;
   EnemyMecanics mecanics;
   Cronometro cronometro = new Cronometro();

   Vector3 posObjetive;

   float speedStart;

   int porcentAttck;

   private void Awake()
   {
      proprerts.rb = GetComponent<Rigidbody>();
      mecanics = new EnemyMecanics(proprerts);
      moviment = new EnemyMoviment();
      speedStart = proprerts.speed;     
   }

   private void Start() {
      porcentAttck = Random.Range(0,100);
   }

   void Update()
   {
      Color cor;
      proprerts.target = target.transform.position;
      if(Vector3.Distance(transform.position,proprerts.target) >= proprerts.distanceShoting + 2){
         if(cronometro.CronometroPorSeg(proprerts.cooldownTime)){
            if(porcentAttck <= proprerts.attakPorcent){
               proprerts.attack = true;
            }else{
               cronometro.Reset();
               porcentAttck = Random.Range(0,100);
            }
         }
      }

      if(proprerts.attack){
         proprerts.speed = speedStart * (speedStart/2);
         posObjetive = proprerts.target;

         if(Vector3.Distance(transform.position,proprerts.target) <= proprerts.distanceShoting){
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
   [HideInInspector] public Vector3 target;
   [HideInInspector] public bool attack;

   [HideInInspector] public List<GameObject> BoxBullet;
   public Transform particleCanon;
   public Transform cxBalas;
   public GameObject bullet;
   [HideInInspector] public float speedBody;
}

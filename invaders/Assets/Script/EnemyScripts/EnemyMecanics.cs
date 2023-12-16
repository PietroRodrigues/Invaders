using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMecanics
{
   Chronometry cronometro = new Chronometry();

   public bool attack;

   public void InAlerte(EnemyParamets paramets){

      if(paramets.target != null){
         
         RaycastHit hit;

         bool see = Physics.Linecast(paramets.rb.transform.position,paramets.target.transform.position,out hit,1,QueryTriggerInteraction.Ignore);
         
         bool Aggressive = Vector3.Distance(paramets.rb.transform.position,paramets.target.transform.position) < 60;
        
         Debug.DrawLine(paramets.rb.transform.position,see? hit.point : paramets.target.transform.position,Aggressive? (see? Color.blue: Color.red) : Color.green);
        

         if(!see){
            if(Aggressive){
               if(cronometro.CronometroPorSeg(Random.Range(10,41))){
                  ShotBullet(paramets.rb.transform.position, 50, paramets);
                  cronometro.Reset();
               }
            }
         }
      }
   }

   void ShotBullet(Vector3 pos, float maxDistanceReset, EnemyParamets paramets)
   {

      if (paramets.cxBalas.childCount == 0)
      {
         GameObject bala = GameObject.Instantiate(paramets.bullet);
         bala.name = paramets.cxBalas.root.name + " Bullet";
         bala.GetComponent<Bullet>().gumOrigen = paramets.cxBalas;
         bala.GetComponent<Bullet>().especie = paramets.rb.GetComponent<Enemy>();
         bala.GetComponent<Bullet>().BulletOrigen();
         paramets.BoxBullet.Add(bala);
      }

      if (paramets.cxBalas.childCount != 0)
      {

         GameObject bullet = paramets.cxBalas.GetChild(0).gameObject;

         if (!bullet.activeSelf)
         {
            bullet.GetComponent<Bullet>().Disparate(paramets.speedBody);
         }
      }

      bulletReturn(pos, maxDistanceReset, paramets);

   }

   void bulletReturn(Vector3 pos, float maxDistanceReset, EnemyParamets paramets)
   {
      for (int i = 0; i < paramets.BoxBullet.Count; i++)
      {
         GameObject bullet = paramets.BoxBullet[i];

         if (bullet.activeSelf)
         {
            if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
            {
               bullet.GetComponent<Bullet>().BulletOrigen();
               i = paramets.BoxBullet.Capacity;
            }
         }
      }
   }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMecanics
{
   EnemyParamets parameters;

   float lastUsedTime = -Mathf.Infinity;

   Chronometry cronometro = new Chronometry();

   public bool attack;

   public EnemyMecanics(EnemyParamets parameters)
   {
      this.parameters = parameters;
   }

   public void Attack()
   {   
      ShotBullet(parameters.rb.transform.position, 50);   
   }

   float ForwardCheck(Transform elementTransform, Vector3 pontVerific, int anguloLimit)
   {

      Vector3 direction = (new Vector3(pontVerific.x, elementTransform.position.y, pontVerific.z) - elementTransform.position).normalized;

      float dot = Vector3.Dot(elementTransform.forward, direction);

      float limit = Mathf.Sin(anguloLimit * Mathf.Deg2Rad);

      if (dot > limit)
      {
         return 1;
      }
      else
      {
         return 0;
      }
   }

   void ShotBullet(Vector3 pos, float maxDistanceReset)
   {

      if (parameters.cxBalas.childCount == 0)
      {
         GameObject bala = GameObject.Instantiate(parameters.bullet);
         bala.name = parameters.cxBalas.root.name + " Bullet";
         bala.GetComponent<Bullet>().gumOrigen = parameters.cxBalas;
         bala.GetComponent<Bullet>().especie = parameters.rb.GetComponent<Enemy>();
         bala.GetComponent<Bullet>().BulletOrigen();
         parameters.BoxBullet.Add(bala);
      }

      if (parameters.cxBalas.childCount != 0)
      {

         GameObject bullet = parameters.cxBalas.GetChild(0).gameObject;

         if (!bullet.activeSelf)
         {
            bullet.GetComponent<Bullet>().Disparate(parameters.speedBody);
            lastUsedTime = Time.time;

         }
      }

      bulletReturn(pos, maxDistanceReset);

   }

   void bulletReturn(Vector3 pos, float maxDistanceReset)
   {
      for (int i = 0; i < parameters.BoxBullet.Count; i++)
      {
         GameObject bullet = parameters.BoxBullet[i];

         if (bullet.activeSelf)
         {
            if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
            {
               bullet.GetComponent<Bullet>().BulletOrigen();
               i = parameters.BoxBullet.Capacity;
            }
         }

      }
   }
}
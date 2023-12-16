using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoviment
{
   Vector3 destino;

   Chronometry cronometro = new Chronometry();

   public void StartDestination(EnemyParamets parameters)
   {
      destino = NewPositionGenerator(parameters);
   }

   public void MovimentaEnemy(EnemyParamets parameters)
   {

      if (destino == Vector3.zero)
         destino = NewPositionGenerator(parameters);

      if (cronometro.CronometryPorMiles(800))
      {
         destino = NewPositionGenerator(parameters);
      }

      if (destino != Vector3.zero)
      {
         if (Vector3.Distance(parameters.rb.transform.position, parameters.waveTransform.position + destino) > parameters.raioDistance)
         {
            parameters.rb.transform.position = Vector3.Lerp(parameters.rb.transform.position, parameters.waveTransform.position + destino, parameters.speed * Time.deltaTime);
         }
         else
         {
            parameters.rb.transform.position = Vector3.MoveTowards(parameters.rb.transform.position, parameters.waveTransform.position + destino, parameters.speed * Time.deltaTime);
         }
      }
   }

   public void NewRotation(EnemyParamets parameters)
   {
      Vector3 tgLook = parameters.target.transform.position;

      Vector3 direction = tgLook - parameters.rb.transform.position;

      Quaternion enemyRotation = Quaternion.LookRotation(direction, Vector3.up);
      Vector3 enemyEulerAngle = enemyRotation.eulerAngles;

      enemyRotation = Quaternion.Euler(enemyEulerAngle);

      float speedRot = parameters.speedRotation / (Vector3.Distance(parameters.rb.transform.position, parameters.posDestination) / 4);

      parameters.rb.transform.rotation = Quaternion.Slerp(parameters.rb.transform.rotation, enemyRotation, speedRot * Time.deltaTime);

   }

   Vector3 NewPositionGenerator(EnemyParamets parameters)
   {
      return parameters.posDestination + Random.insideUnitSphere * parameters.raioDistance;

   }

}

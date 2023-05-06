using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoviment
{  
   Vector3 newPosition;
   float speed;
   Vector3 currentDirection;
   Vector3 smootMoveSpeed;

   float acceleration = 2f;

   public void MovimentaEnemy(EnemyParamets parameters){
   
      Vector3 moveAmount = Vector3.zero;

      currentDirection = parameters.rb.transform.forward * 1 + parameters.rb.transform.right * 0;
      
      speed = Mathf.Lerp(parameters.speed, parameters.speed * currentDirection.magnitude, acceleration * Time.deltaTime);      

      moveAmount = Vector3.SmoothDamp(moveAmount, currentDirection, ref smootMoveSpeed, 0.15f);
      
      parameters.rb.MovePosition((parameters.rb.position + moveAmount.normalized * (speed * Time.fixedDeltaTime)));
      
   }

   public Vector3 GeraPosObjetivo(EnemyParamets parameters){
      
     if (Vector3.Distance(parameters.rb.transform.position, newPosition) < 1)
         newPosition = NewPositionGenerator(parameters);      

      if (newPosition.y > parameters.AlturaMax || newPosition.y < parameters.AlturaMin)
         newPosition = NewPositionGenerator(parameters);

      if(newPosition.x > parameters.eixoX || newPosition.x < -parameters.eixoX)
         newPosition = NewPositionGenerator(parameters);

      if(newPosition.z > parameters.eixoZ || newPosition.z < -parameters.eixoZ)
         newPosition = NewPositionGenerator(parameters);

      if(Vector3.Distance(newPosition, parameters.target.transform.position) < (parameters.raioDistance/2))
         newPosition = NewPositionGenerator(parameters);

      parameters.rb.velocity = new Vector3(0f,0f,0f); 
      parameters.rb.angularVelocity = new Vector3(0f,0f,0f);

      return newPosition;

   }

   public void NewRotation(EnemyParamets parameters, Vector3 posDestino)
   {
      Vector3 direction = posDestino - parameters.rb.transform.position; 
      Quaternion enemyRotation = Quaternion.LookRotation(direction, Vector3.up);
      Vector3 enemyEulerAngle = enemyRotation.eulerAngles;

      enemyRotation = Quaternion.Euler(enemyEulerAngle);

      float speedRot = parameters.speedRotation / (Vector3.Distance(parameters.rb.transform.position,posDestino)/4);

      parameters.rb.transform.rotation = Quaternion.RotateTowards(parameters.rb.transform.rotation,enemyRotation,speedRot * Time.deltaTime);

   }

   Vector3 NewPositionGenerator(EnemyParamets parameters)
   {
      bool see = false;
      bool see2 = false;
      int layerMask = ~LayerMask.GetMask("Enemy");

      Vector3 randomPoint;

      do
      {
         randomPoint = parameters.target.transform.position + Random.insideUnitSphere * parameters.raioDistance;

         see = Physics.Linecast(parameters.rb.transform.position,randomPoint, out RaycastHit hit, layerMask, QueryTriggerInteraction.Ignore);

         see2 = Physics.CheckSphere(randomPoint, parameters.checkRadius, layerMask, QueryTriggerInteraction.Ignore);

      } while (see && see2);


      return parameters.posDestination;

   }

   bool Investidaverific(EnemyParamets parameters)
   {
      bool see = false;
      int layerMask = ~LayerMask.GetMask("Enemy");

      see = Physics.Linecast(parameters.rb.transform.position,parameters.target.transform.position, out RaycastHit hit, layerMask, QueryTriggerInteraction.Ignore);

      return see;

   }

}

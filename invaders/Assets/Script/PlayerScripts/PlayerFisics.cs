using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerFisics
{
   Rigidbody rb;
   Transform baseTank;
   HUD hud;
   Vector3 currentDirection;
   public float speed;
   float acceleration = 10;
   float deceleration = 5;
   Vector3 smootMoveSpeed;

   Vector3 movePos;
   Vector3 moveDirection;
   float DistanceForce;
   Quaternion moveRot;
   Quaternion rot;
   float dashCooldown = 2f;
   float dashTimer = 0f;
   bool dash;

   float motorInput;
   float breakInput;
   float steeringInput;

   bool isGrounded;

   public float speedRotation;

   float distanciaRaio;

   bool rotacionar;

   Quaternion previousDesiredRotation; 
   float smoothFactor = 5f;

   VisualEffect poeira;

   public PlayerFisics(Rigidbody rb, Transform baseTank, HUD hud,VisualEffect poeira)
   {
      this.rb = rb;
      this.hud = hud;
      this.baseTank = baseTank;

      rot = rb.transform.rotation;

      this.poeira = poeira;

   }

   public void MoverAWSD(float x, float z, float speedMax, float distRaycast, float floatingHeight, float dashImpulse)
   {
      Vector3 moveAmount = Vector3.zero;

      if (dashCooldown > 0f)
      {
         dashCooldown -= Time.deltaTime;
      }

      if (dashTimer > 0f)
      {
         dashTimer -= Time.deltaTime;
      }

      if (x != 0 || z != 0)
      {
         currentDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward * z + Camera.main.transform.right * x, Vector3.up);

         if (speed <= speedMax)
         {
            speed = Mathf.Lerp(speed, speedMax * currentDirection.magnitude, acceleration * Time.deltaTime);
         }

      }
      else
      {
         speed = Mathf.Lerp(speed, 0, deceleration * Time.deltaTime);
         currentDirection = Vector3.zero;
      }

      moveAmount = Vector3.SmoothDamp(moveAmount, currentDirection, ref smootMoveSpeed, 0.15f);

      movePos = rb.position + moveAmount.normalized * (speed * Time.fixedDeltaTime);
      moveDirection = moveAmount.normalized;

      AlinharComSuperficie(x, z, currentDirection, distRaycast, floatingHeight);
      moveRot = Quaternion.RotateTowards(moveRot, PlayerRotation(x, z), speedRotation * Time.deltaTime);

   }

   public void Dash(float x, float z,bool inputDash){
      if(x != 0 || z != 0){
         if(inputDash && dashCooldown <= 0){
            dash = true;
            dashCooldown = 1;    
         }
      }
   }


   void AlinharComSuperficie(float moveX, float moveZ, Vector3 currentDirection, float distRaycast, float floatingHeight)
   {
      RaycastHit hit;
      Quaternion currentRotation = rb.transform.rotation;
      rot = currentRotation;
      Quaternion poeiraRot = poeira.transform.localRotation;

      bool see = Physics.Raycast(rb.transform.position, -rb.transform.up, out hit, distRaycast);      

      if (see)
      {
         Vector3 incomingVec = hit.point - rb.transform.position;

         Vector3 adjustedMoveDirection = Vector3.RotateTowards(rb.transform.up, currentDirection, Mathf.Deg2Rad * 10, 0);

         poeira.transform.position = hit.point;
         poeiraRot = Quaternion.FromToRotation(poeira.transform.up, hit.normal);

         Quaternion desiredRotation = Quaternion.FromToRotation(rb.transform.up, Vector3.up) * Quaternion.FromToRotation(rb.transform.up, adjustedMoveDirection) * rb.transform.rotation;

         desiredRotation = SmoothRotation(desiredRotation);
         rot = SmoothRotation(rot);

         if (moveX == 0 && moveZ == 0)
         {
            previousDesiredRotation = rot;
         }

         Vector3 limitPropulsor = new Vector3(1, hit.point.y + floatingHeight, 0);

         DistanceForce = Vector3.Distance(limitPropulsor, new Vector3(1, rb.transform.position.y,0));

         if (rb.transform.position.y > limitPropulsor.y)
         {
            DistanceForce = 0;
         }

      }

      poeira.transform.rotation = poeiraRot;

      if(DistanceForce > .7f){
         poeira.Play();
      }else{
         poeira.Stop();
      }


   }

   Quaternion SmoothRotation(Quaternion targetRotation)
   {
      targetRotation = Quaternion.Slerp(previousDesiredRotation, targetRotation, Time.deltaTime * smoothFactor);
      previousDesiredRotation = targetRotation;
      return targetRotation;
   }

   Quaternion PlayerRotation(float x, float z)
   {
      float camY = Camera.main.transform.eulerAngles.y;
      Quaternion dirRot = rb.transform.rotation;

      dirRot = Quaternion.Slerp(dirRot, Quaternion.Euler(0, camY, 0), Time.deltaTime * ((x != 0 || z != 0) ? 8f : 3f));

      Vector3 dirEuler = dirRot.eulerAngles;
      dirRot = Quaternion.Euler(dirEuler);

      return dirRot;
   }

   public void AplicaMovemento(float powerPropulsor,float dashForce)
   {
      if(!dash)
         rb.AddForce(moveDirection * (speed * (rb.mass)),ForceMode.Force);
      
      if(dash){
         rb.AddForce(moveDirection * (dashForce * (rb.mass)),ForceMode.Impulse);
         dash = false;
      }
      
      rb.AddForce(Vector3.up * (DistanceForce * (powerPropulsor * (rb.mass))), ForceMode.Force);


      rb.MoveRotation(Quaternion.Euler(rot.eulerAngles.x, moveRot.eulerAngles.y, rot.eulerAngles.z));
      
   }

}

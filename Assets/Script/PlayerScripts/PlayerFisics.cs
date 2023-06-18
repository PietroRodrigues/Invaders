using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

   public float SpeedRotation;

   float distanciaRaio;

   bool rotacionar;

   public PlayerFisics(Rigidbody rb, Transform baseTank, HUD hud)
   {
      this.rb = rb;
      this.hud = hud;
      this.baseTank = baseTank;

      rot = rb.transform.rotation;

   }

   public void MoverAWSD(float x, float z, float speedMax, float distRaycast, float floatingHeight, float dashForce)
   {
      Vector3 moveAmount = Vector3.zero;

      if(dashCooldown > 0f){
         dashCooldown -= Time.deltaTime;
      }

      if (dashTimer > 0f)
      {
         dashTimer -= Time.deltaTime;
         
      }else if(dashTimer <= 0){
         dash = false;
      }

      if (x != 0 || z != 0)
      {
         currentDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward * z + Camera.main.transform.right * x, Vector3.up);
         
         if(speed <= speedMax){
            speed = Mathf.Lerp(speed, speedMax * currentDirection.magnitude, acceleration * Time.deltaTime);
         }else{
           speed = Mathf.Lerp(speed, 0, deceleration * Time.deltaTime);
         }
        
      }
      else
      {
         speed = Mathf.Lerp(speed, 0, deceleration * Time.deltaTime);
      }

      moveAmount = Vector3.SmoothDamp(moveAmount, currentDirection, ref smootMoveSpeed, 0.15f);

      movePos = (rb.position + moveAmount.normalized * (speed * Time.fixedDeltaTime));

      moveDirection = new Vector3(movePos.x - rb.transform.position.x, 0f, movePos.z - rb.transform.position.z).normalized;

      AlinharComSuperficie(distRaycast, floatingHeight);
      moveRot = PlayerRotation(x, z, rot);

   }

   public void Dash(float x, float z,bool inputDash){
      if(x != 0 || z != 0){
         if(inputDash && dashCooldown <= 0){         
            if (dashTimer <= 0f)
            {
               dash = true;
               dashTimer = 0.2f;
            }
            dashCooldown = 1;    
         }
      }
   }

   void AlinharComSuperficie(float distRaycast, float floatingHeight)
   {
      RaycastHit hit;
      Quaternion currentRotation = rb.transform.rotation;
      rot = currentRotation;

      bool see = Physics.Raycast(rb.transform.position, -rb.transform.up, out hit, distRaycast);

      if (see)
      {
         Vector3 incomingVec = hit.point - rb.transform.position;
         Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

         Quaternion desiredRotation = Quaternion.FromToRotation(rb.transform.up, reflectVec) * rb.transform.rotation;
         
         if (Quaternion.Angle(currentRotation, desiredRotation) > 10f)
         {
            rot = Quaternion.Slerp(rot, desiredRotation, Time.deltaTime * 2f);

         }

         Vector3 limitPorpulsor = new Vector3(1, hit.point.y + floatingHeight,0);

         DistanceForce = Vector3.Distance(limitPorpulsor,new Vector3(1,rb.transform.position.y,0));
         
         if(rb.transform.position.y > limitPorpulsor.y)
            DistanceForce = 0;
       
      }

   }

   Quaternion PlayerRotation(float x, float z, Quaternion rotAlinhada)
   {

      float camY = Camera.main.transform.eulerAngles.y;
      float currentRotation = rb.transform.eulerAngles.y;
      Quaternion curentRotatin = rb.transform.rotation;
      Quaternion dirRot = curentRotatin;

      Vector3 dirTarget = currentDirection - rb.transform.position;

      dirRot = Quaternion.Slerp(dirRot, Quaternion.Euler(0, camY + rb.transform.rotation.y, 0), Time.deltaTime * ((x != 0 || z != 0) ? 8f : 3f ));
      
      Vector3 dirEuler = dirRot.eulerAngles;
      dirRot = Quaternion.Euler(dirEuler);

      return dirRot;

   }

   public void AplicaMovemento(float powerPropulsor,float dashForce)
   {
      rb.AddForce(moveDirection * (speed * (rb.mass)),ForceMode.Force);
      
      rb.AddForce(Vector3.up * (DistanceForce * (powerPropulsor * (rb.mass))), ForceMode.Force);

      if(dash){
         rb.AddForce(moveDirection * (dashForce * (rb.mass)),ForceMode.Impulse);
      }

      rb.MoveRotation(Quaternion.Euler(rot.eulerAngles.x, moveRot.eulerAngles.y, rot.eulerAngles.z));
      
   }

}

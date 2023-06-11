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
   float acceleration = 0.5f;
   float deceleration = 1f;
   Vector3 smootMoveSpeed;

   bool dirCima = true;
   float heightAtual = 1;
   Vector3 movePos;
   Vector3 pos;
   Quaternion moveRot;
   Quaternion rot;


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

      pos = rb.transform.position;
      rot = rb.transform.rotation;

   }

   public void MoverAWSD(float x, float z, float speedMax, float distRaycast, float floatingHeight)
   {
      Vector3 moveAmount = Vector3.zero;

      if (x != 0 || z != 0)
      {
         currentDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward * z + Camera.main.transform.right * x, Vector3.up);
         speed = Mathf.Lerp(speed, speedMax * currentDirection.magnitude, acceleration * Time.deltaTime);
      }
      else
      {
         speed = Mathf.Lerp(speed, 0, deceleration * Time.deltaTime);
      }

      moveAmount = Vector3.SmoothDamp(moveAmount, currentDirection, ref smootMoveSpeed, 0.15f);

      movePos = (rb.position + moveAmount.normalized * (speed * Time.fixedDeltaTime));

      AlinharComSuperficie(distRaycast, floatingHeight);
      moveRot = PlayerRotation(x, z, rot);

   }

   void AlinharComSuperficie(float distRaycast, float floatingHeight)
   {

      RaycastHit hit;
      pos = rb.transform.position;
      Quaternion currentRotation = rb.transform.rotation;
      rot = currentRotation;

      if (dirCima && heightAtual >= 0.8f)
      {
         dirCima = false;

      }else if (!dirCima && heightAtual <= 0.6f)
      {
         dirCima = true;
      }

      heightAtual = Mathf.MoveTowards(heightAtual, dirCima ? 0.8f : 0.6f, Time.deltaTime * 0.1f);


      bool see = Physics.Raycast(rb.transform.position, -rb.transform.up, out hit, distRaycast);

      if (see)
      {
         Vector3 incomingVec = hit.point - rb.transform.position;
         Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

         Quaternion desiredRotation = Quaternion.FromToRotation(rb.transform.up, reflectVec) * rb.transform.rotation;
         
         if (Quaternion.Angle(currentRotation, desiredRotation) > 10f)
         {
            rot = Quaternion.Slerp(rot, desiredRotation, Time.deltaTime * 5f);

         }

         pos = new Vector3(0, hit.point.y + heightAtual, 0);

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

   public void AplicaMovemento()
   {

      rb.MoveRotation(Quaternion.Euler(rot.eulerAngles.x, moveRot.eulerAngles.y, rot.eulerAngles.z));
      rb.MovePosition(new Vector3(movePos.x, pos.y, movePos.z));

   }

}

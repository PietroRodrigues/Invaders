using System.Net.Http.Headers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UIElements;

public class PlayerFisics
{
   Rigidbody rb;
   public Vector3 currentDirection;
   public float speed;
   float acceleration = 10;
   float deceleration = 5;
   Vector3 smootMoveSpeed;

   Vector3 moveDirection;
   float DistanceForce;
   Quaternion moveRot;
   Quaternion rot;
   float dashCooldown;
   bool dash;
   public bool recuo;

   bool jump;
   bool inGround;
   float jumpCooldown = 2f;

   public float speedRotation;

   Quaternion previousDesiredRotation; 
   float smoothFactor = 5f;
   BoxCollider boxCorpo;

   VisualEffect poeira;
   VisualEffect poeiraJump;
   Dictionary<string,VisualEffect> EffectsVFX = new Dictionary<string, VisualEffect>();

   public PlayerFisics(Rigidbody rb,BoxCollider boxCorpo,VisualEffect poeira,  Transform EffectsVFX)
   {
      this.rb = rb;
      rot = rb.transform.rotation;
      this.poeira = poeira;
      poeiraJump = poeira.transform.GetChild(0).GetComponent<VisualEffect>();
      this.boxCorpo = boxCorpo;

      foreach (Transform effects in EffectsVFX)
      {
         this.EffectsVFX.Add(effects.gameObject.name,effects.GetComponent<VisualEffect>());
      }

   }

   public void MoverAWSD(float x, float z, float speedMax)
   {
      Vector3 moveAmount = Vector3.zero;

      if (dashCooldown > 0f)
      {
         dashCooldown -= Time.deltaTime;
      }

      if(jumpCooldown > 0f){

         jumpCooldown -= Time.deltaTime;

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

      moveDirection = moveAmount.normalized;

   }

   public void Jump(bool JumpImput){
      
      if(inGround && jumpCooldown <= 0){
         if(JumpImput){
            jump = true;
            EffectsVFX["Jump"].Play();
            jumpCooldown = 1f;
         }
      }

      if(jumpCooldown <= 0){
         EffectsVFX["Jump"].Stop();
      }      
   }

   public void Propulsor(float distRaycast, float floatingHeight)
   {
      RaycastHit hitBox;
      RaycastHit hitRay;
      rot = rb.transform.rotation;      

      float height = (currentDirection != Vector3.zero)? floatingHeight + 0.2f : floatingHeight;        

      bool seeBox = Physics.BoxCast(rb.transform.position, new Vector3(boxCorpo.size.x, .1f,boxCorpo.size.z) / 2, -Vector3.up, out hitBox, Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0), distRaycast,1,QueryTriggerInteraction.Ignore);

      bool seeRay = Physics.Raycast(rb.transform.position, -Vector3.up, out hitRay, distRaycast,1,QueryTriggerInteraction.Ignore);

      inGround = seeBox || seeRay;

      if(seeRay){

         Vector3 poeiraPos = new Vector3(rb.transform.position.x, hitRay.point.y,rb.transform.position.z);

         Quaternion poeiraRot = Quaternion.FromToRotation(Vector3.up, hitRay.normal);
         
         poeira.transform.position = poeiraPos;         
         poeira.transform.rotation = poeiraRot;
      
      }              

      if (seeBox)
      {
         Vector3 limitPropulsor = new Vector3(0, hitBox.point.y + height, 0);

         DistanceForce = Vector3.Distance(new Vector3(0, rb.transform.position.y,0),limitPropulsor);
         
      }
         
      if(inGround){
         poeira.Play();
      }else{
         poeira.Stop();
      }
   }

   public void RotationDirection(){

      moveRot = Quaternion.RotateTowards(moveRot, PlayerRotation(rb.linearVelocity.normalized.x, rb.linearVelocity.normalized.z), speedRotation * Time.deltaTime);
      
      Vector3 adjustedRotationDirection = Vector3.RotateTowards(rb.transform.up, currentDirection, Mathf.Deg2Rad * 12, 0);      

      Quaternion desiredRotation = Quaternion.FromToRotation(rb.transform.up, Vector3.up) * Quaternion.FromToRotation(rb.transform.up, adjustedRotationDirection) * rb.transform.rotation;

      rot = SmoothRotation(desiredRotation);

   }   

   public void Dash(float x, float z, bool inputDash){
      if(currentDirection != Vector3.zero){
         if(inputDash && dashCooldown <= 0){
            dash = true;

            if(x > 0)
               EffectsVFX["Right"].Play();
            else if(x < 0)
               EffectsVFX["Left"].Play();;
            
            if(z > 0)
               EffectsVFX["Back"].Play();
            else if(z < 0)
               EffectsVFX["Front"].Play();

            dashCooldown = 1;
         }
      }

      if(dashCooldown <= 0){
         EffectsVFX["Right"].Stop();          
         EffectsVFX["Left"].Stop();        
         EffectsVFX["Back"].Stop();    
         EffectsVFX["Front"].Stop();
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

   public void AplicaMovemento(float dashForce,float jumpForce,float recuoForce,Vector3 recuoDir)
   {
      if(!dash)
         rb.AddForce(moveDirection * (speed * rb.mass),ForceMode.Force);
      
      if(dash){
         rb.AddForce(moveDirection * (dashForce * rb.mass),ForceMode.Impulse);
         dash = false;
      }

      if(recuo){
         rb.AddForce(recuoDir * (recuoForce * rb.mass),ForceMode.Impulse);
         recuo = false;
      }

      if(jump){
         rb.AddForce(rb.transform.up *(jumpForce * rb.mass), ForceMode.Impulse);
         poeiraJump.Play();
         jump = false;
      }
      
      rb.MoveRotation(Quaternion.Euler(rot.eulerAngles.x, moveRot.eulerAngles.y, rot.eulerAngles.z));
      
   }

   public void ImpactForceReaction(Vector3 hitNormal,float impactForce){
      rb.AddForce(hitNormal * (impactForce * rb.mass),ForceMode.Impulse);
   }

   public void AplicaFlutuadores(float powerPropulsor){

      Vector3 force = inGround? Vector3.up * (DistanceForce * powerPropulsor * rb.mass): -Vector3.up * (DistanceForce * (powerPropulsor * .5f) * rb.mass);

      rb.AddForce(force,ForceMode.Force);
      
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : Statos
{
   [SerializeField] Rigidbody rb;
   [SerializeField] HUD hud;
   [SerializeField] Transform shieldParticle;
   [SerializeField] VisualEffect poeira;
   [SerializeField] Transform dashEffects;

   [HideInInspector] public PlayerControler playerControler;
   [HideInInspector] public Inventario inventario;
   PlayerFisics playerFisics;
   Shotting shotting;
   PlayerAnimation playerAnimation;
   [SerializeField] PartsTank partsTank;

   [SerializeField] float speedMax;
   [SerializeField] float dashForce;
   [SerializeField] float jumpForce;
   [SerializeField] float recuoForce;
   [SerializeField] float powerPropulsion;
   [Range(1, 360)][SerializeField] float speedRotation = 1f;   

   [Range(1, 360)][SerializeField] float speedCanonAim = 1f;
   [SerializeField] float minCanonX = 0;
   [SerializeField] float maxCanonX = 1;
  
   [Range(10, 100)][SerializeField] float maxDistanceBullets;

   [SerializeField] float distRaycast;
   [SerializeField] float floatingHeight;

   [SerializeField] ShottingSettings shotingSettings;
   
   [HideInInspector] public Drone drone;
   [SerializeField] DroneSettingsShot droneSettingsShot;
   [SerializeField] DroneStatos droneStatos;

   [HideInInspector] public Vector3 alvoPos;

   void Awake()
   {
      hp = hpMax;
      inventario = new Inventario(100);
      playerControler = new PlayerControler();
      playerFisics = new PlayerFisics(rb,partsTank.cabine.GetComponent<BoxCollider>(),poeira,dashEffects);
      shotting = new Shotting(shotingSettings);
      playerAnimation = new PlayerAnimation(partsTank);
      drone = new Drone(transform,droneStatos,droneSettingsShot);

   }


   void Update()
   {
      playerAnimation.speedCanonAim = speedCanonAim;
      playerFisics.speedRotation = speedRotation;

      playerControler.GameInputs();

      playerFisics.MoverAWSD(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput,speedMax);

      playerFisics.Propulsor(distRaycast,floatingHeight);

      playerFisics.Jump(playerControler.inputsControl.jumpInput);

      playerFisics.Dash(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput,playerControler.inputsControl.Dash);

      playerFisics.RotationDirection();

      AutoDestroyer();

   }

   void FixedUpdate()
   {
      //bool lookMissel = shotting.LockingMissile(playerControler.inputsControl.mirar);
      shotting.MissileShotting(playerControler.inputsControl.disparar, 0, transform.position, maxDistanceBullets,playerFisics);

      playerFisics.AplicaMovemento(dashForce,jumpForce,recuoForce,-partsTank.canon.forward);
      playerFisics.AplicaFlutuadores(powerPropulsion);
      

      drone.MovementDrones();
      drone.LookDroneTarget(alvoPos,100);
      drone.FireDrone(playerControler.inputsControl.disparar, maxDistanceBullets);

   }

   private void LateUpdate()
   {
      playerAnimation.AimCanon(hud.hud_Aim.MouseAimPos,minCanonX, maxCanonX);
   }

   void AutoDestroyer()
   {
      if (hp <= 0)
      {
         this.gameObject.SetActive(false);
      }
   }

   public void Ripples(Vector3 posBullet){
      
      VisualEffect ripples = shieldParticle.Find("ShieldRipples").GetComponent<VisualEffect>();

      Vector3 direction = posBullet - ripples.transform.position;

      Quaternion ripplesRotation = Quaternion.LookRotation(direction,Vector3.up);
      Vector3 ripplesEulerAngle = ripplesRotation.eulerAngles;

      ripples.transform.rotation = Quaternion.Euler(ripplesEulerAngle);

      ripples.Play();

   }

   public void ShieldCharger(){
      
      VisualEffect shield = shieldParticle.Find("Shield").GetComponent<VisualEffect>();
      
      shield.Play();

   }

   private void OnCollisionEnter(Collision other)
   {
      if (!other.collider.isTrigger)
      {
         Vector3 forcaImpact = other.relativeVelocity;

         float magnitudeImpact = forcaImpact.magnitude; 
         
         playerFisics.ImpactForceReaction(other.contacts[0].normal,1.2f + magnitudeImpact/4);
      }
   }

}

[System.Serializable]
public struct Inventario{

   public bool droneActive;
   
   [HideInInspector] public float shield;
   public float ShieldMax;

  public Inventario (float shieldMax){
      this.shield = 0;
      this.ShieldMax = shieldMax;
      this.droneActive = false;
  }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : Statos
{
   [SerializeField] Rigidbody rb;
   [SerializeField] public HUD hud;
   [SerializeField] Transform shieldParticle;
   [SerializeField] VisualEffect poeira;
   [SerializeField] public VisualEffect explosionPlayer;
   [SerializeField] Transform dashEffects;

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

   [SerializeField] public DroneStatos droneStatos;
   Drone drone;

   [HideInInspector] public Vector3 alvoPos;

   [SerializeField] public Transform checkPoint;

   [HideInInspector] public CamControler camControler;

   bool travaReset;

   void Awake()
   {
      hp = hpMax;
      bodies = bodiesMax;
      hud = FindFirstObjectByType<HUD>();
      camControler = FindFirstObjectByType<CamControler>();
      inventario = new Inventario(100);
      playerFisics = new PlayerFisics(rb,partsTank.cabine.GetComponent<BoxCollider>(),poeira,dashEffects);
      shotting = new Shotting(shotingSettings,camControler.mouseSensitivity);
      playerAnimation = new PlayerAnimation(partsTank);
      drone = new Drone();

   }


   void Update()
   {
      playerAnimation.speedCanonAim = speedCanonAim;
      playerFisics.speedRotation = speedRotation;

      shotting.Aim(camControler.playerInputs.inputsControl.Aim, camControler);

      playerFisics.MoverAWSD(camControler.playerInputs.inputsControl.xInput, camControler.playerInputs.inputsControl.zInput,camControler.playerInputs.inputsControl.Aim ? speedMax / 2 : speedMax);

      playerFisics.Propulsor(distRaycast,floatingHeight);

      playerFisics.Jump(camControler.playerInputs.inputsControl.jumpInput);

      playerFisics.Dash(camControler.playerInputs.inputsControl.xInput, camControler.playerInputs.inputsControl.zInput,camControler.playerInputs.inputsControl.Dash);

      playerFisics.RotationDirection();

      AutoDestroyer();

   }


   void FixedUpdate()
   {
      //bool lookMissel = shotting.LockingMissile(playerControler.inputsControl.mirar);
      shotting.MissileShotting(camControler.playerInputs.inputsControl.disparar, 0, transform.position, maxDistanceBullets,playerFisics);

      playerFisics.AplicaMovemento(dashForce,jumpForce,recuoForce,-partsTank.canon.forward);
      playerFisics.AplicaFlutuadores(powerPropulsion);

      drone.MovementDrones(ref droneStatos);
      
      drone.LookDroneTarget(alvoPos,hud.hud_Aim.MiraVeiculo,100,droneStatos);
   }

   private void LateUpdate()
   {        
      playerAnimation.AimCanon(hud.hud_Aim.MouseAimPos,minCanonX, maxCanonX);
   }

   void OnEnable()
   {
      explosionPlayer.transform.SetParent(this.transform);
      explosionPlayer.transform.localPosition = Vector3.zero;
      explosionPlayer.Stop();
   }

   void AutoDestroyer()
   {
      if (hp <= 0)
      {
         explosionPlayer.transform.SetParent(null);
         explosionPlayer.Play();
         droneStatos.droneSettingsShot.ammon = 0;
         droneStatos.active = false;
         droneStatos.drone.transform.SetParent(droneStatos.droneBeg);
         droneStatos.drone.transform.localPosition = droneStatos.posDrone;
         inventario.shield = 0;
         bodies--;
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
   
   [HideInInspector] public float shield;
   public float ShieldMax;

  public Inventario (float shieldMax){
      this.shield = 0;
      this.ShieldMax = shieldMax;
      
  }
   
}

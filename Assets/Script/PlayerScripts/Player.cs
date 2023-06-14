using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : Statos
{
   [SerializeField] Rigidbody rb;
   [SerializeField] HUD hud;
   [SerializeField] CamControler camControler;
   [SerializeField] Transform shildParticle;

   [HideInInspector] public PlayerControler playerControler;
   PlayerFisics playerFisics;
   Shotting shoting;
   PlayerAnimation playerAnimation;
   [SerializeField] PartsTank partsTank;
   
   [SerializeField] ShottingSettings shotingSettings;

   [SerializeField] float speedMax;
   [SerializeField] float dashForce;
   [SerializeField] float powerPropulsor;
   [Range(1, 360)][SerializeField] float speedRotation = 1f;
   

   [Range(1, 360)][SerializeField] float speedCanonAim = 1f;
   [SerializeField] float minCanonX = 0;
   [SerializeField] float maxCanonX = 1;
  
   [Range(10, 100)][SerializeField] float maxDistanceBullets;

   [SerializeField] float distRaycast;
   [SerializeField] float floatingHeight;

   Vector3 grondCheckPos;
   

   private void Awake()
   {
      hp = hpMax;
      shild = ShildMax;
      playerControler = new PlayerControler();
      playerFisics = new PlayerFisics(rb, partsTank.cabine, hud);
      shoting = new Shotting(shotingSettings);
      playerAnimation = new PlayerAnimation(partsTank);
   }

   void Update()
   {
      playerAnimation.speedCanonAim = speedCanonAim;
      playerFisics.SpeedRotation = speedRotation;

      playerControler.GameInputs();

      playerFisics.MoverAWSD(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput,speedMax,distRaycast,floatingHeight,dashForce);

      playerFisics.Dash(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput,playerControler.inputsControl.jumpInput);

      AutoDestruir();

   }

   void FixedUpdate()
   {
      bool lookMissel = shoting.LockingMissile(playerControler.inputsControl.mirar);

      if(!playerControler.inputsControl.mirar)
         shoting.GunShotting(playerControler.inputsControl.disparar, 0, transform.position, maxDistanceBullets);
      
      if(playerControler.inputsControl.mirar)
         shoting.MissileShotting(playerControler.inputsControl.disparar, 0, transform.position, maxDistanceBullets);

      playerFisics.AplicaMovemento(powerPropulsor,dashForce);

   }

   private void LateUpdate()
   {
      playerAnimation.AimCanon(hud.hudComponentes.MouseAimPos,minCanonX, maxCanonX);
   }

   void AutoDestruir()
   {
      if (hp <= 0)
      {
         this.gameObject.SetActive(false);
      }
   }

   public void Ripples(Vector3 posBullet){
      
      VisualEffect ripples = shildParticle.Find("ShieldRipples").GetComponent<VisualEffect>();

      Vector3 direction = posBullet - ripples.transform.position;

      Quaternion ripplesRotation = Quaternion.LookRotation(direction,Vector3.up);
      Vector3 ripplesEulerAngle = ripplesRotation.eulerAngles;

      ripples.transform.rotation = Quaternion.Euler(ripplesEulerAngle);

      ripples.Play();

   }

}

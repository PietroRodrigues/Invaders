using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : Statos
{
   [SerializeField] Rigidbody rb;
   [SerializeField] HUD hud;
   [SerializeField] CamControler camControler;
   [SerializeField] Transform shieldParticle;
   [SerializeField] Transform drones;
   [SerializeField] VisualEffect poeira;

   [HideInInspector] public PlayerControler playerControler;
   [HideInInspector] public Inventario inventario;
   PlayerFisics playerFisics;
   Shotting shotting;
   PlayerAnimation playerAnimation;
   Drones dronesSetting;
   [SerializeField] PartsTank partsTank;

   [SerializeField] float speedMax;
   [SerializeField] float dashForce;
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
   [SerializeField] ShottingDroneSettings[] shottingDroneSettings;
   
   void Awake()
   {
      hp = hpMax;
      inventario = new Inventario(100);
      playerControler = new PlayerControler();
      playerFisics = new PlayerFisics(rb, partsTank.cabine, hud,poeira);
      shotting = new Shotting(shotingSettings);
      playerAnimation = new PlayerAnimation(partsTank);
      dronesSetting = new Drones(drones,rb,shottingDroneSettings);
   }


   void Update()
   {
      playerAnimation.speedCanonAim = speedCanonAim;
      playerFisics.speedRotation = speedRotation;

      playerControler.GameInputs();

      playerFisics.MoverAWSD(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput,speedMax,distRaycast,floatingHeight,dashForce);

      playerFisics.Dash(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput,playerControler.inputsControl.jumpInput);

      AutoDestroyer();
   }

   void FixedUpdate()
   {
      //bool lookMissel = shotting.LockingMissile(playerControler.inputsControl.mirar);
      shotting.MissileShotting(playerControler.inputsControl.disparar, 0, transform.position, maxDistanceBullets,playerFisics);

      dronesSetting.Fire(playerControler.inputsControl.disparar, maxDistanceBullets);

      playerFisics.AplicaMovemento(powerPropulsion,dashForce,recuoForce,-partsTank.canon.forward);

      dronesSetting.MovementDrones(hud.hud_Aim.MouseAimPos,100,inventario);

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

}

[System.Serializable]
public struct Inventario{
   
   [HideInInspector]public int drones;
   [HideInInspector] public float shield;
   public float ShieldMax;

  public Inventario (float shieldMax){
      this.drones = 0;
      this.shield = 0;
      this.ShieldMax = shieldMax;
  }
   
}

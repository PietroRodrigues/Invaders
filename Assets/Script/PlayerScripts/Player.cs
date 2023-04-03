using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Statos
{
   [SerializeField] Rigidbody rb;
   [SerializeField] HUD hud;
   [SerializeField] CamControler camControler;
   [SerializeField] float DistanciaRaioPropulsor;
   [SerializeField] float fatorAmplification;
   [SerializeField] float speedMax;

   [HideInInspector] public PlayerControler playerControler;
   PlayerFisics playerFisics;
   Shoting shoting;
   PlayerAnimation playerAnimation;

   [Range(1, 360)][SerializeField] float speedCanonAim = 1f;
   [Range(1, 360)][SerializeField] float speedRotation = 1f;
   [Range(1, 360)][SerializeField] int anguloDePropulsores = 1;
   [SerializeField] PartsTank partsTank;
   [SerializeField] ShotingSettings shotingSettings;
   [Range(10, 100)][SerializeField] float maxDistanceBullets;

   [SerializeField] Transform[] pointsRaycast;

   Vector3 grondCheckPos;

   private void Awake()
   {
      hp = hpMax;
      shild = ShildMax;
      playerControler = new PlayerControler();
      playerFisics = new PlayerFisics(rb, partsTank.cabine, hud);
      shoting = new Shoting(shotingSettings);
      playerAnimation = new PlayerAnimation(partsTank);
   }

   void Update()
   {
      playerAnimation.speedCanonAim = speedCanonAim;
      playerFisics.SpeedRotation = speedRotation;
      playerAnimation.anguloDePropulsores = anguloDePropulsores;

      playerControler.GameInputs();
      AutoDestruir();

   }

   void FixedUpdate()
   {
      shoting.CanonShoting(playerControler.inputsControl.disparar, playerFisics.speed, transform.position, maxDistanceBullets);

      playerFisics.MoverAWSD(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput, DistanciaRaioPropulsor, fatorAmplification, pointsRaycast,speedMax);

   }

   private void LateUpdate()
   {
      playerAnimation.AimCanon(hud.hudComponentes.MouseAimPos);
      playerAnimation.PropulsoresControler(playerControler.inputsControl.xInput, playerControler.inputsControl.zInput, playerFisics.ditectionRotation);

   }

   void AutoDestruir(){
      if(hp <= 0){
        this.gameObject.SetActive(false);
      }
   }

}

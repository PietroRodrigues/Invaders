using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Statos
{

    [SerializeField] Rigidbody rb;
    [SerializeField] HUD hud;
    [SerializeField] CamControler camControler;

    PlayerControler playerControler;
    PlayerFisics playerFisics;
    Shoting shoting;
    PlayerAnimation playerAnimation;

    [Range(1,360)] [SerializeField] float speedCanonAim = 1f;
    [Range(1,360)] [SerializeField] float speedRotation = 1f;
    [Range(1,360)] [SerializeField] int anguloDePropulsores = 1;
    [SerializeField] PartsTank partsTank;
    [SerializeField] ShotingSettings shotingSettings;
    [Range(10,100)] [SerializeField] float maxDistanceBullets;

    private void Awake() {
        playerControler = new PlayerControler();
        playerFisics = new PlayerFisics(rb,partsTank.cabine,hud);
        shoting = new Shoting(shotingSettings);
        playerAnimation = new PlayerAnimation(partsTank);
    }

    void Update()
    {
        playerAnimation.speedCanonAim = speedCanonAim;
        playerFisics.SpeedRotation = speedRotation;
        playerAnimation.anguloDePropulsores = anguloDePropulsores;

        playerControler.CharacterInputs();
        
    }

    void FixedUpdate() 
    {   
        shoting.CanonShoting(playerControler.inputsControl.disparar,playerFisics.speed,transform.position,maxDistanceBullets);

        playerFisics.MoverCharacterAWSD(playerControler.inputsControl.xInput,playerControler.inputsControl.zInput,speedMax);
    }

    private void LateUpdate() {

        playerAnimation.AimCanon(hud.hudComponentes.MouseAimPos);
        playerAnimation.PropulsoresControler(playerControler.inputsControl.xInput,playerControler.inputsControl.zInput,playerFisics.ditectionRotation);

    }
}

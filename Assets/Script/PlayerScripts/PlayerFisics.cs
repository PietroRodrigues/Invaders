using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFisics
{
    Rigidbody rb;
    Transform baseTank;
    HUD hud;
    public float speed;
    float smootSpeed;
    Vector3 smootMoveSpeed;
    Vector3 moveAmount;
    Vector3 currentDirection;
    Vector3 oldDirection;

    float acceleration = 2f;
    float deceleration = 2f;

    public float SpeedRotation;
    public float ditectionRotation;

    bool rotacionar;

    public PlayerFisics(Rigidbody rb, Transform baseTank, HUD hud){

        this.rb = rb;
        this.hud = hud;
        this.baseTank = baseTank;

    }


    public void MoverCharacterAWSD(float x, float z,float speedMax){
        
        if(x != 0 || z != 0){
            currentDirection = baseTank.forward * z + baseTank.right * x;
            speed = Mathf.Lerp(speed, speedMax * currentDirection.magnitude, acceleration * Time.deltaTime);
            
        }else{
            speed = Mathf.Lerp(speed, 0 , deceleration * Time.deltaTime);
        }

        moveAmount = Vector3.SmoothDamp(moveAmount, currentDirection, ref smootMoveSpeed, 0.15f);

        ditectionRotation = PlayerRotation(x,z);

        rb.MovePosition((rb.position + moveAmount.normalized * (speed * Time.fixedDeltaTime)));

    }

    float PlayerRotation(float x,float z){

        float camY =  Camera.main.transform.eulerAngles.y;
        float currentRotation = baseTank.rotation.eulerAngles.y;
        float turnDirection = 0;

        if(x != 0 || z != 0){
            baseTank.rotation = Quaternion.RotateTowards(baseTank.rotation, Quaternion.Euler(0, camY + baseTank.rotation.y, 0),SpeedRotation * Time.deltaTime);
        }else{

            if(ForwardCheck(baseTank.transform,hud.hudComponentes.MouseAimPos,40) == 0){
                rotacionar = true;
            }

            if(rotacionar){
                baseTank.rotation = Quaternion.RotateTowards(baseTank.rotation, Quaternion.Euler(0, camY + baseTank.rotation.y, 0),SpeedRotation * Time.deltaTime);

                float desiredRotation = camY + baseTank.rotation.y;
                turnDirection = Mathf.Sign(desiredRotation - currentRotation);
            
                if(Quaternion.Angle(baseTank.rotation,Quaternion.Euler(0, camY + baseTank.rotation.y, 0)) < 2){
                    rotacionar = false;                       
                }
            }

        }

        return turnDirection;

    }

    float ForwardCheck(Transform elementTransform, Vector3 pontVerific, int anguloLimit){

        Vector3 direction = (new Vector3(pontVerific.x,elementTransform.position.y,pontVerific.z) - elementTransform.position).normalized;

        float dot = Vector3.Dot(elementTransform.forward, direction);

        float limit = Mathf.Sin(anguloLimit * Mathf.Deg2Rad);

        if(dot > limit){
            return 1;           
        }else{
            return 0; 
        }
    }
}

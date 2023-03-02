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

    public float SpeedRotation;
    public float ditectionRotation;

    float distanciaRaio;

    bool rotacionar;

    public PlayerFisics(Rigidbody rb, Transform baseTank, HUD hud){

        this.rb = rb;
        this.hud = hud;
        this.baseTank = baseTank;

    }

    public void MoverAWSD(float x,float z,float distanciaRaio,float fatorAmplification,Transform[] posicoesRaio){

        ditectionRotation = PlayerRotation(x,z);
        Propussor(distanciaRaio,fatorAmplification,posicoesRaio);

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

    void Propussor(float distanciaRaio,float fatorAmplification,Transform[] posicoesRaio){

        float aceleracaoGravidade = rb.mass * Mathf.Abs(Physics.gravity.magnitude);

        Vector3 forcaPropulsor = Vector3.zero;

        foreach (Transform posicaoRaio in posicoesRaio) {
            
            Ray raio = new Ray(posicaoRaio.position, -posicaoRaio.up);
            RaycastHit hit;
            
            bool see = Physics.Raycast(raio, out hit, distanciaRaio, 1, QueryTriggerInteraction.Ignore);

            Vector3 limitRaio = raio.origin + raio.direction * distanciaRaio;
            float distancia = Vector3.Distance(rb.transform.position,(see)? hit.point :  limitRaio); 
            float distanciaNormalizada = Mathf.InverseLerp(0, distanciaRaio, distancia);
            float fatorForca = Mathf.Lerp(2, 1, distanciaNormalizada);
            forcaPropulsor += posicaoRaio.up * aceleracaoGravidade * (fatorForca / posicoesRaio.Length) * fatorAmplification;
            
            Debug.DrawLine(posicaoRaio.position,(see)? hit.point: limitRaio, Color.red);      
        }

        rb.AddForce(forcaPropulsor,ForceMode.Force); 

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

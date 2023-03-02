using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation
{

    PartsTank parts;
    public float speedCanonAim;
    public int anguloDePropulsores;

    public PlayerAnimation(PartsTank parts){
        
        this.parts = parts;
    }

    public void AimCanon(Vector3 target){

        Vector3 canonDirection = target - parts.canon.position;

        Quaternion canonRotation = Quaternion.LookRotation(canonDirection, parts.cabine.up);
        Vector3 canonEuler = canonRotation.eulerAngles;
       
        if(target.y + 10 < parts.canon.position.y)
            canonEuler.x = parts.canon.rotation.eulerAngles.x;

        canonRotation = Quaternion.Euler(canonEuler);        

        parts.canon.rotation = Quaternion.RotateTowards(parts.canon.rotation, canonRotation, speedCanonAim * Time.deltaTime);
        
    }

    public void PropulsoresControler(float x, float z, float turnDirection){

        Quaternion Front_Left = PropulsorRot(x,z);
        Quaternion Front_Right = PropulsorRot(x,z);
        Quaternion Back_Left = PropulsorRot(x,z);
        Quaternion Back_Right = PropulsorRot(x,z);

        if(turnDirection > 0){
            //Direita
            Front_Left = PropulsorRot(x,1);
            Front_Right = PropulsorRot(x,-1);
            Back_Left = PropulsorRot(x,1);
            Back_Right = PropulsorRot(x,-1);
        }
        
        if(turnDirection < 0){
            //Esquerda
            Front_Left = PropulsorRot(x,-1);
            Front_Right = PropulsorRot(x,1);
            Back_Left = PropulsorRot(x,-1);
            Back_Right = PropulsorRot(x,1);
        }

        parts.propulsorFront_Left.localRotation = Quaternion.RotateTowards(parts.propulsorFront_Left.localRotation,Front_Left, 100 * Time.deltaTime);
        
        parts.propulsorFront_Right.localRotation = Quaternion.RotateTowards(parts.propulsorFront_Right.localRotation,Front_Right, 100 * Time.deltaTime);
        
        parts.propulsorBack_Left.localRotation = Quaternion.RotateTowards(parts.propulsorBack_Left.localRotation,Back_Left, 100 * Time.deltaTime);
        
        parts.propulsorBack_Right.localRotation = Quaternion.RotateTowards(parts.propulsorBack_Right.localRotation,Back_Right, 100 * Time.deltaTime);

    }
    
    Quaternion PropulsorRot(float x, float z){
        
        return Quaternion.Euler(new Vector3(z * anguloDePropulsores,0,-x * anguloDePropulsores));

    }
}

[System.Serializable]
public struct PartsTank
{
    public Transform beseTower;
    public Transform canon;

    public Transform cabine;

    public Transform propulsorFront_Right;
    public Transform propulsorFront_Left;
    public Transform propulsorBack_Right;
    public Transform propulsorBack_Left;    
}

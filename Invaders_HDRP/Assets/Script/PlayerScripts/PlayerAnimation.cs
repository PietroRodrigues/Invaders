using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation
{

    PartsTank parts;
    public float speedCanonAim;

    private Quaternion torreRelativeRotation;
    private Quaternion canonRelativeRotation;

    public PlayerAnimation(PartsTank parts){        
        this.parts = parts;
    }

    public void AimCanon(Vector3 target , float mixCanonAngle, float maxCanonAngle){
        
        // acha a rotação bruta para o alvo
        Quaternion lookAtTorre = Quaternion.LookRotation(target - parts.beseTower.position,parts.cabine.root.up);
        
        Quaternion lookAtCanon = Quaternion.LookRotation(target - parts.canon.position, parts.canon.up);

        // acha a rotação relativa e faz uma transição suavel da rotação
        torreRelativeRotation = Quaternion.Euler(parts.cabine.root.eulerAngles - lookAtTorre.eulerAngles);
        
        canonRelativeRotation = Quaternion.Euler(parts.canon.eulerAngles - lookAtCanon.eulerAngles);

        // rotação final dos componentes 
        Vector3 torreEuler = parts.cabine.root.eulerAngles - torreRelativeRotation.eulerAngles;
        Quaternion torreRot = Quaternion.Euler(torreEuler);
        
        Vector3 canonEuler = parts.canon.eulerAngles - canonRelativeRotation.eulerAngles;
        Quaternion canonRot = Quaternion.Euler(canonEuler);

        // aplica a rotação 
        // **CANHÃO DEVE SER SEMPRE ANTES DA TORRE**
        parts.canon.rotation = Quaternion.RotateTowards(parts.canon.rotation,canonRot,(speedCanonAim * 2)  * Time.deltaTime);
        parts.beseTower.rotation = Quaternion.RotateTowards(parts.canon.rotation,torreRot,speedCanonAim * Time.deltaTime);

        // --- angulo maximo e angulo minimo de rotação do canhão --- //
        float max = 360 - maxCanonAngle;
        float min = mixCanonAngle;
        float currentAngle = parts.canon.localEulerAngles.x;

        if(currentAngle > 180){
            if(currentAngle < max) parts.canon.localEulerAngles = new Vector3(max,0,0);
        }else{
            if(currentAngle > min) parts.canon.localEulerAngles = new Vector3(min,0,0);
        }

        // bloqueia a rotação em certos eixos
        Vector3 towerRotation = parts.beseTower.localEulerAngles;
        Vector3 canonRotation = parts.canon.localEulerAngles;

        parts.beseTower.localRotation = Quaternion.Euler(0,towerRotation.y,0);
        parts.canon.localRotation = Quaternion.Euler(canonRotation.x,0,0);        
        
    }
    
}

[System.Serializable]
public struct PartsTank
{
    public Transform beseTower;
    public Transform canon;

    public Transform cabine;
}

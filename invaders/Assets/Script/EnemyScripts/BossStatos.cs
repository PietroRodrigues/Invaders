using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatos : MonoBehaviour
{
    public bool bossActive;

    [SerializeField] StatosBaseBoss statos;
    [SerializeField] bool baseMecanicActive = true;
    [SerializeField] bool baseMovimentActive = true;

    public BossMovimet bossMovimet;
    public BossMecanics bossMecanics;

    private void Awake() {
        
        if(baseMovimentActive)
            bossMovimet = new BossMovimet();

        if(baseMecanicActive)
            bossMecanics = new BossMecanics();
        
    }

    public virtual void DiferencialMecanic(){
        Base();
    }

    void Base(){
        
        if(baseMecanicActive)
            BaseMecanic();

        if(baseMovimentActive)
            BaseMovement();

    }

    void BaseMovement(){
        
    }

    void BaseMecanic(){

    }
}

[System.Serializable]
public struct StatosBaseBoss{
    
    public float hp;
    public float hpMax;
    public float shild;
    public float shildMax;

    public float speedMove;
    public float speedRot;

    public GameObject target;

}

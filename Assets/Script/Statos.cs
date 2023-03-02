using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statos : MonoBehaviour
{
    public enum Tipo {Player,EnemyRed,EnemyGreen,EnemyBlue,EnemyGray}

    public Tipo tipo = new Tipo();

    [HideInInspector] public float hp;
    [HideInInspector] public float shild;
    [HideInInspector] public bool isGrounded;
    
    public float hpMax;
    public float ShildMax;    

    private void Awake() {
        hp = hpMax;
    }

}

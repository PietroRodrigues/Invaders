using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statos : MonoBehaviour
{
    [HideInInspector] public float hp;
    public float hpMax;
    [HideInInspector] public float shild;
    public float ShildMax;
    public float speedMax;
    [HideInInspector] public bool isGrounded;

    private void Awake() {
        hp = hpMax;
    }

}

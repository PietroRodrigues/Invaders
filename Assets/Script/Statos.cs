using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statos : MonoBehaviour
{
   public enum Tipo { Player, EnemyRed, EnemyGreen, EnemyMagenta, EnemyYellow, EnemyGray }

   public Tipo tipo = new Tipo();

   [HideInInspector] public float hp;
   [HideInInspector] public float shild;

   public float hpMax;
   public float ShildMax;

}

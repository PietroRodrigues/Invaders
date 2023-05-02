using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlocationStage
{

   Transform[] wavePos;
   Vector2 raioMinMaxDistance;
   Vector2 alturaMinMax;

   [HideInInspector] public bool[,,,,] GradeMap = new bool[8,8,8,8,8];
   Vector3[,,,,] Grade = new Vector3[8,8,8,8,8]; 

   public AlocationStage(Transform[] wavePos, Vector2 raioMinMaxDistance, Vector2 alturaMinMax)
   {
      this.wavePos = wavePos;
      this.raioMinMaxDistance = raioMinMaxDistance;
      this.alturaMinMax = alturaMinMax;
   }

   public void Alocar(List<GameObject> enemyInScene){
     
   }
}

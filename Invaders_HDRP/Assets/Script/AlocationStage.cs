using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlocationStage
{
   Vector2 raioMinMaxDistance;
   Vector2 alturaMinMax;

   MapEnemy[,] mapEnemy = new MapEnemy[6, 5];

   MapGridFase mapGridFase;

   public AlocationStage(MapGridFase mapGridFase, Vector2 raioMinMaxDistance, Vector2 alturaMinMax)
   {
      this.raioMinMaxDistance = raioMinMaxDistance;
      this.alturaMinMax = alturaMinMax;

      this.mapGridFase = mapGridFase;

      mapGridFase.ChargedLoad();

   }

   public void Alocar(List<GameObject> enemyInScene, Transform wavePos, int waveFormationIndc, SettingsLocationArea settings)
   {
      Vector3 centerWave = wavePos.position;
      centerWave -= wavePos.right * -settings.horizontalSize / 2;
      centerWave -= wavePos.forward * -settings.verticalSize / 2;

      float espacoH = -settings.horizontalSize / (mapEnemy.GetLength(0) - 1);
      float espacoV = -settings.verticalSize / (mapEnemy.GetLength(1) - 1);

      int indcEnemy = 0;

      if (enemyInScene.Count > 0)
      {

         for (int i = 0; i < mapEnemy.GetLength(0); i++)
         {
            for (int j = 0; j < mapEnemy.GetLength(1); j++)
            {
               Vector3 pos = centerWave + wavePos.right * i * espacoH + wavePos.forward * j * espacoV;

               mapEnemy[i, j].autorization = mapGridFase.GetFormationPos(waveFormationIndc, i, j);
               mapEnemy[i, j].pos = pos;

               if (mapEnemy[i, j].autorization)
               {

                  Enemy enemy = enemyInScene[indcEnemy].GetComponent<Enemy>();

                  enemy.proprerts.posDestination = mapEnemy[i, j].pos;
                  enemy.proprerts.waveTransform = wavePos;
                  indcEnemy++;

                  if (indcEnemy >= enemyInScene.Count)
                  {
                     indcEnemy = enemyInScene.Count - 1;
                     if (indcEnemy < 0)
                        indcEnemy = 0;
                  }
               }
            }
         }
      }
   }

   public void Draw(Transform wavePos, int waveFormationIndc, SettingsLocationArea settings)
   {
      if(mapGridFase != null){
         Vector3 centerWave = wavePos.position;
         centerWave -= wavePos.right * -settings.horizontalSize / 2;
         centerWave -= wavePos.forward * -settings.verticalSize / 2;

         float espacoH = -settings.horizontalSize / (mapEnemy.GetLength(0) - 1);
         float espacoV = -settings.verticalSize / (mapEnemy.GetLength(1) - 1);

         for (int i = 0; i < mapEnemy.GetLength(0); i++)
         {
            for (int j = 0; j < mapEnemy.GetLength(1); j++)
            {
               Vector3 pos = centerWave + wavePos.right * i * espacoH + wavePos.forward * j * espacoV;

               mapEnemy[i, j].autorization = mapGridFase.GetFormationPos(waveFormationIndc, i, j);
               mapEnemy[i, j].pos = pos;

               if (mapEnemy[i, j].autorization)
               {
                  Gizmos.color = Color.blue;
               }
               else
               {
                  Gizmos.color = Color.red;
               }

               Gizmos.DrawWireSphere(pos, 1f);
            }
         }

      }
   }
}

[System.Serializable]
public struct MapEnemy
{
   public bool autorization;
   public Vector3 pos;
}


[System.Serializable]
public struct SettingsLocationArea
{
   [HideInInspector] public int waveIndc;

   public float waveSpeedRotation;
   public int anguloVision;

   public float minDistance;
   public float maxDistance;

   public float verticalSize;
   public float horizontalSize;
}
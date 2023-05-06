using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Designer;
using System.Linq;

public class AlocationStage
{
   int indcWavePos = 0;

   Transform[] wavePos;
   Vector2 raioMinMaxDistance;
   Vector2 alturaMinMax;

   MapEnemy[,] mapEnemy = new MapEnemy[8,5];

   public AlocationStage(Transform[] wavePos, Vector2 raioMinMaxDistance, Vector2 alturaMinMax)
   {
      this.wavePos = wavePos;
      this.raioMinMaxDistance = raioMinMaxDistance;
      this.alturaMinMax = alturaMinMax;
   }

   public void Alocar(List<GameObject> enemyInScene, MapGridFase mapGridFase,SettingsLocationArea settings){

      float espacoH = settings.horizontalSize / (mapEnemy.GetLength(0) - 1);
      float espacoV = settings.verticalSize / (mapEnemy.GetLength(1) - 1);

      Vector3 centerWave = wavePos[indcWavePos].position;
      centerWave -= wavePos[indcWavePos].right * settings.horizontalSize / 2;
      centerWave -= wavePos[indcWavePos].up * settings.verticalSize / 2;
      
      int indcEnemy = 0;
      
      //Debug.Log(waveDesigner[indcWavePos]);

      for (int i = 0; i < mapEnemy.GetLength(0); i++)
      {
         for (int j = 0; j < mapEnemy.GetLength(1); j++)
         {
            Vector3 pos = centerWave + wavePos[indcWavePos].right * i * espacoH + wavePos[indcWavePos].up * j * espacoV;
            
            mapEnemy[i,j].autorization =  mapGridFase.GetWavePos(indcWavePos,i,j);
            mapEnemy[i,j].pos = pos;
            
            if(mapEnemy[i,j].autorization){       
               
               if(mapEnemy[i,j].enemySeted == null){
                  mapEnemy[i,j].enemySeted = enemyInScene[indcEnemy];
                  mapEnemy[i,j].enemySeted.GetComponent<Enemy>().proprerts.posDestination = mapEnemy[i,j].pos;
                  indcEnemy++;

                  if(indcEnemy >= enemyInScene.Count)
                  indcEnemy = enemyInScene.Count - 1;
               }
            }
            
         }            
      }     
   }

   public void Draw(SettingsLocationArea settings,MapGridFase mapGridFase){

      float espacoH = settings.horizontalSize / (mapEnemy.GetLength(0) - 1);
      float espacoV = settings.verticalSize / (mapEnemy.GetLength(1) - 1);

      Vector3 centerWave = wavePos[indcWavePos].position;
      centerWave -= wavePos[indcWavePos].right * settings.horizontalSize / 2;
      centerWave -= wavePos[indcWavePos].up * settings.verticalSize / 2;

      for (int i = 0; i < mapEnemy.GetLength(0); i++)
      {
         for (int j = 0; j < mapEnemy.GetLength(1); j++)
         {
            Vector3 pos = centerWave + wavePos[indcWavePos].right * i * espacoH + wavePos[indcWavePos].up * j * espacoV;
            
            mapEnemy[i,j].autorization = mapGridFase.GetWavePos(indcWavePos,i,j);
            mapEnemy[i,j].pos = pos;

            if(mapEnemy[i,j].autorization){
               Gizmos.color = Color.blue;
            }else{
               Gizmos.color = Color.red;
            }
            
            Gizmos.DrawWireSphere(pos,1f);
         }
      }     
   }

}

[System.Serializable]
public struct MapEnemy
{
   public GameObject enemySeted;
   public bool autorization;
   public Vector3 pos;
}


[System.Serializable]
public struct SettingsLocationArea
{
   public float verticalSize;
   public float horizontalSize;
}
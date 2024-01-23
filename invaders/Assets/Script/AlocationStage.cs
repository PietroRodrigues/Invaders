using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AlocationStage : MonoBehaviour
{

   public List<GameObject> enemyesInScene;
   [SerializeField] GameObject[] enemysFaze;
   [SerializeField] public WaveStage[] waves;

   public int enemysActive;

   [SerializeField] Player player;
   [SerializeField] public Boss boss;

   MapEnemy[,] mapEnemy = new MapEnemy[6, 5];

   [SerializeField] MapGridFase mapGridFase;

   int waveFormationIndc = 0;

   [SerializeField] Transform wavePos;

   [SerializeField] public SettingsLocationArea settings;

   Chronometry cronometro;

   private void Awake()
   {
      cronometro = new Chronometry();

      mapGridFase.ChargedLoad();

      waveFormationIndc = Random.Range(0, mapGridFase.GetFormationsList().Count);

      foreach (WaveStage wave in waves)
      {
         for (int i = 0; i < wave.enemyInWave.Count; i++)
         {
            wave.enemyInWave[i].enemy = EnemyLocationWave(wave.enemyInWave[i].tipo);
         }
      }
   }

   private void FixedUpdate()
   {
      enemysActive = ActiveEnemysInScene(enemyesInScene);

      if (cronometro.CronometroPorSeg(enemysActive))
      {
         enemyesInScene = enemyesInScene.OrderBy(x => Random.value).ToList();
         waveFormationIndc = Random.Range(0, mapGridFase.GetFormationsList().Count);
         cronometro.Reset();
      }

      Alocar(enemyesInScene, wavePos, waveFormationIndc, settings);

      MoveWave();
   }

   int ActiveEnemysInScene(List<GameObject> enemyesInScene)
   {

      int actives = 0;

      foreach (GameObject enemy in enemyesInScene)
      {
         if (enemy.activeSelf)
            actives++;
      }

      return actives;
   }

   GameObject EnemyLocationWave(StatosEnemyes.Tipo tipo)
   {
      GameObject enemy = null;

      foreach (GameObject enemysPrefabs in enemysFaze)
      {
         if (enemysPrefabs.GetComponent<Enemy>().tipo == tipo)
         {
            enemy = enemysPrefabs;
         }
      }

      return enemy;
   }

   void MoveWave(){

      if (Vector3.Distance(wavePos.position, new Vector3(wavePos.parent.transform.position.x, wavePos.position.y, wavePos.parent.transform.position.z)) > 80)
      {
         wavePos.position = new Vector3(wavePos.parent.transform.position.x, wavePos.position.y, wavePos.parent.transform.position.z);
         wavePos.transform.LookAt(new Vector3(Camera.main.transform.position.x, wavePos.transform.position.y, Camera.main.transform.position.z), Vector3.up);
      }

      float distance = Vector3.Distance(new Vector3(player.transform.position.x, wavePos.transform.position.y, player.transform.position.z), wavePos.transform.position);

      Vector3 direction = new Vector3(player.transform.position.x, wavePos.transform.position.y, player.transform.position.z) - wavePos.transform.position;

      Quaternion waveRot = Quaternion.LookRotation(direction, Vector3.up);
      Vector3 waveRotAngle = waveRot.eulerAngles;
      waveRot = Quaternion.Euler(waveRotAngle);

      float speedRotate;

      if (ForwardCheck(wavePos, player.transform.position, settings.anguloVision) == 0)
      {
         speedRotate = settings.waveSpeedRotation;
      }
      else
      {
         speedRotate = settings.waveSpeedRotation / 10;
      }

      wavePos.rotation = Quaternion.RotateTowards(wavePos.rotation, waveRot, speedRotate * Time.deltaTime);

      if (distance < settings.minDistance)
      {

         Vector3 cameraPosition = Camera.main.transform.position;
         Quaternion cameraRotation = Camera.main.transform.rotation;

         Vector3 behindPlayer = cameraPosition - (cameraRotation * Vector3.forward * 30f);

         wavePos.transform.position = new Vector3(behindPlayer.x, wavePos.transform.position.y, behindPlayer.z);

         wavePos.transform.LookAt(new Vector3(cameraPosition.x, wavePos.transform.position.y, cameraPosition.z), Vector3.up);

      }
      else if (distance > settings.maxDistance)
      {
         wavePos.transform.position += direction.normalized * 15 * Time.deltaTime;
      }

   }

   public void Alocar(List<GameObject> enemyInScene, Transform wavePos, int waveFormationIndc, SettingsLocationArea settings)
   {
      Vector3 centerWave = wavePos.position;
      centerWave -= wavePos.right * -settings.horizontalSize / 2;
      centerWave -= (wavePos.forward + (wavePos.up/1.5f)) * -settings.verticalSize / 2;

      float espacoH = -settings.horizontalSize / (mapEnemy.GetLength(0) - 1);
      float espacoV = -settings.verticalSize / (mapEnemy.GetLength(1) - 1);

      int indcEnemy = 0;

      if (enemyInScene.Count > 0)
      {

         for (int i = 0; i < mapEnemy.GetLength(0); i++)
         {
            for (int j = 0; j < mapEnemy.GetLength(1); j++)
            {
               Vector3 pos = centerWave + wavePos.right * i * espacoH + (wavePos.forward + (wavePos.up/1.5f)) * j * espacoV;

               mapEnemy[i, j].autorization = mapGridFase.GetFormationPos(waveFormationIndc, i, j);
               mapEnemy[i, j].pos = pos;

               if (mapEnemy[i, j].autorization)
               {

                  Enemy enemy = enemyInScene[indcEnemy].GetComponent<Enemy>();

                  enemy.proprerts.posDestination = mapEnemy[i, j].pos;
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

   float ForwardCheck(Transform elementTransform, Vector3 pontVerific, int anguloLimit)
   {

      Vector3 direction = (new Vector3(pontVerific.x, elementTransform.position.y, pontVerific.z) - elementTransform.position).normalized;

      float dot = Vector3.Dot(elementTransform.forward, direction);

      float limit = Mathf.Sin(anguloLimit * Mathf.Deg2Rad);

      if (dot > limit)
      {
         return 1;
      }
      else
      {
         return 0;
      }
   }

   private void OnDrawGizmos()
   {
      Draw(wavePos, waveFormationIndc, settings);
   }

   public void Draw(Transform wavePos, int waveFormationIndc, SettingsLocationArea settings)
   {
      if(mapGridFase != null && wavePos != null){
         Vector3 centerWave = wavePos.position;
         centerWave -= wavePos.right * -settings.horizontalSize / 2;
         centerWave -= (wavePos.forward + (wavePos.up/1.5f)) * -settings.verticalSize / 2;

         if (mapEnemy != null && mapEnemy.GetLength(0) > 0 && mapEnemy.GetLength(1) > 0){
            float espacoH = -settings.horizontalSize / (mapEnemy.GetLength(0) - 1);
            float espacoV = -settings.verticalSize / (mapEnemy.GetLength(1) - 1);

            for (int i = 0; i < mapEnemy.GetLength(0); i++)
            {
               for (int j = 0; j < mapEnemy.GetLength(1); j++)
               {
                  Vector3 pos = centerWave + wavePos.right * i * espacoH + (wavePos.forward +(wavePos.up/1.5f)) * j * espacoV;

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
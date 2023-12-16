using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerEnemys : MonoBehaviour
{

   [SerializeField] AlocationStage alocationStage;
   List<GameObject> enemyesCemiterio;

   [SerializeField] int radiusSpawnPont;
   [SerializeField] Transform spawnerBossPoint;
   [SerializeField] Transform[] spawnerPoints;

   //AlocationStage alocationStage;

   [SerializeField] float delayBetweenEnemies;

   [SerializeField] Vector2 raioMinMaxDistance;
   [SerializeField] Vector2 alturaMinMax;
   //[SerializeField] int deleyTentativesAttack = 1;

   Chronometry cronometro = new Chronometry();


   private void Update()
   {
      if (alocationStage.enemyesInScene.Count == 0)
      {
         if (!alocationStage.boss.bossActive)
         {
            StartCoroutine(SpawnEnemies());
         }
         
      }else {
         
         int enemysDeath = 0;

         for (int i = 0; i < alocationStage.enemyesInScene.Count; i++)
         {
            if (!alocationStage.enemyesInScene[i].activeSelf)
            {
               enemysDeath++;
            }
         }

         if (alocationStage.enemyesInScene.Count == enemysDeath)
         {
            foreach (GameObject enemy in alocationStage.enemyesInScene)
            {
               enemyesCemiterio.Add(enemy);
            }

            if (!alocationStage.boss.bossActive)
            {
               if (alocationStage.waves.Length - 1 > alocationStage.settings.waveIndc)
                  alocationStage.settings.waveIndc++;
               else{
                  alocationStage.boss.bossActive = true;
               }
            }

            alocationStage.enemyesInScene.Clear();
         }
      }
   }

   IEnumerator SpawnEnemies()
   {
      if (alocationStage != null && alocationStage.waves != null){
         
         foreach (EnemyInfo enemyInfo in alocationStage.waves[alocationStage.settings.waveIndc].enemyInWave)
         {
            GameObject enemy = null;

            if (enemyesCemiterio != null && spawnerPoints != null && spawnerPoints.Length > 0){

               List<GameObject> objectsToRemove = new List<GameObject>();

               for (int j = 0; j < enemyesCemiterio.Count; j++)
               {
                  if (enemyesCemiterio[j].GetComponent<Enemy>().tipo == enemyInfo.tipo)
                  {
                     enemy = enemyesCemiterio[j];
                     enemy.name = "Enemy";
                     objectsToRemove.Add(enemyesCemiterio[j]);
                     Vector3 randomDirection = Random.insideUnitSphere;
                     Vector3 randomPosition = randomDirection * radiusSpawnPont;
                     enemy.transform.position = spawnerPoints[Random.Range(0, spawnerPoints.Length)].position + randomPosition;
                     enemy.SetActive(true);
                     //enemy.GetComponent<Enemy>().ResetEnemy();
                     j = enemyesCemiterio.Count;
                  }
               }

               foreach (var obj in objectsToRemove)
               {
                  enemyesCemiterio.Remove(obj);
               }

               if (enemy == null)
               {
                  if (enemyInfo.enemy != null){
                     enemy = Instantiate(enemyInfo.enemy);
                     //enemy.GetComponent<Enemy>().proprerts.alocationStage = alocationStage;
                     enemy.name = "Enemy";
                     Vector3 randomDirection = Random.insideUnitSphere;
                     Vector3 randomPosition = randomDirection * radiusSpawnPont;
                     enemy.transform.position = spawnerPoints[Random.Range(0, spawnerPoints.Length)].position + randomPosition;
                  }
               }

               alocationStage.enemyesInScene.Add(enemy);

               yield return new WaitForSeconds(delayBetweenEnemies);
            }
         }
      }
   }

   void FixedUpdate()
   {
      //WaveAttack(deleyTentativesAttack, enemysActive, enemyesInScene);

   }

   // void WaveAttack(int deleytentatives, int enemysActive, List<GameObject> enemyesInScene)
   // {
   //    if (cronometro.CronometryPorMiles(deleytentatives))
   //    {
   //       List<GameObject> activeEnemis = new List<GameObject>();

   //       foreach (GameObject enemy in enemyesInScene)
   //       {
   //          if (enemy.activeSelf)
   //             activeEnemis.Add(enemy);
   //       }

   //       if (activeEnemis.Count > 0)
   //       {
   //          Enemy enemySelected = activeEnemis[Random.Range(0, activeEnemis.Count)].GetComponent<Enemy>();

   //          float porcentagenTiro = (((float)enemyesInScene.Count - (float)enemysActive) / (float)enemyesInScene.Count) * 100;
   //          porcentagenTiro = (porcentagenTiro < 30) ? 30 : porcentagenTiro;
   //          float randonNumber = Random.Range(1f, 100f);

   //          if (randonNumber <= porcentagenTiro)
   //          {
   //             enemySelected.mecanics.Attack();
   //          }
   //       }

   //       cronometro.Reset();
   //    }
   // }
}
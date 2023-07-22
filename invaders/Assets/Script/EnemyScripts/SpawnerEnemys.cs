using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerEnemys : MonoBehaviour
{
   [SerializeField] MapGridFase mapGridFase;

   public List<GameObject> enemyesInScene;   
   public int enemysActive;
   List<GameObject> enemyesCemiterio = new List<GameObject>();

   [SerializeField] Boss boss;
   [SerializeField] GameObject[] enemysFaze;

   [SerializeField] int radiusSpawnPont;
   [SerializeField] Transform spawnerBossPoint;
   [SerializeField] Transform[] spawnerPoints;
   [SerializeField] public Wave[] waves;

   AlocationStage alocationStage;

   [SerializeField] float delayBetweenEnemies;
   [SerializeField] Transform wavePos;

   [SerializeField] Vector2 raioMinMaxDistance;
   [SerializeField] Vector2 alturaMinMax;
   [SerializeField] int deleyTentativesAttack = 1;

   [SerializeField] SettingsLocationArea settings;


   Chronometry[] cronometro = new Chronometry[2];

   Player player;

   int waveFormationIndc = 0;

   private void Awake()
   {
      cronometro[0] = new Chronometry();
      cronometro[1] = new Chronometry();

      alocationStage = new AlocationStage(mapGridFase, raioMinMaxDistance, alturaMinMax);
      int deley = Random.Range(1, 10);

      foreach (Wave wave in waves)
      {
         for (int i = 0; i < wave.enemyInfos.Length; i++)
         {
            wave.enemyInfos[i].enemy = EnemyLocationWave(wave.enemyInfos[i].tipo);
         }
      }

      waveFormationIndc = Random.Range(0, mapGridFase.GetFormationsList().Count);
   }

   private void Start()
   {
      player = FindObjectOfType<Player>();
      boss = FindObjectOfType<Boss>();
   }

   private void Update()
   {

      if (enemyesInScene.Count == 0)
      {
         if (!boss.bossActive)
         {
            StartCoroutine(SpawnEnemies());
         }
      }
      else
      {
         int enemysDeath = 0;

         for (int i = 0; i < enemyesInScene.Count; i++)
         {
            if (!enemyesInScene[i].activeSelf)
            {
               enemysDeath++;
            }
         }

         if (enemyesInScene.Count == enemysDeath)
         {
            foreach (GameObject enemy in enemyesInScene)
            {
               enemyesCemiterio.Add(enemy);
            }

            if (!boss.bossActive)
            {
               if (waves.Length - 1 > settings.waveIndc)
                  settings.waveIndc++;
               else{
                  boss.bossActive = true;
               }
            }

            enemyesInScene.Clear();
         }
      }
   }

   IEnumerator SpawnEnemies()
   {
      foreach (EnemyInfo enemyInfo in waves[settings.waveIndc].enemyInfos)
      {
         GameObject enemy = null;

         for (int j = 0; j < enemyesCemiterio.Count; j++)
         {
            if (enemyesCemiterio[j].GetComponent<Enemy>().tipo == enemyInfo.tipo)
            {
               enemy = enemyesCemiterio[j];
               enemy.name = "Enemy";
               enemyesCemiterio.Remove(enemyesCemiterio[j]);
               Vector3 randomDirection = Random.insideUnitSphere;
               Vector3 randomPosition = randomDirection * radiusSpawnPont;
               enemy.transform.position = spawnerPoints[Random.Range(0, spawnerPoints.Length)].position + randomPosition;
               enemy.SetActive(true);
               enemy.GetComponent<Enemy>().ResetEnemy();
               j = enemyesCemiterio.Count;
            }
         }

         if (enemy == null)
         {
            enemy = Instantiate(enemyInfo.enemy);
            enemy.GetComponent<Enemy>().proprerts.spawnerEnemys = this;
            enemy.name = "Enemy";
            Vector3 randomDirection = Random.insideUnitSphere;
            Vector3 randomPosition = randomDirection * radiusSpawnPont;
            enemy.transform.position = spawnerPoints[Random.Range(0, spawnerPoints.Length)].position + randomPosition;
         }

         enemyesInScene.Add(enemy);

         yield return new WaitForSeconds(delayBetweenEnemies);
      }
   }

   void FixedUpdate()
   {

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

      float speedRotate = 0;

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

      enemysActive = ActiveEnemysInScene(enemyesInScene);

      if (cronometro[0].CronometroPorSeg(enemysActive))
      {
         enemyesInScene = enemyesInScene.OrderBy(x => Random.value).ToList();
         waveFormationIndc = Random.Range(0, mapGridFase.GetFormationsList().Count);
         cronometro[0].Reset();
      }

      WaveAttack(deleyTentativesAttack, enemysActive, enemyesInScene);

      alocationStage.Alocar(enemyesInScene, wavePos, waveFormationIndc, settings);
   }

   void WaveAttack(int deleytentatives, int enemysActive, List<GameObject> enemyesInScene)
   {
      if (cronometro[1].CronometryPorMiles(deleytentatives))
      {
         List<GameObject> activeEnemis = new List<GameObject>();

         foreach (GameObject enemy in enemyesInScene)
         {
            if (enemy.activeSelf)
               activeEnemis.Add(enemy);
         }

         if (activeEnemis.Count > 0)
         {
            Enemy enemySelected = activeEnemis[Random.Range(0, activeEnemis.Count)].GetComponent<Enemy>();

            float porcentagenTiro = (((float)enemyesInScene.Count - (float)enemysActive) / (float)enemyesInScene.Count) * 100;
            porcentagenTiro = (porcentagenTiro < 30) ? 30 : porcentagenTiro;
            float randonNumber = Random.Range(1f, 100f);

            if (randonNumber <= porcentagenTiro)
            {
               enemySelected.mecanics.Attack();
            }
         }

         cronometro[1].Reset();
      }
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

   private void OnDrawGizmos()
   {

      if (alocationStage == null)
         alocationStage = new AlocationStage(mapGridFase, raioMinMaxDistance, alturaMinMax);

      alocationStage.Draw(wavePos, waveFormationIndc, settings);
   }


}

[System.Serializable]
public struct Wave
{
   [SerializeField] public EnemyInfo[] enemyInfos;
}

[System.Serializable]
public struct EnemyInfo
{
   [SerializeField] public StatosEnemyes.Tipo tipo;
   [HideInInspector] public GameObject enemy;
}

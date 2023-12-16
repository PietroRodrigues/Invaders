using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Components WavePosition")]
    [SerializeField] Player player;
    [SerializeField] Transform WaveTransform;
    [SerializeField] bool allMap = true;
    [SerializeField] float waveSpeed;
    [SerializeField] float waveSpeedRotation;
    [SerializeField] int heightWave;
    [SerializeField] Vector3 boxSize;
    [SerializeField] Vector2 mapLarger;


    [Space(1)]
    [Header("Components Spawner")]
    [SerializeField] Boss boss;
    [SerializeField] List<GameObject> thePhasePrefabs;

    [SerializeField] List<Transform> pointsSpawner;
    [SerializeField] int radiusSpawnPont;

    [SerializeField] List<WaveStage> waveList;

    float delayEnemySpwner;

    public static EnemyesInStage enemyesInStage;

    WavePosition wavePosition;

    Chronometry chronometryAttack = new Chronometry();

    private void Awake()
    {
        enemyesInStage = new EnemyesInStage();

        if(waveList.Count > 0){
            wavePosition = new WavePosition(player,WaveTransform,allMap);
        }
        
        foreach (WaveStage wave in waveList)
        {
            foreach (EnemyInfo enemyInfo in wave.enemyInWave)
            {
                enemyInfo.enemy = FillTheEnemies(enemyInfo.tipo);
            }
        }
    }

    private void Update()
    {
        if(enemyesInStage.alive.Count == 0){
        
            if(enemyesInStage.waveCont < waveList.Count)
                enemyesInStage.waveCont++;
            else{
                boss.bossActive = true;
                boss.gameObject.SetActive(boss.bossActive);
                enemyesInStage.waveCont = waveList.Count + 1;
            }

            if(!boss.bossActive){
                StartCoroutine(SpawnEnemy());
            }else{
                if(boss.statos.hp <= 0){
                    Debug.Log("Fim De jogo!!!");
                }
                Debug.Log("Boss Naceu!!!!" );
            }
        
        }else{

            List<GameObject> dead = new List<GameObject>();
            foreach (GameObject enemy in enemyesInStage.alive)
            {
                if(!enemy.activeSelf){
                    dead.Add(enemy);
                }                
            }

            foreach (var obj in dead)
            {
                enemyesInStage.dead.Add(obj);
                enemyesInStage.alive.Remove(obj);
            }
            
            
            wavePosition.WavePositionSet(waveSpeed,mapLarger,heightWave,boxSize);
            
            wavePosition.EnemyPosSet(enemyesInStage.alive,boxSize);
        }
    }

    IEnumerator SpawnEnemy(){

        if(waveList.Count > 0){
            foreach (EnemyInfo enemyInfo in waveList[enemyesInStage.waveCont - 1].enemyInWave)
            {
                GameObject enemy = RecicleEnemy(enemyInfo);

                if(enemy == null){
                    enemy = InstantiateEnemy(enemyInfo);
                }

                enemyesInStage.alive.Add(enemy);

            }
        }

        yield return new WaitForSeconds(delayEnemySpwner);

    }

    GameObject InstantiateEnemy(EnemyInfo enemyInfo){
        
        GameObject enemy = Instantiate(enemyInfo.enemy);
        enemy.GetComponent<Enemy>().proprerts.waveTransform = WaveTransform;
        enemy.GetComponent<Enemy>().proprerts.target = player.gameObject;
        enemy.name = "Enemy";
        SetPositionEnemySpawn(enemy);
        
        return enemy;

    }

    GameObject RecicleEnemy(EnemyInfo enemyInfo){

        GameObject enemyReciclado = null;
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject enemy in enemyesInStage.dead)
        {
            if(enemy.GetComponent<Enemy>().tipo == enemyInfo.tipo){
                
                objectsToRemove.Add(enemy);
                SetPositionEnemySpawn(enemy);
                enemyReciclado = enemy;
                enemyReciclado.SetActive(true);
                break;
            }

        }        

        foreach (GameObject obj in objectsToRemove)
        {
            enemyesInStage.dead.Remove(obj);
        }

        return enemyReciclado;

    }

    void SetPositionEnemySpawn(GameObject enemy){

        Vector3 randomDirection = Random.insideUnitSphere;
        Vector3 randomPosition = randomDirection * radiusSpawnPont;
        enemy.transform.position = pointsSpawner[Random.Range(0, pointsSpawner.Count)].position + randomPosition;
    
    }

    GameObject FillTheEnemies(StatosEnemyes.Tipo tipo)
    {
      GameObject enemy = null;

      foreach (GameObject enemysPrefabs in thePhasePrefabs)
      {
         if (enemysPrefabs.GetComponent<Enemy>().tipo == tipo)
         {
            enemy = enemysPrefabs;
         }
      }

      return enemy;
    }

    void OnDrawGizmosSelected()
    {
        if(wavePosition != null){
            if (wavePosition.quadrants != null){
                
                Gizmos.color = Color.red;

                foreach (Vector3 point in wavePosition.quadrants)
                {
                    Gizmos.DrawSphere(point,1f);                    
                }
            }            
        }
    }

    void OnDrawGizmos()
    {
        if(WaveTransform != null)
            DrawCubeEdges(WaveTransform.position,boxSize);
    }

    private void DrawCubeEdges(Vector3 center, Vector3 size)
    {
        Gizmos.color = Color.green;

        float halfX = size.x / 2f;
        float halfY = size.y / 2f;
        float halfZ = size.z / 2f;

        // Defina as oito arestas do cubo
        Vector3[] edges = new Vector3[]
        {
            center + new Vector3(-halfX, -halfY, -halfZ),
            center + new Vector3(halfX, -halfY, -halfZ),
            center + new Vector3(halfX, -halfY, halfZ),
            center + new Vector3(-halfX, -halfY, halfZ),
            center + new Vector3(-halfX, halfY, -halfZ),
            center + new Vector3(halfX, halfY, -halfZ),
            center + new Vector3(halfX, halfY, halfZ),
            center + new Vector3(-halfX, halfY, halfZ)
        };

        // Desenhe as linhas entre as arestas
        Gizmos.DrawLine(edges[0], edges[1]);
        Gizmos.DrawLine(edges[1], edges[2]);
        Gizmos.DrawLine(edges[2], edges[3]);
        Gizmos.DrawLine(edges[3], edges[0]);

        Gizmos.DrawLine(edges[4], edges[5]);
        Gizmos.DrawLine(edges[5], edges[6]);
        Gizmos.DrawLine(edges[6], edges[7]);
        Gizmos.DrawLine(edges[7], edges[4]);

        Gizmos.DrawLine(edges[0], edges[4]);
        Gizmos.DrawLine(edges[1], edges[5]);
        Gizmos.DrawLine(edges[2], edges[6]);
        Gizmos.DrawLine(edges[3], edges[7]);
    }

}



[System.Serializable]
public class WaveStage
{
   public  List<EnemyInfo> enemyInWave;
}

[System.Serializable]
public class EnemyInfo
{
   public StatosEnemyes.Tipo tipo;
   [HideInInspector] public GameObject enemy;
}

[System.Serializable]
public class EnemyesInStage{

    public int waveCont = 0;
    public List<GameObject> alive = new List<GameObject>();
    public List<GameObject> dead = new List<GameObject>();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemys : MonoBehaviour
{   
    [SerializeField] int radiusSpawnPont;
    int waveCaunt = 0;
    [SerializeField] Transform[] spawnerPoints;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject[] enemysFaze;
    [SerializeField] Wave[] waves;
    [SerializeField] public List<GameObject> enemyesInScene;
    List<GameObject> enemyesCemiterio = new List<GameObject>();

    private void Awake() {
        foreach (Wave wave in waves)
        {
            for (int i = 0; i < wave.enemyInfos.Length; i++)
            {              
                wave.enemyInfos[i].enemy = EnemyLocationWave(wave.enemyInfos[i].tipo);     
            }           
        }       
    }

    private void Update() {

        if(enemyesInScene.Capacity == 0){
            for (int i = 0; i < waves[waveCaunt].enemyInfos.Length; i++)
            {   
                GameObject enemy = null;
                
                for(int j = 0; j < enemyesCemiterio.Count; j++)
                {
                    if(enemyesCemiterio[j].GetComponent<Enemy>().tipo == waves[waveCaunt].enemyInfos[i].tipo){
                        enemy = enemyesCemiterio[j];
                        enemyesCemiterio.Remove(enemyesCemiterio[j]);
                        Vector3 randomDirection = Random.insideUnitSphere;
                        Vector3 randomPosition = randomDirection * radiusSpawnPont;
                        enemy.transform.position = spawnerPoints[Random.Range(0,spawnerPoints.Length)].position + randomPosition;
                        j = enemyesCemiterio.Count;
                    }
                }

                if(enemy == null){
                    enemy = Instantiate(waves[waveCaunt].enemyInfos[i].enemy);
                    Vector3 randomDirection = Random.insideUnitSphere;
                    Vector3 randomPosition = randomDirection * radiusSpawnPont;
                    enemy.transform.position = spawnerPoints[Random.Range(0,spawnerPoints.Length)].position + randomPosition;
                }

                enemyesInScene.Add(enemy);
            }
        }
    }

    GameObject EnemyLocationWave(Statos.Tipo tipo){
        
        GameObject enemy = null;
       
        foreach (GameObject enemysPrefabs in enemysFaze)
        {
            if(enemysPrefabs.GetComponent<Enemy>().tipo == tipo){
                enemy = enemysPrefabs;
            }
        }        

        return enemy;
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
    [SerializeField] public Statos.Tipo tipo;
    //[HideInInspector] 
    public GameObject enemy;
}
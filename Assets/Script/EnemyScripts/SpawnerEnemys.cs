using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemys : MonoBehaviour
{   
    [SerializeField] int radiusSpawnPont;
    [SerializeField] int waveCaunt = 0;
    bool bossFight = false;
    bool EndStage = false;
    [SerializeField] GameObject boss;
    [SerializeField] Transform spawnerBossPoint;
    [SerializeField] Transform[] spawnerPoints;
    [SerializeField] GameObject[] enemysFaze;
    [SerializeField] Wave[] waves;
    [SerializeField] public List<GameObject> enemyesInScene;
    [SerializeField] List<GameObject> enemyesCemiterio = new List<GameObject>();

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

        if(enemyesInScene.Count == 0){

            if(!bossFight){
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
                            enemy.SetActive(true);
                            enemy.GetComponent<Enemy>().ResetEnemy();
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
            }else{
                if(!EndStage){
                    GameObject bossInstantiate = Instantiate(boss);
                    bossInstantiate.transform.position = spawnerBossPoint.position;
                    enemyesInScene.Add(bossInstantiate);
                }
            }
        }else
        {
            int enemysDeath = 0;

            for (int i = 0; i < enemyesInScene.Count; i++)
            {
                if(!enemyesInScene[i].activeSelf){
                    enemysDeath++;
                }
            }
            
            if(enemyesInScene.Count == enemysDeath){
                
                foreach (GameObject enemy in enemyesInScene)
                {
                    enemyesCemiterio.Add(enemy);            
                }
                
                if(!bossFight){
                    if(waves.Length-1 > waveCaunt)
                        waveCaunt++;
                    else
                        bossFight = true;
                }else{
                    EndStage = true;
                    print("Comgratulations!!!");
                }

                enemyesInScene.Clear();
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
    [HideInInspector] public GameObject enemy;
}
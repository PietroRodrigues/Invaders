using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Designer;
using UnityEditor;

public class SpawnerEnemys : MonoBehaviour
{       
    [SerializeField] MapGridFase mapGridFase;
    
    [HideInInspector] public List<GameObject> enemyesInScene;
    List<GameObject> enemyesCemiterio = new List<GameObject>();
    int waveCaunt = 0;
    bool bossFight = false;
    bool EndStage = false;


    [SerializeField] GameObject boss;
    [SerializeField] GameObject[] enemysFaze;
    
    [SerializeField] int radiusSpawnPont;
    [SerializeField] Transform spawnerBossPoint;
    [SerializeField] Transform[] spawnerPoints;
    [SerializeField] public Wave[] waves;

    AlocationStage alocationStage;

    [SerializeField]  Transform[] wavePos;
    [SerializeField] Vector2 raioMinMaxDistance;
    [SerializeField] Vector2 alturaMinMax;

    
    [SerializeField] SettingsLocationArea settings;

    private void Awake() {       

        alocationStage = new AlocationStage(wavePos,raioMinMaxDistance,alturaMinMax);

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
                }

                enemyesInScene.Clear();
            }
        }

        if(!bossFight || !EndStage)
            alocationStage.Alocar(enemyesInScene , mapGridFase, settings);

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

    private void OnDrawGizmos() {
        
        if(alocationStage == null)
            alocationStage = new AlocationStage(wavePos,raioMinMaxDistance,alturaMinMax);
        
        alocationStage.Draw(settings,mapGridFase);
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

namespace Designer
{
    [System.Serializable]
    public struct WaveDesigner
    {
        public bool[,] grade;

        public WaveDesigner(int linha, int colunas)
        {
            this.grade = new bool[linha,colunas];
        }
    }
}
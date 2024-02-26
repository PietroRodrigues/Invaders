using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WavePosition 
{

    Transform waveTransform;
    Chronometry cronometroWavePos;
    Chronometry cronometroEnemyPos;

    int indcQuadrant = 0;

    public Vector3[] quadrants = new Vector3[4];

    Vector3 velocityWavePosParent;

    bool allMap;

    public WavePosition(Transform waveTransform, bool allMap){

        this.allMap = allMap;
        this.waveTransform = waveTransform;
        cronometroWavePos = new Chronometry();
        cronometroEnemyPos = new Chronometry();
    
    }

    void QuadrantSet(float heightWave,float distance, Vector3 boxSize){

        Transform root = waveTransform.root;
        
        FindPointInDirections(root.forward,heightWave,distance,boxSize.z/2,0);
        FindPointInDirections(-root.forward,heightWave,distance,boxSize.z/2,1);
        FindPointInDirections(root.right,heightWave,distance,boxSize.z/2,2);
        FindPointInDirections(-root.right,heightWave,distance,boxSize.z/2,3);

        
    }

    void FindPointInDirections(Vector3 direction, float heightWave,float distance, float boxSizeZ,int index){

        Vector3 point = direction * (distance - boxSizeZ);
        point.y = heightWave;

        quadrants[index] = point;
        
    }

    public void WavePositionSet(float waveSmoothSpeed,Vector3 mapLarger,float heightWave, Vector3 boxSize){

        QuadrantSet(heightWave,mapLarger.x / 2,boxSize);
        
        if(allMap){

            if(cronometroWavePos.CronometroPorSeg(60)){
                int randon;
                do{
                    randon = Random.Range(0,4);
                } while (randon == indcQuadrant);
                indcQuadrant = randon;
                cronometroWavePos.Reset();
            }

            

            waveTransform.position = Vector3.SmoothDamp(waveTransform.position, quadrants[indcQuadrant], ref velocityWavePosParent,  waveSmoothSpeed * Time.deltaTime);
            
            //Rotaciona a Wave na direçao do centro do cenario
            Vector3 direction = new Vector3( waveTransform.root.transform.position.x, waveTransform.transform.position.y, waveTransform.root.transform.position.z) - waveTransform.transform.position;

            Quaternion waveRot = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 waveRotAngle = waveRot.eulerAngles;

            waveRot = Quaternion.Euler(waveRotAngle);

            waveTransform.rotation = waveRot;
        }
        //==================================================

    }

    public void EnemyPosSet(List<GameObject> enemiesAlive, Vector3 boxSize)
    {
        foreach (GameObject enemyGB in enemiesAlive)
        {
            Enemy enemy = enemyGB.GetComponent<Enemy>();
            Vector3 posDstino = enemy.proprerts.posDestination;
            if(posDstino == Vector3.zero || cronometroEnemyPos.CronometroPorSeg(Random.Range(5,31))){
                posDstino = PosEnemyGenerator(enemy.proprerts.posDestination, boxSize);
                cronometroEnemyPos.Reset();
            }
            
            enemy.proprerts.posDestination = posDstino;

        }
    }

    Vector3 PosEnemyGenerator(Vector3 posReal, Vector3 boxSize)
    {
        Vector3 pointRandon;

        do
        {
            float x = Random.Range(-boxSize.x / 2f, boxSize.x / 2f);
            float y = Random.Range(-boxSize.y / 2f, boxSize.y / 2f);
            float z = Random.Range(-boxSize.z / 2f, boxSize.z / 2f);
            pointRandon = new Vector3(x, y, z);

        } while (Vector3.Distance(posReal, waveTransform.position + pointRandon) < 10);

        return pointRandon;
    }

}
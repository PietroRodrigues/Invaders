using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveObstacle : MonoBehaviour
{   
    [SerializeField] Transform WavePos;
    [SerializeField] Transform[] WavePosPoints;
    [SerializeField] GameObject[] plataforms;

    // Update is called once per frame
    void Update()
    {
        int waveIndex = Spawner.enemyesInStage.waveCont;

        if(waveIndex > 0){

            for (int i = 0; i < waveIndex - 1; i++)
            {
                plataforms[i].SetActive(true);
            }

            WavePos.position = WavePosPoints[waveIndex - 1 < WavePosPoints.Length ?waveIndex - 1 : waveIndex - 2].position;            
        }
        
    }
}

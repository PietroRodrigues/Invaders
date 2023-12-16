using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDRadar
{
    GameObject alvo = null;
    int indcAlvo = 0;

    public Vector3 RadarScan(bool switchTarget, Vector3 mouseAimPos)
    {
        List<GameObject> alive = Spawner.enemyesInStage.alive;

        if (alive.Count > 0)
        {
            if (alvo == null || !alive.Contains(alvo))
            {
                indcAlvo = 0;
            }

            if (switchTarget)
            {
                indcAlvo++;
                
            }

            indcAlvo %= alive.Count;
            alvo = alive[indcAlvo];
        }
        else
        {
            alvo = null;
        }

        return alvo != null ? alvo.transform.position : mouseAimPos;
    }
}

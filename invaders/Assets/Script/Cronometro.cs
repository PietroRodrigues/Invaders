using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronometry
{
    public int min = 0;
    public int seg = 0;
    public int miliseg = 0;

    public void Reset(){
        min = 0;
        seg = 0;
        miliseg = 0;
    }

    public bool CronometroPorMin(int minLimite)
    {
        bool limitTimer = false;

        miliseg += (int)(Time.deltaTime * 1000);

        if (miliseg >= 1000)
        {
            seg++;
            miliseg = 0;

            if (seg >= 60)
            {
                min++;
                seg = 0;

                if (min >= minLimite)
                {
                    limitTimer = true;
                    min = 0;

                }

            }

        }

        return limitTimer;
    }

    public bool CronometroPorSeg(int segLimite)
    {
        bool limitTimer = false;

        miliseg += (int)(Time.deltaTime * 1000);

        if (miliseg >= 1000)
        {
            seg++;
            miliseg = 0;

            if (seg >= segLimite)
            {
                limitTimer = true;
                seg = 0;

            }

        }

        return limitTimer;
    }


    public bool CronometryPorMiles(int milisegLimite)
    {
        bool limitTimer = false;

        miliseg += (int)(Time.deltaTime * 1000);

        if (miliseg >= milisegLimite) {

            limitTimer = true;
            miliseg = 0;
        }

        return limitTimer;
    }
}

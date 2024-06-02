using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Statos : MonoBehaviour
{
   [HideInInspector] public int bodies;
   [HideInInspector] public float hp;

   public float hpMax;
   public int bodiesMax;

   [HideInInspector] public int missilGuiado = 0;

   public Buffs buffs;

   Chronometry[] chronometry = { new Chronometry(), new Chronometry(), new Chronometry() };

   public void TimerBuffs()
   {

      buffs.buffDrone = buffs.TimerDroneLife > 0;

      if (buffs.buffDrone)
      {
         if (chronometry[0].CronometroPorSeg(1))
         {
            buffs.TimerDroneLife--;

            if (buffs.TimerDroneLife <= 0)
            {
               buffs.TimerDroneLife = 0;
            }
         }
      }

      buffs.buff2X = buffs.Time2X > 0;

      if (buffs.buff2X)
      {
         if (chronometry[1].CronometroPorSeg(1))
         {
            buffs.Time2X--;

            if (buffs.Time2X <= 0)
            {
               buffs.Time2X = 0;
            }
         }
      }

      buffs.buffFastShot = buffs.TimerFastShot > 0;

      if (buffs.buffFastShot)
      {
         if (chronometry[2].CronometroPorSeg(1))
         {
            buffs.TimerFastShot--;

            if (buffs.TimerFastShot <= 0)
            {
               buffs.TimerFastShot = 0;
            }
         }
      }
   }

}

[System.Serializable]
public class Buffs
{
   //
   [HideInInspector] public bool buffDrone;
   public float MaxTimerDroneLife;
   [HideInInspector] public float TimerDroneLife = 0;

   [HideInInspector] public bool buffTermico;
   public float MaxTimerTermico = 0;
   [HideInInspector] public float TimerTermico;

   [HideInInspector] public bool buff2X;
   public float MaxTimer2X;
   [HideInInspector] public float Time2X = 0;

   [HideInInspector] public bool buffFastShot;
   public float MaxTimerFastShot;
   [HideInInspector] public float TimerFastShot = 0;

}

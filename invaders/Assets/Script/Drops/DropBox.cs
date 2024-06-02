using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : Drop
{

   private void OnTriggerEnter(Collider other)
   {
      MecanicDrop(other);
   }

   public override void MecanicDrop(Collider other)
   {
      if (!other.isTrigger)
      {
         if (other.gameObject.transform.root.GetComponent<Player>() != null)
         {
            if (!getDrop)
            {

               Player player = other.gameObject.transform.root.GetComponent<Player>();

               switch (dropsTipos)
               {
                  case DropsTipos.Life:
                     LifeDrop(player);
                     break;
                  case DropsTipos.Shild:
                     ShildDrop(player);
                     break;
                  case DropsTipos.Drone:
                     DroneDrop(player);
                     break;
                  case DropsTipos.buffTermico:
                     MisselTermico(player);
                     break;
                  case DropsTipos.FastShot:
                     FastShotDrop(player);
                     break;
                  case DropsTipos.buff2X:
                     Buff2XDrop(player);
                     break;
                  case DropsTipos.Special:
                     SpecialDrop(player);
                     break;
                  default:
                     break;
               }
            }
            base.MecanicDrop(other);
         }
      }
   }

   void LifeDrop(Player player)
   {
      if (player.hp < player.hpMax)
      {
         player.hp += 50;
         if (player.hp > player.hpMax)
         {
            player.hp = player.hpMax;
         }
      }
   }

   void ShildDrop(Player player)
   {
      player.ShieldCharger();
      player.inventario.shield = player.inventario.ShieldMax;
   }

   void DroneDrop(Player player)
   {

      player.buffs.TimerDroneLife += 120;

      if (player.buffs.TimerDroneLife > player.buffs.MaxTimerDroneLife)
         player.buffs.TimerDroneLife = player.buffs.MaxTimerDroneLife;

   }

   void FastShotDrop(Player player)
   {
      player.buffs.TimerFastShot += 120;

      if (player.buffs.TimerFastShot > player.buffs.MaxTimerFastShot)
         player.buffs.TimerFastShot = player.buffs.MaxTimerFastShot;

   }

   void Buff2XDrop(Player player)
   {
      player.buffs.Time2X += 120;

      if (player.buffs.Time2X > player.buffs.MaxTimer2X)
         player.buffs.Time2X = player.buffs.MaxTimer2X;

   }

   void MisselTermico(Player player)
   {
      player.buffs.TimerTermico += 120;

      if (player.buffs.TimerTermico > player.buffs.MaxTimerTermico)
         player.buffs.TimerTermico = player.buffs.MaxTimerTermico;

   }


   void SpecialDrop(Player player)
   {

      Debug.Log("Pego Special (CompletoCargas)!");

   }

}

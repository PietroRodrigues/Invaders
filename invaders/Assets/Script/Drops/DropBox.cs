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
            if(!getDrop){

               Player player = other.gameObject.transform.root.GetComponent<Player>();

               switch (dropsTipos)
               {
                  case DropsTipos.Life:
                     LifeDrop(player);
                  break;                  
                  case DropsTipos.Shild:
                     ShildDrop(player);
                  break;
                  case DropsTipos.Drones:
                     DroneDrop(player);
                  break;                  
                  case DropsTipos.Missiles:
                     MissilesDrop(player);
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

   void LifeDrop(Player player){
      if(player.hp < player.hpMax){
         player.hp += 50;
         if(player.hp > player.hpMax){
            player.hp = player.hpMax;
         }
      }
   }

   void ShildDrop(Player player){
      player.ShieldCharger();
      player.inventario.shield = player.inventario.ShieldMax;
   }

   void DroneDrop(Player player){
      if(player.inventario.drones < 3){
         player.inventario.drones++;
      }
   }

   void MissilesDrop(Player player){

   }

   void SpecialDrop(Player player){


   }
   
}

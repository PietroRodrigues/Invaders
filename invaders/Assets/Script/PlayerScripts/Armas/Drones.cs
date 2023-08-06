using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drones
{
   Transform dronesTransform;
   Rigidbody rb;
   int cont;
   Player player;

   DroneStatos[] drones;

   float speed = 5;

   public Drones(Transform dronesTransform, Rigidbody rb,ShottingDroneSettings[] shottingDroneSettings)
   {
      this.rb = rb;
      player = this.rb.GetComponent<Player>();
      this.dronesTransform = dronesTransform;
      drones = new DroneStatos[dronesTransform.childCount];
      
      for (int i = 0; i < dronesTransform.childCount; i++)
      {
         drones[i].gameObject = dronesTransform.GetChild(i).gameObject;
         drones[i].posDrones = dronesTransform.GetChild(i).position - dronesTransform.position;
         drones[i].shotting = new ShottingDrone(shottingDroneSettings[i]);
                  
      }

   }

   public void Reload(DroneStatos drone){
      drone.shotting.shottingDroneSettings.ammon = drone.shotting.shottingDroneSettings.maxAmmon;
   }
   
   public void Fire(bool attack, float maxDistanceReset){
     
         foreach (DroneStatos drone in drones)
         { if (drone.gameObject.activeSelf)
            {
               if(drone.shotting.shottingDroneSettings.ammon > 0){
                  drone.shotting.GunShotting(attack,speed,drone.gameObject.transform.position,maxDistanceReset,drone);
               }else{
                  player.inventario.drones--;
                  drone.gameObject.SetActive(false);
               }
            }
         }
   }

   public void MovementDrones(Vector3 target,float speedAim,Inventario inventario)
   {      
      for (int i = 0; i < inventario.drones; i++)
      {
         if(!drones[i].gameObject.activeSelf){
            if(drones[i].shotting.shottingDroneSettings.ammon <= 0){
               Reload(drones[i]);
               drones[i].gameObject.SetActive(true);
            }
         }
      }
   
      int cont = 0;
      
      foreach (DroneStatos drone in drones)
      {
         if (drone.gameObject.activeSelf)
         {
            Vector3 targetPosition = rb.transform.position + rb.transform.TransformDirection(drones[cont].posDrones);
            Vector3 pos = Vector3.Lerp(drone.gameObject.transform.position,targetPosition, speed * Time.deltaTime);
            
            LookAlvo(drone.gameObject.transform, target,speedAim * 2);
            PosDrones(drone.gameObject.transform, pos);

            cont++;

            if(cont >= drones.Length){
               cont = 0;
            }

            if(drone.gameObject.transform.root != null)
               drone.gameObject.transform.SetParent(null);

         }else{
            if(drone.gameObject.transform.root == null){
               drone.gameObject.transform.SetParent(dronesTransform);
               drone.gameObject.transform.localPosition = drones[cont].posDrones;
            }

            cont++;
         }
      }
   }

   void LookAlvo(Transform drone, Vector3 target,float speedAim)
   {
      Quaternion lookAtDrone = Quaternion.LookRotation(target - drone.position, Vector3.up);
      
      drone.transform.rotation = Quaternion.RotateTowards(drone.transform.rotation, lookAtDrone, (speedAim * 2) * Time.deltaTime);

   }

   void PosDrones(Transform drone, Vector3 tg)
   {    
      drone.GetComponent<Rigidbody>().MovePosition(tg);
   }

}

public struct DroneStatos{
   public GameObject gameObject;
   public Vector3 posDrones;
   public ShottingDrone shotting;   
}

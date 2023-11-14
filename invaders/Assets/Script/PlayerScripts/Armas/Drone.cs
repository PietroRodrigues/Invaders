using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Drone
{
   public DroneStatos statos;
   public DroneSettingsShot settingsShot;
   public ShottingDrone shotting = new ShottingDrone();

   Vector3 targetPosition = Vector3.zero;

   public Drone(Transform player,DroneStatos statos, DroneSettingsShot settingsShot){

      this.statos = statos;
      this.settingsShot = settingsShot;
      this.statos.player = player;
      this.statos.posDrone = Vector3.zero;
   }

   void ActiveDrone()
   {     
      statos.drone.SetActive(statos.active);
   }
   
   public void Fire(bool attack, float maxDistanceReset){
     
         if (statos.drone.gameObject.activeSelf)
         {
            if(settingsShot.ammon > 0){
               shotting.GunShotting(attack,statos.speed,statos.drone.transform.position,maxDistanceReset,settingsShot);
            }else{
               statos.active = false;
            }
         }
   }

   public void MovementDrones(Vector3 target,float speedAim)
   {      
      if (settingsShot.ammon <= 0)
      {
         statos.active = false;
      }
      else
      {
         statos.active = true;
      }

      ActiveDrone();
      
      if (statos.drone.activeSelf)
      {  
         Vector3 pontReference = statos.player.position + statos.player.TransformDirection(statos.posDrone);
         
         pontReference.y += 2f;

         if(Vector3.Distance(targetPosition , pontReference) > 2)
            targetPosition = Vector3.Lerp(targetPosition,pontReference, statos.speed * Time.deltaTime);
         

         Vector3 pos = Vector3.Lerp(statos.drone.gameObject.transform.position,targetPosition, statos.speed * Time.deltaTime);         
         
         LookAlvo(statos.drone.transform, target,speedAim * 2);
         PosDrone(pos);

         if(statos.drone.transform.root != null)
            statos.drone.transform.SetParent(null);

      }else{
         if(statos.drone.transform.root == null){
            statos.drone.transform.SetParent(statos.droneBeg);
            statos.drone.transform.localPosition = statos.posDrone;
         }
      }
   }

   void LookAlvo(Transform drone, Vector3 target,float speedAim)
   {
      Quaternion lookAtDrone = Quaternion.LookRotation(target - drone.position, Vector3.up);
      
      drone.transform.rotation = Quaternion.RotateTowards(drone.transform.rotation, lookAtDrone, speedAim * 2 * Time.deltaTime);

   }

   void PosDrone(Vector3 tg)
   {    
      statos.rb.MovePosition(tg);
   }

}

[System.Serializable]
public struct DroneStatos{
   public bool active;
   public GameObject drone;
   public Transform droneBeg;
   public Rigidbody rb;
   public float speed;
   [HideInInspector]public Transform player;
   [HideInInspector] public Vector3 posDrone;
}

[System.Serializable]
public struct DroneSettingsShot
{
   public Transform cannon;
   public GameObject bullet;
   public VisualEffect muzzleGun;
   public Transform cxBalasGun;
   public int maxAmmon;
   public int ammon;
   [HideInInspector] public float speedBody;
   [HideInInspector] public List<GameObject> BoxBullet;
}

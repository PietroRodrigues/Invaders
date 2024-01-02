using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Drone
{
   public ShottingDrone shotting = new ShottingDrone();

   Vector3 targetPosition = Vector3.zero;  

   void ActiveDrone(DroneStatos statos)
   {  
      statos.drone.SetActive(statos.active);

      if (!statos.drone.activeSelf)
      {         
         targetPosition = statos.drone.transform.position;
      }
   }
   
   void FireDrone(bool attack, float maxDistanceReset,DroneStatos statos){
         
         if (statos.drone.gameObject.activeSelf)
         {
            if(statos.droneSettingsShot.ammon > 0){
               shotting.GunShotting(attack,statos.speed,statos.drone.transform.position,maxDistanceReset,statos.player);
            }
         }
   }

   public void MovementDrones(ref DroneStatos statos)
   {      
      statos.active = statos.droneSettingsShot.ammon > 0;
      
      if (statos.active)
      {  
         Vector3 pontReference = statos.player.transform.position + statos.player.transform.TransformDirection(statos.posDrone);
         
         pontReference.y += 2f;

         if(Vector3.Distance(targetPosition , pontReference) > 2)
            targetPosition = Vector3.Lerp(targetPosition,pontReference, statos.speed * Time.deltaTime);         

         Vector3 pos = Vector3.Lerp(statos.drone.gameObject.transform.position,targetPosition, statos.speed * Time.deltaTime);         
         
         PosDrone(pos,statos);

         if(statos.drone.transform.root != null)
            statos.drone.transform.SetParent(null);

      }else
      {
         if(statos.drone.transform.root == null){
            statos.drone.transform.SetParent(statos.droneBeg);
            statos.drone.transform.localPosition = statos.posDrone;
         }
      }

      ActiveDrone(statos);
    
   }

   public void LookDroneTarget(Vector3 target,Vector3 miraVeiculo,float speedAim,DroneStatos statos)
   {  
      bool lostTarget = Vector3.Distance(statos.player.transform.position,target) > 45;    
      

      Quaternion lookAtDrone = Quaternion.LookRotation((lostTarget ? miraVeiculo : target) - statos.drone.transform.position, Vector3.up);
      
      
      statos.drone.transform.rotation = Quaternion.RotateTowards(statos.drone.transform.rotation, lookAtDrone, speedAim * 4 * Time.deltaTime);

      if(Spawner.enemyesInStage.alive.Count > 0)
         FireDrone(!lostTarget,50,statos);

   }

   void PosDrone(Vector3 tg,DroneStatos statos)
   {    
      statos.rb.MovePosition(tg);
   }

}

[System.Serializable]
public struct DroneStatos{
   public bool active;
   public Player player;
   public GameObject drone;
   public Transform droneBeg;
   public Rigidbody rb;
   public float speed;
   [HideInInspector] public Vector3 posDrone;
   public DroneSettingsShot droneSettingsShot;
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

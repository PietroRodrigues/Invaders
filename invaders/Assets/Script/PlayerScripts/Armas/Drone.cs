using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Drone
{
   public ShottingDrone shotting = new ShottingDrone();

   Vector3 targetPosition = Vector3.zero;

   GameObject target = null;

   public List<GameObject> targets = new List<GameObject>();

   public void LifeTimer(ref DroneStatos statos, ref Buffs buffs){

      statos.drone.SetActive(buffs.buffDrone);

      if (!statos.drone.activeSelf)
      {         
         targetPosition = statos.drone.transform.position;
      }
      
   }

   public void MovementDrones(ref DroneStatos statos , bool buffDrone)
   {      
      if (buffDrone)
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
    
   }

   public void LookDroneTarget(GameObject target,Vector3 miraVeiculo,float speedAim,DroneStatos statos)
   {  
      bool lostTarget = (target != null)? Vector3.Distance(statos.player.transform.position,target.transform.position) > 45 : true;     

      Quaternion lookAtDrone = Quaternion.LookRotation((lostTarget ? miraVeiculo : target.transform.position) - statos.drone.transform.position, Vector3.up);      
      
      statos.drone.transform.rotation = Quaternion.RotateTowards(statos.drone.transform.rotation, lookAtDrone, speedAim * 4 * Time.deltaTime);

      if(Spawner.enemyesInStage.alive.Count > 0)
         FireDrone(!lostTarget,45,statos);

   }

   void FireDrone(bool attack, float maxDistanceReset,DroneStatos statos){
         
         if (statos.drone.gameObject.activeSelf)
         {
           shotting.GunShotting(attack,statos.speed,statos.drone.transform.position,maxDistanceReset,statos.player);
         }
   }

   void PosDrone(Vector3 tg,DroneStatos statos)
   {    
      statos.rb.MovePosition(tg);
   }

   public GameObject RadarScan(Player player , float range)
   {
      targets.Clear();

      foreach (GameObject tg in Spawner.enemyesInStage.alive)
      {
         if(Vector3.Distance(player.transform.position, tg.transform.position) <= range){                
               targets.Add(tg);
               Debug.DrawLine(player.transform.position,tg.transform.position,Color.blue);
         }

      }

      targets.Sort(delegate(GameObject a, GameObject b){

         return Vector3.Distance(player.transform.position, a.transform.position).CompareTo(Vector3.Distance(player.transform.position, b.transform.position));

      });
   
      if(target == null || !target.activeSelf || Vector3.Distance(player.transform.position, target.transform.position) > range){

         targets.Remove(target);
         SelectNewTarget(targets,player,range);

      }

      if (player.camControler.playerInputs.inputsControl.switchTarget)
      {
         targets.Remove(target);
         SelectNewTarget(targets,player,range);
      }

      
      return target;
   }

   void SelectNewTarget(List<GameObject> targets, Player player,float range){

      if(targets.Count > 0 && Vector3.Distance(player.transform.position,targets[0].transform.position) <= range){

         target = targets[0];

      }else{
         
         target = null;

      }

   }

}

[System.Serializable]
public struct DroneStatos{

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
   [HideInInspector] public float speedBody;
   [HideInInspector] public List<GameObject> BoxBullet;
}

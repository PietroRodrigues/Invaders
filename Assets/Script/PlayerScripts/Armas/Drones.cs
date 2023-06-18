using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drones
{
   Transform dronesTransform;
   GameObject[] drones;
   Rigidbody rb;
   Vector3[] posDrones;
   int cont;

   float speed = 5;

   public Drones(Transform dronesTransform, Rigidbody rb)
   {
      this.rb = rb;
      this.dronesTransform = dronesTransform;
      posDrones = new Vector3[dronesTransform.childCount];
      drones = new GameObject[dronesTransform.childCount];

      for (int i = 0; i < dronesTransform.childCount; i++)
      {
         posDrones[i] =  dronesTransform.GetChild(i).position - dronesTransform.position;
         drones[i] = dronesTransform.GetChild(i).gameObject;
      }

   }

   public void MovimentDrones(Vector3 target,float speedAim)
   {
      foreach (GameObject drone in drones)
      {
         if (drone.gameObject.activeSelf)
         {
            Vector3 targetPosition = rb.transform.position + rb.transform.TransformDirection(posDrones[cont]);
            Vector3 pos = Vector3.Lerp(drone.transform.position,targetPosition, speed * Time.deltaTime);
            
            LookAlvo(drone.transform, target,speedAim * 2);
            PosDrones(drone.transform, pos);

            cont++;

            if(cont >= posDrones.Length){
               cont = 0;
            }

            if(drone.transform.root != null)
               drone.transform.SetParent(null);

         }else{
            if(drone.transform.root == null){
               drone.transform.SetParent(dronesTransform);
               drone.transform.localPosition = new Vector3(dronesTransform.position.x,dronesTransform.position.y + 1,dronesTransform.position.z);
            }
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

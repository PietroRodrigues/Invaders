using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShottingDrone
{
   public ShottingDroneSettings shottingDroneSettings;
   Chronometry chronometry = new Chronometry();
   bool primeroTiro = true;
   VisualEffect muzzleGun;

   public ShottingDrone(ShottingDroneSettings settings)
   {
      shottingDroneSettings = settings;
      muzzleGun = shottingDroneSettings.cannon.Find("Particle").GetComponent<VisualEffect>();
      shottingDroneSettings.cxBalasGun = shottingDroneSettings.cannon.Find("CxBalas");

   }

   public void GunShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset,DroneStatos drone)
   {
      if (attack)
      {
         shottingDroneSettings.speedBody = speed;

         if (primeroTiro)
         {
            muzzleGun.Play();
            FireBullet(shottingDroneSettings.cxBalasGun,shottingDroneSettings.bullet);
            drone.shotting.shottingDroneSettings.ammon--;
            primeroTiro = false;
         }
      }

      if (primeroTiro == false)
      {
         if (chronometry.CronometryPorMiles(100))
         {
            chronometry.Reset();
            primeroTiro = true;
         }
      }

      bulletReturn(pos, maxDistanceReset);

   }

   void FireBullet(Transform cxBalas,GameObject bullet)
   {
      if (cxBalas.childCount == 0)
      {
         GameObject bala = GameObject.Instantiate(bullet);
         bala.name = "Bullet " + cxBalas.root.name;
         bala.GetComponent<Bullet>().gumOrigen = cxBalas;
         bala.GetComponent<Bullet>().especie = shottingDroneSettings.cannon.root.GetComponent<Player>();
         bala.GetComponent<Bullet>().BulletOrigen();
         shottingDroneSettings.BoxBullet.Add(bala);
      }

      if (cxBalas.childCount != 0)
      {
         GameObject projetil = cxBalas.GetChild(0).gameObject;

         if (!projetil.activeSelf)
         {
            projetil.GetComponent<Bullet>().Disparate(shottingDroneSettings.speedBody);
         }
      }


   }

   void bulletReturn(Vector3 pos, float maxDistanceReset)
   {
      for (int i = 0; i < shottingDroneSettings.BoxBullet.Count; i++)
      {
         GameObject bullet = shottingDroneSettings.BoxBullet[i];

         if (bullet.activeSelf)
         {
            if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
            {
               bullet.GetComponent<Bullet>().BulletOrigen();
               i = shottingDroneSettings.BoxBullet.Capacity;
            }
         }

      }
   }

   void AnimationStart()
   {
      ParticleSystem p = shottingDroneSettings.particleCanon.GetComponent<ParticleSystem>();
      var main = p.main;
      main.loop = false;
      p.Play();
   }
}

[System.Serializable]
public struct ShottingDroneSettings
{
   public Transform cannon;
   public GameObject bullet;
   public int maxAmmon;
   [HideInInspector] public int ammon;
   [HideInInspector] public float speedBody;
   [HideInInspector] public List<GameObject> BoxBullet;
   [HideInInspector] public Transform particleCanon;
   [HideInInspector] public Transform cxBalasGun;
}
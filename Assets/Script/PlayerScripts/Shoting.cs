using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Shotting
{
   ShottingSettings shottingSettings;
   Chronometry chronometry = new Chronometry();
   bool primeroTiro = true;
   VisualEffect muzzleGun;

   public Shotting(ShottingSettings settings)
   {
      shottingSettings = settings;
      shottingSettings.particleCanon = shottingSettings.cannon.Find("ParticleCanon").transform;
      muzzleGun = shottingSettings.cannon.Find("ParticleGun").GetComponent<VisualEffect>();
      shottingSettings.cxBalasMissel = shottingSettings.cannon.Find("CxBalasMissel");
      shottingSettings.cxBalasGun = shottingSettings.cannon.Find("CxBalasGun");

   }

   public void GunShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset)
   {
      if (attack)
      {
         shottingSettings.speedBody = speed;

         if (primeroTiro)
         {
            muzzleGun.Play();
            FireBullet(shottingSettings.cxBalasGun,shottingSettings.bullet);
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

   public bool LockingMissile(bool mirar){
      return mirar;
   }

   public void MissileShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset)
   {
      if (attack)
      {
         shottingSettings.speedBody = speed;

         if (primeroTiro)
         {
            AnimationStart();
            FireBullet(shottingSettings.cxBalasMissel,shottingSettings.Missel);
            primeroTiro = false;
         }
      }

      if (primeroTiro == false)
      {
         if (chronometry.CronometryPorMiles(600))
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
         bala.GetComponent<Bullet>().especie = shottingSettings.cannon.root.GetComponent<Player>();
         bala.GetComponent<Bullet>().BulletOrigen();
         shottingSettings.BoxBullet.Add(bala);
      }

      if (cxBalas.childCount != 0)
      {
         GameObject projetil = cxBalas.GetChild(0).gameObject;

         if (!projetil.activeSelf)
         {
            projetil.GetComponent<Bullet>().Disparate(shottingSettings.speedBody);
         }
      }

   }

   void bulletReturn(Vector3 pos, float maxDistanceReset)
   {
      for (int i = 0; i < shottingSettings.BoxBullet.Count; i++)
      {
         GameObject bullet = shottingSettings.BoxBullet[i];

         if (bullet.activeSelf)
         {
            if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
            {
               bullet.GetComponent<Bullet>().BulletOrigen();
               i = shottingSettings.BoxBullet.Capacity;
            }
         }

      }
   }

   void AnimationStart()
   {
      ParticleSystem p = shottingSettings.particleCanon.GetComponent<ParticleSystem>();
      var main = p.main;
      main.loop = false;
      p.Play();
   }
}

[System.Serializable]
public struct ShottingSettings
{
   public Transform cannon;
   [HideInInspector] public List<GameObject> BoxBullet;
   [HideInInspector] public Transform particleCanon;
   [HideInInspector] public Transform cxBalasMissel;
   [HideInInspector] public Transform cxBalasGun;
   public GameObject Missel;
   public GameObject bullet;
   [HideInInspector] public float speedBody;
}
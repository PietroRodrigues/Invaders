using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Shotting
{
   ShottingSettings shottingSettings;
   Chronometry chronometry = new Chronometry();
   bool primeroTiro = true;

   public Shotting(ShottingSettings settings)
   {
      shottingSettings = settings;
      shottingSettings.particleCanon = shottingSettings.cannon.Find("ParticleCanon").GetComponentInChildren<VisualEffect>();
      shottingSettings.cxBalasMissel = shottingSettings.cannon.Find("CxBalas");

   }

   public void MissileShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset)
   {
      if (attack)
      {
         shottingSettings.speedBody = speed;

         if (primeroTiro)
         {
            FireBullet(shottingSettings.cxBalasMissel,shottingSettings.Missel);
            primeroTiro = false;
         }
      }

      if (primeroTiro == false)
      {
         if (chronometry.CronometryPorMiles(1000))
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

      AnimationStart();

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
     shottingSettings.particleCanon.Play();
   }
}

[System.Serializable]
public struct ShottingSettings
{
   public Transform cannon;
   [HideInInspector] public List<GameObject> BoxBullet;
   [HideInInspector] public VisualEffect particleCanon;
   [HideInInspector] public Transform cxBalasMissel;
   public GameObject Missel;
   [HideInInspector] public float speedBody;
}
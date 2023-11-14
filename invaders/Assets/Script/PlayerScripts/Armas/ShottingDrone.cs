using UnityEngine;

public class ShottingDrone
{
   Chronometry chronometry = new Chronometry();
   bool primeroTiro = true;

   public void GunShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset, DroneSettingsShot settingsShot)
   {
      if (attack)
      {
         settingsShot.speedBody = speed;

         if (primeroTiro)
         {
            settingsShot.muzzleGun.Play();
            FireBullet(settingsShot);
            settingsShot.ammon--;
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

      bulletReturn(pos, maxDistanceReset,settingsShot);

   }

   void FireBullet(DroneSettingsShot settingsShot)
   {
      if (settingsShot.cxBalasGun.childCount == 0)
      {
         GameObject bala = GameObject.Instantiate(settingsShot.bullet);
         bala.name = "Bullet " + settingsShot.cxBalasGun.root.name;
         bala.GetComponent<Bullet>().gumOrigen = settingsShot.cxBalasGun;
         bala.GetComponent<Bullet>().especie = settingsShot.cannon.root.GetComponent<Player>();
         bala.GetComponent<Bullet>().BulletOrigen();
         settingsShot.BoxBullet.Add(bala);
      }

      if (settingsShot.cxBalasGun.childCount != 0)
      {
         GameObject projetil = settingsShot.cxBalasGun.GetChild(0).gameObject;

         if (!projetil.activeSelf)
         {
            settingsShot.muzzleGun.Play();
            projetil.GetComponent<Bullet>().Disparate(settingsShot.speedBody);
         }
      }
   }

   void bulletReturn(Vector3 pos, float maxDistanceReset, DroneSettingsShot settingsShot)
   {
      for (int i = 0; i < settingsShot.BoxBullet.Count; i++)
      {
         GameObject bullet = settingsShot.BoxBullet[i];

         if (bullet.activeSelf)
         {
            if (Vector3.Distance(bullet.transform.position, pos) >= maxDistanceReset)
            {
               bullet.GetComponent<Bullet>().BulletOrigen();
               i = settingsShot.BoxBullet.Capacity;
            }
         }

      }
   }
}
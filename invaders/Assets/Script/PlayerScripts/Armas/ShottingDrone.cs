using UnityEngine;

public class ShottingDrone
{
   Chronometry chronometry = new Chronometry();
   bool primeroTiro = true;

   public void GunShotting(bool attack, float speed, Vector3 pos, float maxDistanceReset, Player player)
   {
      if (attack)
      {
         player.droneStatos.droneSettingsShot.speedBody = speed;

         if (primeroTiro)
         {
            player.droneStatos.droneSettingsShot.muzzleGun.Play();
            FireBullet(player);
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

      bulletReturn(pos, maxDistanceReset, player.droneStatos.droneSettingsShot);

   }

   void FireBullet(Player player)
   {
      if (player.droneStatos.droneSettingsShot.cxBalasGun.childCount == 0)
      {
         GameObject bala = GameObject.Instantiate(player.droneStatos.droneSettingsShot.bullet);
         bala.name = "Bullet " + player.droneStatos.droneSettingsShot.cxBalasGun.root.name;
         bala.GetComponent<Bullet>().gumOrigen = player.droneStatos.droneSettingsShot.cxBalasGun;
         bala.GetComponent<Bullet>().especie = player.droneStatos.droneSettingsShot.cannon.root.GetComponent<Player>();
         bala.GetComponent<Bullet>().BulletOrigen();
         player.droneStatos.droneSettingsShot.BoxBullet.Add(bala);
      }

      if (player.droneStatos.droneSettingsShot.cxBalasGun.childCount != 0)
      {
         GameObject projetil = player.droneStatos.droneSettingsShot.cxBalasGun.GetChild(0).gameObject;

         if (!projetil.activeSelf)
         {
            player.droneStatos.droneSettingsShot.muzzleGun.Play();
            projetil.GetComponent<Bullet>().Disparate(player.droneStatos.droneSettingsShot.speedBody);
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
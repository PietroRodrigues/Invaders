using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBossNucleo : MonoBehaviour
{
    Boss bossStatos;

    void Start()
    {
        bossStatos = transform.root.GetComponent<Boss>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.collider.isTrigger)
        {
            if (other.gameObject.GetComponent<Bullet>() != null)
            {
                if (other.gameObject.GetComponent<Bullet>().isProjetilPlayer)
                {
                    bossStatos.statos.hp -= other.gameObject.GetComponent<Bullet>().DanoProjetil;

                    if (bossStatos.statos.hp <= 0)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}

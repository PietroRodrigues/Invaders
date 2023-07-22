using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    void OnEnable(){
        Destroy(gameObject, 20 * Time.deltaTime);
    }
}

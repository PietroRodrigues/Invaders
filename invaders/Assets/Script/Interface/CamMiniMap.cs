using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMiniMap : MonoBehaviour
{    
    HUD Hud;

    private void Start() {
        Hud = FindObjectOfType<HUD>();    
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = Hud.player.transform.position;
        newPos.y = transform.position.y;
        transform.position = newPos; 
        
    }
}

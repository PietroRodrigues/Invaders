using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDReticula
{    
    public HudAim hudAim;
    Player player;

    public HUDReticula(Player player,HudAim hudAim){

        this.player = player;
        this.hudAim = hudAim;
    }

   public void ReticulasUpdatePos(){

        if(player.gameObject != null){
            hudAim.miraVeiculoRect.gameObject.SetActive(true);
            hudAim.mousePosRect.gameObject.SetActive(true);
            attPos();
        }else{
            hudAim.miraVeiculoRect.gameObject.SetActive(false);
            hudAim.mousePosRect.gameObject.SetActive(false);
        }
   }

   void attPos(){
        
        if (hudAim.miraVeiculoRect != null)
        {
            hudAim.miraVeiculoRect.position = Vector3.Lerp(hudAim.miraVeiculoRect.position, Camera.main.WorldToScreenPoint(hudAim.MiraVeiculo),0.12f);
            hudAim.miraVeiculoRect.gameObject.SetActive(hudAim.miraVeiculoRect.position.z > 1f);
        }

        if (hudAim.mousePosRect != null)
        {
            hudAim.mousePosRect.position = Vector3.Lerp(hudAim.mousePosRect.position,Camera.main.WorldToScreenPoint(hudAim.MouseAimPos),0.12f);
            hudAim.mousePosRect.gameObject.SetActive(hudAim.mousePosRect.position.z > 1);
        }
    }
}

[System.Serializable]
public struct HudAim
{   
    public float aimDistance;
    public float aimCanonDistance;
    public Transform canon;
    public RectTransform miraVeiculoRect;
    public RectTransform mousePosRect;
    public Transform mouseAim;

    public Vector3 MiraVeiculo
    {
        get
        {   return (canon == null) ? Camera.main.transform.forward * aimCanonDistance
            : canon.transform.forward * aimCanonDistance + canon.transform.position;
        }

    }

    public Vector3 MouseAimPos
    {
        get
        {
            if (mouseAim != null)
            {          
                return mouseAim.position + (mouseAim.forward * aimDistance);// AimCast(mouseAim.position,mouseAim.position + (mouseAim.forward * aimDistance));
            }
            else
            {
                return Camera.main.transform.forward * aimDistance;
            }
        }
    }

    // Vector3 AimCast(Vector3 referencePoint, Vector3 target){

    //     RaycastHit hit;
        
    //     Ray ray = new Ray(referencePoint, target - referencePoint);
        
    //     bool see = Physics.Raycast(ray,out hit,Vector3.Distance(referencePoint,target),1,QueryTriggerInteraction.Ignore);

    //     if(see){
    //         return hit.point;
    //     }else{
    //         return target;
    //     }

    // }
}
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
            hudAim.miraVeiculo.gameObject.SetActive(true);
            hudAim.mousePos.gameObject.SetActive(true);
            attPos();
        }else{
            hudAim.miraVeiculo.gameObject.SetActive(false);
            hudAim.mousePos.gameObject.SetActive(false);
        }
   }

   void attPos(){
        
        if (hudAim.miraVeiculo != null)
        {
            hudAim.miraVeiculo.position = Vector3.Lerp(hudAim.miraVeiculo.position, Camera.main.WorldToScreenPoint(hudAim.MiraVeiculo),0.12f);
            hudAim.miraVeiculo.gameObject.SetActive(hudAim.miraVeiculo.position.z > 1f);
        }

        if (hudAim.mousePos != null)
        {
            hudAim.mousePos.position = Vector3.Lerp(hudAim.mousePos.position,Camera.main.WorldToScreenPoint(hudAim.MouseAimPos),0.12f);
            hudAim.mousePos.gameObject.SetActive(hudAim.mousePos.position.z > 1);
        }
    }
}

[System.Serializable]
public struct HudAim
{   
    public float aimDistance;
    public float aimCanonDistance;
    public Transform canon;
    public RectTransform miraVeiculo;
    public RectTransform mousePos;
    public Transform mouseAim;

    public Vector3 MiraVeiculo
    {
        get
        {   return (canon == null) ? Camera.main.transform.forward * aimCanonDistance
            : canon.transform.forward * Vector3.Distance(canon.transform.position, AimCast(canon.transform.position,(canon.transform.forward * aimCanonDistance) + canon.transform.position)) + canon.transform.position;
        }

    }
    
    public Vector3 MouseAimPos
    {
        get
        {           
            if (mouseAim != null)
            {          
                return AimCast(mouseAim.position,mouseAim.position + (mouseAim.forward * aimDistance));
            }
            else
            {
                return Camera.main.transform.forward * aimDistance;
            }
        }
    }

    Vector3 AimCast(Vector3 referencePoint, Vector3 target){

        RaycastHit hit;
        
        Ray ray = new Ray(referencePoint, target - referencePoint);
        
        bool see = Physics.Raycast(ray,out hit,Vector3.Distance(referencePoint,target),1,QueryTriggerInteraction.Ignore);

        if(see){
            return hit.point;
        }else{
            return target;
        }

    }
}
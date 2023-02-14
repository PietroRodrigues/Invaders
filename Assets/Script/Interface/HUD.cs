using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    [SerializeField] public HudComponentes hudComponentes;  

     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReticulasUpdate();
    }

    void ReticulasUpdate(){
        
        if (hudComponentes.miraVeiculo != null)
        {
            hudComponentes.miraVeiculo.position = Vector3.Lerp(hudComponentes.miraVeiculo.position, Camera.main.WorldToScreenPoint(hudComponentes.MiraVeiculo),0.12f);
            hudComponentes.miraVeiculo.gameObject.SetActive(hudComponentes.miraVeiculo.position.z > 1f);
        }

        if (hudComponentes.mousePos != null)
        {
            hudComponentes.mousePos.position = Vector3.Lerp(hudComponentes.mousePos.position,Camera.main.WorldToScreenPoint(hudComponentes.MouseAimPos),0.12f);
            hudComponentes.mousePos.gameObject.SetActive(hudComponentes.mousePos.position.z > 1);
        }
    }

}


[System.Serializable]
public struct HudComponentes
{
    public float aimDistance;
    public Transform canon;
    public RectTransform miraVeiculo;
    public RectTransform mousePos;
    public Transform mouseAim;

    public Vector3 MiraVeiculo
    {
        get
        {   return canon == null
            ? Camera.main.transform.forward * aimDistance
            : (canon.transform.forward * aimDistance) + canon.transform.position;
        }
    }
    
    public Vector3 MouseAimPos
    {
        get
        {           
            if (mouseAim != null)
            {          
                return mouseAim.position + (mouseAim.forward * aimDistance);
            }
            else
            {
                return Camera.main.transform.forward * aimDistance;
            }
        }
    }
}

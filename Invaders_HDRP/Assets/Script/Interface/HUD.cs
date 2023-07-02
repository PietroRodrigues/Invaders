using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] public HudComponentes hudComponentes;
    [SerializeField] public PlayerHud playerHud;
    [HideInInspector] public Player player;

    void Start() {
        player = FindObjectOfType<Player>();
        Random.InitState((int)Time.time * 1000);
    }

    private void Update() {
        playerHud.bars.transform.Find("LifeBar").GetComponent<Image>().fillAmount = (float) player.hp / player.hpMax;
        playerHud.bars.transform.Find("ShildBar").GetComponent<Image>().fillAmount = (float) player.inventario.shild / player.inventario.ShildMax;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player.gameObject != null){
            hudComponentes.miraVeiculo.gameObject.SetActive(true);
            hudComponentes.mousePos.gameObject.SetActive(true);
            ReticulasUpdate();
        }else{
            hudComponentes.miraVeiculo.gameObject.SetActive(false);
            hudComponentes.mousePos.gameObject.SetActive(false);
        }
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

        Debug.DrawLine(referencePoint,(see)? hit.point : ray.origin + ray.direction * Vector3.Distance(ray.origin,target), Color.yellow);

        if(see){
            return hit.point;
        }else{
            return target;
        }

    }
}

[System.Serializable]
public struct PlayerHud
{
    public GameObject bars;
    public GameObject EnemyDefted;
    public GameObject SpecialShoting;
    public GameObject Itens;
    public GameObject Missiles;
        
}

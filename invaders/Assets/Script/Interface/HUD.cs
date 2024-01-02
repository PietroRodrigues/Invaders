using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{   
    public HudComponents hud_Components;
    public HudAim hud_Aim;
    [HideInInspector] public Player player;  
    
    HUDReticula hud_reticula;
    HUDInventario hud_Inventario;
    HUDRadar hud_Radar;

    float deleyRespaw;

    private void Awake() {
        hud_Inventario = new HUDInventario();
    }

    void Start() {
        
        player = FindObjectOfType<Player>();
        
        hud_reticula = new HUDReticula(hud_Aim);
        hud_Radar = new HUDRadar();

        hud_Inventario.LoadInventario(hud_Components);

        Random.InitState((int)Time.time * 1000);
    }

    private void Update() {

        hud_Components.bars.transform.Find("LifeBar").GetComponent<Image>().fillAmount = (float) player.hp / player.hpMax;
        hud_Components.bars.transform.Find("ShildBar").GetComponent<Image>().fillAmount = (float) player.inventario.shield / player.inventario.ShieldMax;

       
        hud_Components.indcWelpom = hud_Inventario.UpdateIndicWelpon(hud_Components.indcWelpom);

        if(player != null){

            GameObject alvo = hud_Radar.RadarScan(player.camControler.playerInputs.inputsControl.switchTarget);

            player.alvoPos = (alvo != null)? alvo.transform.position : hud_Aim.MiraVeiculo;
        
            RespawPlayer();
            
        }


    }

    void RespawPlayer(){
       
        if(player.hp <= 0 && player.bodies > 0){
            deleyRespaw += Time.deltaTime;
            
            if(deleyRespaw >= 1){
                if(player.explosionPlayer.aliveParticleCount == 0){
                    player.bodies--;
                    player.hp = player.hpMax;
                    player.transform.position = player.checkPoint.position;
                    player.gameObject.SetActive(true);
                    deleyRespaw = 0;
                }
            }
        }

    }

    void LateUpdate()
    {
        hud_reticula.ReticulasUpdatePos(player);
        hud_Inventario.FlipWelpom(hud_Components);
    }

}

[System.Serializable]
public struct HudComponents
{
    public GameObject bars;
    public RectTransform inventario;
    public int indcWelpom;
    public float speedTranslation;
        
}

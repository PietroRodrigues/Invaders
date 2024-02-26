using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{   
    public RectTransform hudGameOver;
    public RectTransform hudGamePlay;

    public HudComponents hud_Components;
    public HudAim hud_Aim;
    public HudGameOver hud_GameOver;
    [HideInInspector] public Player player;
    
    HUDReticula hud_reticula;
    HUDInventario hud_Inventario;
    HUDRadar hud_Radar;

    float deleyRespaw;

    [HideInInspector] public int score;
    public float rankScore;
    public int rankNv;

    Dictionary<int,char> rank = new Dictionary<int,char>{
        {0,' '},
        {1,'F'},
        {2,'D'},
        {3,'C'},
        {4,'B'},
        {5,'A'},
        {6,'S'}
    };

    private void Awake() {
        hud_Inventario = new HUDInventario();
    }

    void Start() {
        
        player = FindFirstObjectByType<Player>();
        
        hud_GameOver = new HudGameOver();
        hud_reticula = new HUDReticula(hud_Aim);
        hud_Radar = new HUDRadar();

        hud_Inventario.LoadInventario(hud_Components);

        Random.InitState((int)Time.time * 1000);
    }

    private void Update() {


        if(player.bodies == 0){
            hudGameOver.gameObject.SetActive(true);
            hudGamePlay.gameObject.SetActive(false);
        }else{
            hudGameOver.gameObject.SetActive(false);
            hudGamePlay.gameObject.SetActive(true);
        }

        hud_Components.bars.transform.Find("LifeBar").GetComponent<Image>().fillAmount = (float) player.hp / player.hpMax;
        
        hud_Components.bars.transform.Find("ShildBar").GetComponent<Image>().fillAmount = (float) player.inventario.shield / player.inventario.ShieldMax;        
       
        hud_Components.indcWelpom = hud_Inventario.UpdateIndicWelpon(hud_Components.indcWelpom);

        hud_Components.bodys_txt.text = player.bodies.ToString() + "x";
        

        if(rankNv > 6)
            rankNv = 6;
        else if(rankNv < 0)
            rankNv = 0;        

        if(rankScore > 0){
            rankScore -= Time.deltaTime * (rankNv + 1);
        }
        
        if(rankScore <= 0){
            if(rankNv > 0){
                rankNv --;
                rankScore = 40 * (rankNv + 1);
            }else{
                rankNv = 0;
                rankScore = 0;
            }
        }

        float maxRankScore = 40 * (rankNv + 1);

        if(rankScore > maxRankScore){
            rankNv++;   
            rankScore = 40 * (rankNv + 1) / 4;
        }

        hud_Components.rankBar.GetComponent<Image>().fillAmount = rankScore / maxRankScore;


        hud_Components.Rank_txt.text = rank[rankNv].ToString();

        hud_Components.score_txt.text = score.ToString();
        

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

    public void GameOverBtns(bool exit){
        if(exit){
            hud_GameOver.BtnExit();
        }else{
            hud_GameOver.BtnContinue();
        }
              
    }

}

[System.Serializable]
public struct HudComponents
{
    public GameObject bars;
    public RectTransform inventario;
    public TextMeshProUGUI score_txt;
    public TextMeshProUGUI Rank_txt;
    public TextMeshProUGUI bodys_txt;
    public RectTransform comboColor;
    public GameObject rankBar;
    [HideInInspector] public int indcWelpom;
    [HideInInspector] public float speedTranslation;
        
}

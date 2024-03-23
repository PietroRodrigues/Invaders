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

    float deleyRespaw;

    [HideInInspector] public int score;
    public float rankScore;
    public int rankNv;

    [SerializeField] GameObject alvo;

    Dictionary<int,char> rank = new Dictionary<int,char>{
        {0,' '},
        {1,'F'},
        {2,'D'},
        {3,'C'},
        {4,'B'},
        {5,'A'},
        {6,'S'}
    };

    void Start() {
        
        player = FindFirstObjectByType<Player>();
        
        hud_GameOver = new HudGameOver();
        hud_reticula = new HUDReticula(hud_Aim);

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

        hud_Components.bars.transform.Find("BkLife").transform.GetChild(0).GetComponent<Image>().fillAmount = (float) player.hp / player.hpMax;
        
        hud_Components.bars.transform.Find("BkShild").transform.GetChild(0).GetComponent<Image>().fillAmount = (float) player.inventario.shield / player.inventario.ShieldMax;

        hud_Components.bodys_txt.text = player.bodies.ToString() + "x";

        Transform buffDrone = hud_Components.Buffs.transform.Find("BuffDrone");
        buffDrone.gameObject.SetActive(player.buffs.buffDrone);
        buffDrone.Find("txt").GetComponent<TextMeshProUGUI>().text = player.buffs.TimerDroneLife.ToString();

        Transform buff2X = hud_Components.Buffs.transform.Find("Buff2X");
        buff2X.gameObject.SetActive(player.buffs.buff2X);
        buff2X.Find("txt").GetComponent<TextMeshProUGUI>().text = player.buffs.Time2X.ToString();

        Transform buffFastShot = hud_Components.Buffs.transform.Find("BuffFastShot");
        buffFastShot.gameObject.SetActive(player.buffs.buffFastShot);
        buffFastShot.Find("txt").GetComponent<TextMeshProUGUI>().text = player.buffs.TimerFastShot.ToString();        
               

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
    public RectTransform Buffs;
    public TextMeshProUGUI score_txt;
    public TextMeshProUGUI Rank_txt;
    public TextMeshProUGUI bodys_txt;
    public RectTransform comboColor;
    public GameObject rankBar;
    [HideInInspector] public int indcWelpom;
    [HideInInspector] public float speedTranslation;
        
}

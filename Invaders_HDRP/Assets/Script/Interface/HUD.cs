using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{   
    [SerializeField] public HudComponents hud_Components;
    [SerializeField] public HudAim hud_Aim;
    [HideInInspector] public Player player;
    
    HUDReticula hud_reticula;
    HUDInventario hud_Inventario;

    private void Awake() {
        hud_Inventario = new HUDInventario();
    }

    void Start() {
        player = FindObjectOfType<Player>();
        
        hud_reticula = new HUDReticula(player,hud_Aim);

        hud_Inventario.LoadInventario(hud_Components);

        Random.InitState((int)Time.time * 1000);
    }

    private void Update() {
        hud_Components.bars.transform.Find("LifeBar").GetComponent<Image>().fillAmount = (float) player.hp / player.hpMax;
        hud_Components.bars.transform.Find("ShildBar").GetComponent<Image>().fillAmount = (float) player.inventario.shild / player.inventario.ShildMax;

        hud_Components.indcWelpom = hud_Inventario.UpdateIndicWelpon(hud_Components.indcWelpom);
    }

    void LateUpdate()
    {
        hud_reticula.ReticulasUpdatePos();
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

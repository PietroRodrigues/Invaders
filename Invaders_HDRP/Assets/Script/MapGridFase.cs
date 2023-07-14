using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fase Grid", menuName = "Fase")]
public class MapGridFase : ScriptableObject
{   
    public int tamanho = 6;
    [SerializeField] private List<bool[,]> fase;

    public const int defaultWidth = 6;
    public const int defaultHeight = 5;

    private const string PlayerPrefsKeyPrefix = "MapGridFase_Toggle_";

    public MapGridFase(){
        
        if(fase == null){
            fase = new List<bool[,]>();
            for (int i = 0; i < tamanho; i++)
            {
                fase.Add(new bool[defaultWidth,defaultHeight]);
            }
        }    
    }

    private void OnEnable() {
        ChargedLoad();
    }

    public void ChargedLoad(){

        for (int i = 0; i < GetFormationsList().Count; i++) {
            LoadTogglesFromPrefs(i);
        }
    }

    public List<bool[,]> GetFormationsList(){
        return fase;
    }

    public bool[,] GeFormation(int i)
    {
        return fase[i];
    }

    public void SetFormation(int i, bool[,] fase)
    {
        this.fase[i] = fase;
    }

    public bool GetFormationPos(int i,int x,int y){
        return fase[i][x,y];
    }

    public void SetFormationPos(int i,int x,int y, bool value){
        fase[i][x,y] = value;
    }

    public void LoadTogglesFromPrefs(int nivel)
    {
        for (int y = 0; y < defaultHeight; y++)
        {
            for (int x = 0; x < defaultWidth; x++)
            {
                string key = PlayerPrefsKeyPrefix + nivel + "_" + x + "_" + y;
                bool value = PlayerPrefs.GetInt(key, 0) != 0;
                SetFormationPos(nivel, x, y, value);
            }
        }
    }

    public void SaveTogglesToPrefs(int nivel)
    {
        for (int y = 0; y < defaultHeight; y++)
        {
            for (int x = 0; x < defaultWidth; x++)
            {
                string key = PlayerPrefsKeyPrefix + nivel + "_" + x + "_" + y;
                bool value = GetFormationPos(nivel, x, y);
                PlayerPrefs.SetInt(key, value ? 1 : 0);
            }
        }
    }
}


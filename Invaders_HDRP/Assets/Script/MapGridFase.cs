using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fase Grid", menuName = "Fase")]
public class MapGridFase : ScriptableObject
{   
    public int tamanho = 10;
    [SerializeField] private List<bool[,]> fase;

    public const int defaultWidth = 10;
    public const int defaultHeight = 7;

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

        for (int i = 0; i < GetNivel().Count; i++) {
            LoadTogglesFromPrefs(i);
        }
    }

    public List<bool[,]> GetNivel(){
        return fase;
    }

    public bool[,] GetFase(int i)
    {
        return fase[i];
    }

    public void SetFase(int i, bool[,] fase)
    {
        this.fase[i] = fase;
    }

    public bool GetWavePos(int i,int x,int y){
        return fase[i][x,y];
    }

    public void SetWavePos(int i,int x,int y, bool value){
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
                SetWavePos(nivel, x, y, value);
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
                bool value = GetWavePos(nivel, x, y);
                PlayerPrefs.SetInt(key, value ? 1 : 0);
            }
        }
    }
}


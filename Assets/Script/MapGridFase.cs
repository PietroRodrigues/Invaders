using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fase Grid", menuName = "Fase")]
public class MapGridFase : ScriptableObject
{   
    public int tamanho = 10;
    [SerializeField] private List<bool[,]> fase;

    public const int defaultWidth = 8;
    public const int defaultHeight = 5;

    public MapGridFase(){
        fase = new List<bool[,]>();
        for (int i = 0; i < tamanho; i++)
        {
            fase.Add(new bool[defaultWidth,defaultHeight]);
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

}


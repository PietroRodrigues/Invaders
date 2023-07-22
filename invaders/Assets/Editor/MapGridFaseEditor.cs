using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGridFase))]
public class MapGridFaseEditor : Editor
{   
    MapGridFase mapGridFase;

    private int width = 6;
    private int height = 5;

    private void OnEnable() {

        mapGridFase = (MapGridFase)target;

        for (int i = 0; i < mapGridFase.GetFormationsList().Count; i++)
        {
            mapGridFase.LoadTogglesFromPrefs(i);
        }

    }

    public override void OnInspectorGUI(){
        
        EditorGUILayout.LabelField("Map Grid Fase");

        for (int i = 0; i < mapGridFase.GetFormationsList().Count; i++)
        {
            if(width != mapGridFase.GeFormation(i).GetLength(0) || height != mapGridFase.GeFormation(i).GetLength(1)){

                bool[,] newValue = new bool[width, height];

                for(int j = 0; j < width; j++){
                    for(int k = 0; k < height; k++){
                        if(j < mapGridFase.GeFormation(i).GetLength(0) && k < mapGridFase.GeFormation(i).GetLength(1)){
                            newValue[j, k] = mapGridFase.GetFormationPos(i,j,k);
                        }
                    }
                }

                mapGridFase.SetFormation(i,newValue);

            }

        }

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < mapGridFase.GetFormationsList().Count; i++)
        {
            EditorGUILayout.LabelField("Formation " + (i+1));
            for (int y = 0; y < height; y++)
            {
                EditorGUILayout.BeginHorizontal();
              
                for (int x = 0; x < width; x++)
                {                   
                    mapGridFase.SetFormationPos(i,x,y,EditorGUILayout.Toggle(mapGridFase.GetFormationPos(i,x,y)));                
                }

                EditorGUILayout.EndHorizontal();

            }
        }

        if(EditorGUI.EndChangeCheck()){
            
            for (int i = 0; i < mapGridFase.GetFormationsList().Count; i++)
            {
                mapGridFase.SaveTogglesToPrefs(i);
            }
            EditorUtility.SetDirty(mapGridFase);
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Designer;

public class WavePaintLocation : EditorWindow
{
   private const string gradeKay = "gradeKay";
   
   List<WaveDesigner> waveDesigner = new List<WaveDesigner>();

   private Vector2 scrollPos = Vector2.zero;

   [MenuItem("GameSettings/WavePaintLocation")]
   public static void ShowWindow()
   {
      GetWindow<WavePaintLocation>();
   }

   void OnDisable(){

      for (int waveCaunt = 0; waveCaunt < waveDesigner.Count; waveCaunt++)
      {
         for (int i = 0; i < waveDesigner[waveCaunt].grade.GetLength(0); i++)
         {
            for (int j = 0; j < waveDesigner[waveCaunt].grade.GetLength(1); j++)
            {
               EditorPrefs.SetBool(gradeKay + ": " + waveCaunt + " spot: "+ i.ToString() + j.ToString(),waveDesigner[waveCaunt].grade[i,j]);
            }
         }
      }      
   }

   void OnEnable(){
      for (int i = 0; i < 11; i++)
      {
         waveDesigner.Add(new WaveDesigner(5,8));
      }

      for (int waveCaunt = 0; waveCaunt < waveDesigner.Count; waveCaunt++)
      {
         for (int i = 0; i < waveDesigner[waveCaunt].grade.GetLength(0); i++)
         {
            for (int j = 0; j < waveDesigner[waveCaunt].grade.GetLength(1); j++)
            {
               waveDesigner[waveCaunt].grade[i,j] = EditorPrefs.GetBool(gradeKay + ": " + waveCaunt + " spot: "+ i.ToString() + j.ToString(),false);
            }
         }
      } 
   }


   private void OnGUI()
   {
      GUILayout.Label("Wave Paint Location", EditorStyles.boldLabel);

      if(waveDesigner.Count == 0) return;

      scrollPos = EditorGUILayout.BeginScrollView(scrollPos); 

      for (int waveCaunt = 0; waveCaunt < waveDesigner.Count; waveCaunt++)
      {          
         GUILayout.Space(20);
         GUILayout.Label("Wave " + (waveCaunt + 1), EditorStyles.boldLabel);
             
         for (int i = 0; i < waveDesigner[waveCaunt].grade.GetLength(0); i++)
         {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < waveDesigner[waveCaunt].grade.GetLength(1); j++)
            {
               waveDesigner[waveCaunt].grade[i, j] = EditorGUILayout.Toggle("", waveDesigner[waveCaunt].grade[i, j], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
         }

      }
      
      EditorGUILayout.EndScrollView();

      EditorGUILayout.BeginHorizontal();
      if(GUILayout.Button("Aplay")){
         AplayMap();
      }
      if(GUILayout.Button("Clear")){
         ClearMap();
      }
      if(GUILayout.Button("Reset")){
         ResetMap();
      }
      EditorGUILayout.EndHorizontal();


   }

   void AplayMap(){
      SpawnerEnemys spawnerEnemys = FindObjectOfType<SpawnerEnemys>();
      spawnerEnemys.waveDesigner = waveDesigner;
      GetWindow<WavePaintLocation>().Close();
      //Quero Acessar o script da Scena aqui!!
   }

   void ClearMap(){
      SpawnerEnemys spawnerEnemys = FindObjectOfType<SpawnerEnemys>();
      spawnerEnemys.waveDesigner.Clear();
      GetWindow<WavePaintLocation>().Close();
      //Quero Acessar o script da Scena aqui!!
   }

   void ResetMap(){
      for (int waveCaunt = 0; waveCaunt < waveDesigner.Count; waveCaunt++)
      {          
         GUILayout.Space(20);
         GUILayout.Label("Wave " + (waveCaunt + 1), EditorStyles.boldLabel);
             
         for (int i = 0; i < waveDesigner[waveCaunt].grade.GetLength(0); i++)
         {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < waveDesigner[waveCaunt].grade.GetLength(1); j++)
            {
               waveDesigner[waveCaunt].grade[i, j] = EditorGUILayout.Toggle("", false, GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
         }
      }
   }

}
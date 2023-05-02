// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// public class TerrainSpawnerObjects : EditorWindow
// {
//     private Texture2D maskTrees;

//     private List<GameObject> treesPrefab = new List<GameObject>();

//     private int nPrefabs = 0;

//     private int numTrees = 1;
//     private float treeSizeMin = 0.8f;
//     private float treeSizeMax = 0.3f;

//     private const string numTreesKey = "treeMax";
//     private const string treesSizeMaxKey = "treeMaxSize";
//     private const string treesSizeMinKey = "treeMinSize";
//     private const string maskTreesKey = "maskTrees";
//     private const string treesPrefabKey = "treesPrefab";
//     private const string nPrefabsKey = "nPrefabs";

//     [MenuItem("TerrainSettings/SpawnerObject")]
//     public static void ShowWindow(){
//         GetWindow<TerrainSpawnerObjects>("SpawnerObjects");
//     }

//     void OnEnable(){

//         //Carregando Variaveis Salvas ou Defaults
        
//         nPrefabs = EditorPrefs.GetInt(nPrefabsKey,0);

//         numTrees = EditorPrefs.GetInt(numTreesKey,1);
//         treeSizeMin = EditorPrefs.GetFloat(treesSizeMinKey,0.2f);
//         treeSizeMax = EditorPrefs.GetFloat(treesSizeMaxKey,0.8f);

//         int maskId = EditorPrefs.GetInt(maskTreesKey,0);
//         maskTrees = EditorUtility.InstanceIDToObject(maskId) as Texture2D;
        
//         int[] treesPrefabID = new int[nPrefabs];
        
//         for (int i = 0; i < nPrefabs; i++)
//         {
//             treesPrefabID[i] = EditorPrefs.GetInt(treesPrefabKey + i,0);
//             treesPrefab.Add(EditorUtility.InstanceIDToObject(treesPrefabID[i]) as GameObject);
//         }
//     }
//     void OnDisable(){

//         //Salvando Variaveis;
//         EditorPrefs.SetInt(nPrefabsKey,nPrefabs);
//         EditorPrefs.SetInt(numTreesKey,numTrees);
//         EditorPrefs.SetFloat(treesSizeMinKey,treeSizeMin);
//         EditorPrefs.SetFloat(treesSizeMaxKey,treeSizeMax);

//         int maskId = maskTrees != null ? maskTrees.GetInstanceID() : 0;
//         EditorPrefs.SetInt(maskTreesKey,maskId);

//         int[] treesPrefabID = new int[nPrefabs];

//         for (int i = 0; i < nPrefabs; i++)
//         {
//             treesPrefabID[i] = treesPrefab[i] != null ? treesPrefab[i].GetInstanceID() : 0;
//             EditorPrefs.SetInt(treesPrefabKey + i,treesPrefabID[i]); 
//         }

//     }

//     void OnGUI()
//     {
//         GUILayout.Label("Tree Settings",EditorStyles.boldLabel);
        
//         GUILayout.Label("Tree Mask",EditorStyles.boldLabel);
        
//         maskTrees = EditorGUILayout.ObjectField(maskTreesKey,maskTrees,typeof(Texture2D),false) as Texture2D;

//         GUILayout.BeginHorizontal();

//         if (GUILayout.Button("Add Prefab"))
//         {
//             treesPrefab.Add(null);
//         }

//         if (GUILayout.Button("Remove Prefab"))
//         {
//             treesPrefab.RemoveAt(treesPrefab.Count -1);
//         }

//         GUILayout.EndHorizontal();

//         nPrefabs = treesPrefab.Count;

//         // List of gameObjects
//         for (int i = 0; i < nPrefabs; i++)
//         {
//             treesPrefab[i] = EditorGUILayout.ObjectField(treesPrefab[i], typeof(GameObject), true) as GameObject;
//         }       
        
//         numTrees = EditorGUILayout.IntSlider(numTreesKey,numTrees,1,5000);
//         treeSizeMin = EditorGUILayout.Slider(treesSizeMinKey,treeSizeMin,0.1f,1.0f);
//         treeSizeMax = EditorGUILayout.Slider(treesSizeMaxKey,treeSizeMax,0.1f,1.0f);

//         if(GUILayout.Button("Carrega Prefabs e Mascara"))
//         {
//             ChargerPrefabs();
//         }

//         if(GUILayout.Button("Aplicate"))
//         {
//             ApplySettings();
//         }
//     }

//     void ApplySettings()
//     {
//         foreach (Terrain terrain in Terrain.activeTerrains)
//         {
//             CreateTreeOnTerrain(terrain);
//         }
//     }

//     void ChargerPrefabs()
//     {
//         foreach (Terrain terrain in Terrain.activeTerrains)
//         {
//             Charge(terrain);
//         }
//     }

//     void Charge(Terrain terrain){
        
//         terrain.terrainData.terrainLayers = AddElementToArray(terrain.terrainData.terrainLayers,new TerrainLayer{diffuseTexture = maskTrees});

//         TerrainLayer newLayer = terrain.terrainData.terrainLayers[terrain.terrainData.terrainLayers.Length -1];

//         newLayer.diffuseTexture = maskTrees;
//         newLayer.tileSize = new Vector2(200,200);
//         newLayer.tileOffset = new Vector2(0,0);

//         foreach (GameObject tree in treesPrefab)
//         {
//             TreePrototype newTree = new TreePrototype();
//             newTree.prefab = tree;
//             terrain.terrainData.treePrototypes = AddElementToArray(terrain.terrainData.treePrototypes,newTree);
//         }
//     }

//     void CreateTreeOnTerrain(Terrain terrain)
//     {

//         List<TreeInstance> trees = new List<TreeInstance>();

//         while (trees.Count < numTrees)
//         {
//             if (trees.Count % 10 == 0 && EditorUtility.DisplayCancelableProgressBar("Criando Árvores...", "Árvores " + trees.Count, trees.Count / (float)numTrees))
//             {
//                 break;
//             }

//             Vector3 pos = new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f));
//             Color pixelMask = terrain.terrainData.terrainLayers[1].diffuseTexture.GetPixelBilinear(pos.x, pos.z);

//             if (pixelMask.g > 0.4f)
//             {
//                 TreeInstance tree = new TreeInstance();
//                 float height = Random.Range(treeSizeMin, treeSizeMax);
//                 float width = height * Random.Range(treeSizeMin, treeSizeMax);
//                 tree.heightScale = height;
//                 tree.widthScale = width;
//                 tree.prototypeIndex = Random.Range(0, terrain.terrainData.treePrototypes.Length);
//                 tree.rotation = Random.Range(0, 360.0f);
//                 tree.position = pos;
//                 trees.Add(tree);
//             }
//         }

//         EditorUtility.ClearProgressBar();
//         terrain.terrainData.SetTreeInstances(trees.ToArray(), true);
//     }

//     public static T[] AddElementToArray<T>(T[] array, T element)
//     {
//         List<T> list = new List<T>(array);
//         list.Add(element);
//         return list.ToArray();
//     }

// }

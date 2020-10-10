using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
public class EnemyCharacterCreator : EditorWindow
{

    string prefabName = "Enemy";
    //bool groupEnabled;
    int layer = 14;
    //----
    //bool damageable, enemyController, sphereCollider, navMeshAgent;
    bool playerInFOV;
    bool patrol, attack;
    bool humanoid;
    Transform handPosition;
    RuntimeAnimatorController animController;
    //----
    GameObject gameObject;
    [MenuItem("Window/Enemy Prefab Creator")]
    static void Init() {
        // Get existing open window or if none, make a new one:
        EnemyCharacterCreator window = (EnemyCharacterCreator)EditorWindow.GetWindow(typeof(EnemyCharacterCreator));
        window.Show();
    }

    void OnGUI() {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        prefabName = EditorGUILayout.TextField("Prefab name", prefabName);
        gameObject = EditorGUILayout.ObjectField("Game Object", gameObject, typeof(GameObject), true) as GameObject;
        if(gameObject != null) {
            animController = EditorGUILayout.ObjectField("Animator Controller", animController, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
        } else {
            animController = null;
        }
        layer = EditorGUILayout.LayerField("Layer", layer);
        playerInFOV = EditorGUILayout.Toggle("Check player in FOV", playerInFOV);
        patrol = EditorGUILayout.Toggle("Patrol", patrol);
        attack = EditorGUILayout.Toggle("Attack", attack);
        humanoid = EditorGUILayout.Toggle("is Humanoid", humanoid);
        if(humanoid) {
            handPosition = EditorGUILayout.ObjectField("Right Hand Transform", handPosition, typeof(Transform), true) as Transform;
            //handPosition = (Transform)EditorGUILayout.ObjectField(new GUIContent("hand"), handPosition, typeof(Transform));
        } else {
            handPosition = null;
        }

        if(GUILayout.Button("Add components")) {
            AddComponents();
        }

        if(GUILayout.Button("Create as prefab.")) {
            CreatePrefab();
        }
        
    }
    void AddComponents() {
        gameObject.AddComponent<Damageable>();
        gameObject.AddComponent<EnemyController>();
        if(animController != null) {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = animController;
            gameObject.GetComponent<Animator>().applyRootMotion = false;
        }
        if(playerInFOV) {
            gameObject.AddComponent<SphereCollider>();
            gameObject.AddComponent<EnemyCheck_PlayerInSight>();
            gameObject.layer = layer;
        }
        if(patrol) {

            gameObject.AddComponent<NavMeshAgent>();
            gameObject.AddComponent<EnemyBehavior_Patrol>();
        }
        if(attack) {
            gameObject.AddComponent<EnemyBehavior_Attack>();
        }
        if(humanoid) {
            GameObject g = new GameObject();
            g.name = "Weapon Position";
            g.transform.SetParent(handPosition);
            g.transform.localPosition = new Vector3(14.07f, 2.86f, -3.16f);
            ;
            g.transform.localRotation = Quaternion.Euler(0f, 90f, -90f);
            g.transform.localScale = new Vector3(100f, 100f, 100f);
            //humanoid weapon position and everything else.
        }
    }

    void CreatePrefab() {
        // Keep track of the currently selected GameObject(s)
        // Loop through every GameObject in the array above
    
        // Set the path as within the Assets folder,
        // and name it as the GameObject's name with the .Prefab format
        string localPath = "Assets/Prefabs/Enemies/" + prefabName + ".prefab";
        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
    }

}

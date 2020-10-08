using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController)), CanEditMultipleObjects]
public class EnemyControllerEditor : Editor
{

    public override void OnInspectorGUI() {
        EnemyController enemy = (EnemyController)target;
        DrawDefaultInspector();

    }

}

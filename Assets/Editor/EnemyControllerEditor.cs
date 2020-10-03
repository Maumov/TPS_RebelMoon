using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController)), CanEditMultipleObjects]
public class EnemyControllerEditor : Editor
{

    int firstSelected = -1;
    protected virtual void OnSceneGUI() {

        EnemyController enemy = (EnemyController)target;
        if(enemy.guardMode == GuardingMode.Patrol) {
            Patrol(enemy);
        } else {
            
            Idle(enemy);
            
        }
    }

    void Patrol(EnemyController enemy) {
        if(enemy.waypoints.Count == 0) {
            CreateFirstWaypoint(enemy);
        }
        if(enemy.waypoints.Count == 1) {
            AddWaypoint(enemy);
        }
        int j = 0;
        while(j < enemy.waypoints.Count){
            Waypoint w = enemy.waypoints[j];
            j++;
            EditorGUI.BeginChangeCheck();
            int indexOfW = enemy.waypoints.IndexOf(w);
            //Show Area of Waypoint
            Handles.color = new Color(1f, 1f, 1f, 1f);
            w.areaRadius = Handles.RadiusHandle(Quaternion.identity, w.position, w.areaRadius);

            //Manages the connection to other waypoints
            bool pressed = Handles.Button(w.position + new Vector3(0, 2, 0), Quaternion.LookRotation(Vector3.up), 0.5f, 0.5f, Handles.RectangleHandleCap);
            if(pressed) {
                if(firstSelected == -1) {
                    firstSelected = indexOfW;
                } else {
                    if(indexOfW == firstSelected) {
                        firstSelected = -1;
                    } else {
                        if(!enemy.waypoints[firstSelected].connections.Contains(indexOfW)) {
                            w.AddConnection(firstSelected);
                            enemy.waypoints[firstSelected].connections.Add(indexOfW);
                        }
                        firstSelected = -1;
                    }
                }
            }
            //Shows connection of the waypoint
            Handles.color = Color.cyan;
            for(int i = 0; i < w.connections.Count; i++) {
                Vector3 direction = enemy.waypoints[w.connections[i]].position - w.position;
                direction.Normalize();

                Vector3 perpendicularStart = new Vector3(-direction.z, direction.y, direction.x);
                //Vector3 perpendicularEnd = new Vector3(direction.z, direction.y, -direction.x);
                Vector3 tangentStart = w.position + direction + perpendicularStart;
                Vector3 tangentEnd = enemy.waypoints[w.connections[i]].position - direction + perpendicularStart;

                Handles.DrawBezier(w.position, enemy.waypoints[w.connections[i]].position, tangentStart, tangentEnd, indexOfW == firstSelected ? Color.cyan : Color.magenta, null, 3f);
                //Handles.DrawLine(w.position, enemy.waypoints[w.connections[i]].position);
            }
            //manages the position of the waypoint
            Vector3 newTargetPosition;
            if(indexOfW != 0) {
                newTargetPosition = Handles.PositionHandle(w.position, Quaternion.identity);
            } else {
                newTargetPosition = enemy.transform.position;
                w.position = newTargetPosition;
            }
            //manages the color of the area of the waypoint.

            if(firstSelected == indexOfW) {
                //this is the waypoint selected
                Handles.color = new Color(1f, 0.2f, 0.2f, 0.2f);
            } else {
                //No Waypont selected
                Handles.color = new Color(0.2f, 1f, 0.2f, 0.2f);
            }
            Handles.DrawSolidDisc(w.position, Vector3.up, w.areaRadius);

            if(EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(enemy, "Change Look At Target Position");
                w.position = newTargetPosition;
            }

            
                //Show UI for selected object;
                SelectedWaypointUI(enemy);
            
        }
    }

    void CreateFirstWaypoint(EnemyController enemy) {
        Waypoint w = new Waypoint();
        w.areaRadius = 1f;
        w.position = enemy.transform.position;
        w.connections = new List<int>();
        enemy.waypoints.Add(w);
    }

    void AddWaypoint(EnemyController enemy) {
        Waypoint w = new Waypoint();
        w.areaRadius = 1f;
        w.position = enemy.waypoints[enemy.waypoints.Count-1].position + Vector3.forward;
        w.connections = new List<int>();
        //set bidirectional waypoint connection
        w.connections.Add(enemy.waypoints.Count - 1);
        enemy.waypoints[enemy.waypoints.Count - 1].connections.Add(enemy.waypoints.Count);

        enemy.waypoints.Add(w);
    }

    void DeleteWaypoint(EnemyController enemy) {
        //Remove connections from other waypoints to the waypoint to be deleted.
        for(int i = 0; i < enemy.waypoints.Count; i++) {
            if(i != firstSelected) {
                if(enemy.waypoints[i].connections.Contains(firstSelected)) {
                    enemy.waypoints[i].connections.Remove(firstSelected);
                }
            }
        }
        for(int i = 0; i < enemy.waypoints.Count; i++) {
            if(i != firstSelected) {
                for(int j = 0; j < enemy.waypoints[i].connections.Count; j++) {
                    if(enemy.waypoints[i].connections[j] >= firstSelected) {
                        enemy.waypoints[i].connections[j]--;
                        if(enemy.waypoints[i].connections[j] < 0) {
                            enemy.waypoints[i].connections.Remove(j);
                        }
                    }
                }
            }
        }

        //remove the waypoint from existance
        enemy.waypoints.Remove(enemy.waypoints[firstSelected]);
        firstSelected = -1;
    }

    void AddWaypointToSelected(EnemyController enemy, int selectedWayPoint) {
        Waypoint w = new Waypoint();
        w.areaRadius = 1f;
        w.position = enemy.waypoints[selectedWayPoint].position + Vector3.forward;
        w.connections = new List<int>();
        //set bidirectional waypoint connection
        w.connections.Add(selectedWayPoint);
        enemy.waypoints[selectedWayPoint].connections.Add(enemy.waypoints.Count);

        enemy.waypoints.Add(w);
    }

    void SelectedWaypointUI(EnemyController enemy) {
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(20, 20, 150, 80));
        var rect = EditorGUILayout.BeginVertical();
        GUI.Box(rect, GUIContent.none);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Waypoints Handler");
        GUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;
        GUILayout.BeginVertical();
        if(GUILayout.Button("Add")) {
            AddWaypoint(enemy);
        }
        if(firstSelected != -1) {
            if(GUILayout.Button("Add connected")) {
                AddWaypointToSelected(enemy, firstSelected);
            }
            if(firstSelected != 0) {
                if(GUILayout.Button("Delete")) {
                    DeleteWaypoint(enemy);
                }
            }
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
        Handles.EndGUI();
    }
    void Idle(EnemyController enemy) {
        enemy.waypoints = new List<Waypoint>();
    }
    public override void OnInspectorGUI() {
        EnemyController enemy = (EnemyController)target;
        DrawDefaultInspector();
        if(GUILayout.Button("Add Waypoint")) {
            AddWaypoint(enemy);
            //add everthing the button would do.
        }
    }

}

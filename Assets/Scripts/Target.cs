using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Material Selected, NotSelected;
    private void Start() {
        UnTargeted();
    }
    public void Targeted() {
        GetComponentInChildren<MeshRenderer>().material = Selected;
    }

    public void UnTargeted() {
        GetComponentInChildren<MeshRenderer>().material = NotSelected;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteracted;

    public void OnInteract() {
        Debug.Log("Interacted with: "+name);
        OnInteracted.Invoke();
    }
}

public interface IInteractable
{
    void OnInteract();

}

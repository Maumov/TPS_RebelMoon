using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    public int LockKeyId = -1;
    public UnityEvent OnSuccess, OnFail;
    public virtual void OnInteract(Gear gear) {
        Debug.Log("Interacted with: "+name);
        if(LockKeyId == -1) {
            OnSuccess.Invoke();
        } else {
            if(gear.HasItemId(LockKeyId)) {
                OnSuccess.Invoke();
            } else {
                OnFail.Invoke();
            }
        }
    }
}

public interface IInteractable
{
    void OnInteract(Gear gear);

}

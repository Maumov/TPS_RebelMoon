using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public Transform weaponPosition;
    public int currentWeaponSelected;
    public Weapon CurrentlyEquipped;
    public GameObject currentWeaponGameObject;
    public List<Weapon> WeaponInventory;
    public List<Item> ItemsInventory;
    public List<MonoBehaviour> OnGearUse;

    private void Start() {
    }

    public void AddItem(Item i) {
        ItemsInventory.Add(i);
    }

    public void AddWeapon(Weapon w) {
        if(!WeaponInventory.Contains(w)) {
            WeaponInventory.Add(w);
        }
        EquipNextWeapon();
    }

    public void EquipNextWeapon() {
        currentWeaponSelected++;
        if(currentWeaponSelected >= WeaponInventory.Count) {
            currentWeaponSelected = 0;
        }

        Equip();
    }
    public void EquipPreviousWeapon() {
        currentWeaponSelected--;
        if(currentWeaponSelected < 0) {
            currentWeaponSelected = WeaponInventory.Count - 1;
        }
        Equip();
    }

    void Equip() {
        CurrentlyEquipped = WeaponInventory[currentWeaponSelected];
        EquipWeapon(CurrentlyEquipped);
    }

    public void EquipWeapon(Weapon w) {
        currentWeaponGameObject = Instantiate(w.gameObject, transform.position, Quaternion.identity);
        CurrentlyEquipped = currentWeaponGameObject.GetComponent<Weapon>();
        currentWeaponGameObject.transform.parent = weaponPosition;
        currentWeaponGameObject.transform.localPosition = Vector3.zero;
        currentWeaponGameObject.transform.localRotation = Quaternion.identity;

        ItemMessage data;
        data.item = CurrentlyEquipped;
        var messageType = MessageType.EQUIP;
        for(var i = 0; i < OnGearUse.Count; ++i) {
            var receiver = OnGearUse[i] as IMessageReceiver;
            receiver.OnReceiveMessage(messageType, this, data);
        }
    }

    public void Attack(Transform cam, bool buttonDown, bool buttonUp) {
        if(CurrentlyEquipped != null) {
            CurrentlyEquipped.Attack(cam.transform, buttonDown, buttonUp);
        }
    }

    public void Reload() {
        if(CurrentlyEquipped != null) {
            CurrentlyEquipped.Reload();
        }
    }


    public void Attack2() {

    }

    public bool HasItemId(int id) {
        foreach(Item i in ItemsInventory) {
            if(i.id == id) {
                return true;
            }
        }
        return false;
    }

    public struct ItemMessage
    {
        public Weapon item;
    }

}

//[System.Serializable]
//public class InventorySlot
//{
//    public Weapon weapon;
//    public bool available;
//}




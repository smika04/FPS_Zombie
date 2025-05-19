using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots = new List<GameObject>();
    public GameObject activeWeaponSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

    }

    public void PickUpWeapon(GameObject weapon)
    {
       AddWeaponIntoActiveSlot(weapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedWeapon)
    {
        DropCurrectWeapon(pickedWeapon);

        // 1) Прикріплюємо під слот, без збереження світових координат
        pickedWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        // 2) Робимо локальну позицію та ротацію = (0,0,0),
        //    щоб зброя опинилася точно там, де сам слот
        pickedWeapon.transform.localPosition = Vector3.zero;
        pickedWeapon.transform.localRotation = Quaternion.Euler(0, 180f, 0);

        // 3) Прапорець активності
        var weapon = pickedWeapon.GetComponent<Weapon>();
        weapon.isActiveWeapon = true;

        // 4) (Опційно) вимикаємо фізику, якщо є Rigidbody
        var rb = pickedWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DropCurrectWeapon(GameObject pickedWeapon)
    {
       if(activeWeaponSlot.transform.childCount > 0)
       {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;

            weaponToDrop.transform.SetParent(pickedWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedWeapon.transform.localRotation;

        }
    }


    public void SwitchActiveSlot(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount > 0 )
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }
}

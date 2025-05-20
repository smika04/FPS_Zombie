using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots = new List<GameObject>();
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

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

    public int CheckAmmoLeftFor(WeaponModel Model)
    {
        switch (Model)
        {
            case WeaponModel.M11911:
                return totalPistolAmmo;
            case WeaponModel.Uzi:
                return totalRifleAmmo;
            case WeaponModel.AK74:
                return totalRifleAmmo;
            default:
                return 0;
        }
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedWeapon)
    {
        DropCurrectWeapon(pickedWeapon);

        pickedWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        pickedWeapon.transform.localPosition = Vector3.zero;
        pickedWeapon.transform.localRotation = Quaternion.Euler(0, 180f, 0);

        var weapon = pickedWeapon.GetComponent<Weapon>();
        weapon.isActiveWeapon = true;

        var rb = pickedWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

       //weapon.animator.enabled = true;
    }

    private void DropCurrectWeapon(GameObject pickedWeapon)
    {
       if(activeWeaponSlot.transform.childCount > 0)
       {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            //weaponToDrop.GetComponent<Weapon>().animator.enabled = true;

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

    public void PickUpAmmoBox(AmmoBox ammo)
    {
        switch(ammo.ammoType)
        {
            case AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
            case AmmoType.GeneralAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsLeft, WeaponModel thisWeaponModel)
    {
        switch(thisWeaponModel)
        {
            case WeaponModel.M11911:
                totalPistolAmmo -= bulletsLeft;
                break;
            case WeaponModel.Uzi:
                totalRifleAmmo -= bulletsLeft;
                break;
            case WeaponModel.AK74:
                totalRifleAmmo -= bulletsLeft;
                break;
        }
    }
}

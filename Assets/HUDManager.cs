using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethaUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySprite;

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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();

        GameObject unActiveSlot = GetUnActiveWeaponSlot();
        Debug.Log("UnActive Weapon Slot: " + (unActiveSlot != null ? unActiveSlot.name : "null"));

        Weapon unActiveWeapon = unActiveSlot != null ? unActiveSlot.GetComponentInChildren<Weapon>() : null;
        Debug.Log("UnActive Weapon: " + (unActiveWeapon != null ? unActiveWeapon.thisWeaponModel.ToString() : "null"));

        Debug.Log("Active Weapon Model: " + (activeWeapon != null ? activeWeapon.thisWeaponModel.ToString() : "null"));

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

            WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                Sprite weaponSprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
                Debug.Log("Weapon sprite loaded: " + (weaponSprite != null));
                unActiveWeaponUI.sprite = weaponSprite;
            }
        }
        else
        {
            magazineAmmoUI.text = " ";
            totalAmmoUI.text = " ";

            ammoTypeUI.sprite = emptySprite;

            activeWeaponUI.sprite = emptySprite;
            unActiveWeaponUI.sprite = emptySprite;
        }
    }


    private Sprite GetWeaponSprite(WeaponModel model)
    {
        string path = "";

        switch (model)
        {
            case WeaponModel.M11911:
                path = "M11911_Weapon";
                break;
            case WeaponModel.Uzi:
                path = "Uzi_Weapon";
                break;
            case WeaponModel.AK74:
                path = "AK74_Weapon";
                break;
            default:
                Debug.LogWarning("Unknown weapon model: " + model);
                return null;
        }

        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"Prefab '{path}' not found in Resources folder!");
            return null;
        }
        else
        {
            Debug.Log($"Prefab '{path}' loaded successfully.");
        }

        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError($"No SpriteRenderer component found on prefab '{path}'!");
            return null;
        }
        else
        {
            Debug.Log($"SpriteRenderer found on prefab '{path}', sprite name: {sr.sprite.name}");
        }

        return sr.sprite;
    }





    private Sprite GetAmmoSprite(WeaponModel model)
    {
        GameObject prefab = null;

        switch (model)
        {
            case WeaponModel.M11911:
                prefab = Resources.Load<GameObject>("Pistol_Ammo");
                break;
            case WeaponModel.Uzi:
                prefab = Resources.Load<GameObject>("Rifle_Ammo");
                break;
            case WeaponModel.AK74:
                prefab = Resources.Load<GameObject>("Rifle_Ammo");
                break;
        }

        if (prefab != null)
        {
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null)
                return sr.sprite;
            else
                Debug.LogWarning("SpriteRenderer не знайдений на префабі " + prefab.name);
        }
        else
        {
            Debug.LogWarning("Префаб для патронів не знайдений для моделі " + model);
        }

        return null;
    }



    private GameObject GetUnActiveWeaponSlot()
    {


        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }
}

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
        Weapon unActiveWeapon = ((GameObject)GetUnActiveWeaponSlot()).GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

            WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            if(unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
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
        switch(model)
        {
            case WeaponModel.M11911:
                return Instantiate(Resources.Load<GameObject>("M11911_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case WeaponModel.Uzi:
                return Instantiate(Resources.Load<GameObject>("Uzi_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case WeaponModel.AK74:
                return Instantiate(Resources.Load<GameObject>("AK74_Weapon")).GetComponent<SpriteRenderer>().sprite; 

            default:
                return null;

        }
    }

    private Sprite GetAmmoSprite(WeaponModel model)
    {
        switch (model)
        {
            case WeaponModel.M11911:
                return Instantiate(Resources.Load<GameObject>("Pistol_Ammo")).GetComponent<SpriteRenderer>().sprite;
            case WeaponModel.Uzi:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponent<SpriteRenderer>().sprite;
            case WeaponModel.AK74:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;

        }
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

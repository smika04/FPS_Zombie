using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;

    public AudioSource shootingSound_M11911;
    public AudioSource relodingSound_M11911;

    public AudioClip shootSound_Uzi;
    public AudioSource relodingSound_Uzi;

    public AudioSource emptySound_M11911;

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

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M11911:
                shootingSound_M11911.Play();
                break;
            case WeaponModel.Uzi:
                ShootingChannel.PlayOneShot(shootSound_Uzi);
                break;

        }
    }

    public void PlayEmptySound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M11911:
                emptySound_M11911.Play();
                break;
            case WeaponModel.Uzi:
                emptySound_M11911.Play();
                break;

        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M11911:
                relodingSound_M11911.Play();
                break;
            case WeaponModel.Uzi:
                relodingSound_Uzi.Play();
                break;

        }
    }
}
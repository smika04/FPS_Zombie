using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    public int bulletsPerBurst = 3;
    public int burstBullletsLeft;

    public float spreadIntensity;

    public GameObject bulletPrefab;             
    public Transform bulletSpawn;             
    public float bulletVelocity = 30;         
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    //internal Animator animator;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isRealoding;

    public ShootingMode currentShootingMode;
    public WeaponModel thisWeaponModel;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
            
    private void Awake()
    {
        readyToShoot = true; // ����� ������ �� �������
        burstBullletsLeft = bulletsPerBurst; // ����������� ������� ������� � ����

        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            if (bulletsLeft <= 0 && isShooting)
            {
                SoundManager.Instance.PlayEmptySound(thisWeaponModel);
            }

            // ���������, �� ������� ������� ������ ������� ������� �� ������ �������
            if (currentShootingMode == ShootingMode.Auto)
            {
                // ��� ������������� ������ ��������� ������ ����
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                // ��� ���������� ��� ��������� ������ � �������� ���������� ������ ����
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isRealoding == false)
            {
                Reload();
                //SoundManager.Instance.realodingSound_M.Play();

                SoundManager.Instance.PlayReloadSound(thisWeaponModel);
            }

            //Automatic
            //if(readyToShoot && isShooting == false && isRealoding == false && bulletsLeft <= 0)
            //{
            //    Reload();
            //}

            // ���� ����� ������ �� ������� � ������� ������

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBullletsLeft = bulletsPerBurst; // ������� ������� ������� � ����
                FireWeapon(); // ��������� �������
            }
        }
    }

    // ����� ��������� �� ������� ���
    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // ��������� ���� � ������ �������
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        // ������ ���� �� ��� � �������� forward ����� ������
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // ��������� �������� ��� �������� ��� ����� ������� ���
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if(currentShootingMode == ShootingMode.Burst && burstBullletsLeft > 1)
        {
            burstBullletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        isRealoding = true;

        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isRealoding = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    // ����� ���������� �������� ������� � ����������� ����������
    public Vector3 CalculateDirectionAndSpread()
    {
        // ��������� ������ �� ������ ������
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        // ���� ������ ���� ������ � ������ ����� ��������
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            // ���� � � �������� "�������"
            targetPoint = ray.GetPoint(100);
        }

        // ����������� �������� �� ����� ����� ��� �� ���
        Vector3 direction = targetPoint - bulletSpawn.position;

        // ������ ���������� �� ���� X �� Y
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // ��������� ���������� �������� � ����������� ����������
        return direction + new Vector3(x, y, 0);
    }

    // �������� ����� ��������� ���
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
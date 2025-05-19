using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isRealoding;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true; // ����� ������ �� �������
        burstBullletsLeft = bulletsPerBurst; // ����������� ������� ������� � ����

        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (bulletsLeft <= 0 && isShooting)
        {
            SoundManager.Instance.emptySound_M.Play();
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

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isRealoding == false)
        {
            Reload();
            SoundManager.Instance.realodingSound_M.Play();
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

        if (AmmoManager_.Instance.ammoDisplay != null)
        {
            AmmoManager_.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
        }
    }

    // ����� ��������� �� ������� ���
    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        SoundManager.Instance.shootingSound_M.Play();

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
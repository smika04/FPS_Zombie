using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    void Update()
    {
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

        // ���� ����� ������ �� ������� � ������� ������
        if (readyToShoot && isShooting)
        {
            burstBullletsLeft = bulletsPerBurst; // ������� ������� ������� � ����
            FireWeapon(); // ��������� �������
        }
    }

    // ����� ��������� �� ������� ���
    private void FireWeapon()
    {
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
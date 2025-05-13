using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �����������, ���� ��'��� ����������� � ����� ����������
    private void OnCollisionEnter(Collision objectHit)
    {
        // ����������, �� ��'���, � ���� ������� ��������, �� ��� "Target"
        if (objectHit.gameObject.CompareTag("Target"))
        {
            print("hit " + objectHit.gameObject.name + "!");

            CreateBulletImpactEffect(objectHit);

            // ������� ���� ���� ���������
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Wall"))
        {
            print("hit " + objectHit.gameObject.name + "!");

            CreateBulletImpactEffect(objectHit);

            // ������� ���� ���� ���������
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Beer"))
        {
            print("hit " + objectHit.gameObject.name + "!");

            objectHit.gameObject.GetComponent<BeerBottle>().Shatter();
        }
    }

    // ����� ��� ��������� ������ �������� ��� �� ��'����
    void CreateBulletImpactEffect(Collision objectHit)
    {
        // �������� ����� ����� �������� ��� �������
        ContactPoint contact = objectHit.contacts[0];

        // ��������� ����� (���������, ��� ���) � ����� ��������
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab, // ������ ������ � ����������� ���������
            contact.point,                                      // ���������� ����� ��������
            Quaternion.LookRotation(contact.normal)            // �������� ������ �� �������� �������
        );

        // ����'����� ��������� ��'��� �� ��'����, � ���� ������� ����
        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}

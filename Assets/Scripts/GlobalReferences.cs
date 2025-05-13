using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��� ��������� ���������� �������� (������ Singleton)
public class GlobalReferences : MonoBehaviour
{
    // �������� ��������� �� �������� ��������� �����
    public static GlobalReferences Instance { get; set; }

    // ������ ������ �������� ���
    public GameObject bulletImpactEffectPrefab;

    // ����� Awake ����������� �� ��� ����������� ��'����
    private void Awake()
    {
        // ���� ��������� ��� ���� � �� �� ��� ����� ��'��� � ������� ��������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // ������ �������� ��� ��'��� �� ���������� ���������
            Instance = this;
        }
    }
}
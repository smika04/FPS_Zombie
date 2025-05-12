using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;    

    float xRotation = 0f;                   
    public float yRotation = 0f;           

    public float topClamp = -90f;            
    public float bottomClamp = 90f;         

    void Start()
    {
        // ������ ������ ���� �� ������� ���� � ����� ������
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // �������� ��� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ���������� ������� �� X (�����-����)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // ���������� ������� �� Y (����-������)
        yRotation += mouseX;

        // ����������� ��������� �� ��'���� (�������, ������)
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}

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
        // Ховаємо курсор миші та блокуємо його в центрі екрану
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Отримуємо рух миші
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Обчислюємо поворот по X (вверх-вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Обчислюємо поворот по Y (вліво-вправо)
        yRotation += mouseX;

        // Застосовуємо обертання до об'єкта (ймовірно, камери)
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}

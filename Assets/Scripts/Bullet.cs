using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Викликається, коли об'єкт зіштовхується з іншим колайдером
    private void OnCollisionEnter(Collision collision)
    {
        // Перевіряємо, чи об'єкт, з яким сталося зіткнення, має тег "Target"
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + "!");
            // Знищуємо кулю після попадання
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit " + collision.gameObject.name + "!");
            // Знищуємо кулю після попадання
            Destroy(gameObject);
        }
    }
}

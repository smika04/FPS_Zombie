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

            CreateBulletImpactEffect(collision);

            // Знищуємо кулю після попадання
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit " + collision.gameObject.name + "!");

            CreateBulletImpactEffect(collision);

            // Знищуємо кулю після попадання
            Destroy(gameObject);
        }
    }

    // Метод для створення ефекту влучення кулі по об'єкту
    void CreateBulletImpactEffect(Collision objectHit)
    {
        // Отримуємо першу точку контакту при зіткненні
        ContactPoint contact = objectHit.contacts[0];

        // Створюємо ефект (наприклад, слід кулі) у точці контакту
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab, // беремо префаб з глобального посилання
            contact.point,                                      // координати точки зіткнення
            Quaternion.LookRotation(contact.normal)            // орієнтація ефекту за нормаллю поверхні
        );

        // Прив'язуємо створений об'єкт до об'єкта, в який влучила куля
        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}

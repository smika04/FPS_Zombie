using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Викликається, коли об'єкт зіштовхується з іншим колайдером
    private void OnCollisionEnter(Collision objectHit)
    {
        // Перевіряємо, чи об'єкт, з яким сталося зіткнення, має тег "Target"
        if (objectHit.gameObject.CompareTag("Target"))
        {
            print("hit " + objectHit.gameObject.name + "!");

            CreateBulletImpactEffect(objectHit);

            // Знищуємо кулю після попадання
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Wall"))
        {
            print("hit " + objectHit.gameObject.name + "!");

            CreateBulletImpactEffect(objectHit);

            // Знищуємо кулю після попадання
            Destroy(gameObject);
        }
        if (objectHit.gameObject.CompareTag("Beer"))
        {
            print("hit " + objectHit.gameObject.name + "!");

            objectHit.gameObject.GetComponent<BeerBottle>().Shatter();
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

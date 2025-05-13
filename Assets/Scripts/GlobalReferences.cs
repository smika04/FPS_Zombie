using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  лас дл€ збер≥ганн€ глобальних посилань (патерн Singleton)
public class GlobalReferences : MonoBehaviour
{
    // —татичне посиланн€ на поточний екземпл€р класу
    public static GlobalReferences Instance { get; set; }

    // ѕрефаб ефекту влученн€ кул≥
    public GameObject bulletImpactEffectPrefab;

    // ћетод Awake викликаЇтьс€ п≥д час ≥н≥ц≥ал≥зац≥њ об'Їкта
    private void Awake()
    {
        // якщо екземпл€р вже ≥снуЇ ≥ це не цей самий об'Їкт Ч знищити поточний
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // ≤накше зберегти цей об'Їкт €к глобальний екземпл€р
            Instance = this;
        }
    }
}
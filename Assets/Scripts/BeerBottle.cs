using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottle : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

    private void Awake()
    {
        // «бираЇ вс≥ доч≥рн≥ Rigidbody, кр≥м цього об'Їкта
        allParts = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        allParts.Remove(GetComponent<Rigidbody>());
    }

    public void Shatter()
    {
        foreach (var part in allParts)
        {
            part.isKinematic = false;
        }
    }
}

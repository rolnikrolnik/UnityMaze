using UnityEngine;
using System.Collections;
using Treasure_Hunter.Interfaces;

public class SpikeTrapScript : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(0.05f);
            Debug.Log("Stepped into a spike trap!");
        }
    }
}

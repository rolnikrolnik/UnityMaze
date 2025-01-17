﻿using UnityEngine;
using System.Collections;
using Treasure_Hunter.Interfaces;

public class FireballTrapScript : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        IDamageable damageableObject = other.GetComponentInParent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(0.05f);
            Debug.Log("Stepped into a fireball!");
        }
    }
}

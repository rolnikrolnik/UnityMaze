using UnityEngine;
using System.Collections;
using Treasure_Hunter.Controllers;
using Treasure_Hunter.Interfaces;

public class EnemyHealth : MonoBehaviour, IDamageable
{

	public bool is_dead = false;
	public HealthBar Healthbar;
    public GameObject EnemyRoot;

    private float current_health = 1;

	void Start () 
    {
        current_health = 1;
        Healthbar.SetValue(current_health);
	}

	public void TakeDamage(float amount) 
    {
        current_health -= amount;
        Healthbar.SetValue(current_health);
        if (current_health <= 0)
        {
            Die();
        }
	}

	public void Die() 
    {
		is_dead = true;
        Healthbar.gameObject.SetActive(false);
	}

    public void DestroyObject()
    {
        Destroy(EnemyRoot);
    }
}

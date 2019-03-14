using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyCombatController : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float currentHealth;
    public float attackSpeed;
    public float damage;
    public float ultimateCD;
    public string ultimateDescription;
    public bool isDead;
    public bool isEliminated;// nu se spawneaza runda viitoare
    public GameObject projectile;

    public Sprite[] damageShipSprites;
    public Sprite deadSprite;

    private void OnValidate()
    {
        OnDamageReceive();
    }

    public void OnDamageReceive(float damageAmount = 0)
    {
        float procentHealth = (currentHealth * 2.0f) / maxHealth;
        currentHealth -= damageAmount;
        if (currentHealth <= maxHealth && currentHealth > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = damageShipSprites[(int)procentHealth];
        }
        if (currentHealth <= 0)
        {
            isDead = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
            StartCoroutine(DisableObject(2f,() => gameObject.SetActive(false)));
        }
    }

    IEnumerator DisableObject(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }
}

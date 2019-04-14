using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

[Serializable]
public class EntityData
{

    public float speed;
    public float maxHealth;
    public float currentHealth;
    public float attackSpeed;
    public float damage;
    public float deffense;
    public float ultimateCD;
    public Sprite ultimateImage;
    public string ultimateDescription;
    public bool isDead;
    public bool isEliminated;// nu se spawneaza runda viitoare
    public GameObject ultimateImageHolder;
    public GameObject projectile;
    public float range;
    public Sprite[] damagedSpries;
    public Sprite deadSprite;

    public void OnDamageReceive(GameObject gj, float damageAmount = 0)
    {
        float procentHealth = (currentHealth * 2.0f) / maxHealth;
        currentHealth -= damageAmount;
        if (currentHealth <= maxHealth && currentHealth > 0)
        {
            gj.GetComponent<SpriteRenderer>().sprite = damagedSpries[(int)procentHealth];
        }
        if (currentHealth <= 0)
        {
            isDead = true;
            gj.GetComponent<SpriteRenderer>().sprite = deadSprite;
            gj.GetComponent<IEntityData>().CallDisableObject();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyCombatController : MonoBehaviour
{
    private Unit unit;

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

    public float range;

    public Sprite[] damageShipSprites;
    public Sprite deadSprite;

    private float fireRate;
    private float nextFire;
    [HideInInspector]
    public bool continousAttack;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    private void OnValidate()
    {
        OnDamageReceive();
    }

    private void Update()
    {
        if (unit.target != null)
        {
            float distance = new Vector3(unit.target.transform.position.x - transform.position.x, unit.target.transform.position.y - transform.position.y, 0).sqrMagnitude;
            if (continousAttack == true || distance <= range * range)
            {
                fireRate = attackSpeed;
                continousAttack = true;
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    if (distance <= range * range)
                    {
                        GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
                        obj.transform.up = new Vector3(unit.target.transform.position.x - transform.position.x, unit.target.transform.position.y - transform.position.y, 0);
                        obj.GetComponent<CannonballController>().shotter = gameObject;
                    }
                    else
                    {
                        continousAttack = false;
                    }
                }
            }
        }
        else
        {
            continousAttack = false;
        }
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

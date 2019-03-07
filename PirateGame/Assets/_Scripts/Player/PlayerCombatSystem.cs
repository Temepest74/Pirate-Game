using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatSystem : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float currentHealth;
    public float attackSpeed;
    public float damage;
    public float ultimateCD;
    public Sprite ultimateImage;
    public string ultimateDescription;
    public bool isDead;
    public bool isEliminated;// nu se spawneaza runda viitoare
    public GameObject ultimateImageHolder;
    public GameObject projectile;

    public float range;

    private float fireRate;
    private float nextFire;
    [HideInInspector]
    public bool continousAttack;

    Ray2D ray;
    RaycastHit2D raycastHit2D;

    public Sprite[] damageShipSprites;
    public Sprite deadSprite;

    private void Start()
    {
        currentHealth = maxHealth;
        fireRate = attackSpeed;
        ultimateImageHolder = GameObject.FindGameObjectWithTag("UltimateImageHolder");
        if (ultimateImage != null)
        {
            ultimateImageHolder.GetComponent<Image>().sprite = ultimateImage;
        }
        else
        {
            Debug.Log("The game object with tag :UltimateImageHolder has not been found");
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || continousAttack)
        {
            fireRate = attackSpeed;
            if (continousAttack == false || Input.GetMouseButtonDown(0))
            {
                continousAttack = true;
                ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
            }
            if (Time.time > nextFire)
            {
                //opreste in momentul in care obiectul e mort
                nextFire = Time.time + fireRate;
                if (raycastHit2D.collider.Equals(gameObject.GetComponent<CapsuleCollider2D>()))
                {
                }
                else if (raycastHit2D.collider.CompareTag("Enemy") || raycastHit2D.collider.CompareTag("Player"))
                {
                    Vector3 dir = new Vector3(raycastHit2D.collider.transform.position.x - transform.position.x,
                    raycastHit2D.collider.transform.position.y - transform.position.y, 0);
                    if (dir.sqrMagnitude < range * range)
                    {
                        GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
                        obj.transform.up = dir;
                        obj.GetComponent<CannonballController>().shotter = gameObject;
                        Debug.Log(obj.GetComponent<CannonballController>().shotter);
                    }
                    else
                    {
                        continousAttack = false;
                    }
                }
            }
        }
    }

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
        if (currentHealth == 0)
        {
            isDead = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            Destroy(gameObject, 2);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerCombatSystem : MonoBehaviour, IEntityData
{
    public EntityData GetEntityData()
    {
        return entityData;
    }
    public EntityData entityData;

    private float fireRate;
    private float nextFire;
    [HideInInspector]
    public bool continousAttack;

    Ray2D ray;
    RaycastHit2D raycastHit2D;


    private void Start()
    {
        entityData.currentHealth = entityData.maxHealth;
        fireRate = entityData.attackSpeed;
        entityData.ultimateImageHolder = GameObject.FindGameObjectWithTag("UltimateImageHolder");
        if (entityData.ultimateImage != null)
        {
            entityData.ultimateImageHolder.GetComponent<Image>().sprite = entityData.ultimateImage;
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
            fireRate = entityData.attackSpeed;
            if (continousAttack == false || Input.GetMouseButtonDown(0))
            {
                continousAttack = true;
                ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
            }
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                if (raycastHit2D.collider.Equals(gameObject.GetComponent<CapsuleCollider2D>()))
                {
                }
                else if (raycastHit2D.collider.CompareTag("Enemy") || raycastHit2D.collider.CompareTag("Player"))
                {
                    Vector3 dir = new Vector3(raycastHit2D.collider.transform.position.x - transform.position.x,
                    raycastHit2D.collider.transform.position.y - transform.position.y, 0);
                    if (dir.sqrMagnitude < entityData.range * entityData.range && !raycastHit2D.collider.GetComponent<IEntityData>().GetEntityData().isDead)
                    {
                        GameObject obj = Instantiate(entityData.projectile, transform.position, Quaternion.identity) as GameObject;
                        obj.transform.up = dir;
                        obj.GetComponent<CannonballController>().shotter = gameObject;
                    }
                    else
                    {
                        continousAttack = false;
                    }
                }
            }
        }
    }

    public void OnDamageReceive(float damage = 0)
    {
        entityData.OnDamageReceive(gameObject, damage);
        if (entityData.currentHealth <= 0)
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<PlayerCombatSystem>().enabled = false;
        }
    }

    public void CallDisableObject()
    {
        if (Application.isPlaying)
            StartCoroutine(DisableObject(2f, () => gameObject.SetActive(false)));
    }

    public IEnumerator DisableObject(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

}

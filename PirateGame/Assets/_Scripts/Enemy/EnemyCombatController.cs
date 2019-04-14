using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyCombatController : MonoBehaviour, IEntityData
{
    public EntityData GetEntityData()
    {
        return entityData;
    }
    public EntityData entityData;

    private Unit unit;

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
            if (continousAttack == true || distance <= entityData.range * entityData.range)
            {
                fireRate = entityData.attackSpeed;
                continousAttack = true;
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    if (distance <= entityData.range * entityData.range)
                    {
                        GameObject obj = Instantiate(entityData.projectile, transform.position, Quaternion.identity) as GameObject;
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

    public void OnDamageReceive(float damage = 0)
    {
        entityData.OnDamageReceive(gameObject, damage);
        if (entityData.currentHealth <= 0)
        {
            gameObject.GetComponent<Unit>().enabled = false;
            gameObject.GetComponent<EnemyCombatController>().enabled = false;
        }
    }

    public void CallDisableObject()
    {
        if(Application.isPlaying)
        StartCoroutine(DisableObject(2f, () => gameObject.SetActive(false)));
    }

    public IEnumerator DisableObject(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

}

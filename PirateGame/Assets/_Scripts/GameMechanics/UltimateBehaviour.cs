using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBehaviour : MonoBehaviour
{
    public GameObject barrel;
    public Color cdColor;
    private IEntityData entityData;
    private float ultimateCD;
    private float nextUltimate = 0;
    public enum ShipType
    {
        White,
        Black,
        Blue,
        Red,
        Green,
        Yellow,
        Random
    };
    void Awake()
    {
        entityData = GetComponent<IEntityData>();
    }
    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            ShipType shipType = FindShipType(gameObject);
            if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextUltimate)
            {
                ultimateCD = entityData.GetEntityData().ultimateCD;
                nextUltimate = Time.time + ultimateCD;
                entityData.GetEntityData().ultimateImageHolder.GetComponent<Image>().color = cdColor;
                CallUltimate(shipType);
            }
        }
    }
    void LateUpdate()
    {
        if (Time.time > nextUltimate && !Input.GetKeyDown(KeyCode.Z))
        { 
            entityData.GetEntityData().ultimateImageHolder.GetComponent<Image>().color = Color.white;
        }
    }
    ShipType FindShipType(GameObject gj)
    {
        for (int i = 0; (ShipType)i != ShipType.Random; i++)
        {
            ShipType shipType = (ShipType)i;
            if (gj.name.Contains(shipType.ToString()))
            {
                return shipType;
            }
        }
        return ShipType.Random;
    }

    void CallUltimate(ShipType shipType)
    {
        switch(shipType)
        {
            case ShipType.White:
                StartCoroutine(UltimateWhite());
                break;
            case ShipType.Yellow:
                StartCoroutine(UltimateYellow());
                break;
            /* case ShipType.Black:
                StartCoroutine(UltimateBlack());
                break;*/
            case ShipType.Red:
                StartCoroutine(UltimateRed());
                break;
            case ShipType.Blue:
                StartCoroutine(UltimateBlue());
                break;
            case ShipType.Green:
                StartCoroutine(UltimateGreen());
                break;
            default:
                Debug.Log("Not a known ship");
                break;
        }
    }
    IEnumerator UltimateWhite()
    {
        // Increase the maximum health with 20 percent for 5 seconds
        float addedHpAmount = entityData.GetEntityData().maxHealth * 0.2f;
        entityData.GetEntityData().maxHealth += addedHpAmount;
        entityData.GetEntityData().currentHealth += addedHpAmount;
        yield return new WaitForSeconds(5);
        entityData.GetEntityData().maxHealth -= addedHpAmount;
        if(entityData.GetEntityData().currentHealth > entityData.GetEntityData().maxHealth)
        {
            entityData.GetEntityData().currentHealth = entityData.GetEntityData().maxHealth;
        }
    }
    IEnumerator UltimateYellow()
    {
        //Repair 30 percent of your max health
        float addedHpAmount = entityData.GetEntityData().maxHealth * 0.3f;
        entityData.GetEntityData().currentHealth += addedHpAmount;
        if(entityData.GetEntityData().currentHealth > entityData.GetEntityData().maxHealth)
        {
            entityData.GetEntityData().currentHealth = entityData.GetEntityData().maxHealth;
        }
        yield return null;
    }
    /*IEnumerator UltimateBlack()
    {

    }*/
    IEnumerator UltimateRed()
    {
        //Launch a projectile who gives 100% damage and 30% damage over time for 5 seconds
        Transform target = GetComponent<IEntityData>()?.GetEntityData().target;
        if(GetComponent<IEntityData>().GetEntityData().inCombat && target != null)
        {
            Vector3 dir = new Vector3 (target.position.x - transform.position.x,
                            target.position.y - transform.position.y, 0);
            GameObject obj = Instantiate (barrel, transform.position, Quaternion.identity) as GameObject;
            obj.transform.up = dir;
            obj.GetComponent<ExplosiveBarrel> ().shotter = gameObject;
        }
        yield return null;
    }
    IEnumerator UltimateBlue()
    {
        //Increase the speed of the boat with 40 percent for 5 seconds
        float addedSpeed = entityData.GetEntityData().speed * 0.4f;
        entityData.GetEntityData().speed += addedSpeed;
        yield return new WaitForSeconds(5);
        entityData.GetEntityData().speed -= addedSpeed;
    }
    IEnumerator UltimateGreen()
    {
        //50% damage up for 5 seconds
        float addedDamageMin = entityData.GetEntityData().minDamage * 0.5f;
        float addedDamageMax = entityData.GetEntityData().maxDamage * 0.5f;
        entityData.GetEntityData().minDamage += addedDamageMin;
        entityData.GetEntityData().maxDamage += addedDamageMax;
        yield return new WaitForSeconds(5);
        entityData.GetEntityData().minDamage -= addedDamageMin;
        entityData.GetEntityData().maxDamage -= addedDamageMax;
    }
}
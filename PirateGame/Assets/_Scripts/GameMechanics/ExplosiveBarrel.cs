using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject shotter;
    public float speed;
    private void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((shotter != null) && (collision.gameObject != shotter))
        {
            float shotterDamage;
            float minDamage = shotter.GetComponent<IEntityData>().GetEntityData().minDamage;
            float maxDamage = shotter.GetComponent<IEntityData>().GetEntityData().maxDamage;
            float trueDamage = UnityEngine.Random.Range(minDamage, maxDamage);
            Debug.Log(collision.gameObject.GetComponent<IEntityData>().GetEntityData().deffense);
            Debug.Log(collision.gameObject.GetComponent<IEntityData>().GetEntityData() == null);
            Debug.Log(collision.gameObject.GetComponent<IEntityData>() == null);
            Debug.Log(collision.gameObject == null);
            Debug.Log(collision.name);
            shotterDamage = trueDamage * trueDamage / (trueDamage + collision.gameObject.GetComponent<IEntityData>().GetEntityData().deffense);
            if (collision.GetComponent<IEntityData>() != null)
            {
                collision.GetComponent<IEntityData>().OnDamageReceive(shotterDamage);
            }
            Destroy(gameObject);
        }
    }
}

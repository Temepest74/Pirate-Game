using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CannonballController : MonoBehaviour
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
            shotterDamage = shotter.GetComponent<IEntityData>().GetEntityData().damage;
            if (collision.GetComponent<IEntityData>() != null)
            {
                collision.GetComponent<IEntityData>().OnDamageReceive(shotterDamage);
            }
            Destroy(gameObject);
        }
    }
}

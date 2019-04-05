using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Object enemy;
            float shotterDamage;
            if(shotter.GetComponent<PlayerCombatSystem>() != null)
            {
                shotterDamage = shotter.GetComponent<PlayerCombatSystem>().damage;
            }else if(shotter.GetComponent<EnemyCombatController>() != null)
            {
                shotterDamage = shotter.GetComponent<EnemyCombatController>().damage;
            }
            else
            {
                Debug.Log("ERROR 0");
            }
            if (collision.GetComponent<PlayerCombatSystem>() != null)
            {
                enemy = collision.GetComponent<PlayerCombatSystem>();
            }
            else if (collision.GetComponent<EnemyCombatController>() != null)
            {
                enemy = collision.GetComponent<EnemyCombatController>();
            }
            float gotDamage = shotterDamage * (100 / (100 + enemy.GetInstanceID.
            Destroy(gameObject);
        }
    }
}

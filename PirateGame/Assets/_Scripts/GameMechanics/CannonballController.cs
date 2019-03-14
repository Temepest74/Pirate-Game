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
        if (collision.gameObject != shotter)
        {
            if (collision.GetComponent<PlayerCombatSystem>() != null)
            {
                PlayerCombatSystem playerCombatSystem;
                playerCombatSystem = collision.GetComponent<PlayerCombatSystem>();
                playerCombatSystem.OnDamageReceive(playerCombatSystem.damage);
            }
            else if (collision.GetComponent<EnemyCombatController>() != null)
            {
                EnemyCombatController enemyCombatController;
                enemyCombatController = collision.GetComponent<EnemyCombatController>();
                enemyCombatController.OnDamageReceive(enemyCombatController.damage);
            }
            Destroy(gameObject);
        }
    }
}

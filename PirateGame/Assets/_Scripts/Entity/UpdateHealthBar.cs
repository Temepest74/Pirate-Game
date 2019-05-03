using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public IEntityData entityData;

    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxHealth;

    private void Update()
    {
        currentHealth = entityData.GetEntityData().currentHealth;
        maxHealth = entityData.GetEntityData().maxHealth;
        float proccentHealth = currentHealth / maxHealth;
        transform.localScale = new Vector3(proccentHealth, transform.localScale.y, transform.localScale.y);
        if(currentHealth <= 0)
        {
            Destroy(healthBar);
        }
    }
}

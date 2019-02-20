using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatSystem : MonoBehaviour
{
    public BoatStats boatStats;
    public GameObject ultimateImage;
    public GameObject projectile;

    public float range;

    private float fireRate;
    private float nextFire = 0.0F;
    private void Start()
    {
        boatStats = new BoatStats();
        ultimateImage = GameObject.FindGameObjectWithTag("UltimateImageHolder");
        if (ultimateImage != null)
        {
            //ultimateImage.GetComponent<Image>().sprite = boatStats.ultimateImage;
        }
        else
        {
            Debug.Log("The game object with tag :UltimateImageHolder has not been found");
        }
    }
    private void Update()
    {
        //fireRate = 1 / boatStats.attackSpeed;
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        }
    }
}
[System.Serializable]
public class BoatStats
{
    public float speed;
    public float health;
    public float attackSpeed;
    public float damage;
    public float ultimateCD;
    public Sprite ultimateImage;
    public string ultimateDescription;
    public bool isDead;
    public bool isEliminated;// nu se spawneaza runda viitoare
}



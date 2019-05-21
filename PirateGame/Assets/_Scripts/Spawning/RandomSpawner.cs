using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject shipHealthBar;
    public GameObject shipParent;
    public GameObject[] spawnPoints;
    public GameObject[] prefabSpawnPoints;
    public GameObject[] prefabEnemyShips;
    public GameObject[] prefabPlayerShips;
    Queue<GameObject> takenSpawnPoints;

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
        public ShipType shipType;

        private void Start ()
        {
        Random.InitState (System.Environment.TickCount);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
        int rn = Random.Range (0, spawnPoints.Length);
        var tmpry = spawnPoints[i];
        spawnPoints[i] = spawnPoints[rn];
        spawnPoints[rn] = tmpry;
        }
        bool instantiatePlayer = (shipType == ShipType.Random);
        takenSpawnPoints = new Queue<GameObject> ();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            while (true)
            {
                int rn = Random.Range (0, prefabSpawnPoints.Length - 1);
                if (!takenSpawnPoints.Contains (prefabSpawnPoints[rn]))
                {
                    takenSpawnPoints.Enqueue (prefabSpawnPoints[rn]);
                    if (i != 0 || instantiatePlayer)
                    {
                        Instantiate (prefabSpawnPoints[rn], spawnPoints[i].transform.position, Quaternion.identity);
                    }
                    break;
                }
            }
        }
        int currentNoOfShips = 0;
        if (!instantiatePlayer) // if someone chose the class type
        {
            int i;
            for (i = 0; i < prefabPlayerShips.Length; i++)
            {
                string firstWord = prefabPlayerShips[i].name.IndexOf (" ", System.StringComparison.Ordinal) > -1 ? prefabPlayerShips[i].name.Substring (0, prefabPlayerShips[i].name.IndexOf (" ", System.StringComparison.Ordinal)) : prefabPlayerShips[i].name;
                if (shipType.ToString () == firstWord)
                {
                    break;
                }
            }
            int j;
            for (j = 0; j < prefabSpawnPoints.Length; j++)
            {
                if (prefabSpawnPoints[j].name == prefabPlayerShips[i].name)
                {
                    break;
                }
            }
            Instantiate (prefabSpawnPoints[j], spawnPoints[0].transform.position, Quaternion.identity);
            takenSpawnPoints.Dequeue ();
            GameObject parent = new GameObject ();
            parent.transform.SetParent (shipParent.transform);
            parent.name = "Player " + prefabPlayerShips[i].name;
            GameObject ship = Instantiate (prefabPlayerShips[i], spawnPoints[0].transform.position, Quaternion.identity, parent.transform);
            GameObject healthbar = Instantiate (shipHealthBar, spawnPoints[0].transform.position, Quaternion.identity, parent.transform); //spawnPoints[0].transform.position can be anything
            healthbar.GetComponent<Follow> ().objectToFollow = ship;
            healthbar.GetComponentInChildren<UpdateHealthBar> ().entityData = ship.GetComponent<IEntityData> ();
            currentNoOfShips++;
            GameObject.FindWithTag ("ChinematicCamera").GetComponent<Cinemachine.CinemachineVirtualCamera> ().m_Follow = ship.transform;
        }
        while (takenSpawnPoints.Count != 0)
        {
            GameObject toInstantiateSpawnPoint;
            toInstantiateSpawnPoint = takenSpawnPoints.Dequeue ();
            if (instantiatePlayer)
            {
                instantiatePlayer = false;
                int i;
                for (i = 0; i < prefabPlayerShips.Length; i++)
                {
                    if (toInstantiateSpawnPoint.name == prefabPlayerShips[i].name)
                    {
                        break;
                    }
                }
                GameObject parent = new GameObject ();
                parent.transform.SetParent (shipParent.transform);
                parent.name = "Player " + prefabPlayerShips[i].name;
                GameObject ship = Instantiate (prefabPlayerShips[i], spawnPoints[0].transform.position, Quaternion.identity, parent.transform);
                GameObject healthbar = Instantiate (shipHealthBar, spawnPoints[0].transform.position, Quaternion.identity, parent.transform); //spawnPoints[0].transform.position can be anything
                healthbar.GetComponent<Follow> ().objectToFollow = ship;
                healthbar.GetComponentInChildren<UpdateHealthBar> ().entityData = ship.GetComponent<IEntityData> ();
                currentNoOfShips++;
                GameObject.FindWithTag ("ChinematicCamera").GetComponent<Cinemachine.CinemachineVirtualCamera> ().m_Follow = ship.transform;
            }
            else
            {
                int i;
                for (i = 0; i < prefabEnemyShips.Length; i++)
                {
                    if (toInstantiateSpawnPoint.name == prefabEnemyShips[i].name)
                    {
                        break;
                    }
                }
                GameObject parent = new GameObject ();
                parent.transform.SetParent (shipParent.transform);
                parent.name = "Enemy " + prefabEnemyShips[i].name;
                GameObject ship = Instantiate (prefabEnemyShips[i], spawnPoints[currentNoOfShips].transform.position, Quaternion.identity, parent.transform);
                GameObject healthbar = Instantiate (shipHealthBar, spawnPoints[0].transform.position, Quaternion.identity, parent.transform); //spawnPoints[0].transform.position can be anything
                healthbar.GetComponent<Follow> ().objectToFollow = ship;
                healthbar.GetComponentInChildren<UpdateHealthBar> ().entityData = ship.GetComponent<IEntityData> ();
                currentNoOfShips++;
            }
        }
    }
}
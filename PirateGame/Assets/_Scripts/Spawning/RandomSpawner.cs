using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] prefabSpawnPoints;
    public GameObject[] prefabEnemyShips;
    public GameObject[] prefabPlayerShips;
    Queue<GameObject> takenSpawnPoints;

    public enum ShipType
    {
        White, Black, Blue, Red, Green, Yellow, Random
    };
    public ShipType shipType;

    private void Start()
    {
        takenSpawnPoints = new Queue<GameObject>();
        foreach (GameObject spawnPoint in spawnPoints)
        {
            while (true)
            {
                int rn = Random.Range(0, prefabSpawnPoints.Length - 1);
                if(!takenSpawnPoints.Contains(prefabSpawnPoints[rn]))
                {
                    takenSpawnPoints.Enqueue(prefabSpawnPoints[rn]);
                    Instantiate(prefabSpawnPoints[rn], spawnPoint.transform.position, Quaternion.identity);
                    break;
                }
            }
        }
        bool instantiatePlayer = (shipType == ShipType.Random);
        int currentNoOfShips = 0;
        while (takenSpawnPoints.Count != 0)
        {
            GameObject toInstantiateSpawnPoint;
            toInstantiateSpawnPoint = takenSpawnPoints.Dequeue();
            if(instantiatePlayer)
            {
                instantiatePlayer = false;
                int i;
                for(i = 0; i < prefabPlayerShips.Length; i++)
                {
                    if(toInstantiateSpawnPoint.name == prefabPlayerShips[i].name)
                    {
                        break;
                    }
                }
                Instantiate(prefabPlayerShips[i], spawnPoints[0].transform.position, Quaternion.identity);
                currentNoOfShips++;
                GameObject.FindWithTag("ChinematicCamera").GetComponent<Cinemachine>().
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
                Instantiate(prefabEnemyShips[i], spawnPoints[currentNoOfShips].transform.position, Quaternion.identity);
                currentNoOfShips++;
            }
        }
    }
}

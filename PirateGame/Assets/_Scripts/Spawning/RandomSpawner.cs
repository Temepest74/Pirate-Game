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
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            int rn = Random.Range(0, spawnPoints.Length - 1);
            var tmpry = spawnPoints[i];
            spawnPoints[i] = spawnPoints[rn];
            spawnPoints[rn] = tmpry; 
        }
        bool instantiatePlayer = (shipType == ShipType.Random);
        takenSpawnPoints = new Queue<GameObject>();
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            while (true)
            {
                int rn = Random.Range(0, prefabSpawnPoints.Length - 1);
                if(!takenSpawnPoints.Contains(prefabSpawnPoints[rn]))
                {
                    takenSpawnPoints.Enqueue(prefabSpawnPoints[rn]);
                    if (i != 0 || instantiatePlayer)
                    {
                        Instantiate(prefabSpawnPoints[rn], spawnPoints[i].transform.position, Quaternion.identity);
                    }
                    break;
                }
            }
        }
        int currentNoOfShips = 0;
        if(!instantiatePlayer)// if someone chose the class type
        {
            int i;
            for (i = 0; i < prefabPlayerShips.Length; i++)
            {
                string firstWord = prefabPlayerShips[i].name.IndexOf(" ", System.StringComparison.Ordinal) > -1 ? prefabPlayerShips[i].name.Substring(0, prefabPlayerShips[i].name.IndexOf(" ", System.StringComparison.Ordinal)) : prefabPlayerShips[i].name;
                if (shipType.ToString() == firstWord)
                {
                    break;
                }
            }
            int j;
            for(j = 0; j < prefabSpawnPoints.Length; j++)
            {
                if(prefabSpawnPoints[j].name == prefabPlayerShips[i].name)
                {
                    break;
                }
            }
            Instantiate(prefabSpawnPoints[j], spawnPoints[0].transform.position, Quaternion.identity);
            takenSpawnPoints.Dequeue();
            GameObject gm = Instantiate(prefabPlayerShips[i], spawnPoints[0].transform.position, Quaternion.identity);
            currentNoOfShips++;
            GameObject.FindWithTag("ChinematicCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = gm.transform;
        }
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
                GameObject gm = Instantiate(prefabPlayerShips[i], spawnPoints[0].transform.position, Quaternion.identity);
                currentNoOfShips++;
                GameObject.FindWithTag("ChinematicCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = gm.transform;
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

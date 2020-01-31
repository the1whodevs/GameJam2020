using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject _playerPrefab;

    public bool LocalPlay = false;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    public static GameManager GetInstance() { return _instance; }

    public void SpawnPlayers(params GameObject[] playersToSpawn)
    {
        foreach (GameObject go in playersToSpawn)
        {
            SpawnRandom(go, transform);
        }
    }

    void SpawnRandom(GameObject toSpawn)
    {

        Vector3 randomPos = new Vector3(
            Random.Range(-5, 5), //x
            0.0f, //y
            Random.Range(-5, 5)); //z

    }

    void SpawnRandom(GameObject toSpawn, Transform parentToSet)
    {

        Vector3 randomPos = new Vector3(
            Random.Range(-5, 5), //x
            0.0f, //y
            Random.Range(-5, 5)); //z

        if (LocalPlay)
        {
            toSpawn.transform.SetParent(parentToSet);
            toSpawn.transform.position = randomPos;
            //TODO: Enable the actual PlayerController toSpawn.GetComponent<SantaController>().enabled = true;
        }
    }

}

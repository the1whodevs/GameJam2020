using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
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

        if (!LocalPlay)
        {
            GameObject spawnedPlayer = PhotonNetwork.Instantiate(toSpawn.name, randomPos, Quaternion.identity);
            spawnedPlayer.GetComponentInChildren<TextMeshProUGUI>().text = PhotonNetwork.NickName;
        }
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
            toSpawn.GetComponent<SantaController>().enabled = true;
        }
    }

    public override void OnJoinedRoom()
    {
        if (!LocalPlay)
        {
            Debug.Log("Player joined the room!");
            SpawnRandom(_playerPrefab);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!LocalPlay)
        {
            Debug.Log(otherPlayer.NickName + " left the room, what a weakling!"); 
        }
    }
}

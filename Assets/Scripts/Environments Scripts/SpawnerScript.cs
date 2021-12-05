using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public float minDisToSpawn = 15.0f;
    public int numberOfPrefabs = 1;
    public bool ManualSpawn = false;
    public float delaySpawning = 1f;

    private int _numberPrefabs;
    private bool isSpawned;
    private Transform player;
    private ManagerPOScript poolObject;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _numberPrefabs = numberOfPrefabs;
        poolObject = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ManagerPOScript>();
        if (!poolObject)
        {
            Debug.LogError("not found ManagerPOScript! in the scene");
        }
    }
    private void Update()
    {
        if (ManualSpawn)
        {
            ManualSpawn = false;
            isSpawned = false;
            _numberPrefabs = 1;
        }
        if (Vector2.Distance(transform.position, player.position) < minDisToSpawn && !isSpawned)
        {
            isSpawned = true;
            StartCoroutine(Spawning());
        }
    }

    IEnumerator Spawning()
    {
        for (int i = 0; i < _numberPrefabs; i++)
        {
            poolObject.CreateZombie(transform.position, transform.rotation);
            yield return new WaitForSeconds(delaySpawning);
        }
    }
}

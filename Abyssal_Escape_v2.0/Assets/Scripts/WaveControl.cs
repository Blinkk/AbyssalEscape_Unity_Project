using UnityEngine;
using System.Collections;

public class WaveControl : MonoBehaviour 
{
    public GameObject enemy;            // Enemies to be spawned
    private int waveCount = 10;         // Number to spawn
    private GameObject[] currentWave;   // Array of current wave enemies
    private int waveNumber;             // Current wave number
    private bool newWave;               // Flag to show new wave GUI
    private Transform playerTrans;      // Player transform
    private float minDistToPlayer;      // Distance enmies must spawn from player
    private GameGUI gui;

	// Use this for initialization
	void Start () 
    {
        waveNumber = 0;
        newWave = true;
        playerTrans = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();
        minDistToPlayer = 10.0f;
        gui = GameObject.FindGameObjectWithTag("GUI").gameObject.GetComponent<GameGUI>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        currentWave = GameObject.FindGameObjectsWithTag("Enemy");   // Find enemies in level

        // If enemies have been eliminated, spawn new wave
	    if (currentWave.GetLength(0) <= 0)
        {
            newWave = true;
            gui.SetDrawMenu(true);
        }   
	}

    public void SpawnWave()
    {
        gui.SetDrawMenu(false);     // Remove menu
            
        if (waveNumber > 1)
            waveCount += 2 * waveNumber;

        // Spawn a wave of enemies
        for (int i = 0; i < waveCount; i++)
        {
            Vector3 spawnPos = GetSpawnPos();
            if (Vector3.Distance(spawnPos, playerTrans.position) <= minDistToPlayer)
                spawnPos += new Vector3(minDistToPlayer, 0, minDistToPlayer);
            
            // Instantiate new enemy at 'spawnPos'
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }

        newWave = false;    // New wave flag reset
    }

    private Vector3 GetSpawnPos()
    {
        // Get spawn x and z
        float x = Random.Range(playerTrans.position.x - 20, playerTrans.position.x + 20);
        float z = Random.Range(playerTrans.position.z - 20, playerTrans.position.z + 20);

        Vector3 pos = new Vector3(x, 0, z);

        return pos;
    }

    public int GetCurrentWave()
    {
        return waveNumber;
    }

    public bool IsNewWave()
    {
        return newWave;
    }

    public void SetWaveNumber(int value)
    {
        waveNumber = value;
    }

}

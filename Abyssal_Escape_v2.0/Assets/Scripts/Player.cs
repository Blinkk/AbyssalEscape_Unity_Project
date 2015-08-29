using UnityEngine;
using System.Collections;

public class Player : Entity 
{
    private int level;
    private float currentExp;
    private float expToLevel;
    private int enemiesKilled;
    private int currentWave;

    private GameGUI gui;
    private WaveControl waveController;

    void Start()
    {
        currentWave = 0;
        enemiesKilled = 0;
        gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GameGUI>();
        waveController = Camera.main.GetComponent<WaveControl>();
        LevelUp();  // Init to level 1
    }

    public void AddExperience(float exp)
    {
        currentExp += exp;
        if (currentExp >= expToLevel)
        {
            currentExp -= expToLevel;
            LevelUp();
        }

        // Update exp bar
        gui.SetPlayerExp(currentExp / expToLevel, level);

        // Update enemies killed (only if an enemy was killed)
        if (exp > 0)
            enemiesKilled++;
    }

    private void LevelUp()
    {
        level++;
        expToLevel = level * 50 + Mathf.Pow(level * 2, 2);

        AddExperience(0);       // Run check again after leveling up
    }


    // Player Update
    private void Update()
    {
        currentWave = waveController.GetCurrentWave();      // Get current wave from wave controller
        gui.SetScoreInfo(enemiesKilled, currentWave);       // Update score GUI
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        gui.SetHealth(currentHealth / maxHealth);           // Update health bar
    }

    public override void Die()
    {
        //base.Die();           // Destroys game object
        Debug.Log("Player is dead");
        Application.LoadLevel("gameOver_scene");
    }

    public void SetHealthValue(float value)
    {
        if (value <= maxHealth)
            currentHealth = value;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetLevel()
    {
        return level;
    }
}

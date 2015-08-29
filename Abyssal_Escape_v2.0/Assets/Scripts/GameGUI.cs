using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour
{
    public Transform expBar;
    public TextMesh levelText;
    public TextMesh ammoText;
    public TextMesh scoreText;
    public Transform healthBar;
    public Texture2D boxBorder;
    public Texture2D groupBBackground;
    public Texture2D subBoxBBackground;
    private WaveControl wc;
    private PlayerController playerController;
    private Player player;

    // Point variables for Upgrades
    private int pointsToSpend;          // Points for between-wave menu
    private bool pointsCalc = false;    // Flag to determine if point were calc'd

    // Flag to draw menu / sub menu
    private bool drawMenu = true;
    private bool drawSubMenu = false;

    // Previous level variable
    private int previousLevel = 1;

    public void Start()
    {
        // Get wavecontrol reference
        wc = Camera.main.GetComponent<WaveControl>();

        // Get player controller reference
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Get player reference
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void SetPlayerExp(float percentToLevel, int playerLevel)
    {
        levelText.text = "Level: " + playerLevel;

        expBar.GetComponent<Transform>().localScale = new Vector3(percentToLevel, 1, 1);
    }

    public void SetAmmoInfo(int currentAmmo, int ammoInMag)
    {
        ammoText.text = ammoInMag + "/" + currentAmmo;
    }

    public void SetScoreInfo(int enemiesKilled, int currentWave)
    {
        scoreText.text = "Wave: " + currentWave + "\tEnemies Killed: " + enemiesKilled;
    }

    public void SetHealth(float percentOfTotal)
    {
        healthBar.GetComponent<Transform>().localScale = new Vector3(percentOfTotal, 1, 1);
    }

    public void SetDrawMenu(bool value)
    {
        drawMenu = value;
    }

    public bool IsDrawMenu()
    {
        return drawMenu;
    }

    private void CalcPoints()
    {
        // Determine points to spend
        if (!pointsCalc)
        {
            pointsToSpend = player.GetLevel() - previousLevel;
            pointsCalc = true;
        }
        else
            return;
    }

    public void SpendPoint(int multiplier)
    {
        for (int i = 0; i < multiplier; i++)
            pointsToSpend--;
    }
	
    // Additional in-game GUI (manual)
    public void OnGUI()
    {
        #region Gun Inventory GUI
        // Group parameters
        float groupW = Screen.width;
        float groupH = 240;
        float groupX = (Screen.width / 2) - 120;
        float groupY = Screen.height - 85;

        // Main box parameters
        float mainBoxW = 241;
        float mainBoxH = 81;
        float mainBoxX = 0;
        float mainBoxY = 0;

        // Sub-box parameters
        float subW = 75;
        float subH = 75;
        float subY = mainBoxY + 3;
        float subX = mainBoxX + 3;
        float subXOffset = 80;

        // Sub-box style (border)
        GUIStyle boxStyle = new GUIStyle("box");
        boxStyle.normal.background = boxBorder;
        boxStyle.fontSize = 18;
        boxStyle.wordWrap = true;

        // Sub-box style (no border)
        GUIStyle boxStyle2 = new GUIStyle("box");
        boxStyle2.fontSize = 18;
        boxStyle2.wordWrap = true;

        // Get current gun
        float id = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>().GetGunID();

        // Begin group
        GUI.BeginGroup(new Rect(groupX, groupY, groupW, groupH));
            GUI.Box(new Rect(mainBoxX, mainBoxY, mainBoxW, mainBoxH), "");                              // Main box

            // Determine which box has border
            if (id == 1)
                GUI.Box(new Rect(subX, subY, subW, subH), "1 - Assault Rifle", boxStyle);               // Sub-box 1
            else
                GUI.Box(new Rect(subX, subY, subW, subH), "1 - Assault Rifle", boxStyle2);              // Sub-box 1
            if (id == 2)
                GUI.Box(new Rect(subX + subXOffset, subY, subW, subH), "2 - Shotgun", boxStyle);        // Sub-box 2
            else
                GUI.Box(new Rect(subX + subXOffset, subY, subW, subH), "2 - Shotgun", boxStyle2);       // Sub-box 2
            if (id == 3)
                GUI.Box(new Rect(subX + (subXOffset * 2), subY, subW, subH), "3 - Pistol", boxStyle);   // Sub-box 3
            else
                GUI.Box(new Rect(subX + (subXOffset * 2), subY, subW, subH), "3 - Pistol", boxStyle2);  // Sub-box 3

        GUI.EndGroup();     // End group
        #endregion

        #region Between Waves GUI
        // If in-between waves
        if (drawMenu)
        {
            CalcPoints();       // Get points for upgrades

            // GroupB parameters
            float groupBW = 800.0f;
            float groupBH = 575.0f;
            float groupBX = (Screen.width / 2) - (groupBW / 2);
            float groupBY = (Screen.height / 2) - (groupBH / 2);

            // Sub-Box parameters
            float subBW = 150.0f;
            float subBH = 125.0f;
            float subBX = 30.0f;
            float subBY = 75.0f;
            float subBYOffset = subBH + 20;

            // Sub-Button parameters
            float subButtonW = 200.0f;
            float subButtonH = 60.0f;
            float subButtonX = subBX + subBW + 15;
            float subButtonY = subBY;
            float subButtonYOffset = subButtonH + 5.0f;

            // Health button parameters
            float healthButtonW = 360.0f;
            float healthButtonH = 200.0f;
            float healthButtonX = groupBW / 2 + 10;
            float healthButtonY = subBY;

            // Ammo button parameters
            float ammoButtonW = 116.3f;
            float ammoButtonH = 200.0f;
            float ammoButtonX = groupBW / 2 + 10;
            float ammoButtonY = healthButtonY + healthButtonH + 15;
            float ammoButtonXOffset = 5;

            // Close-Button parameters
            float closeButtonW = 150.0f;
            float closeButtonH = 75.0f;

            // GroupB style
            GUIStyle groupBStyle = new GUIStyle("box");
            groupBStyle.fontSize = 42;
            groupBStyle.normal.background = groupBBackground;

            // GroupB Label style
            GUIStyle labelStyle = new GUIStyle("label");
            labelStyle.fontSize = 28;

            // GroupB small Label style
            GUIStyle smallLabelStyle = new GUIStyle("label");
            smallLabelStyle.fontSize = 12;

            // GroupB sub-Box style
            GUIStyle subBoxBStyle = new GUIStyle("box");
            subBoxBStyle.fontSize = 32;
            subBoxBStyle.wordWrap = true;
            subBoxBStyle.normal.background = subBoxBBackground;
            subBoxBStyle.alignment = TextAnchor.MiddleCenter;

            // GroupB sub-button style
            GUIStyle subButtonStyle = new GUIStyle("button");
            subButtonStyle.fontSize = 20;
            subButtonStyle.wordWrap = true;


            ////////////////
            // Draw GUI
            ////////////////
            GUI.BeginGroup(new Rect(groupBX, groupBY, groupBW, groupBH));
                GUI.Box(new Rect(0, 0, groupBW, groupBH), "Upgrades", groupBStyle);  // Main-box

                GUI.Label(new Rect(groupBW - 150, 10, 150, 45),                      // Points label
                    "Points: " + pointsToSpend.ToString(), labelStyle);

                #region Gun Upgrades
                // Gun upgrades boxes & buttons
                //////////////////
                // Assault Rifle
                //////////////////
                GUI.Box(new Rect(subBX, subBY, subBW, subBH), "Assault Rifle", subBoxBStyle);
                GUI.Label(new Rect(subBX + 5, (subBY + subBH) - 18, subBW, 20), 
                    "Damage: " + playerController.GetGunByID(1).GetDamage() + "   Clip Size: " + playerController.guns[0].GetAmmoPerMag(), smallLabelStyle);

                // If gun at max damage, disable button
                if (playerController.GetGunByID(1).GetDamage() >= 6 || pointsToSpend <= 0
                    || (playerController.GetGunByID(1).GetDamage() >= 3 && pointsToSpend <= 1))
                    GUI.enabled = false;
                if (GUI.Button(new Rect(subButtonX, subButtonY, subButtonW, subButtonH), "Increase Damage", subButtonStyle))
                {
                    if (playerController.GetGunByID(1).GetDamage() < 3)
                    {
                        if (pointsToSpend >= 1)
                        {
                            playerController.GetGunByID(1).SetDamage(playerController.GetGunByID(1).GetDamage() + 1);
                            SpendPoint(1);
                        }
                    }
                    else
                    {
                        if (pointsToSpend >= 2)
                        {
                            playerController.GetGunByID(1).SetDamage(playerController.GetGunByID(1).GetDamage() + 1);
                            SpendPoint(2);
                        }
                    }
                    
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                // If gun is at max clip size, disable button
                if (playerController.GetGunByID(1).GetAmmoPerMag() >= 64 || pointsToSpend <= 0
                    || (playerController.GetGunByID(1).GetAmmoPerMag() >= 48 && pointsToSpend <= 1))
                    GUI.enabled = false;
                if (GUI.Button(new Rect(subButtonX, subButtonY + subButtonYOffset, subButtonW, subButtonH), "Increase Clip Size", subButtonStyle))
                {
                    if (playerController.GetGunByID(1).GetAmmoPerMag() < 48)
                    {
                        if (pointsToSpend >= 1)
                        {
                            playerController.GetGunByID(1).SetAmmoPerMag(playerController.GetGunByID(1).GetAmmoPerMag() + 8);
                            SpendPoint(1);
                        }  
                    }
                    else
                    {
                        if (pointsToSpend >= 2)
                        {
                            playerController.GetGunByID(1).SetAmmoPerMag(playerController.GetGunByID(1).GetAmmoPerMag() + 8);
                            SpendPoint(2);
                        }
                    }
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                //////////////////
                // Shotgun
                //////////////////
                GUI.Box(new Rect(subBX, subBY + subBYOffset, subBW, subBH), "Shotgun", subBoxBStyle);
                GUI.Label(new Rect(subBX + 5, (subBY + subBH + subBYOffset) - 18, subBW, 20),
                        "Damage: " + playerController.GetGunByID(2).GetDamage() + "   Clip Size: " + playerController.GetGunByID(2).GetAmmoPerMag(), smallLabelStyle);

                // If gun at max damage, disable button
                if (playerController.GetGunByID(2).GetDamage() >= 15 || pointsToSpend <= 0
                    || (playerController.GetGunByID(2).GetDamage() >= 9 && pointsToSpend <= 1))
                    GUI.enabled = false;
                if (GUI.Button(new Rect(subButtonX, subButtonY + subBYOffset, subButtonW, subButtonH), "Increase Damage", subButtonStyle))
                {
                    if (playerController.GetGunByID(2).GetDamage() < 9)
                    {
                        if (pointsToSpend >= 1)
                        {
                            playerController.GetGunByID(2).SetDamage(playerController.GetGunByID(2).GetDamage() + 3);
                            SpendPoint(1);
                        }      
                    }
                    else
                    {
                        if (pointsToSpend >= 2)
                        {
                            playerController.GetGunByID(2).SetDamage(playerController.GetGunByID(2).GetDamage() + 3);
                            SpendPoint(2);
                        }    
                    }
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                // If gun is at max clip size, disable button
                if (playerController.GetGunByID(2).GetAmmoPerMag() >= 6 || pointsToSpend <= 0
                    || (playerController.GetGunByID(2).GetAmmoPerMag() >= 4 && pointsToSpend <= 1))
                    GUI.enabled = false;
                if (GUI.Button(new Rect(subButtonX, subButtonY + subBYOffset + subButtonYOffset, subButtonW, subButtonH), "Increase Clip Size", subButtonStyle))
                {
                    if (playerController.GetGunByID(2).GetAmmoPerMag() < 4)
                    {
                        if (pointsToSpend >= 1)
                        {
                            playerController.GetGunByID(2).SetAmmoPerMag(playerController.GetGunByID(2).GetAmmoPerMag() + 2);
                            SpendPoint(1);
                        }  
                    }
                    else
                    {
                        if (pointsToSpend >= 2)
                        {
                            playerController.GetGunByID(2).SetAmmoPerMag(playerController.GetGunByID(2).GetAmmoPerMag() + 2);
                            SpendPoint(2);
                        }  
                    }
                    
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                //////////////////
                // Pistol
                //////////////////
                GUI.Box(new Rect(subBX, subBY + (subBYOffset * 2), subBW, subBH), "Pistol", subBoxBStyle);
                GUI.Label(new Rect(subBX + 5, (subBY + subBH + (subBYOffset * 2)) - 18, subBW, 20),
                        "Damage: " + playerController.GetGunByID(3).GetDamage() + "   Clip Size: " + playerController.guns[2].GetAmmoPerMag(), smallLabelStyle);

                // If gun at max damage, disable button
                if (playerController.GetGunByID(3).GetDamage() >= 12 || pointsToSpend <= 0
                    || (playerController.GetGunByID(3).GetDamage() >= 6 && pointsToSpend <= 1))
                    GUI.enabled = false;
                if (GUI.Button(new Rect(subButtonX, subButtonY + (subBYOffset * 2), subButtonW, subButtonH), "Increase Damage", subButtonStyle))
                {
                    if (playerController.GetGunByID(3).GetDamage() < 6)
                    {
                        if (pointsToSpend >= 1)
                        {
                            playerController.GetGunByID(3).SetDamage(playerController.GetGunByID(3).GetDamage() + 2);
                            SpendPoint(1);
                        }  
                    }
                    else
                    {
                        if (pointsToSpend >= 2)
                        {
                            playerController.GetGunByID(3).SetDamage(playerController.GetGunByID(3).GetDamage() + 2);
                            SpendPoint(2);
                        }  
                    }
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                // If gun is at max clip size, disable button
                if (playerController.GetGunByID(3).GetAmmoPerMag() >= 30 || pointsToSpend <= 0
                    || (playerController.GetGunByID(3).GetAmmoPerMag() >= 20 && pointsToSpend <= 1))
                    GUI.enabled = false;
                if (GUI.Button(new Rect(subButtonX, subButtonY + (subBYOffset * 2) + subButtonYOffset, subButtonW, subButtonH), "Increase Clip Size", subButtonStyle))
                {
                    if (playerController.GetGunByID(3).GetAmmoPerMag() < 20)
                    {
                        if (pointsToSpend >= 1)
                        {
                            playerController.GetGunByID(3).SetAmmoPerMag(playerController.GetGunByID(3).GetAmmoPerMag() + 5);
                            SpendPoint(1);
                        }  
                    }
                    else
                    {
                        if (pointsToSpend >= 2)
                        {
                            playerController.GetGunByID(3).SetAmmoPerMag(playerController.GetGunByID(3).GetAmmoPerMag() + 5);
                            SpendPoint(2);
                        }  
                    }
                    
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;
                #endregion

                /////////////////
                // Health button
                /////////////////
                if (player.GetHealth() >= player.GetMaxHealth() || pointsToSpend <= 0)
                    GUI.enabled = false;
                subButtonStyle.fontSize = 60;
                if (GUI.Button(new Rect(healthButtonX, healthButtonY, healthButtonW, healthButtonH), "Refill Health", subButtonStyle))
                {
                    player.SetHealthValue(player.GetMaxHealth());    // Reset health
                    SetHealth(player.GetHealth() / player.GetMaxHealth());  // Reset health bar
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                /////////////////
                // Ammo buttons
                /////////////////
                if (pointsToSpend <= 0)
                    GUI.enabled = false;
                subButtonStyle.fontSize = 24;
                if (playerController.guns[0].GetTotalAmmo() == 128 && playerController.guns[0].GetCurrentMagAmmo() == playerController.guns[0].GetAmmoPerMag())
                    GUI.enabled = false;
                if (GUI.Button(new Rect(ammoButtonX, ammoButtonY, ammoButtonW, ammoButtonH), "Refill Assault Rifle Ammo", subButtonStyle))
                {
                    playerController.guns[0].SetCurrentMagAmmo(playerController.guns[0].GetAmmoPerMag());
                    playerController.guns[0].SetTotalAmmo(128);
                    SetAmmoInfo(playerController.guns[0].GetTotalAmmo(), playerController.guns[0].GetCurrentMagAmmo());
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                if (pointsToSpend <= 0)
                    GUI.enabled = false;
                if (playerController.guns[1].GetTotalAmmo() == 20 && playerController.guns[1].GetCurrentMagAmmo() == playerController.guns[1].GetAmmoPerMag())
                    GUI.enabled = false;
                if (GUI.Button(new Rect(ammoButtonX + ammoButtonW + ammoButtonXOffset, ammoButtonY, ammoButtonW, ammoButtonH), "Refill Shotgun Ammo", subButtonStyle))
                {
                    playerController.guns[1].SetCurrentMagAmmo(playerController.guns[1].GetAmmoPerMag());
                    playerController.guns[1].SetTotalAmmo(20);
                    SetAmmoInfo(playerController.guns[1].GetTotalAmmo(), playerController.guns[1].GetCurrentMagAmmo());
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                if (pointsToSpend <= 0)
                    GUI.enabled = false;
                if (playerController.guns[2].GetTotalAmmo() == 40 && playerController.guns[2].GetCurrentMagAmmo() == playerController.guns[2].GetAmmoPerMag())
                    GUI.enabled = false;
                if (GUI.Button(new Rect(ammoButtonX + (ammoButtonW * 2) + (ammoButtonXOffset * 2), ammoButtonY, ammoButtonW, ammoButtonH), "Refill Pistol Ammo", subButtonStyle))
                {
                    playerController.guns[2].SetCurrentMagAmmo(playerController.guns[2].GetAmmoPerMag());
                    playerController.guns[2].SetTotalAmmo(40);
                    SetAmmoInfo(playerController.guns[2].GetTotalAmmo(), playerController.guns[2].GetCurrentMagAmmo());
                }
                // Re-enable GUI after each button
                if (!GUI.enabled) GUI.enabled = true;

                /////////////////
                // Ready Button
                /////////////////
                subButtonStyle.fontSize = 32;
                if (GUI.Button(new Rect((groupBW / 2) - (closeButtonW / 2), groupBH - closeButtonH - 5, closeButtonW, closeButtonH), "Ready", subButtonStyle))
                {
                    drawMenu = false;
                    wc.SetWaveNumber(wc.GetCurrentWave() + 1);
                    wc.SpawnWave();
                    previousLevel = player.GetLevel();
                    pointsCalc = false;     // Allow points to be recalculated

                    Debug.Log(playerController.GetGunByID(1).GetDamage());
                    Debug.Log(playerController.GetGunByID(2).GetDamage());
                    Debug.Log(playerController.GetGunByID(3).GetDamage());

                }

            GUI.EndGroup();     // End of menu group
        }
        #endregion
    }
}

using UnityEngine;
using System.Collections;

public class UIEvent : MonoBehaviour 
{
    private bool drawInstructions = false;
    private bool drawCredits = false;

	// Play event
	public void OnClick_Play() 
	{
		Application.LoadLevel ("main_scene");
	}

    // How To Event
    public void OnClick_HowTo()
    {
        if (!drawInstructions)
            drawInstructions = true;
        else
            drawInstructions = false;
    }

    // Credits Event
    public void OnClick_Credits()
    {
        if (!drawCredits)
            drawCredits = true;
        else
            drawCredits = false;
    }

    // Exit event
    public void OnClick_Exit()
    {
        Debug.Log("Application Exited...");
        Application.Quit();
    }


    // GUI Update
    public void OnGUI()
    {
        // Custom GUIStyle (identical to box-style, change font size)
        GUIStyle style = new GUIStyle("box");
        style.fontSize = 18;

        /////////////////
        // Instructions
        /////////////////
        // Label parameters (Instructions)
        int boxW = 325, boxH = 475;
        int posX = 80, posY = (Screen.height / 2) - (boxH / 2);
        if (drawInstructions)
        {
            GUI.Label(new Rect(new Vector2(posX, posY), 
                new Vector2(boxW, boxH)), 
                "How To Play\n" +
                "______________________\n" + 
                "1. Aim with mouse\n" + 
                "2. Move with WASD keys\n" +
                "(Hold Shift-key to sprint)\n" +
                "3. Shoot with Left-Click\n" + 
                "4. Reload weapon with 'R'\n" + 
                "5. Cycle through weapons\nwith number keys (1-3)\n\n\n" +
                "Objective:\nWaves of enemies will spawn\nand you must fight them off.\n" +
                "You will earn a score based off\nof the number of enemies killed\n" +
                "and the number of waves completed.\nIf you die, the game is over.",
                style);

            // Calc button position and dimensions
            int buttonW = (boxW / 3) * 2, buttonH = (boxH / 6) - 25;
            int buttonX = posX + ((boxW - buttonW) / 2);
            int buttonY = (posY + boxH) - (buttonH + 10);

            // Keep height of button between 25-50
            if (buttonH < 25) buttonH = 25;
            else if (buttonH > 50) buttonH = 50;

            // Create a button style
            GUIStyle buttonStyle = new GUIStyle("button");
            buttonStyle.fontSize = 30;

            // Draw the button
            if (GUI.Button(new Rect(new Vector2(buttonX, buttonY), new Vector2(buttonW, buttonH)),"Close", buttonStyle))
                drawInstructions = false;   // If button is clicked, do this
        }


        //////////////
        // Credits
        //////////////
        // Label parameters (Credits)
        int credW = 325, credH = 170;
        int credX = (Screen.width / 2) + 265, credY = (Screen.height / 2) - (credH / 2);
        if (drawCredits)
        {
            GUI.Label(new Rect(new Vector2(credX, credY),
                new Vector2(credW, credH)),
                "Credits\n" +
                "______________________\n" +
                "Developer: Austin Ivicevic\n" +
                "3D Models: Thomas Lague,\n BUMSTRUM\n",
                style);

            // Calc button position and dimensions
            int buttonW = (credW / 3) * 2, buttonH = (credH / 5);
            int buttonX = credX + ((credW - buttonW) / 2);
            int buttonY = (credY + credH) - (buttonH + 10);

            // Keep height of button between 25-50
            if (buttonH < 25) buttonH = 25;
            else if (buttonH > 50) buttonH = 50;

            // Create a button style
            GUIStyle buttonStyle = new GUIStyle("button");
            buttonStyle.fontSize = 30;

            // Draw the button
            if (GUI.Button(new Rect(new Vector2(buttonX, buttonY), new Vector2(buttonW, buttonH)), "Close", buttonStyle))
                drawCredits = false;   // If button is clicked, do this
        }
    }

}

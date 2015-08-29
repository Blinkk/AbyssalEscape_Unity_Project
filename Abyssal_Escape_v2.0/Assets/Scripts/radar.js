var orbspot : Texture;
var playerPos : Transform;
private var mapScale = 5;

private var radarSpotX: float;
private var radarSpotY: float;

private var radarWidth = 200;
private var radarHeight = 200;

function OnGUI () 
{
    GUI.BeginGroup (Rect (10, Screen.height - radarHeight - 175, radarWidth, radarHeight));
    	GUI.Box (Rect (0, 0, radarWidth, radarHeight), "Radar");
    	DrawSpotsForOrbs();
    GUI.EndGroup();
}

function DrawRadarBlip(go, spotTexture)
{
    var gameObjPos = go.transform.position;
    
    //find distance between object and player
    var dist = Vector3.Distance(playerPos.position, gameObjPos);
    
    //find the horizontal distances along the x and z between player and object
    var dx = playerPos.position.x - gameObjPos.x;
    var dz = playerPos.position.z - gameObjPos.z;
    
    
    ////////////////////////////////////////////////////////////////////////
    // Note: This math is sort of backwards, but it functions as intended
    ////////////////////////////////////////////////////////////////////////
   
    //determine the angle of rotation between the player and the location
    //of the object
    deltay = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;

	//orient the object on the radar
    radarSpotX = dist * Mathf.Cos(deltay * Mathf.Deg2Rad) * mapScale;
    radarSpotY = dist * Mathf.Sin(deltay * Mathf.Deg2Rad) * mapScale;
   
    //draw a spot on the radar
    GUI.DrawTexture(Rect(radarWidth/2.0 + radarSpotY * -1, radarHeight/2.0 + radarSpotX, 5, 5), spotTexture);
}

function DrawSpotsForOrbs()
{
    var gos : GameObject[];
    //look for all objects with a tag of enemy
    gos = GameObject.FindGameObjectsWithTag("Enemy");

    var distance = Mathf.Infinity;
    var position = transform.position;

    for (var go : GameObject in gos)  
    {
	   DrawRadarBlip(go,orbspot);
    }
}

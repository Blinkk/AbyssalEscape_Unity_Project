using UnityEngine;
using System.Collections;

public class flocking : MonoBehaviour 
{
    public float speed = 0.001f;
    private float rotationSpeed = 5.0f;
    private float minDist = 2.0f;
    private Transform target;
	private float neighbourDistance = 2.0f;
    private GameObject player;
    private float prevDmgTime = 0.0f;

	// Use this for initialization
	void Start () 
    {
        // Set random enemy speed between 1 & 2
        speed = Random.Range(1.5f, 3.0f);

        // Get target transform
        target = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Transform>();

        // Get reference to player
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
       // Applies rules approx. 1/5 frames
        if (Random.Range(0, 5) < 1)
            ApplyRules();
        if (Vector3.Distance(target.position, this.transform.position) > minDist)
            this.GetComponent<Transform>().Translate(0, 0, Time.deltaTime * speed); // Move object on z-axis
        else if (Time.time >= prevDmgTime + 0.5f)
        {
            player.GetComponent<Player>().TakeDamage(1);
            prevDmgTime = Time.time;
        }  
	}

    
    // Make enemies face the target (player)
    public void ApplyRules()
    {
        this.transform.LookAt(target);     
    }
}

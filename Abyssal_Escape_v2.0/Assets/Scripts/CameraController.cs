using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    private Transform target;

    private Vector3 cameraTarget;
    private float z_offset;

	// Use this for initialization
	void Start () 
    {
        // Make the player transform the target
        target = GameObject.FindGameObjectWithTag("Player").transform;
        z_offset = 5.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z - z_offset);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime * 8);
	}
}

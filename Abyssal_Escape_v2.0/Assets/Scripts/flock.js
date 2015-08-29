var speed : float = 0.001;
private var rotationSpeed : float = 5.0;
var averageHeading:Vector3;
var averagePosition:Vector3;
private var neighbourDistance = 2.0;
function Start()
{
	speed = Random.Range(1.1,2);
}
function Update () 
{
	if(Random.Range(0,5) < 1)
		ApplyRules();
	transform.Translate(0,0,Time.deltaTime * speed);
}

function ApplyRules()
{
	var gos : GameObject[];
	gos = GameObject.FindGameObjectsWithTag("Enemy");
	
	var vcentre: Vector3;
	var vavoid: Vector3;
	var gSpeed: float;
	
	var wind : Vector3 = new Vector3(1,0,1);
	var goalPos:Vector3 = GameObject.FindGameObjectWithTag("Player").GetComponent.<Transform>().position;
	
	var dist: float;
	var groupSize: int = 0;
	for(var go: GameObject in gos)
	{
		if( go!= this.gameObject)
		{
			dist = 
				Vector3.Distance(go.transform.position,this.transform.position);
		
			if(dist <= neighbourDistance)
			{
				vcentre += go.transform.position;
				groupSize++;
				gSpeed = gSpeed + go.GetComponent("flock").speed;
			}
		
		}
	}
	if(groupSize)
	{
		vcentre = vcentre/ groupSize + wind + (goalPos - this.transform.position);
		
		var direction = vcentre - this.transform.position;
		if(direction != Vector3.zero)
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),rotationSpeed * Time.deltaTime);
		speed = gSpeed/groupSize;
	}
	
}

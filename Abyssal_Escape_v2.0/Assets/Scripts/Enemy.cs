using UnityEngine;
using System.Collections;

public class Enemy : Entity 
{
    public float expOnDeath;
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();           
    }

    public override void Die()
    {
		//this.GetComponent<Animator> ().SetTrigger ("Die");
        player.AddExperience(expOnDeath);   // Add experience, then die
        base.Die();
    }
}

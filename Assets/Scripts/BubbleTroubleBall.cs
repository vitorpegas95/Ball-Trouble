using UnityEngine;
using System.Collections;

public class BubbleTroubleBall : MonoBehaviour {

	public int health;
	public int initialHealth;
	private Vector3 healthV;

	public Material[] mats;

	public GameObject dieParticle;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().material = mats [Random.Range (0, mats.Length - 1)];

		healthV = new Vector3(health, health, health);
		transform.localScale = healthV;
		initialHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale != healthV) {
			transform.localScale = healthV;
		}
	}

	public void Die()
	{
		Instantiate (dieParticle, transform.position, Quaternion.identity);

		BubbleTroubleSpawner.spawner.BallDie ();
		float randValue = Random.value;
		if (randValue > .70f) 
		{
			Instantiate(BubbleTroubleSpawner.spawner.powerups[0],
			            transform.position,
			            Quaternion.identity);
		}
		else if (randValue > .69f && randValue < .9f) 
		{
			Instantiate(BubbleTroubleSpawner.spawner.powerups[1],
			            transform.position,
			            Quaternion.identity);
		}
		else if( randValue >= .9f) // 10% of the time
		{
			Instantiate(BubbleTroubleSpawner.spawner.powerups[2],
			            transform.position,
			            Quaternion.identity);
		}

		Destroy (gameObject);
	}

	public void setHealth(int hp)
	{
		health = hp;
		healthV.Set (health, health, health);
	}

	public void TakeHit()
	{
		BubbleTroublePlayer.player.Score(health * initialHealth);
		if (health > 1) {
			health --;

			/*
			 * This feature gets very crowded gameplay. Maybe in other map its useful.
				SpawnOtherBall();
			*/

		} else {
			Die();
		}
		
		healthV.Set (health, health, health);
	}

	void SpawnOtherBall()
	{
		GameObject b = Instantiate (BubbleTroubleSpawner.spawner.ballPrefab, 
		                            GetComponent<Transform> ().position,
		                            Quaternion.identity) as GameObject;
		
		b.GetComponent<BubbleTroubleBall> ().setHealth (health);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name.Equals ("Ceiling")) {
			TakeHit();
		}
	}
}

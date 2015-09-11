using UnityEngine;
using System.Collections;

public class BubbleTroubleSpawner : MonoBehaviour {

	public static BubbleTroubleSpawner spawner;

	public int curBalls;
	public int maxBalls;
	public float nextSpawn;
	public float spawnDelay;

	public GameObject ballPrefab;
	
	public GameObject[] powerups;

	public AudioSource sound;

	// Use this for initialization
	void Awake () {
		if (spawner == null) {
			spawner = this;
		}
		else
		{
			Destroy(gameObject);
		}
		sound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		maxBalls = BubbleTroublePlayer.player.score / 20;
		if (maxBalls < 2)
			maxBalls = 2;
		if (curBalls < maxBalls && Time.time > nextSpawn) {
			Spawn();

			nextSpawn = Time.time + spawnDelay;
		}
	}

	void Spawn()
	{
		GameObject b = Instantiate (ballPrefab, 
		                           GetComponent<Transform> ().position,
		                           Quaternion.identity) as GameObject;

		b.GetComponent<BubbleTroubleBall> ().setHealth (Random.Range(1, 5));

		curBalls ++;
	}

	public void BallDie()
	{
		curBalls --;
	}
}

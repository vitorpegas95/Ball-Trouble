using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BubbleTroublePlayer : MonoBehaviour {

	public static BubbleTroublePlayer player;

	public float speed = 6f;
	private Rigidbody body;

	public GameObject projectile;
	public Transform spawnPoint;

	public float shootDelay = 0.4f;
	public float nextShoot;

	public AudioClip hitmark;
	public AudioClip gameOver;

	public int score;
	public Text txtScore;

	public AudioClip[] scoreSounds;
	public int[] scoreSoundsValues;

	public bool protection;
	public bool doublePoints;
	public bool tripleFire;

	public float resetPowerUpTime = 5;

	
	public RawImage pw_doublePoints;
	public RawImage pw_tripleFire;
	public RawImage pw_protection;

	private AudioSource sound;

	public bool paused;

	public Sprite[] soundImage;
	public bool muted;
	public Button btnMute;


	//Object Pooling
	public int pooledAmount = 24;
	private List<GameObject> bullets;
	private int tripleFireCounter = 0;

	// Use this for initialization
	void Start () {

		if (player == null) {
			player = this;
		} else {
			Destroy(gameObject);
		}

		body = GetComponent<Rigidbody> ();
		sound = GetComponent<AudioSource> ();

		bullets = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate(projectile);
			bullets.Add(obj);
			obj.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		pw_doublePoints.enabled = doublePoints;
		pw_protection.enabled = protection;
		pw_tripleFire.enabled = tripleFire;

		SideMovement ();
		UpdateScore ();
		Shoot ();

		if (Input.GetKeyDown (KeyCode.P)) {
			Pause ();
		}
	}

	public void Mute()
	{
		muted = !muted;


		if (muted) {
			btnMute.image.sprite = soundImage[0];

		} else {
			btnMute.image.sprite = soundImage[1];

		}

		BT_MusicPlayer.musicPlayer.mute = muted;
	}

	public void Pause()
	{
		paused = !paused;
		Time.timeScale = paused ? 0 : 1;
	}

	public void Shoot()
	{
		if((Input.GetButton("Jump") || 
		    Input.GetButton("Vertical") ||
		    Input.GetButton("Fire1") || 
		    Input.GetKeyDown(KeyCode.UpArrow)) &&
		   Time.time > nextShoot)
		{
			/*
			Old Shoot
			if(tripleFire)
			{
				Instantiate(projectile, spawnPoint.position, Quaternion.identity);
				Instantiate(projectile, spawnPoint.position, Quaternion.identity);
			}
			Instantiate(projectile, spawnPoint.position, Quaternion.identity);

*/
			tripleFireCounter = 0;
			for(int i = 0; i < bullets.Count; i++)
			{
				if(!bullets[i].activeInHierarchy)
				{
					//Bullet is not active
					bullets[i].transform.position = spawnPoint.position;
					bullets[i].transform.rotation = spawnPoint.rotation;
					bullets[i].SetActive(true);
					tripleFireCounter++;

					if(tripleFire)
					{
						if(tripleFireCounter > 2)
							break;
						else
							continue;
					}
					else
					{
						break;
					}
				}
			}


			nextShoot = Time.time + shootDelay;
		}
	}

	public void Score(int s)
	{
		if (doublePoints)
			s = s * 2;
		score += s;
		ScoreSounds ();
	}

	public void UpdateScore()
	{
		txtScore.text = score.ToString();
	}

	IEnumerator ResetPowerUps(string i)
	{
		yield return new WaitForSeconds(resetPowerUpTime);
		switch (i) {
		case "double":
			doublePoints = false;
			break;

		case "triple":
			tripleFire = false;
			break;

		case "protection":
			protection = false;
			break;

		case "all":
			doublePoints = protection = tripleFire = false;
			break;
		}
	}

	public void ScoreSounds()
	{
		for (int i = 0; i < scoreSoundsValues.Length; i++) {
			if(i+1 < scoreSoundsValues.Length)
			{
				if(score >= scoreSoundsValues[i] && score < scoreSoundsValues[i+1])
				{
					if(!sound.isPlaying)
					{
						sound.clip = scoreSounds[i];
						sound.Play();
					}
				}
				else if(score > scoreSoundsValues[scoreSoundsValues.Length-1])
				{
					if(!sound.isPlaying)
					{
						sound.clip = scoreSounds[scoreSoundsValues.Length-1];
						sound.Play();
					}
				}
			}
		}
	}

	public void PlayHitmark()
	{
		BubbleTroubleSpawner.spawner.sound.PlayOneShot (hitmark);
	}

	void SideMovement()
	{
		if ((Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) || Input.GetKey(KeyCode.RightArrow)) {
			// Move to the right
			body.MovePosition(transform.position + 
			                  Vector3.right * 
			                  Time.deltaTime * 
			                  speed
			                  );
		} 
		
		if ((Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) || Input.GetKey(KeyCode.LeftArrow)) {
			// Move to the left
			body.MovePosition(transform.position + 
			                  Vector3.left * 
			                  Time.deltaTime * 
			                  speed
			                  );
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag.Equals("Ball") && this.enabled)
		{
			if(!protection)
			{
				Die ();
			}
			protection = false;
			Destroy(other.gameObject);
		}

		if (other.gameObject.tag.Equals ("PowerUp")) {
			if(other.gameObject.name.Contains("DoublePoints"))
			{
				doublePoints = true;
				StartCoroutine(ResetPowerUps("double"));
			}
			else if(other.gameObject.name.Contains("TripleFire"))
			{
				tripleFire = true;
				StartCoroutine(ResetPowerUps("triple"));
			}
			else
			{
				protection = true;
			}
			
			Destroy(other.gameObject);
		}
	}

 	public void Die()
	{
		BT_DieMenu.dieMenu.scoreValue = score;
		BT_DieMenu.dieMenu.scoreText = score.ToString ();
		sound.PlayOneShot (gameOver);
		this.enabled = false;
		BubbleTroubleSpawner.spawner.enabled = false;
		BT_DieMenu.dieMenu.DisplayButtons (true);
	}
}

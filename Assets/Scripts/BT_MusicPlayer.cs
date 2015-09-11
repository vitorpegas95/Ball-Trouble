using UnityEngine;
using System.Collections;

public class BT_MusicPlayer : MonoBehaviour {

	public static BT_MusicPlayer musicPlayer;

	public AudioClip menu;
	public AudioClip game;

	public bool mute;

	public AudioSource sound;

	public AudioSource[] sources;

	// Use this for initialization
	void Awake () {
		if (musicPlayer == null) {
			musicPlayer = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (gameObject);
		}

		sound = GetComponent<AudioSource> ();	
		Play ("menu");
	}

	// Update is called once per frame
	void Update () {
		sound.mute = mute;

		if (sources.Length != GameObject.FindObjectsOfType<AudioSource> ().Length) {
			sources = GameObject.FindObjectsOfType<AudioSource> ();
		}

		foreach (AudioSource a in sources) {
			if(a != null)
			{
				if(a.mute != mute)
					a.mute = mute;
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public void Play(string type)
	{
		if (type == "menu") {
			sound.clip = menu;
		} else if (type == "game") {
			sound.clip = game;
		}
		sound.loop = true;
		sound.Play ();
	}
}

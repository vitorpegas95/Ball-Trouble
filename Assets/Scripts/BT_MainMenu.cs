using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BT_MainMenu : MonoBehaviour {

	public Text title;

	void Start () {
		BT_MusicPlayer.musicPlayer.Play ("menu");
	}
	
	// Update is called once per frame
	void Update () {
		title.fontSize = (int)(Mathf.PingPong (Time.time * 20, 20) + 100);
	}

	public void Play()
	{
		BT_MusicPlayer.musicPlayer.Play ("game");
		Application.LoadLevel (1);
	}
}

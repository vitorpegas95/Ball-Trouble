using UnityEngine;
using System.Collections;

public class BT_DieMenu : MonoBehaviour {

	public static BT_DieMenu dieMenu;

	public GameObject buttons;

	public bool isSignedIn;

	public int scoreValue;
	public string scoreText;
	public int tableID = 94664;
	public string extraData = "";
	public int lastScore;

	// Use this for initialization
	void Start () {
		buttons.SetActive (false);
		if (dieMenu == null) {
			dieMenu = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
	}

	public void DisplayButtons( bool active )
	{
		buttons.SetActive (active);
	}

	public void Retry()
	{
		buttons.SetActive (false);
		Application.LoadLevel (1);
	}

	void SubmitUserScore()
	{
		if (scoreValue > lastScore) {
			GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) => {
				Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
			});
			lastScore = scoreValue;
		}
	}

	void SubmitGuestScore()
	{
		string guestName = "Guest";
		GameJolt.API.Scores.Add(scoreValue, scoreText, guestName, tableID, extraData, (bool success) => {
			Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
		});
	}

	public void Leaderboards()
	{
		if (isSignedIn) {
			SubmitUserScore();
			
			Invoke ("ShowScores", 2);
		} else {
			GameJolt.UI.Manager.Instance.ShowSignIn((bool success) => {
				if (success)
				{
					Debug.Log("The user signed in!");
					SubmitUserScore();
					
					Invoke ("ShowScores", 2);
				}
				else
				{
					Debug.Log("The user failed to signed in or closed the window :(");
					SubmitGuestScore();
					
					Invoke ("ShowScores", 2);
				}
			});
		}
	}

	void ShowScores()
	{
		GameJolt.UI.Manager.Instance.ShowLeaderboards();
	}

	public void Home()
	{
		buttons.SetActive (false);
		Application.LoadLevel (0);
	}
}

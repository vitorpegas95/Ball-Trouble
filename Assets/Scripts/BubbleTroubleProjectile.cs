using UnityEngine;
using System.Collections;

public class BubbleTroubleProjectile : MonoBehaviour {

	public float initThrust = 10f;

	void OnEnable()
	{
		GetComponent<Rigidbody> ().isKinematic = false;
		GetComponent<Rigidbody> ().AddForce (transform.up * initThrust);
	}

	void OnDisable()
	{
		GetComponent<Rigidbody> ().isKinematic = true;
	}

	void OnCollisionEnter(Collision other)
	{
		BubbleTroublePlayer.player.PlayHitmark ();
		if(other.gameObject.tag.Equals("Ball"))
		{
			other.gameObject.GetComponent<BubbleTroubleBall>().TakeHit();
			other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
			gameObject.SetActive (false);
		}
		else if(other.gameObject.tag.Equals("Player"))
		{
			gameObject.SetActive (false);
		}
	}
}

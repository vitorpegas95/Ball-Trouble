using UnityEngine;
using System.Collections;

public class BallInitialThrust : MonoBehaviour {

	public float initThrust = 500f;
	public float hitThrust = 50f;
	public Vector3 curVelocity;

	public Vector3 maxVelocity;
	public int direction;
	Rigidbody body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		body.AddForce (Vector3.left * initThrust);
	}

	void OnCollisionEnter(Collision col) {

		curVelocity = body.velocity;

		if (body.velocity.x < 3) 
		{
			body.AddForce(Vector3.left * hitThrust * direction);
		}

		if (body.velocity.y < 1) {
			body.AddForce(Vector3.up * hitThrust);
		}

		if (body.velocity.magnitude < maxVelocity.magnitude) {
			if(col.gameObject.name.Contains("Right"))
			{
				body.AddForce (Vector3.left * hitThrust);
				direction = 1;
			}
			else if(col.gameObject.name.Contains("Left"))
			{
				body.AddForce (Vector3.right * hitThrust);
				direction = -1;
			}
			
			body.AddForce (Vector3.up * hitThrust);
		}		
	}
}

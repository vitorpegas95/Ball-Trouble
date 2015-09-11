using UnityEngine;
using System.Collections;
public class ProjectileDestroyer : MonoBehaviour {

	public float time = 3f;

	// Use this for initialization
	void Start () {
		Invoke ("Destroy", time);
	}
	
	void Destroy()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		CancelInvoke ();
	}
}

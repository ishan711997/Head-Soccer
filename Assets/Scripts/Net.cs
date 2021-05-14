using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ball") {
			col.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (col.gameObject.GetComponent<Rigidbody2D> ().velocity.x / 3.0f,
				col.gameObject.GetComponent<Rigidbody2D> ().velocity.y);
		}
	}
}

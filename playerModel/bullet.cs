using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	GameObject player;
	float lifeSpan = 5.0f;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Mathf.Round(transform.position.x) == Mathf.Round(player.transform.position.x) && Mathf.Round(transform.position.z) == Mathf.Round(player.transform.position.z)) 
		{
			if (this.gameObject.tag == "StickyBullet") 
			{
				player.GetComponent<shooting> ().stickyAmmo++;
				Destroy(this.gameObject);
			}
				
		}

		if (this.gameObject.tag == "bouncyBullet") 
		{
			lifeSpan -= Time.deltaTime;

			if (lifeSpan <= 0) 
			{
				player.GetComponent<shooting> ().bouncyAmmo++;
				Destroy(this.gameObject);
			}
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		if (this.gameObject.tag == "StickyBullet") 
		{
				FixedJoint fj = this.gameObject.AddComponent(typeof (FixedJoint)) as FixedJoint;
				fj.connectedBody = collision.rigidbody;	
		}
			
	}
}

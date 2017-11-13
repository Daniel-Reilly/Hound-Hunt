using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour {

	GameObject player;
	public GameObject StickyBullet;
	public GameObject BouncyBullet;
	public int stickyAmmo = 3;
	public int bouncyAmmo = 2;
	public float bulletImpulse = 10f;
	private Transform cam;
	LineRenderer lr;
	public float meshWidth;
	public float angle = 45;
	float g;
	float radianAngle;
	public int resolution = 10;
	public float lastAngle = 0f;
	public AudioClip thump;
	public AudioClip lowThump;

	void Awake()
	{
		lr = GetComponent<LineRenderer> ();
		g = Mathf.Abs (Physics.gravity.y);
	}

	// Use this for initialization
	void Start () 
	{
		//cam = playerController.FindObjectOfType<Camera> ().transform;
		cam = GameObject.FindWithTag ("player1cam").transform;
		player = GameObject.FindWithTag ("Player");
		angle = Mathf.Clamp(angle, player.GetComponent<playerController> ().verticalRotation * -2,0);

		RenderArc ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKey(KeyCode.C)) 
		{
			player.GetComponent<CharacterController> ().height = 1;
		} 
		else 
		{
			player.GetComponent<CharacterController> ().height = 2;
		}

		if (Input.GetKey(KeyCode.Q) && lastAngle != player.GetComponent<playerController> ().verticalRotation * -2) 
		{
			angle = Mathf.Clamp(angle, player.GetComponent<playerController> ().verticalRotation * -2,0);
			lastAngle = angle;
			RenderArc ();
		} 
		else if (Input.GetKey(KeyCode.Q) && lastAngle == player.GetComponent<playerController> ().verticalRotation * -2) 
		{
			
		}
		else 
		{
			angle = 0;
			RenderArc ();
		}
			
		if (Input.GetButtonDown("Fire1") && stickyAmmo > 0) 
		{
			if (StickyBullet != null) 
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.clip = thump;
				audio.Play();

				GameObject theBullet = (GameObject)Instantiate(StickyBullet, transform.position + cam.transform.forward, transform.rotation);
				theBullet.GetComponent<Rigidbody> ().AddForce (cam.forward * bulletImpulse + new Vector3(0,4,0), ForceMode.Impulse);

				stickyAmmo--;
			} 
			else 
			{
				Debug.LogWarning ("No stickybullet object attached to player/shooting script");
			}

		}

		if (Input.GetButtonDown("Fire2") && bouncyAmmo > 0) 
		{
			if (BouncyBullet != null) 
			{
				AudioSource audio = GetComponent<AudioSource>();
				audio.clip = lowThump;
				audio.Play();

				GameObject theBullet = (GameObject)Instantiate(BouncyBullet, transform.position + cam.transform.forward, transform.rotation);
				theBullet.GetComponent<Rigidbody> ().AddForce (cam.forward * bulletImpulse + new Vector3(0,4,0), ForceMode.Impulse);

				bouncyAmmo--;
			}
			else 
			{
				Debug.LogWarning ("No bouncybullet object attached to player/shooting script");
			}

		}

	}

	void RenderArc()
	{
		lr.positionCount = (resolution + 1);
		lr.SetPositions (CalculateArray ());
	}

	Vector3[] CalculateArray()
	{
		Vector3[] arcArray = new Vector3[resolution + 1];
		radianAngle = Mathf.Deg2Rad * angle;
		float maxDistance = (bulletImpulse * bulletImpulse * Mathf.Sin (2 * radianAngle)) / g;

		for (int i = 0; i <= resolution; i++) 
		{
			float t = (float)i / (float)resolution;
			arcArray [i] = CalculateArcPoint (t, maxDistance);
		}
		return arcArray;
	}

	Vector3 CalculateArcPoint (float t, float maxDistance)
	{
		float z = t * maxDistance;
		float y = z * Mathf.Tan (radianAngle) - ((g * z * z) / (2 * bulletImpulse * bulletImpulse * Mathf.Cos (radianAngle) * Mathf.Cos (radianAngle)));
		return new Vector3 (0.5f,y,z);
	}
}

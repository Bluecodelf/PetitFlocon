﻿using UnityEngine;
using System.Collections;

public class StormParticleController : MonoBehaviour {

	public float damage;
	private Camera cam;

	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance(transform.position, cam.transform.position) > 170)
			gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other){
		PlayerController player;
		if ((player = other.gameObject.GetComponent<PlayerController> ()) != null) {
			player.health -= damage;
		}
		Construction construction;
		if ((construction = other.gameObject.GetComponent<Construction> ()) != null) {
			construction.health -= damage;
			gameObject.SetActive(false);
		}
	}
}

﻿using UnityEngine;
using EZObjectPools;
using System.Collections;

public class WeatherController : MonoBehaviour {

	public bool storm;
	public float maxStrenght;
	public float timeScaling;
	public GameObject stormParticlePrefab;

	public float nextTimer;
	public float durationTimer;
	public float strenght;

	private float frequency;
	private int density;
	private float speed;
	private float shakeIntensity;
	private float particleScale;
	private float damage;
	public Vector2 stormDirection;
	private float timer;
	private Rigidbody2D particle;
	private GameObject spawn;
	private float spawnDelay;

	private Vector2 direction;
	private Vector2 parallel;
	private Vector2 stormOrigin;
	private AudioSource audioSource;
	private Vector3 particleInitialPosition;
	private CameraShake cam;

	private EZObjectPool particlePool;

	// Use this for initialization
	void Start () {
		timer = 0;
		spawnDelay = 1 / frequency;
		GetComponent<SpriteRenderer> ().enabled = false;
		parallel = Rotate(stormDirection, 45f);
		audioSource = GetComponent<AudioSource> ();
		GenerateStorm (strenght);
		particlePool = EZObjectPool.CreateObjectPool (stormParticlePrefab, "Storm Pool", 100, true, true, false);
		cam = Camera.main.GetComponent<CameraShake> ();
	}

	void FixedUpdate() {
		if (maxStrenght < 7f)
			maxStrenght += Time.fixedDeltaTime * timeScaling / 100;
		if (storm) {
			if (!audioSource.isPlaying) {
				audioSource.volume = strenght / maxStrenght;
				audioSource.Play ();
			}
			stormOrigin = (Vector2)Camera.main.transform.position - stormDirection * 120;
			cam.shake (shakeIntensity, 2f);

			timer += Time.fixedDeltaTime;
			if (timer >= spawnDelay) {
				for (int i = 0; i < density; i++) {
					particleInitialPosition = Vector3.Lerp (stormOrigin - parallel * 100, stormOrigin + parallel * 100, Random.Range (0f, 1f));
					particlePool.TryGetNextObject (particleInitialPosition, Quaternion.identity, out spawn);
					particle = spawn.GetComponent<Rigidbody2D> ();
					direction = Vector3.Slerp (Rotate (stormDirection, 45f), Rotate (stormDirection, -45f), Random.Range (0f, 1f));
					particle.AddForce (direction * speed * 1000);
					particle.GetComponent<StormParticleController> ().damage = damage;
				}
				timer = 0;
			}
			durationTimer -= Time.fixedDeltaTime;
			if (durationTimer <= 0) {
				storm = false;
				strenght = Random.Range (1, maxStrenght);
				GenerateStorm (strenght);
			}
		} else {
			if (audioSource.isPlaying)
				audioSource.Stop ();
			nextTimer -= Time.fixedDeltaTime;
			if (nextTimer <= 0)
				storm = true;
		}
	}

	public void GenerateStorm(float strenghtFactor) {
		stormDirection = Rotate(Vector2.up, 90f * Random.Range(0, 4));
		parallel = Rotate(stormDirection, 90f);
		density = Mathf.RoundToInt(strenghtFactor * 1.5f);
		frequency = 1f + strenghtFactor * 3f;
		spawnDelay = 1 / frequency;
		speed = 8f + strenghtFactor * 1.5f;
		damage = 1f + strenghtFactor / 10f;
		damage *= 1.5f;
		shakeIntensity = 8f + strenghtFactor * 5f;
		particleScale = 0.5f + strenghtFactor / 10;
		durationTimer = Random.Range(25f, 35f) - strenghtFactor * 2;
		durationTimer /= 1.5f;
		nextTimer = Random.Range(15f, 30f) + strenghtFactor * strenghtFactor * 2;
	}

	private Vector2 Rotate( Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}
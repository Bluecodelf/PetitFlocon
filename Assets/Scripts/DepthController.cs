﻿using UnityEngine;
using System.Collections;

public class DepthController : MonoBehaviour {

	public bool alwaysUnder;
	public bool alwaysAbove;
	// Use this for initialization
	void Start () {
		if (alwaysUnder)
			GetComponent<SpriteRenderer> ().sortingOrder = -32768;
		else if (alwaysAbove)
			GetComponent<SpriteRenderer> ().sortingOrder = 32767;
		else
			GetComponent<SpriteRenderer> ().sortingOrder = Mathf.RoundToInt (-transform.position.y / 8);
	}
}

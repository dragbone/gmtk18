using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour, ITarget
{

	public bool gameOver { get; private set; }
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private float hitPoints = 10;
	public void Hit(float damage)
	{
		hitPoints -= damage;
		Debug.Log("you got hit :( " + hitPoints + "hp remaining");
		if (Math.Abs(hitPoints) < 0.1f)
		{
			Debug.Log("game over boyy");
			gameOver = true;
		}
	}
}

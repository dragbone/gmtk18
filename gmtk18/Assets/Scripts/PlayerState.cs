using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour, ITarget
{

	public bool gameOver { get; private set; }
	public bool levelComplete { get; private set; }
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckLevelComplete(Enemy killedEnemy)
	{
		var foundEnemies = FindObjectsOfType<Enemy>();
		if (!foundEnemies.Any(e => e != killedEnemy))
		{
			levelComplete = true;
			var nextScene = GetNextScene();
			if (nextScene != null)
			{
				SceneManager.LoadScene(nextScene);
			}
			else
			{
				// game complete
			}
		}
	}

	private string GetNextScene()
	{
		switch (SceneManager.GetActiveScene().name)
		{
			case "SampleScene":
				return "level1";
			case "level1":
				return "level2";
			default:
				return null;
		}
	}

	private float _hitPoints = 10;
	public void Hit(float damage)
	{
		_hitPoints -= damage;
		if (_hitPoints <= 0f)
		{
			Debug.Log("game over boyy");
			gameOver = true;
		}
	}
}

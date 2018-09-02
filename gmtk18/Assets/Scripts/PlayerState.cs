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

	private GameObject _level;
	
	// Use this for initialization
	void Start () {
		_level = GameObject.Find("Level");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckLevelComplete(Enemy killedEnemy)
	{
		var foundEnemies = _level.GetComponentsInChildren<Enemy>(includeInactive: true);
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
			case "tutorial":
				return "SampleScene";
			case "SampleScene":
				return "level1";
			case "level1":
				return "level2";
			default:
				return null;
		}
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

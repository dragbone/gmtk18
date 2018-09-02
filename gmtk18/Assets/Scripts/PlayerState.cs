using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour, ITarget
{

	public bool gameOver { get; private set; }
	public bool levelComplete { get; private set; }

	private GameObject _level;
	private float _hitPoints = 3;
	private TextMeshProUGUI _text;
	
	// Use this for initialization
	void Start () {
		_level = GameObject.Find("Level");
		_text = GetComponentInChildren<TextMeshProUGUI>();
		_text.text = $"HP: {_hitPoints:0}";
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
			case "level2":
				return "credits";
			default:
				return null;
		}
	}

	public void Hit(float damage)
	{
		_hitPoints -= damage;
		_text.text = $"HP: {_hitPoints:0}";
		if (_hitPoints <= 0f)
		{
			Debug.Log("game over boyy");
			gameOver = true;
		}
	}
}

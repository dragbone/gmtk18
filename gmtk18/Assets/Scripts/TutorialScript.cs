using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
	private GameObject _player;
	private GameObject _tutorialTextObject;
	private Text _tutorialText;
	private Image _tutorialTextBoxImage;
	private GameObject _currentRoom;
	
	private 
	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player");
		_tutorialTextObject = GameObject.Find("TutorialText");
		_tutorialText = _tutorialTextObject.GetComponentInChildren<Text>();
		_currentRoom = GameObject.Find("Room 1");
		_tutorialText = _tutorialTextObject.GetComponentInChildren<Text>();
		_tutorialTextBoxImage = _tutorialTextObject.GetComponentInChildren<Image>();
		
		StartTutorial();
	}

	void StartTutorial()
	{
		_tutorialText.text =
			"Welcome to [Gamename]. You play as a Scriptkid with an awesome Aimbot and Wallhack. Unfortunately, you forgot how to turn it off, so you have to navigate with the help of your enemies";

		Invoke(nameof(Step1), 10);
	}

	void Step1()
	{
		StartCoroutine(FadeTextToZeroAlpha(2f, () => {}));
		Invoke(nameof(Step2), 3);
	}

	void Step2()
	{
		_tutorialText.text =
			"Use A and D to switch the targeted enemy. Try to get behind the Cube";
		StartCoroutine(FadeTextToFullAlpha(2f, () =>
			{
				Invoke(nameof(Step3), 3);
			}));
	}

	void Step3()
	{
		StartCoroutine(FadeTextToZeroAlpha(2f, () => { }));
	}

	void Step4()
	{
		_tutorialText.text =
			"Well done! Enemies begin to charge their attacks if you are visible to them. To avoid being hit, get behind a Wall or shoot them yourself";
		StartCoroutine(FadeTextToFullAlpha(2f, () =>
		{
			Invoke(nameof(Step5), 3);
		}));
	}
	
	void Step5()
	{
		StartCoroutine(FadeTextToZeroAlpha(2f, () => { }));
	}
	
	// Update is called once per frame
	void Update ()
	{
		var door = _currentRoom.transform.Find("Door");
		if (Vector3.Distance(_player.transform.position, door.position) < 3f)
		{
			var collider = door.gameObject.GetComponent<BoxCollider>();
			collider.enabled = false;
			var renderer = door.gameObject.GetComponent<Renderer>();
			renderer.enabled = false;
			_currentRoom = GameObject.Find("Room 2");
			StopAllCoroutines();
			InstantTextFade();
			Step4();
		}
	}

	private void InstantTextFade()
	{
		_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, 0);
		_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, 0);
	}
	
	private IEnumerator FadeTextToZeroAlpha(float t, Action callback)
	{
		_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, 1);
		while (_tutorialText.color.a > 0.0f || _tutorialTextBoxImage.color.a > 0.0f)
		{
			_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, _tutorialText.color.a - (Time.deltaTime / t));
			if (_tutorialTextBoxImage.color.a > 0.0f)
			{
				_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, _tutorialTextBoxImage.color.a - (Time.deltaTime / t));
			}
			yield return null;
		}

		callback();
	}
	
	private IEnumerator FadeTextToFullAlpha(float t, Action callback)
	{
		_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, 0);
		while (_tutorialText.color.a < 1.0f)
		{
			_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, _tutorialText.color.a + (Time.deltaTime / t));
			yield return null;
		}

		callback();
	}
}

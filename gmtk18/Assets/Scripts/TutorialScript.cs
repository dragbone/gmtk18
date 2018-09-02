using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
	private GameObject _player;
	private GameObject _tutorialTextObject;
	private Text _tutorialText;
	private Image _tutorialTextBoxImage;
	private GameObject _currentRoom;
	private GameObject[] Rooms;
	
	private 
	// Use this for initialization
	void Start () {
		_player = GameObject.Find("Player");
		_tutorialTextObject = GameObject.Find("TutorialText");
		_tutorialText = _tutorialTextObject.GetComponentInChildren<Text>();
		_tutorialText = _tutorialTextObject.GetComponentInChildren<Text>();
		_tutorialTextBoxImage = _tutorialTextObject.GetComponentInChildren<Image>();

		Rooms = GameObject.FindGameObjectsWithTag("Room")
			.OrderBy(r => r.name)
			.ToArray();
		
		StartTutorial();
	}

	void StartTutorial()
	{
		_currentRoom = Rooms[0];
		DespawnRoom(Rooms[1]);
		DespawnRoom(Rooms[2]);

		DisplayText(@"Welcome to aimnot. You play as a script kiddie with an awesome aimbot and wallhack.
Unfortunately, you forgot how to turn it off, so you have to navigate with the help of objects in the world");

		Invoke(nameof(Step1), 10);
	}

	void Step1()
	{
		DisplayText("Use A and D to switch the targeted object. Try to get behind the cube.");
	}

	void Step2()
	{
		DisplayText("Well done! The Enemy in the next room begins to charge its attack if you are visible to them. To avoid being hit, get behind a wall or shoot them yourself");
	}

	void Step3()
	{
		DisplayText("Well done! Eliminate all enemies in the next room to complete the tutorial and move to the first level");
	}

	void DisplayText(string text)
	{
		_tutorialText.text = text;
		StartCoroutine(FadeTextToFullAlpha(2f));
		Invoke(nameof(FadeText), 5);
	}

	void FadeText()
	{
		StartCoroutine(FadeTextToZeroAlpha(2f));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_currentRoom == Rooms[0])
		{
			var door = _currentRoom.transform.Find("Door");
			if (Vector3.Distance(_player.transform.position, door.position) < 4f)
			{
				door.gameObject.SetActive(false);
				InterruptCurrentTutorialStep();
				_currentRoom = Rooms[1];
				SpawnRoom(_currentRoom);
				Step2();
			}
		}
		
		if (_currentRoom == Rooms[1])
		{
			var enemies = _currentRoom.gameObject.GetComponentsInChildren<Enemy>();
			if (!enemies.Any())
			{
				var door = _currentRoom.transform.Find("Door");
				door.gameObject.SetActive(false);
				_currentRoom = Rooms[2];
				SpawnRoom(_currentRoom);
				Step3();
			}
		}
	}

	void DespawnRoom(GameObject room)
	{
		var enemies = room.gameObject.GetComponentsInChildren<Enemy>();
		foreach (var enemy in enemies)
		{
			enemy.gameObject.SetActive(false);
		}
		var friendlyTargets = room.gameObject.GetComponentsInChildren<FriendlyTarget>();
		foreach (var target in friendlyTargets)
		{
			target.gameObject.SetActive(false);
		}
	}
	
	void SpawnRoom(GameObject room)
	{
		var enemies = room.gameObject.GetComponentsInChildren<Enemy>(true);
		foreach (var enemy in enemies)
		{
			enemy.gameObject.SetActive(true);
		}
		var friendlyTargets = room.gameObject.GetComponentsInChildren<FriendlyTarget>(true);
		foreach (var target in friendlyTargets)
		{
			target.gameObject.SetActive(true);
		}
	}

	private void InterruptCurrentTutorialStep()
	{
		StopAllCoroutines();
		CancelInvoke();
		_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, 0);
		_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, 0);
	}
	
	private IEnumerator FadeTextToZeroAlpha(float t)
	{
		_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, 1);
		_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, 0.5f);
		while (_tutorialText.color.a > 0.0f)
		{
			_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, _tutorialText.color.a - (Time.deltaTime / t));
			if (_tutorialTextBoxImage.color.a > 0.0f)
			{
				_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, _tutorialTextBoxImage.color.a - (Time.deltaTime / t / 2));
			}
			yield return null;
		}
	}
	
	private IEnumerator FadeTextToFullAlpha(float t)
	{
		_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, 0);
		_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, 0);
		while (_tutorialText.color.a < 1.0f)
		{
			_tutorialText.color = new Color(_tutorialText.color.r, _tutorialText.color.g, _tutorialText.color.b, _tutorialText.color.a + (Time.deltaTime / t));
			if (_tutorialTextBoxImage.color.a < 0.5f)
			{
				_tutorialTextBoxImage.color = new Color(_tutorialTextBoxImage.color.r, _tutorialTextBoxImage.color.g, _tutorialTextBoxImage.color.b, _tutorialTextBoxImage.color.a + (Time.deltaTime / t / 2));
			}
			yield return null;
		}
	}
}

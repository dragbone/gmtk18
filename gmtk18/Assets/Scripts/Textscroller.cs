using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textscroller : MonoBehaviour
{
	private RectTransform _textPosition;
	// Use this for initialization
	void Start () {
		_textPosition = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_textPosition.position.y < 318f)
		{
			_textPosition.SetPositionAndRotation(new Vector3(_textPosition.position.x, _textPosition.position.y + Time.deltaTime * 100f, 0f), Quaternion.identity);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    public float State { get; private set; } = 0f;
    public float ShootSpeed = 0.5f;

    public GameObject ProgressBarPrefab;

    private GameObject stateProgressBar;

    private Canvas _canvas;

    private Camera _camera;

    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        _canvas = FindObjectOfType<Canvas>();
        _camera = FindObjectOfType<Camera>();
        stateProgressBar = Instantiate(ProgressBarPrefab, _canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < 5f)
        {
            State += Time.deltaTime * ShootSpeed;
            while (State >= 1f)
            {
                State -= 1f;
                // Shooty shoot now!
            }
        }
        else
        {
            if (State < 0f)
            {
                State = Math.Min(State + Time.deltaTime * ShootSpeed, 0f);
            }
            else if (State > 0f)
            {
                State = Math.Max(State - Time.deltaTime * ShootSpeed, 0f);
            }
        }
        
        var canvasRect = _canvas.GetComponent<RectTransform>();
        
        var viewportPoint = _camera.WorldToViewportPoint(transform.position);
        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y > 0 && viewportPoint.y <= 1 && viewportPoint.z >= 0)
        {
            stateProgressBar.SetActive(true);
        }
        else
        {
            stateProgressBar.SetActive(false);
            return;
        }
        
        var progressBarCanvasPosition=new Vector2(
            ((viewportPoint.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
            ((viewportPoint.y*canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)));
 
        var progressbarRectTransform = stateProgressBar.GetComponent<RectTransform>();
        progressbarRectTransform.anchoredPosition=progressBarCanvasPosition - new Vector2(-50, 0);
        
        var progressbarSlider = stateProgressBar.GetComponent<Slider>();
        progressbarSlider.value = State;
    }
    
    void OnDestroy()
    {
        Destroy(stateProgressBar);
    }
}
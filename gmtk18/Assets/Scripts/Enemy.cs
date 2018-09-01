﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private Slider _stateProgressBar;
    
    public float State { get; private set; } = 0f;
    public float ShootSpeed = 0.5f;

    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        _stateProgressBar = GetComponentInChildren<Slider>();
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

        if (State <= -1f)
        {
            Destroy(gameObject);
        }
        _stateProgressBar.value = State;
    }

    public void Hit(float damage)
    {
        State -= damage;
    }
}
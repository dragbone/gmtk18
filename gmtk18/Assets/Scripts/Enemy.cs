using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    public float State { get; private set; } = 0f;
    public float ShootSpeed = 0.5f;

    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().gameObject;
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
    }
}
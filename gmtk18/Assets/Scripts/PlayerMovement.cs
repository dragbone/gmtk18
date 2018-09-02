﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public PlayerTargeting PlayerTargeting;
    private float _speed = 4f;

    private bool canJump = false;

    private void OnCollisionStay(Collision other)
    {
        canJump = true;
    }

    private void OnCollisionExit(Collision other)
    {
        canJump = false;
    }

    void Update()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerTargeting.SwitchEnemy(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerTargeting.SwitchEnemy(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5f, rigidbody.velocity.z);
        }

        if (Input.GetMouseButton(0))
        {
            PlayerTargeting.Shoot();
        }

        transform.Translate(direction * _speed * Time.deltaTime);
    }

}
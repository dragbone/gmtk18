using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private PlayerTargeting _playerTargeting;
    private PlayerState _playerState;
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

    void Start()
    {
        _playerTargeting = GetComponent<PlayerTargeting>();
        _playerState = GetComponent<PlayerState>();
    }

    private Vector3 moveDirection;

    void Update()
    {
        if (_playerState.gameOver)
        {
            // play death animation
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (transform.position.y < -50f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

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
            _playerTargeting.SwitchTarget(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _playerTargeting.SwitchTarget(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5f, rigidbody.velocity.z);
        }

        if (Input.GetMouseButton(0))
        {
            _playerTargeting.Shoot();
        }

        moveDirection = direction * _speed;
    }

    private void FixedUpdate()
    {
        transform.Translate(moveDirection * Time.deltaTime);
    }
}
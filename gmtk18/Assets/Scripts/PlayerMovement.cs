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
    public PlayerTargeting PlayerTargeting;
    public PlayerState PlayerState;
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
        if (PlayerState.gameOver)
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

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5f, rigidbody.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerTargeting.SwitchEnemy();
        }

        if (Input.GetMouseButton(0))
        {
            PlayerTargeting.Shoot();
        }

        transform.Translate(direction * _speed * Time.deltaTime);
    }

}
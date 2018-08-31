using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;

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

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        var eulerAngles = Quaternion.LookRotation(Enemy.transform.position - transform.position, Vector3.up)
            .eulerAngles;
        Player.transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        Player.transform.Translate(direction * Time.deltaTime);

        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    }
}
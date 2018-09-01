using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Camera;
    public Text GuiText;
    public GameObject ShotPrefab;
    private float _speed = 4f;

    private GameObject currentEnemy;

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

        /*if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }*/

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5f, rigidbody.velocity.z);
        }

        var enemies = Physics.OverlapSphere(transform.position, 20f)
            .Select(c => c.gameObject)
            .Where(g => g.CompareTag("Enemy"))
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
            .ToList();

        if (currentEnemy == null)
        {
            currentEnemy = enemies.FirstOrDefault()?.gameObject;
        }

        if (currentEnemy == null) return;

        var enemyTexts = enemies.Select(GetEnemyRadarText);

        GuiText.text = "Enemies:" + Environment.NewLine + String.Join(Environment.NewLine, enemyTexts);

        var currentEnemyIndex = enemies.IndexOf(currentEnemy);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var nextEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
            currentEnemy = enemies[nextEnemyIndex];
        }

        if (Input.GetMouseButtonDown(0) && currentEnemy != null)
        {
            var shot = Instantiate(ShotPrefab, Camera.transform.position, Quaternion.identity);
            shot.GetComponent<Shot>().Construct(currentEnemy);
        }

        UpdatedOrientation();
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void UpdatedOrientation()
    {
        var targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - Camera.transform.position, Vector3.up);
        var lerpEulerAngles = Quaternion.Lerp(Camera.transform.rotation, targetRotation, 15f * Time.deltaTime).eulerAngles;

        transform.rotation = Quaternion.Euler(0f, lerpEulerAngles.y, 0f);
        Camera.transform.rotation = Quaternion.Euler(lerpEulerAngles.x, lerpEulerAngles.y, lerpEulerAngles.z);
    }

    private string GetEnemyRadarText(GameObject enemy)
    {
        var distanceString = Vector3.Distance(transform.position, enemy.gameObject.transform.position)
            .ToString("0.00");
        var enemyMarker = enemy == currentEnemy ? " <" : "";
        return $"{enemy.name}: [distance: {distanceString}]{enemyMarker}";
    }
}
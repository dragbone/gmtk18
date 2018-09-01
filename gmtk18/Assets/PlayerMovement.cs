using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Player;
    public Text GuiText;

    private GameObject currentEnemy;

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
        
        var hitColliders = Physics.OverlapSphere(Player.transform.position, 20f);

        var enemies = new List<GameObject>();
        
        foreach(var collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(collider.gameObject);
                if (currentEnemy == null)
                {
                    currentEnemy = collider.gameObject;
                }
            }
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
            
        var eulerAngles = Quaternion.LookRotation(currentEnemy.transform.position - transform.position, Vector3.up)
            .eulerAngles;
        
        var playerTargetRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
        if (Player.transform.rotation != playerTargetRotation)
        {
            Player.transform.rotation = Quaternion.Slerp(transform.rotation, playerTargetRotation, 15f * Time.deltaTime);
        }
        
        var targetRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
        }
        
        Player.transform.Translate(direction * Time.deltaTime);
    }

    private string GetEnemyRadarText(GameObject enemy)
    {
        var isCurrentEnemy = enemy == currentEnemy;
        return enemy.name + " : Distance: " + Vector3
                   .Distance(Player.transform.position, enemy.gameObject.transform.position).ToString().Substring(0, 5)
            + (isCurrentEnemy ? " <" : "");
    }
}
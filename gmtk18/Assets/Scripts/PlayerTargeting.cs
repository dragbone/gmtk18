﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    public Camera Camera;
    public Enemy CurrentEnemy { get; private set; }
    public Text GuiText;
    public GameObject ShotPrefab;

    private PlayerState _playerState;

    public void Start()
    {
        _playerState = FindObjectOfType<PlayerState>();
    }

    void Update()
    {
        if (_playerState.gameOver)
        {
            return;
        }
        if (CurrentEnemy != null)
        {
            UpdatedOrientation();
        }
        else
        {
            SwitchEnemy(true);
        }

        var enemies = GetEnemies();
        var enemyTexts = enemies.Select(GetEnemyRadarText);
        GuiText.text = "Enemies:" + Environment.NewLine + String.Join(Environment.NewLine, enemyTexts);
        _shooting = Math.Max(_shooting - Time.deltaTime, 0f);
    }

    public const float PlayerTargetDistance = 80f;
    private List<Enemy> GetEnemies()
    {
        return Physics.OverlapSphere(transform.position, PlayerTargetDistance)
            .Select(c => c.gameObject)
            .Where(g => g.GetComponent<Enemy>() != null)
            .Select(g => g.GetComponent<Enemy>())
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
            .ToList();
    }

    public void SwitchEnemy(bool right)
    {
        var enemies = GetEnemies();

        if (enemies.Any())
        {
            if (CurrentEnemy == null)
            {
                CurrentEnemy = enemies.First();
            }
            else
            {
                var enemyPositions = enemies.Select(e => new
                {
                    Enemy = e,
                    Pos = Camera.WorldToScreenPoint(e.transform.position)
                }).ToList();
                var currentEnemyPos = enemyPositions.Single(e => e.Enemy == CurrentEnemy).Pos;
                if (right)
                {
                    enemyPositions = enemyPositions.Where(e => e.Pos.x > currentEnemyPos.x)
                        .OrderBy(e => e.Pos.x)
                        .ToList();
                }
                else
                {
                    enemyPositions = enemyPositions.Where(e => e.Pos.x < currentEnemyPos.x)
                        .OrderByDescending(e => e.Pos.x)
                        .ToList();
                }

                var nextEnemy = enemyPositions.FirstOrDefault();
                if (nextEnemy != null)
                {
                    CurrentEnemy = nextEnemy.Enemy;
                }
            }
        }
    }

    private string GetEnemyRadarText(Enemy enemy)
    {
        var distance = Vector3.Distance(transform.position, enemy.transform.position);

        var enemyMarker = enemy == CurrentEnemy ? " <" : "";
        return $"{enemy.name}: [distance: {distance:0.00}, state: {enemy.State:0.00}]{enemyMarker}";
    }

    private void UpdatedOrientation()
    {
        var targetRotation =
            Quaternion.LookRotation(CurrentEnemy.transform.position - Camera.transform.position, Vector3.up);

        var lerpEulerAngles = Quaternion.Lerp(Camera.transform.rotation, targetRotation, 15f * Time.deltaTime)
            .eulerAngles;

        transform.rotation = Quaternion.Euler(0f, lerpEulerAngles.y, 0f);
        Camera.transform.rotation = Quaternion.Euler(lerpEulerAngles.x, lerpEulerAngles.y, lerpEulerAngles.z);
    }

    private float _shooting = 0f;
    private const float ShootingTime = 0.25f;

    public void Shoot()
    {
        if (CurrentEnemy != null)
        {
            if (_shooting <= 0f)
            {
                var shot = Instantiate(ShotPrefab, transform.position, Quaternion.identity);
                shot.GetComponent<Shot>().Construct(CurrentEnemy.gameObject, 0.25f);
                _shooting = ShootingTime;
            }
        }
    }
}
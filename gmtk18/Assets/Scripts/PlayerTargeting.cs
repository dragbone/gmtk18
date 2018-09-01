using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    public GameObject Camera;
    public Enemy CurrentEnemy { get; private set; }
    public Text GuiText;
    public GameObject ShotPrefab;

    void Update()
    {
        if (CurrentEnemy != null)
        {
            UpdatedOrientation();
        }
        else
        {
            SwitchEnemy();
        }

        var enemies = Physics.OverlapSphere(transform.position, 20f)
            .Select(c => c.gameObject)
            .Where(g => g.GetComponent<Enemy>() != null)
            .Select(g => g.GetComponent<Enemy>())
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
            .ToList();
        var enemyTexts = enemies.Select(GetEnemyRadarText);
        GuiText.text = "Enemies:" + Environment.NewLine + String.Join(Environment.NewLine, enemyTexts);
        _shooting = Math.Max(_shooting - Time.deltaTime, 0f);
    }

    public void SwitchEnemy()
    {
        var enemies = Physics.OverlapSphere(transform.position, 20f)
            .Select(c => c.gameObject)
            .Where(g => g.GetComponent<Enemy>() != null)
            .Select(g => g.GetComponent<Enemy>())
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
            .ToList();

        if (enemies.Any())
        {
            var currentEnemyIndex = enemies.IndexOf(CurrentEnemy);
            var nextEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
            CurrentEnemy = enemies[nextEnemyIndex];
        }

        CurrentEnemy = CurrentEnemy ?? enemies.FirstOrDefault();
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
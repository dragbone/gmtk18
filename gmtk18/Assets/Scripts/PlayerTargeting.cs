using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    public const float PlayerTargetDistance = 32f;
    public Camera Camera;
    public GameObject CurrentTarget { get; private set; }
    public Text GuiText;
    public GameObject ShotPrefab;

    private PlayerState _playerState;

    public void Start()
    {
        _playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (_playerState.gameOver)
        {
            return;
        }

        if (CurrentTarget != null)
        {
            UpdatedOrientation();
        }
        else
        {
            SwitchTarget(true);
        }

        var enemies = GetEnemies();
        var enemyTexts = enemies.Select(GetEnemyRadarText);
        GuiText.text = "Enemies:" + Environment.NewLine + String.Join(Environment.NewLine, enemyTexts);
        _shooting = Math.Max(_shooting - Time.deltaTime, 0f);
    }


    private List<GameObject> GetTargets()
    {
        return Physics.OverlapSphere(transform.position, PlayerTargetDistance)
            .Select(c => c.gameObject)
            .Where(g => g.GetComponent<ITarget>() != null && !(g.GetComponent<ITarget>() is PlayerState))
            .ToList();
    }

    private List<Enemy> GetEnemies()
    {
        return GetTargets()
            .Select(g => g.GetComponent<Enemy>())
            .Where(e => e != null)
            .ToList();
    }

    private static Vector3 ZeroY(Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }

    public void SwitchTarget(bool right)
    {
        var targets = GetTargets();

        if (targets.Any())
        {
            var targetPositions = targets
                .Where(t => t != CurrentTarget)
                .Select(e => new
                {
                    Target = e,
                    Dir = Vector3.SignedAngle(
                        @from: (transform.rotation * Vector3.forward).normalized,
                        to: ZeroY(e.transform.position - transform.position).normalized,
                        axis: Vector3.up)
                })
                .ToList();
            if (CurrentTarget == null)
            {
                targetPositions = targetPositions
                    .OrderBy(e => Math.Abs(e.Dir))
                    .ToList();
            }
            else if (right)
            {
                targetPositions = targetPositions.Where(e => e.Dir > 0)
                    .OrderBy(e => e.Dir)
                    .ToList();
            }
            else
            {
                targetPositions = targetPositions.Where(e => e.Dir < 0)
                    .OrderByDescending(e => e.Dir)
                    .ToList();
            }

            var nextEnemy = targetPositions.FirstOrDefault(e => e.Target != CurrentTarget);
            if (nextEnemy != null)
            {
                CurrentTarget = nextEnemy.Target;
            }
        }
    }

    private string GetEnemyRadarText(Enemy enemy)
    {
        var distance = Vector3.Distance(transform.position, enemy.transform.position);

        var enemyMarker = enemy == CurrentTarget ? " <" : "";
        return $"{enemy.name}: [distance: {distance:0.00}, state: {enemy.State:0.00}]{enemyMarker}";
    }

    private void UpdatedOrientation()
    {
        var targetRotation =
            Quaternion.LookRotation(CurrentTarget.transform.position - Camera.transform.position, Vector3.up);

        var lerpEulerAngles = Quaternion.Lerp(Camera.transform.rotation, targetRotation, 15f * Time.deltaTime)
            .eulerAngles;

        transform.rotation = Quaternion.Euler(0f, lerpEulerAngles.y, 0f);
        Camera.transform.rotation = Quaternion.Euler(lerpEulerAngles.x, lerpEulerAngles.y, lerpEulerAngles.z);
    }

    private float _shooting = 0f;
    private const float ShootingTime = 0.25f;

    public void Shoot()
    {
        if (CurrentTarget != null)
        {
            if (Physics.Linecast(Camera.transform.position, CurrentTarget.transform.position, ~(1 << 9)))
            {
                return;
            }

            if (_shooting <= 0f)
            {
                var gun = GetComponentInChildren<Gun>();
                var shot = Instantiate(ShotPrefab, gun.transform.position, Quaternion.identity);
                shot.GetComponent<Shot>().Construct(CurrentTarget.gameObject, 0.25f);
                _shooting = ShootingTime;
            }
        }
    }
}
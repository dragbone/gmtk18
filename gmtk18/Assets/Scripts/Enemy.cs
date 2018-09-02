using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, ITarget
{
    private GameObject _player;
    private PlayerState _playerState;
    private Slider _stateProgressBar;

    public float State { get; private set; } = 0f;
    public float ShootSpeed = 5f;
    public GameObject ShotPrefab;

    private Material _wobbleMaterial;
    private float _wobbleState;

    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().gameObject;
        _playerState = FindObjectOfType<PlayerState>();
        _stateProgressBar = GetComponentInChildren<Slider>();

        var renderers = GetComponentsInChildren<MeshRenderer>();
        _wobbleMaterial = renderers.Select(mr => mr.material)
            .First(m => m.name.Contains("EnemyWobble"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= PlayerTargeting.PlayerTargetDistance)
        {
            State += Time.deltaTime * ShootSpeed;
            while (State >= 1f)
            {
                State -= 1f;
                _wobbleState += 8f;
                // Shooty shoot now!
                Shoot();
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

        if (State <= -1f)
        {
            Destroy(gameObject);
        }

        _stateProgressBar.value = State / 2f + 0.5f;
        _shooting = Math.Max(_shooting - Time.deltaTime, 0f);

        _wobbleState = Mathf.Lerp(_wobbleState, State, 0.1f);
        _wobbleMaterial.SetFloat("_Strength", Math.Max(_wobbleState * 0.5f, 0f));
        _wobbleMaterial.SetFloat("_Speed", Math.Max(_wobbleState * 0.5f, 0f));
    }

    public void Hit(float damage)
    {
        State -= damage;
    }


    private float _shooting = 0f;
    private const float ShootingTime = 0.25f;

    public void Shoot()
    {
        if (_shooting <= 0f)
        {
            var shot = Instantiate(ShotPrefab, transform.position, Quaternion.identity);
            shot.GetComponent<Shot>().Construct(_playerState.gameObject, 1f);
            _shooting = ShootingTime;
        }
    }
}
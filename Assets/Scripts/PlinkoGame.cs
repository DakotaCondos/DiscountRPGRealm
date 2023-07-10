using System;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoGame : SceneSingleton<PlinkoGame>
{
    [Header("Ball")]
    [SerializeField] private GameObject _ball;
    [SerializeField] private Transform _ballStartTransform;
    private Rigidbody _ballRigidBody;
    private SphereCollider _ballCollider;

    [Header("Physics")]
    [SerializeField] private List<GameObject> _colliders;
    [SerializeField] private PhysicMaterial _bounceMaterial;
    [SerializeField] private PhysicMaterial _noBounceMaterial;

    [Header("Random Force")]
    [SerializeField] private float minForce = 1f;
    [SerializeField] private float maxForce = 5f;

    [Header("Game")]
    [SerializeField] private bool _gameInProgress = true;
    [SerializeField] private AudioClip _pegHitSound;
    [SerializeField] private AudioClip _successSound;
    [SerializeField] private AudioClip _failSound;
    [SerializeField] private float _timeHitSoundPlayed = 0;
    [SerializeField] private float _minDelayHitSound = 0.2f;


    private new void Awake()
    {
        base.Awake();
        _ballRigidBody = _ball.GetComponent<Rigidbody>();
        _ballCollider = _ball.GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        ResetBall();

    }

    private void OnDisable()
    {
        ResetBall();
    }

    public void ResetBall()
    {
        _ballRigidBody.useGravity = false;
        _ballCollider.material = _bounceMaterial;
        _ballRigidBody.velocity = Vector3.zero;
        _ballRigidBody.angularVelocity = Vector3.zero;
        _ball.transform.position = _ballStartTransform.position;
        SetCollidersActive(true);
    }

    public void Play()
    {
        _ballRigidBody.useGravity = true;
        _gameInProgress = true;
        ApplyRandomMomentum();
    }

    public void TriggerOuter()
    {
        _gameInProgress = false;
        _ballCollider.material = _noBounceMaterial;
        PlaySound(_failSound);
        SetCollidersActive(false);
    }

    public void TriggerCenter()
    {
        _gameInProgress = false;
        _ballCollider.material = _noBounceMaterial;
        PlaySound(_successSound);
        SetCollidersActive(false);
    }

    private void SetCollidersActive(bool value)
    {
        foreach (var item in _colliders)
        {
            item.SetActive(value);
        }
    }

    private void ApplyRandomMomentum()
    {
        float forceMagnitude = UnityEngine.Random.Range(minForce, maxForce);
        Vector3 forceDirection = UnityEngine.Random.insideUnitCircle;
        Vector3 force = forceDirection * forceMagnitude;
        _ballRigidBody.AddForce(force, ForceMode.Impulse);
    }

    public void BallHit()
    {
        if (!_gameInProgress) { return; }
        if (Time.time - _timeHitSoundPlayed > _minDelayHitSound)
        {
            _timeHitSoundPlayed = Time.time;
            PlaySound(_pegHitSound);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        AudioManager.Instance.PlaySound(clip, AudioChannel.SFX);
    }
}

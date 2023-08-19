using System;
using System.Collections.Generic;
using UnityEngine;
using SystemRandom = System.Random;

public class ExplosiveProjectile : MonoBehaviour
{
    public static Action<SplintersDamage, Vector3, Quaternion, Tank> OnProjectileHit;

    [SerializeField] private float _projectileSpeed = 1f;
    [SerializeField] private float _splintersRadius = 12;
    [SerializeField] private int _splintersCount = 40;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private BoxCollider _collider;

    private bool _alreadyExplosive;


    private void Start()
    {
        _rigidbody.AddForce(transform.forward * 3000);
    }

    private void OnCollisionEnter(Collision tankCollision)
    {
        if (_alreadyExplosive)
        {
            return;
        }
        _collider.enabled = false;
        if (tankCollision.collider.transform.root.TryGetComponent<Tank>(out var tank))
        {
            OnProjectileHit?.Invoke(ExplosionSimulation(), transform.forward, transform.rotation, tank);
        }
        _alreadyExplosive = true;
        Destroy(gameObject);
    }

    private SplintersDamage ExplosionSimulation()
    {
        var splinters = new List<Splinter>();
        var explosionPosition = transform.position + (transform.forward * 1.5f);
        var killedCrewMembers = new List<CrewMember>();
        var random = new SystemRandom();
        for (int i = 0; i < _splintersCount; i++)
        {
            var quaternion = new Quaternion()
            {
                eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360))
            };
            if (Splinter.CheckSplinterHit(explosionPosition, transform.forward, quaternion, _splintersRadius, out var splinter, out var killedCrew))
            {
                killedCrewMembers.AddRange(killedCrew);
                splinters.Add(splinter);
            }
        }
        return new SplintersDamage(splinters, explosionPosition, killedCrewMembers);
    }

    private SplintersDamage KineticSimulation()
    {
        var splinters = new List<Splinter>();
        var explosionPosition = transform.position + (transform.forward * 1f);
        var killedCrewMembers = new List<CrewMember>();
        var random = new SystemRandom();
        var radius = 25;
        for (int i = 0; i < _splintersCount; i++)
        {
            var quaternion = new Quaternion()
            {
                eulerAngles = new Vector3(random.NextFloat(-radius, radius), random.NextFloat(-radius, radius), random.NextFloat(-radius, radius))
            };
            if (Splinter.CheckSplinterHit(explosionPosition, transform.forward, quaternion, _splintersRadius, out var splinter, out var killedCrew))
            {
                killedCrewMembers.AddRange(killedCrew);
                splinters.Add(splinter);
            }
        }
        return new SplintersDamage(splinters, explosionPosition, killedCrewMembers);
    }
}

public static class RandomExtension
{
    public static float NextFloat(this SystemRandom random, int min, int max, int coof = 100)
    {
        var value = random.Next(min * coof, max * coof);
        return ((float)value) / ((float)coof);
    }
}
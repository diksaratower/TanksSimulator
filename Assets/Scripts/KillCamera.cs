using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Tank _tankPrefab;
    [SerializeField] private Transform _tankPosition;
    [SerializeField] private KillCameraProjectile _killCameraProjectilePrefab;
    [SerializeField] private RawImage _killCameraUI;
    
    private Tank _tankView;
    private KillCameraProjectile _killCameraProjectile;

    private void Start()
    {
        ExplosiveProjectile.OnProjectileHit += KillCameraVisualise;
    }

    private void KillCameraVisualise(SplintersDamage explosion, Vector3 projectileDirection, Quaternion projectileRotation, Tank hitTank)
    {
        Clear();
        _killCameraUI.gameObject.SetActive(true);
        _camera.gameObject.SetActive(true);
        transform.position = hitTank.transform.position;
        var tank = Instantiate(_tankPrefab, hitTank.transform.position, hitTank.transform.rotation);
        var crewMembersVisualise = tank.GetComponentsInChildren<CrewMember>();
        var crewMembers = hitTank.GetComponentsInChildren<CrewMember>();
        var killedMembersViews = new List<CrewMember>();
        for (int i = 0; i < crewMembersVisualise.Length; i++)
        {
            if (crewMembers[i].IsDead() && !explosion.KilledCrewMembers.Contains(crewMembers[i]))
            {
                crewMembersVisualise[i].Colored(Color.red);
            }
            if (crewMembers[i].IsDead() && explosion.KilledCrewMembers.Contains(crewMembers[i]))
            {
                killedMembersViews.Add(crewMembersVisualise[i]);
            }
        }
        StartCoroutine(ViewKilledCrewMembers(killedMembersViews));
        tank.GetComponent<Rigidbody>().isKinematic = true;
        tank.IsUsebleTank = false;
        _tankView = tank;
        SetLayerAllChildren(tank.transform, 6);
        var killCameraProjectile = Instantiate(_killCameraProjectilePrefab, explosion.StartPosition
            + (projectileDirection * -6f), projectileRotation);
        killCameraProjectile.StartVisualision(explosion);
        _killCameraProjectile = killCameraProjectile;
        killCameraProjectile.OnEndSimulation += delegate
        {
            Clear();
        };
    }

    private IEnumerator ViewKilledCrewMembers(List<CrewMember> crewMembers)
    {
        yield return new WaitForSeconds(1f);
        foreach (var crewMember in crewMembers)
        {
            crewMember.Colored(Color.red);
            var outline = crewMember.gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 4;
        }
    }

    private void Clear()
    {
        if (_tankView != null)
        {
            Destroy(_tankView.gameObject);
        }
        if (_killCameraProjectile != null)
        {
            Destroy(_killCameraProjectile.gameObject);
        }
        _killCameraUI.gameObject.SetActive(false);
        _camera.gameObject.SetActive(false);
    }

    private void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}

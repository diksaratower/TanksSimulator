using System.Collections.Generic;
using UnityEngine;


public class Tank : MonoBehaviour
{
    public bool IsUsebleTank;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private ExplosiveProjectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private Material _tankTransparent;
    [SerializeField] private OrbitCamera _tankCamera;
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _turretPivot;
    [SerializeField] private List<Track> _tracks = new List<Track>();
    [SerializeField] private List<CrewMember> _crewMembers = new List<CrewMember>();
    [SerializeField] private GameObject _killedEffectPrefab;

    private Vector3 _previewMousePos;

    private void Start()
    {
        _tankCamera.gameObject.SetActive(IsUsebleTank);
    }

    private void Update()
    {
        if (_crewMembers.Count == _crewMembers.FindAll(member => member.IsDead() == true).Count)
        {
            Instantiate(_killedEffectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (IsUsebleTank == false)
        {
            return;
        }
        Move();
        RotateCamera();
        RotateTurret();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    public void SetTransparent()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = _tankTransparent;
        }
    }

    private void Shoot()
    {
        Instantiate(_projectilePrefab, _projectileSpawnPoint.transform.position, _projectileSpawnPoint.transform.rotation);
    }

    private void Move()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        foreach (var track in _tracks)
        {
            track.Move(horizontal, vertical);
        }
    }

    private void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 mouseDelta = Input.mousePosition - _previewMousePos;
            Vector3 moveDelta = mouseDelta * (360f / Screen.height);
            _tankCamera.Move(moveDelta.x, -moveDelta.y);
        }
        _previewMousePos = Input.mousePosition;
    }

    private void RotateTurret()
    {
        var ray = new Ray(_tankCamera.transform.position, _tankCamera.transform.forward);
        var hits = Physics.RaycastAll(ray, 1000);
        var target = ray.GetPoint(300);
        foreach (var hit in hits)
        {
            if (hit.collider.transform.root != transform)
            {
                target = hit.point;
                break;
            }
        }
        Rotate(_turret, new Vector3(target.x, _turret.transform.position.y, target.z));
        Rotate(_turretPivot, new Vector3(target.x, target.y, target.z));
    }

    private void Rotate(Transform _tower, Vector3 target)
    {
        Vector3 dir = target - _tower.transform.position;

        Quaternion rot = Quaternion.LookRotation(dir);
        _tower.transform.rotation = Quaternion.Slerp(_tower.transform.rotation, rot, 8 * Time.deltaTime);
    }
}

public static class Vector3Extend
{
    public static Vector3 GetMiddlePoint(Vector3 a, Vector3 b)
    {
        return a + ((b - a) / 2);
    }
}
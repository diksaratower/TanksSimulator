using System;
using System.Collections;
using UnityEngine;


public class KillCameraProjectile : MonoBehaviour
{
    public Action OnEndSimulation;

    [SerializeField] private SplinterView _splinterViewPrefab;
    [SerializeField] private GameObject _projectileModel;
    [SerializeField] private float _projectileSpeed = 1f;
    [SerializeField] private float _splintersRadius = 10;
    [SerializeField] private int _splintersCount = 70;
    [SerializeField] private LayerMask _layerMask;


    public void StartVisualision(SplintersDamage explosion)
    {
        StartCoroutine(VisualisionCorotinue(explosion));
    }

    private IEnumerator VisualisionCorotinue(SplintersDamage explosion)
    {
        var speed = 2f;
        while (Vector3.Distance(transform.position, explosion.StartPosition) > 0.1f)
        {
            yield return null;
            transform.position = Vector3.Lerp(transform.position, explosion.StartPosition, speed * Time.deltaTime);
        }
        transform.position = explosion.StartPosition;
        yield return new WaitForSeconds(0.1f);
        foreach (var splinter in explosion.Splinters)
        {
            var splinterView = Instantiate(_splinterViewPrefab, transform.position, _splinterViewPrefab.transform.rotation);
            splinterView.transform.SetParent(transform);
            splinterView.Refresh(explosion.StartPosition, splinter.HitPosition);
        }
        yield return new WaitForSeconds(5f);
        OnEndSimulation?.Invoke();
    }

    private void OnTriggerEnter(Collider tank)
    {
        tank.transform.root.GetComponent<Tank>().SetTransparent();
    }
}

using System.Collections;
using UnityEngine;


public class SplinterView : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    public void Refresh(Vector3 start, Vector3 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, start);
        StartCoroutine(MoveSplinter(start, end));
    }

    private IEnumerator MoveSplinter(Vector3 start, Vector3 end)
    {
        var speed = 0.0001f;
        var updatesCount = 30f;
        for (int i = 0; i < updatesCount; i++)
        {
            yield return new WaitForSeconds(speed);
            var position = Vector3.Lerp(start, end, i / updatesCount);
            _lineRenderer.SetPosition(1, position);
        }
    }
}

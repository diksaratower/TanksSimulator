using UnityEngine;


public class FocusPoint : MonoBehaviour
{
    public float YawLimit { get { return _yawLimit; } }
    public float PitchLimit { get { return _pitchLimit; } }

    [SerializeField] private float _yawLimit = 45f;
    [SerializeField] private float _pitchLimit = 45;
}

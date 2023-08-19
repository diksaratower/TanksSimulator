using System.Collections.Generic;
using UnityEngine;


public class Track : MonoBehaviour
{
    [SerializeField] private List<WheelCollider> _wheelColliders = new List<WheelCollider>();
    [SerializeField] private float _power;
    [SerializeField] private float _maxSpeedRpm = 100f;

    private float _trackPositionCooficent = 0f;

    private void Start()
    {
        if (transform.localPosition.x < 0)
        {
            _trackPositionCooficent = 1f;
        }
        if (transform.localPosition.x > 0)
        {
            _trackPositionCooficent = -1f;
        }
    }

    public void Move(float horizontal, float vertical)
    {
        foreach (var wheelCollider in _wheelColliders) 
        {
            if (wheelCollider.rpm < _maxSpeedRpm && wheelCollider.rpm > -(_maxSpeedRpm * 0.7f))
            {
                wheelCollider.brakeTorque = 0f;
                wheelCollider.motorTorque = 0;
                wheelCollider.motorTorque = _power * vertical;
                if (horizontal != 0)
                {
                    wheelCollider.motorTorque += (_power * _trackPositionCooficent * horizontal);
                }
            }
            else
            {
                wheelCollider.brakeTorque = 1000f;
            }
        }
    }
}

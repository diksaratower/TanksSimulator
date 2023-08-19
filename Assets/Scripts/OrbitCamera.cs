using UnityEngine;


public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private FocusPoint _target;
    [SerializeField] private float _distance = 5;

    [SerializeField] private float _damping = 2;

    private Quaternion _pitch;
    private Quaternion _yaw;
    private Quaternion _targetRotation;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _pitch = Quaternion.Euler(this.transform.rotation.eulerAngles.x, 0, 0);
        _yaw = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
    }

    private void Update()
    {
        _targetRotation = _yaw * _pitch;
        _targetPosition = _target.transform.position + _targetRotation * (-Vector3.forward * _distance);

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _targetRotation, Mathf.Clamp01(Time.smoothDeltaTime * _damping));


        Vector3 offset = this.transform.rotation * (-Vector3.forward * _distance);
        this.transform.position = _target.transform.position + offset;
    }

    public void Move(float yawDelta, float pitchDelta)
    {
        _yaw = _yaw * Quaternion.Euler(0, yawDelta, 0);
        _pitch = _pitch * Quaternion.Euler(pitchDelta, 0, 0);
        ApplyConstraints();
    }

    private void ApplyConstraints()
    {
        Quaternion targetYaw = Quaternion.Euler(0, _target.transform.rotation.eulerAngles.y, 0);
        Quaternion targetPitch = Quaternion.Euler(_target.transform.rotation.eulerAngles.x, 0, 0);

        float yawDifference = Quaternion.Angle(_yaw, targetYaw);
        float pitchDifference = Quaternion.Angle(_pitch, targetPitch);

        float yawOverflow = yawDifference - _target.YawLimit;
        float pitchOverflow = pitchDifference - _target.PitchLimit;


        if (yawOverflow > 0) { _yaw = Quaternion.Slerp(_yaw, targetYaw, yawOverflow / yawDifference); }
        if (pitchOverflow > 0) { _pitch = Quaternion.Slerp(_pitch, targetPitch, pitchOverflow / pitchDifference); }
    }
}

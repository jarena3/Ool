using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _originPosition;

    private Quaternion _originRotation;

    public float ShakeDecay;

    public float ShakeIntensity;

    private bool _shaking;

    private void OnEnable()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (!_shaking)

            return;

        if (ShakeIntensity > 0f)
        {
            _transform.localPosition = _originPosition + Random.insideUnitSphere*ShakeIntensity;

            _transform.localRotation = new Quaternion(
                _originRotation.x + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
                _originRotation.y,
                _originRotation.z + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
                _originRotation.w + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f);

            ShakeIntensity -= ShakeDecay;
        }
        else
        {
            _shaking = false;
            _transform.localPosition = _originPosition;
            _transform.localRotation = _originRotation;
        }
    }

    public void Shake(float intensity, float decay)
    {
        if (!_shaking)
        {
            _originPosition = _transform.localPosition;
            _originRotation = _transform.localRotation;
        }

        _shaking = true;

        ShakeIntensity = intensity;

        ShakeDecay = decay;
    }
}

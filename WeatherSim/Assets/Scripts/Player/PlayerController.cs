using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Material weatherMaterial;
    public Material skyboxMaterial;

    [Header("Control Stats")]
    [SerializeField] private float throttleIncrement = 0.1f;
    [SerializeField] private float maxThrust = 200.0f;
    [SerializeField] private float responsiveness = 10.0f;
    [SerializeField] float moveSpeed;
    private Rigidbody _rb;

    private PlayerInput _playerControl;
    private InputAction _rollPitchAction;
    private InputAction _yawAction;
    private InputAction _throttleAction;

    private Vector2 _rollPitch;
    private float _yaw;
    private float _throttle;

    private float _responseModifier
    {
        get 
        {
            return _rb.mass * 0.1f * responsiveness;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _playerControl = GetComponent<PlayerInput>();
        _rollPitchAction = _playerControl.actions["RollPitch"];
        _yawAction = _playerControl.actions["Yaw"];
        _throttleAction = _playerControl.actions["Throttle"];
    }

    // Update is called once per frame
    void Update()
    {
        //_rollPitch = _rollPitchAction.ReadValue<Vector2>();
        CollectInput();
        if (WeatherSystem.Instance != null)
        {
            WeatherSystem.Instance.setPlayerPosition(this.transform.position);
            weatherMaterial.SetFloat("_WeatherType", WeatherSystem.Instance.getCurrentWeatherType());
            weatherMaterial.SetFloat("_WeatherIntensity", WeatherSystem.Instance.getCurrentWeatherIntensity());
            skyboxMaterial.SetFloat("_WeatherType", WeatherSystem.Instance.getCurrentWeatherType());
            skyboxMaterial.SetFloat("_WeatherIntensity", WeatherSystem.Instance.getCurrentWeatherIntensity());
        }
    }

    private void FixedUpdate()
    {
        //_rb.velocity = new Vector3(_rollPitch.x * Time.fixedDeltaTime, _rollPitch.y * Time.fixedDeltaTime, _yaw * Time.fixedDeltaTime) * moveSpeed;
        _rb.AddForce(transform.forward * maxThrust * _throttle);
        _rb.AddTorque(transform.up * _yaw * responsiveness);
        _rb.AddTorque(transform.right * _rollPitch.y * responsiveness);
        _rb.AddTorque(transform.forward * _rollPitch.x * responsiveness);

    }

    #region Input Management
    private void CollectInput()
    {
        _rollPitch = _rollPitchAction.ReadValue<Vector2>();
        _yaw = _yawAction.ReadValue<float>();
        _throttle += _throttleAction.ReadValue<float>() * throttleIncrement;
        _throttle = Mathf.Clamp(_throttle, 0.0f, 100.0f);
    }
    #endregion
}

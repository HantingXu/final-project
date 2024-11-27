using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform[] povs;
    [SerializeField] private float speed;
    [SerializeField] private PlayerInput _refPlayerControl;
    private InputAction _switchViewAction;

    private int _index = 1;
    private Vector3 _target;
    // Start is called before the first frame update
    void Start()
    {
        _switchViewAction = _refPlayerControl.actions["SwitchView"];
    }

    // Update is called once per frame
    void Update()
    {
        if (_switchViewAction.IsPressed())
            _index = (_index + 1) % 4;
        _target = povs[_index].position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
        transform.forward = povs[_index].forward;
    }
}

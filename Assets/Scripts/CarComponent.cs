using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class CarComponent : MonoBehaviour
{
    [SerializeField] private List<Wheels> _frontWheels;
    [SerializeField] private List<Wheels> _rearWheels;
    [SerializeField] private float _speed;
    [SerializeField] private Light[] _backLights;

    private List<Wheels> _wheels = new List<Wheels>();
    private Vector2 _direction;
    private float _steer = 40f;

    private void Start()
    {
        _wheels = (List<Wheels>)_frontWheels.Concat(_rearWheels).ToList<Wheels>();
    }

    public void FixedUpdate()
    {
        foreach (var wheel in _frontWheels)
        {
            if (_direction.y == 0) wheel.Collider.brakeTorque = 70f;
            else wheel.Collider.brakeTorque = 0f;

            wheel.Collider.motorTorque = _speed * _direction.y;
            
            SetLights(_direction.y);
        }

        if (_direction.x != 0)
        {
            foreach (var wheel in _frontWheels)
            {
                wheel.Collider.steerAngle = Mathf.Clamp(wheel.Collider.steerAngle += _direction.x, _steer * -1, _steer);
            }
        }
        else
        {
            foreach (var wheel in _frontWheels)
            {
                wheel.Collider.steerAngle = Mathf.Lerp(wheel.Collider.steerAngle, 0f, 0.1f);
            }
        }
            


        foreach (var wheel in _wheels)
        {
            UpdateWheelPosition(wheel);
        }

        if (Vector3.Dot(Vector3.up, transform.up) < 0) RestartLevel();     
    }

    private void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void SetLights(float y)
    {
        foreach (var light in _backLights)
        {
            light.intensity = y < 0 ? 8 : 4;
        }
    }

    private void UpdateWheelPosition(Wheels wheel)
    {
        Vector3 position;
        Quaternion rotation;
        wheel.Collider.GetWorldPose(out position, out rotation);

        wheel.Mesh.transform.position = position;
        wheel.Mesh.transform.rotation = rotation;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    [Serializable]
    public class Wheels
    {
        [SerializeField] private WheelCollider _collider;
        [SerializeField] private GameObject _mesh;

        public WheelCollider Collider => _collider;
        public GameObject Mesh => _mesh;
    }
}



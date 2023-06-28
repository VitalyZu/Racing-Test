using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _lookAt;
    [SerializeField] private float _speed = 3f;

    void Start()
    {
        transform.position = _target.position;
    }

    void FixedUpdate()
    {
        Vector3 pos = _target.position;
        transform.position = Vector3.Lerp(transform.position, pos, _speed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _lookAt.transform.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}

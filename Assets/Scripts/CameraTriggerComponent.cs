using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerComponent : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Camera _camera;

    private Vector3 _startPosition;

    Coroutine _routine;

    private void Start()
    {
        transform.position = _startPoint.position;
        _startPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _routine = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        _startPoint.position = Vector3.Lerp(_startPoint.position, _targetPoint.position, 10 * Time.fixedDeltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _routine = null;
        }
        _routine = StartCoroutine(MoveBack());
    }

    private IEnumerator MoveBack()
    {
        while (_startPoint.localPosition != _startPosition)
        {
            _startPoint.localPosition = Vector3.Lerp(_startPoint.localPosition, _startPosition, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

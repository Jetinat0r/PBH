using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    private List<ForceAcceptor> forceTargets = new List<ForceAcceptor>();
    [SerializeField]
    private float force = 10f;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.TryGetComponent(out ForceAcceptor _forceAcceptor))
        {
            if (!forceTargets.Contains(_forceAcceptor))
            {
                forceTargets.Add(_forceAcceptor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.TryGetComponent(out ForceAcceptor _forceAcceptor))
        {
            if (forceTargets.Contains(_forceAcceptor))
            {
                forceTargets.Remove(_forceAcceptor);
            }
        }
    }

    private void OnDisable()
    {
        forceTargets.Clear();
    }

    private void FixedUpdate()
    {
        foreach (ForceAcceptor _forceAcceptor in forceTargets)
        {
            _forceAcceptor.AddForce(transform.up * force);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAcceptor : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    public float forceDecayMultiplier = 0.65f;
    [SerializeField]
    public float forceMagnitudeCutoff = 0.2f; //At what appliedVelocity.magnitude to simply cut off force application
    private Vector2 appliedVelocity = Vector2.zero;

    private void FixedUpdate()
    {
        //If force is too small, don't bother
        //Also saves us from div by 0 errors w/ normalization
        if(appliedVelocity.magnitude < forceMagnitudeCutoff)
        {
            appliedVelocity = Vector2.zero;
            return;
        }

        //Debug.Log($"B: {rb.velocity}");
        rb.velocity += Time.fixedDeltaTime * appliedVelocity;
        //Debug.Log($"A: {rb.velocity}");
        float _endForce = appliedVelocity.magnitude * forceDecayMultiplier;
        appliedVelocity = _endForce * appliedVelocity.normalized;
    }

    public void AddForce(Vector2 _newForce)
    {
        appliedVelocity += _newForce;
    }
}

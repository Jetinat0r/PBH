using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb;
    [SerializeField]
    public float moveSpeed = 20f;
    [SerializeField]
    public float burstForce = 5000f;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.TryGetComponent(out ForceAcceptor _forceAcceptor))
        {
            _forceAcceptor.AddForce(transform.up * burstForce);
            Destroy(gameObject);
        }


        if(_collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }


        //Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * moveSpeed * Time.fixedDeltaTime;
    }
}

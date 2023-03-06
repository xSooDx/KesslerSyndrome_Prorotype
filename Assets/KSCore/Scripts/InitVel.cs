using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitVel : MonoBehaviour
{
    [SerializeField] Vector2 startVelocity;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.velocity = startVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, startVelocity);
    }
}

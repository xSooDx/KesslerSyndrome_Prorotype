using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(GravityTarget2D))]
public class Stage : MonoBehaviour
{
    public float startingDeltaV;
    [SerializeField] float deltaV;//{ get; private set; }

    [SerializeField] float acceleration;
    [SerializeField] Transform centerOfThrust;
    public Rigidbody2D rb2d { get; private set; }
    public Collider2D bodyCollider { get; private set; }


    GravityTarget2D gravityTarget2D;
    Rocket attachedRocket;
    bool stageStarted = false;

    public void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        gravityTarget2D = GetComponent<GravityTarget2D>();
    }

    public void OnEnable()
    {
        stageStarted = false;
        deltaV = startingDeltaV;
    }

    public void AttachToRocket(Rocket rocket)
    {
        attachedRocket = rocket;
        rb2d.simulated = false;
        gravityTarget2D.isAffectedByGravity = false;
    }
    public void DetachFromRocket()
    {
        attachedRocket = null;
        gravityTarget2D.isAffectedByGravity = true;
    }

    private void FixedUpdate()
    {
        if (!stageStarted) return;

        StageUpdate();
    }

    public void OnRocketStart()
    {
        rb2d.simulated = true;
    }

    public virtual void StartStage()
    {
        stageStarted = true;
    }

    public virtual void StageUpdate()
    {
        float ddv = Mathf.Min(acceleration * Time.fixedDeltaTime, deltaV);
        deltaV -= ddv;

        Rigidbody2D targetRb = attachedRocket.Rb2D ?? this.rb2d;
        targetRb.velocity += (Vector2) centerOfThrust.up * ddv;

        //float delta = Mathf.Min(force * Time.fixedDeltaTime, totalThrust);
        //totalThrust -= delta;

        //Rigidbody2D targetRb = attachedRocket.Rb2D ?? this.rb2d;
        ////ToDo - Add Rotation here
        //targetRb.AddForceAtPosition((Vector2)centerOfThrust.up * delta, centerOfThrust.position);
    }

    public virtual void EndStage()
    {

    }
}

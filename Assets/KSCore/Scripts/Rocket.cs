using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{

    public Rigidbody2D Rb2D { get { return rb2d; } }
    public float turnStrength = 1f;

    Stage[] stages;
    int currentStageIdx = 0;
    Rigidbody2D rb2d;
    GravityTarget2D gravityTarget2D;
    public float rotationSpeed = 10f;
    private Vector2 centerOfRotation;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.simulated = false;
        gravityTarget2D = GetComponent<GravityTarget2D>();
    }

    void Start()
    {
        LoadStages();
    }

    private void FixedUpdate()
    {
   
    }

    void LateUpdate()
    {
        velocity = rb2d.velocity;
    }

    void LoadStages()
    {
        stages = GetComponentsInChildren<Stage>();
        currentStageIdx = -1;
        gravityTarget2D.isAffectedByGravity = false;
        Vector2 centerOfMass = Vector2.zero;
        foreach (Stage s in stages)
        {
            //Debug.Log($"Rocket {this.name} Found Stage {s.gameObject.name}");
            s.AttachToRocket(this);
            centerOfMass += s.rb2d.centerOfMass;
        }
        centerOfRotation = centerOfMass / stages.Length;
    }

    public void ChangeStage()
    {
        Debug.Log($"ChangeStage {currentStageIdx}, {stages.Length}");
        if (currentStageIdx > stages.Length - 1) return;

        if (currentStageIdx == -1)
        {
            rb2d.simulated = true;
            gravityTarget2D.isAffectedByGravity = true;
            foreach (Stage s in stages)
            {
                s.OnRocketStart();
            }

            currentStageIdx++;
            stages[currentStageIdx].StartStage();
        }
        else if (currentStageIdx == stages.Length - 1)
        {
            StartCoroutine(SpawnSatelliteRoutine());
        }
        else
        {
            Stage currentStage = stages[currentStageIdx];
            currentStage.transform.SetParent(null);
            currentStage.EndStage();

            currentStageIdx++;
            stages[currentStageIdx].StartStage();
        }
    }

    IEnumerator SpawnSatelliteRoutine()
    {
        GameObject obj = InputManager.SpawnObject(transform.position, OrbitalDirection.ClockWise);
        int oldLayer = obj.layer;
        //obj.layer = Shatter.Shatterable2D.noCollisionLayer;
        Destroy(this.gameObject);
        yield return new WaitForSeconds(0.25f);
        //obj.layer = oldLayer;
    }

    public void Steer(float input)
    {
        Debug.Log("Adding Torque " + input);
        transform.RotateAround(transform.position + (Vector3)centerOfRotation, Vector3.forward, -input * rotationSpeed * Time.deltaTime);
    }


#if UNITY_EDITOR
    [SerializeField] Vector3 velocity;

    private void OnDrawGizmos()
    {
        
    }
#endif
}

using UnityEngine;
using Sirenix.OdinInspector;
using System;
using SoodUtils;
using Cinemachine;

public class InputManager : GenericSingletonClass<InputManager>
{
    [AssetsOnly] public GameObject prefab;
    [SceneObjectsOnly] public GravitySource gSource;

    [SerializeField] LayerMask selectableLayerMask;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Select();
            
            SpawnObject(OrbitalDirection.CounterClockWise);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //UnlockCamera();
            SpawnObject(OrbitalDirection.ClockWise);
        }

        //HandleMovementInput();
        //HandleRocketInput();

    }


    private void Select()
    {
        Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, 10000f, selectableLayerMask);
        Debug.DrawRay(r.origin, r.direction, Color.red, 5f);
        if (hit)
        {
            Debug.Log("Input Manager: " + hit);
            //hit.transform.GetComponent<ISelectable>()?.OnSelect();
        }
    }

    // ToDo: Move this
    public GameObject SpawnObject(OrbitalDirection dir)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        Vector3 spawnPos = mainCamera.ScreenToWorldPoint(mousePos);
        return SpawnObject(spawnPos, dir);
    }

    public static GameObject SpawnObject(Vector3 spawnPos, OrbitalDirection dir)
    {
        Rigidbody2D rb2d = Instantiate(Instance.prefab, spawnPos, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb2d.velocity = Instance.gSource.GetOrbitalVel(spawnPos, dir);
        return rb2d.gameObject;   
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RocketLaunchPlatform : MonoBehaviour
{
    public static RocketLaunchPlatform Instance { get; private set; }

    public Rigidbody2D rocketPrefab;
    [SerializeField] Transform rocketSpawn;

    [SerializeField] Rigidbody2D rocketSpawnInstance = null;

    [SerializeField] float launchForce = 10f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(rocketSpawnInstance == null)
            {
                rocketSpawnInstance = Instantiate(rocketPrefab, rocketSpawn.position, Quaternion.identity);
            }
        }
    }

    public void LaunchRocket(Vector3 launchVec)
    {
        if (rocketSpawnInstance == null) return;

        rocketSpawnInstance.AddForce(launchVec * launchForce, ForceMode2D.Impulse);
        rocketSpawnInstance = null;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
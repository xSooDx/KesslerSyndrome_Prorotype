using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryPlotter : MonoBehaviour
{
    // Start is called before the first frame update
    Scene predictionScene;
    PhysicsScene2D physicsPredictionScene2D;

    void Start()
    {
        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("Prediction", sceneParams);
        predictionScene.GetPhysicsScene2D();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

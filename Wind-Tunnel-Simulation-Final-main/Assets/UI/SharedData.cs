using UnityEngine;
using Unity.Mathematics;

public static class SharedData
{
    public static Vector3 windForce = new Vector3(0, 0, 5);
    public static float alpha = 0.5f;
    public static float timeStep = 0.02f;
    public static float forceThreshold = 1.5f;
    public static float massMin = 1f;
    public static float massMax = 1.5f;
    public static float deformationDuration = 5.0f;


    // Simulation settings

    public static bool showLines = false;
    public static float timeScale = 1;
    public static bool fixedTimeStep = true;
    public static int iterationsPerFrame = 1;
    public static float gravity = 0;
    public static float3 windDirection = new float3(1, 0, 0);
    public static float windStrength = 0.01f;
    public static float collisionDamping = 0.99f;
    public static float smoothingRadius = 0.1f;
    public static float targetDensity = 1.225f;
    public static float pressureMultiplier = 20.0f;
    public static float nearPressureMultiplier = 20.0f;
    public static float viscosityStrength = 0.000181f;
}



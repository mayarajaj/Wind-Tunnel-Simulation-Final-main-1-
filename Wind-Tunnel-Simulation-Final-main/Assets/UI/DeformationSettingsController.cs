using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Numerics;
using Unity.Mathematics;
using Vector3 = UnityEngine.Vector3;

public class DeformationSettingsController : MonoBehaviour
{
    private string windForceXInput;
    private string windForceYInput;
    private string windForceZInput;
    private string alphaInput;
    private string timeStepInput;
    private string forceThresholdInput;
    private string massMinInput;
    private string massMaxInput;
    private string deformationDurationInput;

    void Start()
    {
        // Initialize input fields with default values from SharedData
        windForceXInput = SharedData.windForce.x.ToString(); // Change here
        windForceYInput = SharedData.windForce.y.ToString(); // Change here
        windForceZInput = SharedData.windForce.z.ToString(); // Change here
        alphaInput = SharedData.alpha.ToString();
        timeStepInput = SharedData.timeStep.ToString();
        forceThresholdInput = SharedData.forceThreshold.ToString();
        massMinInput = SharedData.massMin.ToString();
        massMaxInput = SharedData.massMax.ToString();
        deformationDurationInput = SharedData.deformationDuration.ToString();
    }

    public void ApplySettings()
    {
        // Set SharedData deformation settings
        SharedData.windForce = new Vector3(
            float.Parse(windForceXInput),
            float.Parse(windForceYInput),
            float.Parse(windForceZInput)
        );
        SharedData.alpha = float.Parse(alphaInput);
        SharedData.timeStep = float.Parse(timeStepInput);
        SharedData.forceThreshold = float.Parse(forceThresholdInput);
        SharedData.massMin = float.Parse(massMinInput);
        SharedData.massMax = float.Parse(massMaxInput);
        SharedData.deformationDuration = float.Parse(deformationDurationInput);
    }

    public void ReadwindForceXInput(string _windForceXInput)
    {
        windForceXInput = _windForceXInput;
        Debug.Log(windForceXInput);
    } 
    public void ReadwindForceYInput(string _windForceYInput)
    {
        windForceYInput = _windForceYInput;
        Debug.Log(windForceYInput);
    } 
    public void ReadwindForceZInput(string _windForceZInput)
    {
        windForceZInput = _windForceZInput;
        Debug.Log(windForceZInput);
    } 
    public void ReadalphaInput(string _alphaInput)
    {
        alphaInput = _alphaInput;
        Debug.Log(alphaInput);
    } 
    public void ReadtimeStepInput(string _timeStepInput)
    {
        timeStepInput = _timeStepInput;
        Debug.Log(timeStepInput);
    } 
    public void ReadforceThresholdInput(string _forceThresholdInput)
    {
        forceThresholdInput = _forceThresholdInput;
        Debug.Log(forceThresholdInput);
    } 
    public void ReadmassMinInput(string _massMinInput)
    {
        massMinInput = _massMinInput;
        Debug.Log(massMinInput);
    } 
    public void ReadmassMaxInput(string _massMaxInput)
    {
        massMaxInput = _massMaxInput;
        Debug.Log(massMaxInput);
    } 
    public void ReaddeformationDurationInput(string _deformationDurationInput)
    {
        deformationDurationInput = _deformationDurationInput;
        Debug.Log(deformationDurationInput);
    } 
}

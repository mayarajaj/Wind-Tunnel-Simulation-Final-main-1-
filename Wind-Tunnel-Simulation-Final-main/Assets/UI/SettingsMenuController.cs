using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    private string timeScaleInput;
    public Toggle fixedTimeStepToggle;
    private string iterationsPerFrameInput;
    private string gravityInput;
    public Slider collisionDampingSlider;
    private string smoothingRadiusInput;
    private string targetDensityInput;
    private string pressureMultiplierInput;
    private string nearPressureMultiplierInput;
    private string viscosityStrengthInput;
  
      void Start()
    {
        // Initialize input fields with default values from SharedData
        timeScaleInput = SharedData.timeScale.ToString();
        fixedTimeStepToggle.isOn = SharedData.fixedTimeStep;
        iterationsPerFrameInput = SharedData.iterationsPerFrame.ToString();
        gravityInput = SharedData.gravity.ToString();
        collisionDampingSlider.value = SharedData.collisionDamping;
        smoothingRadiusInput = SharedData.smoothingRadius.ToString();
        targetDensityInput = SharedData.targetDensity.ToString();
        pressureMultiplierInput = SharedData.pressureMultiplier.ToString();
        nearPressureMultiplierInput = SharedData.nearPressureMultiplier.ToString();
        viscosityStrengthInput = SharedData.viscosityStrength.ToString();
    }
    /*public void ApplySettings()
    {
        SharedData.timeScale = float.Parse(timeScaleInput);
        SharedData.fixedTimeStep = fixedTimeStepToggle.isOn;
        SharedData.iterationsPerFrame = int.Parse(iterationsPerFrameInput);
        SharedData.gravity = float.Parse(gravityInput);
        SharedData.collisionDamping = collisionDampingSlider.value;
        SharedData.smoothingRadius = float.Parse(smoothingRadiusInput);
        SharedData.targetDensity = float.Parse(targetDensityInput);
        SharedData.pressureMultiplier = float.Parse(pressureMultiplierInput);
        SharedData.nearPressureMultiplier = float.Parse(nearPressureMultiplierInput);
        SharedData.viscosityStrength = float.Parse(viscosityStrengthInput);
    }*/
        public void ApplySettings()
    {
        // Use default values if the input fields are empty
        SharedData.timeScale = string.IsNullOrEmpty(timeScaleInput) ? SharedData.timeScale : float.Parse(timeScaleInput);
        SharedData.fixedTimeStep = fixedTimeStepToggle.isOn;
        SharedData.iterationsPerFrame = string.IsNullOrEmpty(iterationsPerFrameInput) ? SharedData.iterationsPerFrame : int.Parse(iterationsPerFrameInput);
        SharedData.gravity = string.IsNullOrEmpty(gravityInput) ? SharedData.gravity : float.Parse(gravityInput);
        SharedData.collisionDamping = collisionDampingSlider.value;
        SharedData.smoothingRadius = string.IsNullOrEmpty(smoothingRadiusInput) ? SharedData.smoothingRadius : float.Parse(smoothingRadiusInput);
        SharedData.targetDensity = string.IsNullOrEmpty(targetDensityInput) ? SharedData.targetDensity : float.Parse(targetDensityInput);
        SharedData.pressureMultiplier = string.IsNullOrEmpty(pressureMultiplierInput) ? SharedData.pressureMultiplier : float.Parse(pressureMultiplierInput);
        SharedData.nearPressureMultiplier = string.IsNullOrEmpty(nearPressureMultiplierInput) ? SharedData.nearPressureMultiplier : float.Parse(nearPressureMultiplierInput);
        SharedData.viscosityStrength = string.IsNullOrEmpty(viscosityStrengthInput) ? SharedData.viscosityStrength : float.Parse(viscosityStrengthInput);
    }

    public void ReadTimeScaleInput(string _timeScale){
        timeScaleInput = _timeScale;
        Debug.Log(timeScaleInput);
    } 

    public void ReadIterationsPerFrameInput(string _iterationsPerFrame){
        iterationsPerFrameInput = _iterationsPerFrame;
        Debug.Log(iterationsPerFrameInput);
    }

    public void ReadGravityInput(string _gravity){
        gravityInput = _gravity;
        Debug.Log(gravityInput);
    }

    public void ReadSmoothingRadiusInput(string _smoothingRadius){
        smoothingRadiusInput = _smoothingRadius;
        Debug.Log(smoothingRadiusInput);
    }

    public void ReadTargetDensityInput(string _targetDensity){
        targetDensityInput = _targetDensity;
        Debug.Log(targetDensityInput);
    }

    public void ReadPressureMultiplierInput(string _pressureMultiplier){
        pressureMultiplierInput = _pressureMultiplier;
        Debug.Log(pressureMultiplierInput);
    }

    public void ReadNearPressureMultiplierInput(string _nearPressureMultiplier){
        nearPressureMultiplierInput = _nearPressureMultiplier;
        Debug.Log(nearPressureMultiplierInput);
    }

    public void ReadViscosityStrengthInput(string _viscosityStrength){
        viscosityStrengthInput = _viscosityStrength;
        Debug.Log(viscosityStrengthInput);
    }
}

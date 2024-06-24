using UnityEngine;
using UnityEngine.SceneManagement;

public class ModelSelectionController : MonoBehaviour
{
    public void LoadModelScene(string modelName)
    {
        // Assuming each button calls this method with the model name as a parameter
        SceneManager.LoadScene(modelName);
    }
}

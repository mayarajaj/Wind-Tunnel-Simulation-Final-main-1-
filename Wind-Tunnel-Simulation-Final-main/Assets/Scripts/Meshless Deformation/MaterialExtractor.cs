using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MaterialExtractor : MonoBehaviour
{
    public string savePath = "Assets/ExtractedMaterials";

    void Start()
    {
        ExtractMaterials();
    }

    void ExtractMaterials()
    {
        // Get all MeshRenderer components in the model
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        List<Material> materials = new List<Material>();

        // Collect all unique materials
        foreach (MeshRenderer renderer in meshRenderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (!materials.Contains(mat))
                {
                    materials.Add(mat);
                }
            }
        }

        // Save materials as assets
        SaveMaterials(materials);
    }

    void SaveMaterials(List<Material> materials)
    {
#if UNITY_EDITOR
        // Ensure the save path exists
        if (!AssetDatabase.IsValidFolder(savePath))
        {
            AssetDatabase.CreateFolder("Assets", "ExtractedMaterials");
        }

        // Save each material as a separate asset
        foreach (Material mat in materials)
        {
            string materialPath = $"{savePath}/{mat.name}.mat";
            AssetDatabase.CreateAsset(new Material(mat), materialPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}

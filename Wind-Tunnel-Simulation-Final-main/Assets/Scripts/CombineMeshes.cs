using UnityEngine;

public class CombineMeshes
{
    public static Mesh CombineAllChildMeshes(GameObject model)
    {
        // Get all MeshFilter components in the children of this GameObject
        MeshFilter[] meshFilters = model.GetComponentsInChildren<MeshFilter>();

        // Array to hold the combine instances
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        // Loop through each mesh filter and assign it to the combine array
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;

            // Apply the transformation relative to the parent model
            combine[i].transform = model.transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
        }

        // Create a new mesh and assign the combined mesh to it
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine);

        return combinedMesh;
    }
}

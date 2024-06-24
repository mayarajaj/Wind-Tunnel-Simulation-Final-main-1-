using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshCombinerWithAtlas : MonoBehaviour
{
    public string meshName = "CombinedMesh";
    public string savePath = "Assets/GeneratedMeshes";
    public int atlasSize = 1024;

    void Start()
    {
        CombineMeshesWithAtlas();
    }

    void CombineMeshesWithAtlas()
    {
        // Get all MeshFilter and MeshRenderer components in child objects
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        if (meshFilters.Length == 0 || meshRenderers.Length == 0)
        {
            Debug.LogWarning("No meshes or renderers found to combine.");
            return;
        }

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        List<Material> materials = new List<Material>();
        List<Texture2D> textures = new List<Texture2D>();
        Vector2[][] oldUVs = new Vector2[meshFilters.Length][];

        // Collect all unique materials and their textures
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            oldUVs[i] = meshFilters[i].sharedMesh.uv;

            MeshRenderer renderer = meshRenderers[i];
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (!materials.Contains(mat))
                {
                    materials.Add(mat);
                    textures.Add((Texture2D)mat.mainTexture);
                }
            }

            meshFilters[i].gameObject.SetActive(false); // Disable the individual mesh
        }

        // Create a new MeshFilter and MeshRenderer for the combined mesh
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh.CombineMeshes(combine);

        // Create the texture atlas
        TextureAtlasGenerator atlasGenerator = new TextureAtlasGenerator { textures = textures.ToArray(), atlasSize = atlasSize };
        Texture2D atlas = atlasGenerator.CreateTextureAtlas();
        Rect[] rects = atlas.PackTextures(textures.ToArray(), 0, atlasSize);

        // Remap UV coordinates to the atlas
        Vector2[] newUVs = new Vector2[meshFilter.mesh.vertexCount];
        int vertexIndex = 0;
        for (int i = 0; i < combine.Length; i++)
        {
            MeshRenderer renderer = meshRenderers[i];
            Material mat = renderer.sharedMaterial;
            int matIndex = materials.IndexOf(mat);

            if (matIndex < 0 || matIndex >= rects.Length)
            {
                Debug.LogError($"Material index {matIndex} out of range for material {mat.name}");
                continue;
            }

            Rect rect = rects[matIndex];
            for (int j = 0; j < oldUVs[i].Length; j++)
            {
                newUVs[vertexIndex + j] = new Vector2(
                    Mathf.Lerp(rect.xMin, rect.xMax, oldUVs[i][j].x),
                    Mathf.Lerp(rect.yMin, rect.yMax, oldUVs[i][j].y));
            }
            vertexIndex += oldUVs[i].Length;
        }
        meshFilter.mesh.uv = newUVs;

        // Create a new material with the texture atlas
        Material combinedMaterial = new Material(Shader.Find("Standard"));
        combinedMaterial.mainTexture = atlas;

        // Add a MeshRenderer to render the combined mesh
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = combinedMaterial;

        // Save the combined mesh and atlas
        SaveMesh(meshFilter.mesh, meshName, savePath);
        SaveTexture(atlas, "CombinedTextureAtlas", savePath);

        gameObject.SetActive(true);
    }

    void SaveMesh(Mesh mesh, string name, string path)
    {
#if UNITY_EDITOR
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets", "GeneratedMeshes");
        }

        AssetDatabase.CreateAsset(mesh, $"{path}/{name}.asset");
        AssetDatabase.SaveAssets();
#endif
    }

    void SaveTexture(Texture2D texture, string name, string path)
    {
#if UNITY_EDITOR
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets", "GeneratedTextures");
        }

        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes($"{path}/{name}.png", bytes);
        AssetDatabase.ImportAsset($"{path}/{name}.png");
#endif
    }
}

public class TextureAtlasGenerator
{
    public Texture2D[] textures;
    public int atlasSize;

    public Texture2D CreateTextureAtlas()
    {
        Texture2D atlas = new Texture2D(atlasSize, atlasSize);
        atlas.PackTextures(textures, 0, atlasSize);
        return atlas;
    }
}

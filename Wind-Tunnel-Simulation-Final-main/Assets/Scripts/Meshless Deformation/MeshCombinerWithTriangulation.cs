/*using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MIConvexHull;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshCombinerWithTriangulation : MonoBehaviour
{
    public string meshName = "CombinedMesh";
    public string savePath = "Assets/GeneratedMeshes";
    public int atlasSize = 1024;
    public float simplificationFactor = 0.5f;

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

        List<CombineInstance> combine = new List<CombineInstance>();
        List<Material> materials = new List<Material>();
        List<Texture2D> textures = new List<Texture2D>();

        // Extract and simplify meshes
        foreach (var meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                continue;
            }

            Vector3[] vertices = mesh.vertices;
            Vector3[] simplifiedVertices = DelaunayTriangulationUtility.GetVerticesAfterTriangulate(
                meshFilter.gameObject,
                meshFilter.transform.position,
                meshFilter.transform.lossyScale,
                simplificationFactor,
                meshFilter.transform.rotation
            );

            Mesh simplifiedMesh = new Mesh();
            simplifiedMesh.vertices = simplifiedVertices;
            simplifiedMesh.triangles = Enumerable.Range(0, simplifiedVertices.Length).ToArray(); // Assuming the vertices form valid triangles

            CombineInstance instance = new CombineInstance
            {
                mesh = simplifiedMesh,
                transform = meshFilter.transform.localToWorldMatrix
            };
            combine.Add(instance);

            MeshRenderer renderer = meshFilter.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.sharedMaterials)
                {
                    if (!materials.Contains(mat))
                    {
                        materials.Add(mat);
                        textures.Add((Texture2D)mat.mainTexture);
                    }
                }
            }
        }

        // Create a new MeshFilter and MeshRenderer for the combined mesh
        MeshFilter combinedMeshFilter = gameObject.AddComponent<MeshFilter>();
        combinedMeshFilter.mesh = new Mesh();
        combinedMeshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Ensure UInt32 index format
        combinedMeshFilter.mesh.CombineMeshes(combine.ToArray(), true, true);

        // Create the texture atlas
        TextureAtlasGenerator atlasGenerator = new TextureAtlasGenerator { textures = textures.ToArray(), atlasSize = atlasSize };
        Texture2D atlas = atlasGenerator.CreateTextureAtlas();
        Rect[] rects = atlas.PackTextures(textures.ToArray(), 0, atlasSize);

        // Remap UV coordinates to the atlas
        Vector2[] newUVs = new Vector2[combinedMeshFilter.mesh.vertexCount];
        Vector2[] oldUVs = combinedMeshFilter.mesh.uv;
        for (int i = 0; i < oldUVs.Length; i++)
        {
            int matIndex = i / 3; // Assuming each set of 3 vertices corresponds to a material
            if (matIndex >= rects.Length)
            {
                Debug.LogError("Material index out of range.");
                continue;
            }
            Rect rect = rects[matIndex];
            newUVs[i] = new Vector2(
                Mathf.Lerp(rect.xMin, rect.xMax, oldUVs[i].x),
                Mathf.Lerp(rect.yMin, rect.yMax, oldUVs[i].y)
            );
        }
        combinedMeshFilter.mesh.uv = newUVs;

        // Create a new material with the texture atlas
        Material combinedMaterial = new Material(Shader.Find("Standard"));
        combinedMaterial.mainTexture = atlas;

        // Add a MeshRenderer to render the combined mesh
        MeshRenderer combinedMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        combinedMeshRenderer.sharedMaterial = combinedMaterial;

        // Save the combined mesh and atlas
        SaveMesh(combinedMeshFilter.mesh, meshName, savePath);
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
}*/

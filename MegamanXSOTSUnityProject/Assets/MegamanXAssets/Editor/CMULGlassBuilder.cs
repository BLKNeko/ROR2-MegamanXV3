using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Separa o vidro da Ultimate Armor CM em uma malha própria.
// Menu: MegamanX > Model > Build Ultimate CM Glass
//
// Motivo: o CharacterModel do RoR2 aplica UM material por renderer. O XCMULBodyMesh tem 2 submeshes
// (corpo + vidro), então a segunda submesh fica sem material e não aparece no jogo.
// Esta ferramenta gera:
//   Mesh/XCMULGlassMesh.asset -> só a submesh do vidro (mesmos ossos/bindposes do corpo)
//   Materials/matCMULGlass.mat -> shader MegamanX/GlassFresnel
// No jogo, o XArmorVisualsComponent cria um segundo SkinnedMeshRenderer com essa malha,
// usando os mesmos ossos do XBodyMesh, e o liga só enquanto o X está com a Ultimate Armor CM.
public static class CMULGlassBuilder
{
    private const string BundleRoot = "Assets/MegamanXAssets/MegamanXBundle";
    private const string SourceMeshPath = BundleRoot + "/Mesh/XCMULBodyMesh.mesh";
    private const string GlassMeshPath = BundleRoot + "/Mesh/XCMULGlassMesh.asset";
    private const string GlassMatPath = BundleRoot + "/Materials/matCMULGlass.mat";
    private const int GlassSubmesh = 1;

    [MenuItem("MegamanX/Model/Build Ultimate CM Glass")]
    [MenuItem("Tools/MegamanX/Build Ultimate CM Glass")]
    [MenuItem("Assets/MegamanX/Build Ultimate CM Glass")]
    public static void Build()
    {
        Mesh source = AssetDatabase.LoadAssetAtPath<Mesh>(SourceMeshPath);
        if (!source)
        {
            EditorUtility.DisplayDialog("Ultimate CM Glass", "Malha não encontrada: " + SourceMeshPath, "OK");
            return;
        }
        if (source.subMeshCount <= GlassSubmesh)
        {
            EditorUtility.DisplayDialog("Ultimate CM Glass",
                source.name + " tem só " + source.subMeshCount + " submesh. Esperava o vidro na submesh " + GlassSubmesh + ".", "OK");
            return;
        }

        Mesh glass = ExtractSubmesh(source, GlassSubmesh);
        glass.name = "XCMULGlassMesh";
        SaveMesh(glass, GlassMeshPath);

        Shader shader = Shader.Find("MegamanX/GlassFresnel");
        if (!shader)
        {
            EditorUtility.DisplayDialog("Ultimate CM Glass", "Shader MegamanX/GlassFresnel não encontrado.", "OK");
            return;
        }

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(GlassMatPath);
        if (!mat)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, GlassMatPath);
        }
        mat.shader = shader; // os valores de cor ficam como você deixar no material
        EditorUtility.SetDirty(mat);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[CMULGlass] {GlassMeshPath} gerado ({glass.vertexCount} vértices, {glass.triangles.Length / 3} triângulos).");
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Mesh>(GlassMeshPath);
    }

    // Copia só os vértices usados pela submesh, mantendo pesos de osso e bindposes (para skinning).
    private static Mesh ExtractSubmesh(Mesh source, int submesh)
    {
        int[] tris = source.GetTriangles(submesh);

        Vector3[] vertices = source.vertices;
        Vector3[] normals = source.normals;
        Vector4[] tangents = source.tangents;
        Vector2[] uv = source.uv;
        Vector2[] uv2 = source.uv2;
        Color[] colors = source.colors;
        BoneWeight[] weights = source.boneWeights;

        var remap = new Dictionary<int, int>();
        var order = new List<int>();
        var newTris = new int[tris.Length];
        for (int i = 0; i < tris.Length; i++)
        {
            if (!remap.TryGetValue(tris[i], out int idx))
            {
                idx = order.Count;
                remap[tris[i]] = idx;
                order.Add(tris[i]);
            }
            newTris[i] = idx;
        }

        T[] Pick<T>(T[] src) where T : struct
        {
            if (src == null || src.Length != vertices.Length) return null;
            var dst = new T[order.Count];
            for (int i = 0; i < order.Count; i++) dst[i] = src[order[i]];
            return dst;
        }

        var mesh = new Mesh();
        mesh.vertices = Pick(vertices);
        if (Pick(normals) is Vector3[] n) mesh.normals = n;
        if (Pick(tangents) is Vector4[] t) mesh.tangents = t;
        if (Pick(uv) is Vector2[] u) mesh.uv = u;
        if (Pick(uv2) is Vector2[] u2) mesh.uv2 = u2;
        if (Pick(colors) is Color[] c) mesh.colors = c;
        if (Pick(weights) is BoneWeight[] w) mesh.boneWeights = w;
        mesh.bindposes = source.bindposes;
        mesh.triangles = newTris;
        mesh.RecalculateBounds();
        return mesh;
    }

    private static void SaveMesh(Mesh mesh, string path)
    {
        Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existing)
        {
            EditorUtility.CopySerialized(mesh, existing);
            existing.name = mesh.name;
            EditorUtility.SetDirty(existing);
            Object.DestroyImmediate(mesh);
            return;
        }
        AssetDatabase.CreateAsset(mesh, path);
    }
}

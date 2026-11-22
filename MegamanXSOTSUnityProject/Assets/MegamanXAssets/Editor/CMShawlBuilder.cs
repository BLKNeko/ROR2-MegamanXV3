using UnityEditor;
using UnityEngine;

// Prepara a echarpe da skin Command Mission.
// Menu: MegamanX > Model > Build CM Shawl
//
// Usa a malha X_CM_Shawl (cópia da ShawlMesh_e do X_CM_Test2.fbx). A ordem dos ossos dela é a mesma do
// XBodyMesh do mdlX4, então o mod usa a malha direto com os ossos do corpo.
// Esta ferramenta:
//   - liga o Read/Write da malha (a física de tecido, Cloth, precisa ler os vértices no jogo)
//   - cria/atualiza Materials/matCMShawl.mat com o shader MegamanX/TranslucentCloth e a textura ch_001_001_st
//
// Não precisa adicionar nada no mdlX4: o mod cria o renderer da echarpe, liga só na skin CM e adiciona o Cloth.
public static class CMShawlBuilder
{
    private const string BundleRoot = "Assets/MegamanXAssets/MegamanXBundle";
    private const string ShawlMeshPath = BundleRoot + "/X_CM_Shawl.mesh";
    private const string ShawlMatPath = BundleRoot + "/Materials/matCMShawl.mat";
    private const string TexturePath = BundleRoot + "/Textures/ch_001_001_st.png";

    [MenuItem("MegamanX/Model/Build CM Shawl")]
    [MenuItem("Tools/MegamanX/Build CM Shawl")]
    [MenuItem("Assets/MegamanX/Build CM Shawl")]
    public static void Build()
    {
        Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(ShawlMeshPath);
        if (!mesh)
        {
            EditorUtility.DisplayDialog("CM Shawl", "Malha não encontrada: " + ShawlMeshPath, "OK");
            return;
        }

        // Read/Write ligado
        SerializedObject serializedMesh = new SerializedObject(mesh);
        SerializedProperty readable = serializedMesh.FindProperty("m_IsReadable");
        if (readable != null && !readable.boolValue)
        {
            readable.boolValue = true;
            serializedMesh.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(mesh);
        }

        Shader shader = Shader.Find("MegamanX/TranslucentCloth");
        if (!shader)
        {
            EditorUtility.DisplayDialog("CM Shawl", "Shader MegamanX/TranslucentCloth não encontrado.", "OK");
            return;
        }

        Material mat = AssetDatabase.LoadAssetAtPath<Material>(ShawlMatPath);
        if (!mat)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, ShawlMatPath);
        }
        mat.shader = shader;
        mat.SetTexture("_MainTex", AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath)); // demais valores: ajuste no material
        EditorUtility.SetDirty(mat);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string report = $"{mesh.name}: {mesh.vertexCount} vértices, {mesh.bindposes.Length} ossos, Read/Write ligado.\nMaterial: {ShawlMatPath}";
        Debug.Log("[CMShawl] " + report);
        EditorUtility.DisplayDialog("CM Shawl", report, "OK");
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// Gera o ghost da onda de corte da Blade Armor (BladeWaveGhost) dentro do MegamanXBundle.
// Menu: MegamanX > VFX > Build Blade Wave Ghost
// Pode rodar quantas vezes quiser: malhas e materiais são atualizados no lugar (GUID mantido),
// e o prefab é sobrescrito. Ajustes finos de cor/tamanho: edite as constantes abaixo e rode de novo,
// ou mexa direto nos materiais (os valores dos materiais são reescritos a cada build).
public static class BladeWaveVFXBuilder
{
    private const string BundleRoot = "Assets/MegamanXAssets/MegamanXBundle";
    private const string VfxDir = BundleRoot + "/VFX/BladeWave";
    private const string PrefabPath = BundleRoot + "/BladeWaveGhost.prefab";

    // Forma da onda (em metros, já na escala do mundo: o hitbox do projétil tem ~6 m de largura)
    private const float ArcRadius = 3.6f;        // raio do arco
    private const float ArcHalfAngle = 75f;      // metade da abertura do arco (graus)
    private const float CrescentThickness = 1.1f;// espessura máxima da meia-lua deitada
    private const float WallHeight = 0.9f;       // meia-altura da "parede" de corte vista por trás
    private const int Segments = 48;

    // Paleta (HDR)
    private static readonly Color Core = new Color(1f, 1f, 1f, 1f);
    private static readonly Color Main = new Color(0.25f, 0.85f, 1f, 1f);
    private static readonly Color Edge = new Color(0.35f, 0.2f, 1f, 1f);
    private static readonly Color Afterimage = new Color(0.55f, 0.4f, 1f, 1f);

    [MenuItem("MegamanX/VFX/Build Blade Wave Ghost")]
    [MenuItem("Tools/MegamanX/Build Blade Wave Ghost")]
    [MenuItem("Assets/MegamanX/Build Blade Wave Ghost")]
    public static void Build()
    {
        Shader waveShader = Shader.Find("MegamanX/AnimeSlashWave");
        Shader sparkShader = Shader.Find("MegamanX/AnimeSpark");
        if (!waveShader || !sparkShader)
        {
            EditorUtility.DisplayDialog("Blade Wave VFX",
                "Shaders MegamanX/AnimeSlashWave ou MegamanX/AnimeSpark não encontrados.\n" +
                "Confira se os .shader em " + VfxDir + " compilaram sem erro.", "OK");
            return;
        }

        // ---------- Malhas ----------
        Mesh crescent = SaveMesh(BuildCrescentMesh(), VfxDir + "/meshBladeWaveCrescent.asset");
        Mesh wall = SaveMesh(BuildWallMesh(), VfxDir + "/meshBladeWaveWall.asset");

        // ---------- Materiais ----------
        Material matCrescent = SaveMaterial(waveShader, VfxDir + "/matBladeWaveCrescent.mat", m =>
        {
            SetWave(m, Core, Main, Edge, 2.2f, 0.35f, 0f);
        });
        Material matWall = SaveMaterial(waveShader, VfxDir + "/matBladeWaveWall.mat", m =>
        {
            SetWave(m, Core, Main, Edge, 2.2f, 0.35f, 1f);
            m.SetFloat("_CoreWidth", 0.14f);
            m.SetFloat("_MainWidth", 0.5f);
        });
        Material matAfterimage = SaveMaterial(waveShader, VfxDir + "/matBladeWaveAfterimage.mat", m =>
        {
            SetWave(m, new Color(0.85f, 0.8f, 1f), Afterimage, Edge * 0.8f, 1.4f, 0.1f, 1f);
            m.SetFloat("_CoreWidth", 0.06f);
            m.SetFloat("_StrandCut", 0.62f);
        });
        Material matSpark = SaveMaterial(sparkShader, VfxDir + "/matBladeWaveSpark.mat", m =>
        {
            m.SetColor("_Color", Main);
            m.SetColor("_CoreColor", Core);
            m.SetFloat("_Intensity", 2.5f);
            m.SetFloat("_CoreSize", 0.45f);
            m.SetFloat("_EdgeSharpness", 0.06f);
            m.SetFloat("_Solidity", 0.2f);
        });

        // ---------- Prefab ----------
        GameObject root = new GameObject("BladeWaveGhost");

        // Meia-lua deitada (lê bem vista de cima) + parede curva (lê bem vista por trás)
        BuildMainMesh(root.transform, "CrescentMain", crescent, matCrescent);
        BuildMainMesh(root.transform, "WallMain", wall, matWall);

        // Imagens residuais que ficam para trás (como as meias-luas extras da referência)
        BuildAfterimages(root.transform, "AfterimageWall", wall, matAfterimage);
        BuildAfterimages(root.transform, "AfterimageCrescent", crescent, matAfterimage);

        BuildSpeedLines(root.transform, crescent, matSpark);
        BuildShards(root.transform, crescent, matSpark);

        Light light = root.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(0.4f, 0.8f, 1f);
        light.range = 9f;
        light.intensity = 2.5f;
        light.shadows = LightShadows.None;

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
        Object.DestroyImmediate(root);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = prefab;
        EditorGUIUtility.PingObject(prefab);
        Debug.Log("[BladeWaveVFX] BladeWaveGhost gerado em " + PrefabPath);
    }

    // ===================== Partículas =====================

    // A lâmina principal é uma partícula de malha (Local), assim dá para fazer o "pop" de entrada
    // e o fade sem script nenhum dentro do bundle.
    private static void BuildMainMesh(Transform parent, string name, Mesh mesh, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, name, mat);

        var main = ps.main;
        main.duration = 1.5f;
        main.loop = false;
        main.startLifetime = 1.5f;
        main.startSpeed = 0f;
        main.startSize = 1f;
        main.maxParticles = 1;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 1) });

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
            new Keyframe(0f, 0.3f), new Keyframe(0.05f, 1.15f), new Keyframe(0.12f, 1f), new Keyframe(1f, 1f)));

        var color = ps.colorOverLifetime;
        color.enabled = true;
        color.color = MakeGradient(Color.white, Color.white, new[] { (0f, 1f), (0.8f, 1f), (1f, 0f) });

        SetMeshRenderer(ps, mesh);
    }

    // Cópias da lâmina soltas no mundo conforme a onda anda (rate over distance).
    private static void BuildAfterimages(Transform parent, string name, Mesh mesh, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, name, mat);

        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = 0.2f;
        main.startSpeed = 0f;
        main.startSize = 0.95f;
        main.maxParticles = 40;
        main.startColor = Afterimage;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 0.6f; // uma cópia a cada ~1,7 m

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0.8f));

        var color = ps.colorOverLifetime;
        color.enabled = true;
        color.color = MakeGradient(Color.white, Afterimage, new[] { (0f, 0.7f), (1f, 0f) });

        SetMeshRenderer(ps, mesh);
    }

    // Linhas de velocidade saindo da borda da lâmina e ficando para trás.
    private static void BuildSpeedLines(Transform parent, Mesh emitFrom, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, "SpeedLines", mat);

        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.1f, 0.18f);
        main.startSpeed = 0f;
        main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.1f);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.white, Main);
        main.maxParticles = 200;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 70f;

        SetBladeVolume(ps.shape, 1.2f, 0.3f);

        var velocity = ps.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        // Unity exige o mesmo modo de curva nos três eixos
        velocity.x = new ParticleSystem.MinMaxCurve(0f, 0f);
        velocity.y = new ParticleSystem.MinMaxCurve(0f, 0f);
        velocity.z = new ParticleSystem.MinMaxCurve(-12f, -6f);

        var color = ps.colorOverLifetime;
        color.enabled = true;
        color.color = MakeGradient(Color.white, Color.white, new[] { (0f, 1f), (1f, 0f) });

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        // Comprimento vem do tamanho (linha fina e longa), não da velocidade
        renderer.velocityScale = 0.03f;
        renderer.lengthScale = 14f;
        renderer.cameraVelocityScale = 0f;
    }

    // Estilhaços de luz em losango espalhados pela lâmina.
    private static void BuildShards(Transform parent, Mesh emitFrom, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, "Shards", mat);

        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.25f, 0.4f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.12f, 0.3f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.white, Main);
        main.maxParticles = 100;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 35f;

        var shape = ps.shape;
        SetBladeVolume(shape, 2.2f, -0.2f);
        shape.randomDirectionAmount = 1f;

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
    }

    // Volume em caixa cobrindo a lâmina (largura do arco, altura da parede).
    // depth = profundidade da caixa, zCenter = posição em Z (o topo do arco fica em +1).
    private static void SetBladeVolume(ParticleSystem.ShapeModule shape, float depth, float zCenter)
    {
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.position = new Vector3(0f, 0f, zCenter);
        shape.rotation = Vector3.zero;
        shape.scale = new Vector3(ArcRadius * 2f * Mathf.Sin(ArcHalfAngle * Mathf.Deg2Rad) * 0.85f, WallHeight * 1.4f, depth);
    }

    private static ParticleSystem NewSystem(Transform parent, string name, Material mat)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.playOnAwake = true;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        main.startColor = Color.white;

        var shape = ps.shape;
        shape.enabled = false;

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = mat;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.lightProbeUsage = LightProbeUsage.Off;
        renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

        return ps;
    }

    private static void SetMeshRenderer(ParticleSystem ps, Mesh mesh)
    {
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Mesh;
        renderer.mesh = mesh;
        renderer.alignment = ParticleSystemRenderSpace.Local;
        renderer.enableGPUInstancing = false;
    }

    private static Gradient MakeGradient(Color from, Color to, (float time, float alpha)[] alphas)
    {
        var g = new Gradient();
        var a = new GradientAlphaKey[alphas.Length];
        for (int i = 0; i < alphas.Length; i++)
            a[i] = new GradientAlphaKey(alphas[i].alpha, alphas[i].time);
        g.SetKeys(new[] { new GradientColorKey(from, 0f), new GradientColorKey(to, 1f) }, a);
        return g;
    }

    // ===================== Malhas =====================

    // Meia-lua deitada no plano XZ, curvada para a frente (+Z).
    // UV.x = ao longo do arco | UV.y: 0 = borda de trás, 1 = borda da frente (o fio).
    private static Mesh BuildCrescentMesh()
    {
        var verts = new Vector3[(Segments + 1) * 2];
        var uvs = new Vector2[verts.Length];
        var normals = new Vector3[verts.Length];
        var tris = new int[Segments * 6];

        float zOffset = ArcRadius - 1f; // topo do arco fica 1 m à frente do centro do projétil

        for (int s = 0; s <= Segments; s++)
        {
            float u = s / (float)Segments;
            float angle = Mathf.Lerp(-ArcHalfAngle, ArcHalfAngle, u) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            Vector3 outer = dir * ArcRadius - new Vector3(0f, 0f, zOffset);

            float thick = CrescentThickness * Mathf.Pow(Mathf.Sin(Mathf.PI * u), 0.7f);
            Vector3 inner = outer - dir * thick;

            verts[s * 2] = inner;
            verts[s * 2 + 1] = outer;
            uvs[s * 2] = new Vector2(u, 0f);
            uvs[s * 2 + 1] = new Vector2(u, 1f);
            normals[s * 2] = normals[s * 2 + 1] = Vector3.up;
        }

        for (int s = 0; s < Segments; s++)
        {
            int i = s * 2, t = s * 6;
            tris[t] = i; tris[t + 1] = i + 1; tris[t + 2] = i + 2;
            tris[t + 3] = i + 1; tris[t + 4] = i + 3; tris[t + 5] = i + 2;
        }

        var mesh = new Mesh { name = "meshBladeWaveCrescent" };
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        return mesh;
    }

    // Parede curva em pé seguindo o mesmo arco (é o que a câmera vê por trás do X).
    // Altura afina nas pontas e o topo/base dobram levemente para trás, como uma lâmina.
    // UV.y: 0 = base, 1 = topo (o shader usa perfil de centro).
    private static Mesh BuildWallMesh()
    {
        const int rows = 5;
        var verts = new Vector3[(Segments + 1) * rows];
        var uvs = new Vector2[verts.Length];
        var normals = new Vector3[verts.Length];
        var tris = new int[Segments * (rows - 1) * 6];

        float zOffset = ArcRadius - 1f;

        for (int s = 0; s <= Segments; s++)
        {
            float u = s / (float)Segments;
            float angle = Mathf.Lerp(-ArcHalfAngle, ArcHalfAngle, u) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            Vector3 center = dir * ArcRadius - new Vector3(0f, 0f, zOffset);
            float height = WallHeight * Mathf.Pow(Mathf.Sin(Mathf.PI * u), 0.8f);

            for (int r = 0; r < rows; r++)
            {
                float v = r / (float)(rows - 1);
                float y = (v * 2f - 1f) * height;
                float bend = Mathf.Abs(v * 2f - 1f);
                Vector3 p = center + Vector3.up * y - dir * (bend * bend * 0.35f);

                int idx = s * rows + r;
                verts[idx] = p;
                uvs[idx] = new Vector2(u, v);
                normals[idx] = dir;
            }
        }

        int t = 0;
        for (int s = 0; s < Segments; s++)
        {
            for (int r = 0; r < rows - 1; r++)
            {
                int a = s * rows + r;
                int b = a + rows;
                tris[t++] = a; tris[t++] = b; tris[t++] = a + 1;
                tris[t++] = a + 1; tris[t++] = b; tris[t++] = b + 1;
            }
        }

        var mesh = new Mesh { name = "meshBladeWaveWall" };
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        return mesh;
    }

    // ===================== Assets =====================

    private static Mesh SaveMesh(Mesh mesh, string path)
    {
        EnsureFolder(VfxDir);
        Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existing)
        {
            EditorUtility.CopySerialized(mesh, existing);
            existing.name = System.IO.Path.GetFileNameWithoutExtension(path);
            EditorUtility.SetDirty(existing);
            Object.DestroyImmediate(mesh);
            return existing;
        }

        mesh.name = System.IO.Path.GetFileNameWithoutExtension(path);
        AssetDatabase.CreateAsset(mesh, path);
        return mesh;
    }

    private static Material SaveMaterial(Shader shader, string path, System.Action<Material> setup)
    {
        EnsureFolder(VfxDir);
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (!mat)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, path);
        }
        mat.shader = shader;
        setup(mat);
        EditorUtility.SetDirty(mat);
        return mat;
    }

    private static void SetWave(Material m, Color core, Color main, Color edge, float intensity, float solidity, float centerProfile)
    {
        m.SetColor("_CoreColor", core);
        m.SetColor("_MainColor", main);
        m.SetColor("_EdgeColor", edge);
        m.SetFloat("_Intensity", intensity);
        m.SetFloat("_Solidity", solidity);
        m.SetFloat("_CenterProfile", centerProfile);
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path));
    }
}

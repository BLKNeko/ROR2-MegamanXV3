using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// Gera o VFX do sabre da Blade Armor (BladeSaberVFX) dentro do MegamanXBundle.
// Menu: MegamanX > VFX > Build Blade Saber VFX
//
// O prefab é montado ao longo de +Z com 1 m de comprimento (cabo em Z = 0, ponta em Z = 1).
// No jogo, o XArmorVisualsComponent posiciona a raiz no cabo, aponta para a ponta e ajusta o
// tamanho das formas (Shape) para o comprimento real da lâmina.
//
// Filhos:
//   EdgeLightning / EdgeSparks -> só durante o Blade Edge (o componente liga e desliga)
//   AuraSparks / AuraSpecter   -> sempre que a Blade Armor está ativa, bem discretos
public static class BladeSaberVFXBuilder
{
    private const string BundleRoot = "Assets/MegamanXAssets/MegamanXBundle";
    private const string VfxDir = BundleRoot + "/VFX/BladeWave";
    private const string PrefabPath = BundleRoot + "/BladeSaberVFX.prefab";

    private static readonly Color Core = Color.white;
    private static readonly Color Main = new Color(0.25f, 0.85f, 1f, 1f);
    private static readonly Color Soft = new Color(0.45f, 0.75f, 1f, 1f);

    [MenuItem("MegamanX/VFX/Build Blade Saber VFX")]
    [MenuItem("Tools/MegamanX/Build Blade Saber VFX")]
    [MenuItem("Assets/MegamanX/Build Blade Saber VFX")]
    public static void Build()
    {
        Shader lightningShader = Shader.Find("MegamanX/AnimeLightning");
        Shader sparkShader = Shader.Find("MegamanX/AnimeSpark");
        if (!lightningShader || !sparkShader)
        {
            EditorUtility.DisplayDialog("Blade Saber VFX",
                "Shaders MegamanX/AnimeLightning ou MegamanX/AnimeSpark não encontrados.\n" +
                "Confira se os .shader em " + VfxDir + " compilaram sem erro.", "OK");
            return;
        }

        Material matLightning = SaveMaterial(lightningShader, VfxDir + "/matSaberLightning.mat", m =>
        {
            m.SetColor("_CoreColor", Core);
            m.SetColor("_GlowColor", Main);
            m.SetFloat("_Intensity", 3f);
            m.SetFloat("_CoreWidth", 0.35f);
            m.SetFloat("_GlowFalloff", 2f);
            m.SetFloat("_Solidity", 0.1f);
        });
        Material matSpark = SaveMaterial(sparkShader, VfxDir + "/matSaberSpark.mat", m =>
        {
            m.SetColor("_Color", Main);
            m.SetColor("_CoreColor", Core);
            m.SetFloat("_Intensity", 2.5f);
            m.SetFloat("_CoreSize", 0.45f);
            m.SetFloat("_EdgeSharpness", 0.06f);
            m.SetFloat("_Solidity", 0.15f);
        });
        Material matSpecter = SaveMaterial(sparkShader, VfxDir + "/matSaberSpecter.mat", m =>
        {
            m.SetColor("_Color", Soft);
            m.SetColor("_CoreColor", new Color(0.8f, 0.9f, 1f));
            m.SetFloat("_Intensity", 1.2f);
            m.SetFloat("_CoreSize", 0.7f);
            m.SetFloat("_EdgeSharpness", 0.35f); // borda bem macia: vira um "fantasma"
            m.SetFloat("_Solidity", 0f);
        });

        GameObject root = new GameObject("BladeSaberVFX");

        BuildEdgeLightning(root.transform, matSpark, matLightning);
        BuildEdgeSparks(root.transform, matSpark);
        BuildAuraSparks(root.transform, matSpark);
        BuildAuraSpecter(root.transform, matSpecter);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
        Object.DestroyImmediate(root);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = prefab;
        EditorGUIUtility.PingObject(prefab);
        Debug.Log("[BladeSaberVFX] BladeSaberVFX gerado em " + PrefabPath);
    }

    // Raios curtos e tortos correndo pela lâmina: partículas invisíveis com Noise forte deixam
    // um Trail fino, e o trail desenha o zigue-zague.
    private static void BuildEdgeLightning(Transform parent, Material particleMat, Material trailMat)
    {
        ParticleSystem ps = NewSystem(parent, "EdgeLightning", particleMat, ParticleSystemSimulationSpace.Local);

        var main = ps.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.08f, 0.16f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.4f, 1.6f);
        main.startSize = 0.01f; // a partícula em si quase não aparece, quem desenha é o trail
        main.startColor = new ParticleSystem.MinMaxGradient(Core, Main);
        main.maxParticles = 80;

        var emission = ps.emission;
        emission.rateOverTime = 45f;

        SetBladeShape(ps.shape, 0.05f);
        var shape = ps.shape;
        shape.randomDirectionAmount = 1f;

        var noise = ps.noise;
        noise.enabled = true;
        noise.strength = new ParticleSystem.MinMaxCurve(2.5f);
        noise.frequency = 6f;
        noise.scrollSpeed = 4f;
        noise.damping = true;
        noise.quality = ParticleSystemNoiseQuality.High;

        var trails = ps.trails;
        trails.enabled = true;
        trails.mode = ParticleSystemTrailMode.PerParticle;
        trails.ratio = 1f;
        trails.lifetime = new ParticleSystem.MinMaxCurve(0.6f);
        trails.minVertexDistance = 0.015f;
        trails.worldSpace = false;
        trails.dieWithParticles = true;
        trails.inheritParticleColor = true;
        trails.textureMode = ParticleSystemTrailTextureMode.Stretch;
        trails.widthOverTrail = new ParticleSystem.MinMaxCurve(0.035f, AnimationCurve.Linear(0f, 1f, 1f, 0.2f));

        var color = ps.colorOverLifetime;
        color.enabled = true;
        color.color = MakeGradient(Color.white, Color.white, new[] { (0f, 1f), (0.7f, 1f), (1f, 0f) });

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.trailMaterial = trailMat;
    }

    // Faíscas que pulam da lâmina durante o Blade Edge.
    private static void BuildEdgeSparks(Transform parent, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, "EdgeSparks", mat, ParticleSystemSimulationSpace.World);

        var main = ps.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.12f, 0.25f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.5f, 4f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.09f);
        main.startColor = new ParticleSystem.MinMaxGradient(Core, Main);
        main.maxParticles = 60;

        var emission = ps.emission;
        emission.rateOverTime = 14f;

        SetBladeShape(ps.shape, 0.05f);
        var shape = ps.shape;
        shape.randomDirectionAmount = 1f;

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        renderer.velocityScale = 0.04f;
        renderer.lengthScale = 2f;
    }

    // Blade Armor: poucas faíscas pequenas, de vez em quando.
    private static void BuildAuraSparks(Transform parent, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, "AuraSparks", mat, ParticleSystemSimulationSpace.World);

        var main = ps.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.15f, 0.3f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.3f, 1f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.03f, 0.06f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.startColor = new ParticleSystem.MinMaxGradient(Core, Main);
        main.maxParticles = 20;

        var emission = ps.emission;
        emission.rateOverTime = 4f;

        SetBladeShape(ps.shape, 0.04f);
        var shape = ps.shape;
        shape.randomDirectionAmount = 1f;

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));
    }

    // Blade Armor: "espectro" fraco que fica para trás só quando o sabre se move (rate over distance).
    private static void BuildAuraSpecter(Transform parent, Material mat)
    {
        ParticleSystem ps = NewSystem(parent, "AuraSpecter", mat, ParticleSystemSimulationSpace.World);

        var main = ps.main;
        main.startLifetime = 0.2f;
        main.startSpeed = 0f;
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.18f);
        main.startColor = new Color(Soft.r, Soft.g, Soft.b, 0.45f);
        main.maxParticles = 60;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 12f;

        SetBladeShape(ps.shape, 0.02f);

        var color = ps.colorOverLifetime;
        color.enabled = true;
        color.color = MakeGradient(Color.white, Color.white, new[] { (0f, 1f), (1f, 0f) });
    }

    // Caixa fina ao longo da lâmina (0..1 em Z). O componente do jogo reescala para o comprimento real.
    private static void SetBladeShape(ParticleSystem.ShapeModule shape, float thickness)
    {
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.position = new Vector3(0f, 0f, 0.5f);
        shape.rotation = Vector3.zero;
        shape.scale = new Vector3(thickness, thickness, 1f);
    }

    private static ParticleSystem NewSystem(Transform parent, string name, Material mat, ParticleSystemSimulationSpace space)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.playOnAwake = true;
        main.simulationSpace = space;
        main.scalingMode = ParticleSystemScalingMode.Shape; // escala da raiz só afeta a forma, não o tamanho das partículas
        // O corpo do X tem Rigidbody (cinemático, velocidade 0). Sem isso o "rate over distance" mede o Rigidbody e nunca emite.
        main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = mat;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.lightProbeUsage = LightProbeUsage.Off;
        renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

        return ps;
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

    private static Material SaveMaterial(Shader shader, string path, System.Action<Material> setup)
    {
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
}

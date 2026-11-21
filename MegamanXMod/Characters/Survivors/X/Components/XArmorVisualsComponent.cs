using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.Components
{
    // Efeitos visuais das armaduras que dependem do estado atual do X. Roda em todos os clientes
    // (só lê buffs e o mesh atual, que já são sincronizados), sem nada de rede.
    //
    //  - Blade Armor: prende o BladeSaberVFX na lâmina do sabre.
    //      AuraSparks / AuraSpecter -> sempre que a Blade Armor está ativa
    //      EdgeLightning / EdgeSparks -> só durante o Blade Edge
    //  - Ultimate Armor CM: liga o renderer do vidro (XCMULGlass) só enquanto o corpo usa o mesh da CM.
    public class XArmorVisualsComponent : MonoBehaviour
    {
        public const string GlassObjectName = "XCMULGlass";

        // Onde a lâmina está desenhada no quad do sabre (Blade1), em coordenadas locais do quad.
        // Tirado do alpha da textura Saber_008_FX_000_1 (lâmina na horizontal, u = 0.078 .. 0.912).
        private static readonly Vector3 bladeEndA = new Vector3(-0.422f, 0f, 0f);
        private static readonly Vector3 bladeEndB = new Vector3(0.412f, 0f, 0f);

        private static readonly string[] edgeOnlyChildren = { "EdgeLightning", "EdgeSparks" };

        // Corrente de raios do Blade Edge: linhas em zigue-zague do cabo à ponta, redesenhadas várias vezes por segundo
        public static int chainCount = 3;
        public static int chainSegments = 14;
        public static float chainAmplitude = 0.07f;
        public static float chainRefreshInterval = 0.045f;
        public static float chainWidth = 0.05f;

        private LineRenderer[] chains;
        private float chainTimer;

        private CharacterBody body;
        private ModelLocator modelLocator;
        private ChildLocator childLocator;

        private SkinnedMeshRenderer bodyRenderer;
        private GameObject glassObject;

        private Transform saber;
        private Transform bladeQuad;
        private GameObject saberVFX;
        private GameObject[] edgeObjects;
        private ParticleSystem[] saberSystems;
        private float currentLength = -1f;

        private void Awake()
        {
            body = GetComponent<CharacterBody>();
            modelLocator = GetComponent<ModelLocator>();
        }

        private bool EnsureModel()
        {
            if (childLocator)
                return true;
            if (!modelLocator || !modelLocator.modelTransform)
                return false;

            Transform model = modelLocator.modelTransform;
            childLocator = model.GetComponent<ChildLocator>();
            if (!childLocator)
                return false;

            Transform bodyMesh = childLocator.FindChild("XBodyMesh");
            bodyRenderer = bodyMesh ? bodyMesh.GetComponent<SkinnedMeshRenderer>() : null;

            Transform glass = bodyMesh ? bodyMesh.parent.Find(GlassObjectName) : null;
            glassObject = glass ? glass.gameObject : null;

            saber = childLocator.FindChild("XBladeSaber");
            bladeQuad = saber ? saber.Find("Blade1") : null;
            return true;
        }

        private void LateUpdate()
        {
            if (!body || !EnsureModel())
                return;

            UpdateGlass();
            UpdateSaber();
        }

        private void UpdateGlass()
        {
            if (!glassObject || !bodyRenderer)
                return;

            bool show = XAssets.UltimateCMBodyMesh && bodyRenderer.sharedMesh == XAssets.UltimateCMBodyMesh && bodyRenderer.enabled;
            if (glassObject.activeSelf != show)
                glassObject.SetActive(show);
        }

        private void UpdateSaber()
        {
            bool bladeArmor = body.HasBuff(XBuffs.BladeArmorBuff) && saber && bladeQuad && saber.gameObject.activeInHierarchy;

            if (!bladeArmor)
            {
                if (saberVFX && saberVFX.activeSelf)
                    saberVFX.SetActive(false);
                return;
            }

            if (!saberVFX)
            {
                if (!XAssets.BladeSaberVFX)
                    return;
                saberVFX = Instantiate(XAssets.BladeSaberVFX, modelLocator.modelTransform);
                saberSystems = saberVFX.GetComponentsInChildren<ParticleSystem>(true);
                edgeObjects = new GameObject[edgeOnlyChildren.Length];
                for (int i = 0; i < edgeOnlyChildren.Length; i++)
                {
                    Transform child = saberVFX.transform.Find(edgeOnlyChildren[i]);
                    edgeObjects[i] = child ? child.gameObject : null;
                }

                // O corpo tem Rigidbody (cinemático, velocidade 0): o "rate over distance" do espectro
                // precisa medir o movimento do transform, senão nunca emite.
                foreach (ParticleSystem ps in saberSystems)
                {
                    var main = ps.main;
                    main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
                }

                CreateChains();
                currentLength = -1f;
            }

            if (!saberVFX.activeSelf)
                saberVFX.SetActive(true);

            bool bladeEdge = body.HasBuff(XBuffs.BladeEdgeBuff);
            foreach (GameObject edge in edgeObjects)
            {
                if (edge && edge.activeSelf != bladeEdge)
                    edge.SetActive(bladeEdge);
            }
            if (chains != null)
            {
                foreach (LineRenderer chain in chains)
                {
                    if (chain && chain.enabled != bladeEdge)
                        chain.enabled = bladeEdge;
                }
            }

            // Cabo = ponta da lâmina mais perto do antebraço
            Vector3 a = bladeQuad.TransformPoint(bladeEndA);
            Vector3 b = bladeQuad.TransformPoint(bladeEndB);
            Vector3 arm = saber.parent ? saber.parent.position : saber.position;
            bool aIsHilt = (a - arm).sqrMagnitude <= (b - arm).sqrMagnitude;
            Vector3 hilt = aIsHilt ? a : b;
            Vector3 tip = aIsHilt ? b : a;

            Vector3 along = tip - hilt;
            float length = along.magnitude;
            if (length < 0.01f)
                return;

            saberVFX.transform.SetPositionAndRotation(hilt, Quaternion.LookRotation(along / length, bladeQuad.up));

            if (Mathf.Abs(length - currentLength) > 0.01f)
            {
                currentLength = length;
                foreach (ParticleSystem ps in saberSystems)
                {
                    var shape = ps.shape;
                    Vector3 scale = shape.scale;
                    shape.scale = new Vector3(scale.x, scale.y, length);
                    shape.position = new Vector3(0f, 0f, length * 0.5f);
                }
                chainTimer = 0f; // redesenha já com o comprimento novo
            }

            if (bladeEdge)
                UpdateChains(length);
        }

        private void CreateChains()
        {
            if (!XAssets.MatSaberLightning)
                return;

            chains = new LineRenderer[chainCount];
            for (int i = 0; i < chainCount; i++)
            {
                GameObject go = new GameObject("EdgeChain" + i);
                go.transform.SetParent(saberVFX.transform, false);

                LineRenderer line = go.AddComponent<LineRenderer>();
                line.useWorldSpace = false;
                line.positionCount = chainSegments;
                line.sharedMaterial = XAssets.MatSaberLightning;
                line.textureMode = LineTextureMode.Stretch;
                line.alignment = LineAlignment.View;
                line.numCornerVertices = 0;
                line.numCapVertices = 0;
                line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                line.receiveShadows = false;
                line.widthMultiplier = chainWidth * (i == 0 ? 1f : 0.6f); // uma principal e outras mais finas
                line.widthCurve = new AnimationCurve(new Keyframe(0f, 0.4f), new Keyframe(0.15f, 1f), new Keyframe(0.85f, 1f), new Keyframe(1f, 0.3f));

                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(new Color(0.6f, 0.9f, 1f), 1f) },
                    new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
                line.colorGradient = gradient;
                line.enabled = false;

                chains[i] = line;
            }
        }

        private void UpdateChains(float length)
        {
            if (chains == null)
                return;

            chainTimer -= Time.deltaTime;
            if (chainTimer > 0f)
                return;
            chainTimer = chainRefreshInterval;

            for (int c = 0; c < chains.Length; c++)
            {
                LineRenderer line = chains[c];
                if (!line)
                    continue;

                float amplitude = chainAmplitude * (c == 0 ? 1f : 1.6f);
                for (int i = 0; i < chainSegments; i++)
                {
                    float t = i / (float)(chainSegments - 1);
                    // pontas presas na lâmina, meio solto
                    float envelope = Mathf.Sin(t * Mathf.PI) * 0.8f + 0.2f;
                    Vector2 jitter = Random.insideUnitCircle * amplitude * envelope;
                    line.SetPosition(i, new Vector3(jitter.x, jitter.y, t * length));
                }

                // pisca: às vezes um dos raios secundários some por um instante
                if (c > 0)
                    line.widthMultiplier = Random.value < 0.25f ? 0f : chainWidth * 0.6f;
            }
        }

        private void OnDestroy()
        {
            if (saberVFX)
                Destroy(saberVFX);
        }
    }
}

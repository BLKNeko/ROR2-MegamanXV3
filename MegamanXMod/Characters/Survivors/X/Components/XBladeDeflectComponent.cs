using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.Components
{
    // Blade Edge (BladeEdgeBuff ativo), só para ataques vindos da frente do X (ver frontHalfAngle):
    //  - Projéteis inimigos que chegam perto do X são rebatidos de volta para quem os lançou.
    //    O projétil passa a ser do X, então causa dano no atirador se o atingir.
    //  - Tiros hitscan (BulletAttack) não são objetos e não podem ser rebatidos: o X apenas bloqueia o tiro
    //    (ver BlockBullet, registrado em XSurvivor.AddHooks).
    public class XBladeDeflectComponent : MonoBehaviour
    {
        public static float deflectRadius = 5f;
        public static float checkInterval = 0.05f;
        public static float minReflectSpeed = 40f;

        // Só protege o que vem da frente do X (para onde ele está mirando).
        // Meio-ângulo: 45 = cone de 90° à frente. Use 60 para um cone de 120°.
        public static float frontHalfAngle = 45f;

        // true se 'point' está dentro do cone à frente do X (ignora a altura)
        public static bool IsInFront(CharacterBody body, Vector3 point)
        {
            if (!body)
                return false;

            Vector3 forward = body.inputBank ? body.inputBank.aimDirection : body.transform.forward;
            Vector3 toPoint = point - body.corePosition;
            forward.y = 0f;
            toPoint.y = 0f;

            if (toPoint.sqrMagnitude < 0.01f || forward.sqrMagnitude < 0.0001f)
                return true; // praticamente em cima do X: conta como frente

            return Vector3.Angle(forward, toPoint) <= frontHalfAngle;
        }

        private CharacterBody body;
        private InputBankTest inputBank;
        private float timer;
        private readonly Collider[] results = new Collider[32];

        private void Awake()
        {
            body = GetComponent<CharacterBody>();
            inputBank = GetComponent<InputBankTest>();
        }

        private void FixedUpdate()
        {
            if (!NetworkServer.active || !body || !body.HasBuff(XBuffs.BladeEdgeBuff))
                return;

            timer -= Time.fixedDeltaTime;
            if (timer > 0f)
                return;
            timer = checkInterval;

            TeamIndex myTeam = body.teamComponent ? body.teamComponent.teamIndex : TeamIndex.Player;
            Vector3 center = body.corePosition;

            int count = Physics.OverlapSphereNonAlloc(center, deflectRadius, results, LayerIndex.projectile.mask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < count; i++)
            {
                Collider col = results[i];
                if (!col)
                    continue;

                ProjectileController projectile = col.GetComponentInParent<ProjectileController>();
                if (!projectile || !projectile.teamFilter || projectile.teamFilter.teamIndex == myTeam)
                    continue;

                if (!IsInFront(body, projectile.transform.position))
                    continue; // vindo pelos lados ou por trás: acerta normalmente

                Reflect(projectile, myTeam);
            }
        }

        private void Reflect(ProjectileController projectile, TeamIndex myTeam)
        {
            // Guarda quem lançou antes de trocar o dono
            GameObject shooter = projectile.owner;
            CharacterBody shooterBody = shooter ? shooter.GetComponent<CharacterBody>() : null;

            Vector3 position = projectile.transform.position;
            Vector3 direction;
            if (shooterBody && shooterBody.healthComponent && shooterBody.healthComponent.alive)
                direction = (shooterBody.corePosition - position).normalized;
            else
                direction = inputBank ? inputBank.aimDirection : transform.forward; // atirador morto: manda para a mira

            projectile.teamFilter.teamIndex = myTeam;
            projectile.owner = gameObject;

            ProjectileDamage projectileDamage = projectile.GetComponent<ProjectileDamage>();
            if (projectileDamage)
                projectileDamage.damage = Mathf.Max(projectileDamage.damage, body.damage * XStaticValues.BladeEdgeReflectDamageCoefficient);

            // Teleguiados passam a perseguir quem os lançou
            ProjectileTargetComponent targetComponent = projectile.GetComponent<ProjectileTargetComponent>();
            if (targetComponent)
                targetComponent.target = shooterBody ? shooterBody.coreTransform : null;

            // ProjectileSimple usa o forward do transform para a velocidade
            projectile.transform.rotation = Util.QuaternionSafeLookRotation(direction);

            Rigidbody rb = projectile.rigidbody ? projectile.rigidbody : projectile.GetComponent<Rigidbody>();
            if (rb)
            {
                float speed = Mathf.Max(rb.velocity.magnitude, minReflectSpeed);
                rb.velocity = direction * speed;
            }

            EffectManager.SimpleEffect(XAssets.swordHitImpactEffect, position, Quaternion.identity, true);
            EffectManager.SimpleSoundEffect(XAssets.swordHitSoundEvent.index, position, true);
        }

        // Hook de BulletAttack: tiro hitscan que acertaria o X com Blade Edge é bloqueado (sem dano).
        public static bool BlockBullet(On.RoR2.BulletAttack.orig_DefaultHitCallbackImplementation orig,
            BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo)
        {
            HurtBox hurtBox = hitInfo.hitHurtBox;
            CharacterBody victim = hurtBox && hurtBox.healthComponent ? hurtBox.healthComponent.body : null;

            if (victim && victim.HasBuff(XBuffs.BladeEdgeBuff) && bulletAttack.owner
                && TeamComponent.GetObjectTeam(bulletAttack.owner) != victim.teamComponent.teamIndex
                && IsInFront(victim, bulletAttack.origin)) // só bloqueia tiros vindos da frente
            {
                EffectManager.SimpleEffect(XAssets.swordHitImpactEffect, hitInfo.point, Quaternion.identity, true);
                EffectManager.SimpleSoundEffect(XAssets.swordHitSoundEvent.index, hitInfo.point, true);
                return true; // o tiro para na espada
            }

            return orig(bulletAttack, ref hitInfo);
        }
    }
}

using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.Components
{
    // Enquanto o X tem o BladeEdgeBuff, reflete projéteis inimigos que chegam perto dele.
    // Roda só no servidor: troca o time e o dono do projétil e o manda na direção da mira.
    // Obs.: tiros hitscan (bullets/lasers) não são objetos e não podem ser refletidos.
    public class XBladeDeflectComponent : MonoBehaviour
    {
        public static float deflectRadius = 5f;
        public static float checkInterval = 0.05f;

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

                Reflect(projectile, myTeam, center);
            }
        }

        private void Reflect(ProjectileController projectile, TeamIndex myTeam, Vector3 center)
        {
            Vector3 direction = inputBank ? inputBank.aimDirection : transform.forward;

            projectile.teamFilter.teamIndex = myTeam;
            projectile.owner = gameObject;

            ProjectileDamage projectileDamage = projectile.GetComponent<ProjectileDamage>();
            if (projectileDamage)
            {
                projectileDamage.damage = Mathf.Max(projectileDamage.damage, body.damage * XStaticValues.BladeEdgeReflectDamageCoefficient);
            }

            // Projéteis teleguiados não devem voltar para o X
            ProjectileTargetComponent targetComponent = projectile.GetComponent<ProjectileTargetComponent>();
            if (targetComponent)
                targetComponent.target = null;

            // ProjectileSimple usa o forward do transform para a velocidade
            projectile.transform.rotation = Util.QuaternionSafeLookRotation(direction);

            Rigidbody rb = projectile.rigidbody ? projectile.rigidbody : projectile.GetComponent<Rigidbody>();
            if (rb)
            {
                float speed = Mathf.Max(rb.velocity.magnitude, 40f);
                rb.velocity = direction * speed;
            }

            EffectManager.SimpleEffect(XAssets.swordHitImpactEffect, projectile.transform.position, Quaternion.identity, true);
            EffectManager.SimpleSoundEffect(XAssets.swordHitSoundEvent.index, projectile.transform.position, true);
        }
    }
}

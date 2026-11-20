using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Funções compartilhadas pelos mísseis da Ultimate Armor CM (primária, secundária e barragem).
    public static class UltimateCMMissileHelper
    {
        // Saída dos mísseis relativa à mira: alterna esquerda/direita, um pouco acima (ombros/costas)
        public static float sideOffset = 0.7f;
        public static float upOffset = 0.8f;
        public static float launchUpAngle = 25f; // lança um pouco para cima antes de curvar para o alvo

        public static HurtBox GetTarget(GameObject owner)
        {
            HuntressTracker tracker = owner.GetComponent<HuntressTracker>();
            return tracker ? tracker.GetTrackingTarget() : null;
        }

        public static void FireMissile(GameObject owner, Ray aimRay, GameObject prefab, float damage, bool crit,
            HurtBox target, int index, DamageTypeCombo? damageTypeOverride = null)
        {
            Vector3 right = Vector3.Cross(Vector3.up, aimRay.direction).normalized;
            float side = (index % 2 == 0) ? 1f : -1f;
            Vector3 position = aimRay.origin + right * sideOffset * side + Vector3.up * upOffset;

            // Abre um pouco para os lados e para cima, o MissileController corrige até o alvo
            Vector3 direction = Quaternion.AngleAxis(-launchUpAngle, right) * aimRay.direction;
            direction = Quaternion.AngleAxis(10f * side, Vector3.up) * direction;

            FireProjectileInfo info = new FireProjectileInfo
            {
                projectilePrefab = prefab,
                position = position,
                rotation = Util.QuaternionSafeLookRotation(direction),
                owner = owner,
                damage = damage,
                force = 200f,
                crit = crit,
                damageColorIndex = DamageColorIndex.Default,
                target = target ? target.gameObject : null,
            };

            if (damageTypeOverride.HasValue)
                info.damageTypeOverride = damageTypeOverride.Value;

            ProjectileManager.instance.FireProjectile(info);
        }
    }
}

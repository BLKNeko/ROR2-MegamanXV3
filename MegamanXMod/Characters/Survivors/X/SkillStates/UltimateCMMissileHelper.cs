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

        public const string MuzzleRight = "CMULMuzzR";
        public const string MuzzleLeft = "CMULMuzzL";

        public static string GetMuzzle(int index)
        {
            return (index % 2 == 0) ? MuzzleRight : MuzzleLeft;
        }

        // Mesma paridade do muzzle: índice par = lançador direito / Shoot1, ímpar = esquerdo / Shoot2
        public static string GetShootAnimation(int index)
        {
            return (index % 2 == 0) ? "ULCM_Shoot1" : "ULCM_Shoot2";
        }

        private static Transform FindMuzzle(GameObject owner, string muzzle)
        {
            ModelLocator modelLocator = owner.GetComponent<ModelLocator>();
            if (!modelLocator || !modelLocator.modelTransform)
                return null;

            ChildLocator childLocator = modelLocator.modelTransform.GetComponent<ChildLocator>();
            return childLocator ? childLocator.FindChild(muzzle) : null;
        }

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

            // Sai dos lançadores da armadura (CMULMuzzR / CMULMuzzL), alternando direita/esquerda.
            // Se o muzzle não existir (ex.: modelo sem os pontos), cai no offset relativo à mira.
            string muzzle = GetMuzzle(index);
            Transform muzzleTransform = FindMuzzle(owner, muzzle);
            Vector3 position = muzzleTransform
                ? muzzleTransform.position
                : aimRay.origin + right * sideOffset * side + Vector3.up * upOffset;

            EffectManager.SimpleMuzzleFlash(XAssets.RocketMuzzleFlash, owner, muzzle, true);

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

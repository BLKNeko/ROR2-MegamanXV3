using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Special da Blade Armor: um corte pesado que solta ondas de corte em linha reta (multihit).
    public class XBladeHeavySlash : BaseMeleeAttack2
    {
        public static int waveCount = 3;
        public static float waveSpreadAngle = 15f; // ângulo entre as ondas (yaw)

        private bool firedWaves;

        public override void OnEnter()
        {
            hitboxGroupName = "ShadowSaberGroup";

            damageType = DamageTypeCombo.GenericSpecial;
            damageCoefficient = XStaticValues.XBladeHeavySlashDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 1500f;
            bonusForce = Vector3.zero;
            baseDuration = 1.1f;

            attackStartPercentTime = 0.35f;
            attackEndPercentTime = 0.55f;
            earlyExitPercentTime = 0.9f;

            hitStopDuration = 0.08f;
            attackRecoil = 1.5f;
            hitHopVelocity = 6f;

            swingSoundString = XStaticValues.X_Rathalos_Swing_SFX;
            hitSoundString = "";
            muzzleString = "SwordMuzzPos";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = EntityStates.Merc.WhirlwindBase.swingEffectPrefab;
            hitEffectPrefab = XAssets.swordHitImpactEffect;

            impactSound = XAssets.swordHitSoundEvent.index;

            firedWaves = false;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            // Reaproveita a animação do corte pesado do Rathalos
            PlayAnimation("FullBody, Override", "GigaBladeStart", "attackSpeed", duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!firedWaves && stopwatch >= duration * attackStartPercentTime)
            {
                firedWaves = true;
                FireWaves();
            }
        }

        private void FireWaves()
        {
            if (!isAuthority)
                return;

            Ray aimRay = GetAimRay();
            Vector3 forward = aimRay.direction;
            bool crit = RollCrit();

            float startAngle = -waveSpreadAngle * (waveCount - 1) * 0.5f;
            for (int i = 0; i < waveCount; i++)
            {
                Vector3 direction = Quaternion.AngleAxis(startAngle + waveSpreadAngle * i, Vector3.up) * forward;

                ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                {
                    projectilePrefab = XAssets.BladeWaveProjectile,
                    position = aimRay.origin,
                    rotation = Util.QuaternionSafeLookRotation(direction),
                    owner = gameObject,
                    damage = damageStat * XStaticValues.XBladeWaveDamageCoefficient,
                    force = 300f,
                    crit = crit,
                    damageColorIndex = DamageColorIndex.Default,
                });
            }
        }

        public override void OnExit()
        {
            PlayAnimation("FullBody, Override", "GigaBladeEnd", "attackSpeed", duration);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

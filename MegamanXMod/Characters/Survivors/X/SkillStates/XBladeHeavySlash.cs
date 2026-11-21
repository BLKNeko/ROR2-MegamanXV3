using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Special da Blade Armor: um corte pesado que solta uma onda de corte gigante em linha reta (multihit).
    public class XBladeHeavySlash : BaseMeleeAttack2
    {

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

        // Chamado pelo base.OnEnter() depois de calcular a duration.
        // Antes do base.OnEnter() a duration ainda é 0 e o RoR2 ignora PlayAnimation com duração zero.
        protected override void PlayAttackAnimation()
        {
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

            ProjectileManager.instance.FireProjectile(new FireProjectileInfo
            {
                projectilePrefab = XAssets.BladeWaveProjectile,
                position = aimRay.origin,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                owner = gameObject,
                damage = damageStat * XStaticValues.XBladeWaveDamageCoefficient,
                force = 300f,
                crit = RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
            });
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

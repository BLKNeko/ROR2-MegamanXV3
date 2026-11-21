using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Special da Blade Armor: um corte pesado que solta uma onda de corte gigante em linha reta (multihit).
    //
    // Sincronia (tudo escala com attack speed):
    //   0%  -> 40%  GigaBladeStart (preparo). O clipe é esticado para terminar exatamente no momento do corte.
    //   40%         corte: hitbox melee + onda + GigaBladeEnd (continuação do golpe)
    //   40% -> 55%  janela do hitbox melee
    // O estado GigaBladeStart/End no Animator usa o parâmetro "Slash.playbackRate" como multiplicador de velocidade,
    // então o hitstop do BaseMeleeAttack2 também congela a animação.
    public class XBladeHeavySlash : BaseMeleeAttack2
    {
        public static float slashPercentTime = 0.4f;   // momento do corte (fim do GigaBladeStart)
        public static float baseEndAnimDuration = 0.35f; // duração do GigaBladeEnd

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

            attackStartPercentTime = slashPercentTime;
            attackEndPercentTime = 0.55f;
            earlyExitPercentTime = 0.9f;

            hitStopDuration = 0.08f;
            attackRecoil = 1.5f;
            hitHopVelocity = 6f;

            swingSoundString = XStaticValues.X_Rathalos_Swing_SFX;
            hitSoundString = "";
            muzzleString = "SwordMuzzRPos";
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
            PlayAnimation("FullBody, Override", "GigaBladeStart", playbackRateParam, duration * slashPercentTime);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!firedWaves && stopwatch >= duration * attackStartPercentTime)
            {
                firedWaves = true;
                PlayAnimation("FullBody, Override", "GigaBladeEnd", playbackRateParam, baseEndAnimDuration / attackSpeedStat);
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
            // Se foi interrompido antes do corte, limpa a pose. Senão o GigaBladeEnd volta sozinho para o BufferEmpty.
            if (!firedWaves)
                PlayCrossfade("FullBody, Override", "BufferEmpty", 0.1f);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

using EntityStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Special da Ultimate Armor CM: barragem rápida de 5 mísseis pequenos + 5 grandes em um único alvo, com ignite.
    public class UltimateCMBarrage : BaseSkillState
    {
        public static int totalMissiles = 10;      // alterna pequeno / grande
        public static float baseFireDuration = 0.7f;
        public static float baseEndDelay = 0.3f;

        private HurtBox target;
        private float fireInterval;
        private float fireTimer;
        private float duration;
        private int missilesFired;
        private bool crit;

        public override void OnEnter()
        {
            base.OnEnter();

            fireInterval = baseFireDuration / attackSpeedStat / totalMissiles;
            duration = (baseFireDuration + baseEndDelay) / attackSpeedStat;
            fireTimer = 0f;
            missilesFired = 0;
            crit = RollCrit();

            characterBody.SetAimTimer(duration + 1f);

            if (isAuthority)
                target = UltimateCMMissileHelper.GetTarget(gameObject);

            AkSoundEngine.PostEvent(XStaticValues.X_Charge_Shot, gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            fireTimer -= Time.fixedDeltaTime;
            if (missilesFired < totalMissiles && fireTimer <= 0f)
            {
                fireTimer = fireInterval;
                FireOne();
            }

            if (fixedAge >= duration && missilesFired >= totalMissiles && isAuthority)
                outer.SetNextStateToMain();
        }

        private void FireOne()
        {
            bool big = missilesFired % 2 == 1;

            AkSoundEngine.PostEvent(XStaticValues.X_Mid_Bullet, gameObject);
            PlayAnimation("Gesture, Override", "XBusterChargeAttack", "ShootGun.playbackRate", fireInterval * 2f);

            if (isAuthority)
            {
                // Se o alvo morreu no meio da barragem, tenta o alvo atual do tracker
                if (!target || !target.healthComponent || !target.healthComponent.alive)
                    target = UltimateCMMissileHelper.GetTarget(gameObject);

                GameObject prefab = big ? XAssets.UltimateCMBigMissileProjectile : XAssets.UltimateCMSmallMissileProjectile;
                float coefficient = big ? XStaticValues.UltimateCMBigMissileDamageCoefficient : XStaticValues.UltimateCMSmallMissileDamageCoefficient;

                UltimateCMMissileHelper.FireMissile(gameObject, GetAimRay(), prefab, damageStat * coefficient, crit, target,
                    missilesFired, DamageTypeCombo.GenericSpecial | DamageType.IgniteOnHit);
            }

            missilesFired++;
        }

        public override void OnExit()
        {
            PlayCrossfade("Gesture, Override", "BufferEmpty", "ShootGun.playbackRate", 0.2f, 0.03f);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}

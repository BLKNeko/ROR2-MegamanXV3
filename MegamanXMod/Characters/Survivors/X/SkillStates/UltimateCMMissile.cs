using EntityStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Primária da Ultimate Armor CM: dois mísseis pequenos e rápidos que seguem o alvo.
    public class UltimateCMMissile : BaseSkillState
    {
        public static float baseDuration = 0.45f;
        public static int missilesPerShot = 2;

        private float duration;
        private float fireInterval;
        private float fireTimer;
        private int missilesFired;
        private HurtBox target;
        private bool crit;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireInterval = duration * 0.4f / missilesPerShot;
            fireTimer = 0f;
            missilesFired = 0;

            characterBody.SetAimTimer(2f);
            crit = RollCrit();

            if (isAuthority)
                target = UltimateCMMissileHelper.GetTarget(gameObject);

            PlayAnimation("Gesture, Override", "ULCM_Shoot1", "ShootGun.playbackRate", duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            fireTimer -= Time.fixedDeltaTime;
            if (missilesFired < missilesPerShot && fireTimer <= 0f)
            {
                fireTimer = fireInterval;
                FireOne();
            }

            if (fixedAge >= duration && isAuthority)
                outer.SetNextStateToMain();
        }

        private void FireOne()
        {
            AkSoundEngine.PostEvent(XStaticValues.X_Mid_Bullet, gameObject);

            if (isAuthority)
            {
                UltimateCMMissileHelper.FireMissile(gameObject, GetAimRay(), XAssets.UltimateCMSmallMissileProjectile,
                    damageStat * XStaticValues.UltimateCMSmallMissileDamageCoefficient, crit, target, missilesFired);
            }

            missilesFired++;
        }

        public override void OnExit()
        {
            PlayCrossfade("Gesture, Override", "BufferEmpty", "ShootGun.playbackRate", duration, 0.03f);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

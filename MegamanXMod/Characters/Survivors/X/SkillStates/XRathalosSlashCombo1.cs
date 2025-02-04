using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XRathalosSlashCombo1 : BaseMeleeAttack2
    {

        private bool rathalosExplosionVFXplayed = false;
        private float rathalosexplosiontimer = 0f;

        public override void OnEnter()
        {
            hitboxGroupName = "ShadowSaberGroup";

            damageType = DamageType.IgniteOnHit;
            damageCoefficient = XStaticValues.XRathalosSlashCombo1DamageCoefficient;
            procCoefficient = 1f;
            pushForce = 800f;
            bonusForce = Vector3.up;
            baseDuration = 1f;            

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.8f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.8f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            swingSoundString = XStaticValues.X_Rathalos_Swing_SFX;
            hitSoundString = "";
            muzzleString = "SwordMuzzPos";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = EntityStates.Merc.WhirlwindBase.swingEffectPrefab;
            hitEffectPrefab = XAssets.swordHitImpactEffect;

            impactSound = XAssets.swordHitSoundEvent.index;

            XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            SetNextEntityState(xRathalosSlashCombo2);

            SetHitReset(true, 3);

            rathalosExplosionVFXplayed = false;
            rathalosexplosiontimer = 0f;

            EffectManager.SimpleMuzzleFlash(XAssets.RathalosFlamesVFX, base.gameObject, "SwordBasePos", true);

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "XRSlash1", "attackSpeed", this.duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //characterMotor.velocity *= 1.5f;

            if (!rathalosExplosionVFXplayed)
                rathalosexplosiontimer += Time.fixedDeltaTime;

            if (rathalosexplosiontimer > duration * 0.75f && !rathalosExplosionVFXplayed)
            {
                rathalosExplosionVFXplayed = true;
                rathalosexplosiontimer = 0f;

                if (isAuthority)
                {
                    EffectManager.SimpleMuzzleFlash(XAssets.RathalosExplosionVFX, gameObject, "MeltCreeperFrontPos", true);
                    AkSoundEngine.PostEvent(XStaticValues.X_RathalosBusterCharge_SFX, this.gameObject);
                }
                    

            }

        }

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            rathalosExplosionVFXplayed = false;

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
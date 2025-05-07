using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class RisingFireCharge : BaseMeleeAttack2
    {

        private float upwardForce = 2000f;
        private float yVelocityCurve = 20f;
        private float moveSpeedBonusCoefficient = 4f;

        public override void OnEnter()
        {
            hitboxGroupName = "ShadowSaberGroup";

            damageType |= DamageType.IgniteOnHit;
            damageType |= DamageTypeCombo.GenericSpecial;
            damageCoefficient = (XStaticValues.RisingFireDamageCoefficient * XStaticValues.XFullChargeDamageCoefficient);
            procCoefficient = 1f;
            pushForce = 1000f;
            bonusForce = Vector3.up * upwardForce;
            baseDuration = 1.1f;            

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.01f;
            attackEndPercentTime = 0.99f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.99f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            hitSoundString = "";
            muzzleString = "RisingFirePos";
            playbackRateParam = "Slash.playbackRate";
            //swingEffectPrefab = XAssets.RisingFireVFX;
            //hitEffectPrefab = XAssets.swordHitImpactEffect;

            //impactSound = XAssets.swordHitSoundEvent.index;

            XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            SetHitReset(true, 8);

            EffectManager.SimpleMuzzleFlash(XAssets.RisingFireVFX, gameObject, muzzleString, true);

            if (XConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(XStaticValues.X_Attack_VSFX, this.gameObject);
            }
            AkSoundEngine.PostEvent(XStaticValues.X_RisingFireCharged_SFX, this.gameObject);



            base.OnEnter();

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, duration * 3);
            }

        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "RisingFireStart", "attackSpeed", this.duration);
        }

        protected override void PlaySwingEffect()
        {
            //base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //characterMotor.velocity *= 1.5f;
            if (base.isAuthority)
            {

                base.characterMotor.Motor.ForceUnground(0.1f);


                if (!inHitPause)
                {
                    if (base.characterMotor && base.characterDirection)
                    {
                        Vector3 velocity = base.characterDirection.forward * this.moveSpeedStat * Mathf.Lerp(moveSpeedBonusCoefficient, 0f, base.age / this.duration);
                        velocity.y = Mathf.Lerp(yVelocityCurve, yVelocityCurve/2, fixedAge / duration);
                        base.characterMotor.velocity = velocity;
                    }
                }
                else
                {
                    base.characterMotor.velocity = Vector3.zero;
                }

            }
        }


 

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "RisingFireEnd", "attackSpeed", this.duration);
            //base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
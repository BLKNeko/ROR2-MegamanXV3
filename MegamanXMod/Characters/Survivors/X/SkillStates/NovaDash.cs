using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class NovaDash : BaseMeleeAttack2
    {

        public static float initialSpeedCoefficient = 7f;
        public static float finalSpeedCoefficient = 6f;
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        private ChildLocator childLocator;

        public override void OnEnter()
        {
            hitboxGroupName = "BodyDashHitbox";

            //his.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();


            EffectManager.SimpleMuzzleFlash(XAssets.NovaStrikeVFX, base.gameObject, "NovaDashPos", true);

            //EffectManager.SpawnEffect(XAssets.NovaStrikeVFX, new EffectData
            //{
            //    origin = childLocator.FindChild("NovaDashPos").transform.position,
            //    scale = 8f,
            //    rootObject = characterBody.transform.gameObject,
            //    //rotation = Quaternion.Euler(0, 0, 180),


            //}, true);

            damageType = DamageType.Generic;
            damageCoefficient = XStaticValues.NovaDashDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.8f;            

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.1f;
            attackEndPercentTime = 0.9f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 1f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            hitSoundString = "";

            playbackRateParam = "attackSpeed";

            impactSound = XAssets.swordHitSoundEvent.index;

            AkSoundEngine.PostEvent(XStaticValues.X_NovaStrike_SFX, this.gameObject);

            //XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            //SetNextEntityState(xRathalosSlashCombo2);

            SetHitReset(true, 3);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction.normalized;
            }

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * initialSpeedCoefficient;
            }

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "NovaDashStart", "attackSpeed", this.duration);
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

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (characterDirection) characterDirection.forward = forwardDirection;

            if (cameraTargetParams)
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);


            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
            }

        }

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "NovaDashEnd", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
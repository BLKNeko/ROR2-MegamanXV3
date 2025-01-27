using EntityStates;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class FalconDash : BaseSkillState
    {
        public static float duration = 0.5f;
        public static float initialSpeedCoefficient = 6f;
        public static float finalSpeedCoefficient = 5f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";
        private string FWingR1 = "FWingR1";
        private string FWingR2 = "FWingR2";
        private string FWingR3 = "FWingR3";
        private string FWingL1 = "FWingL1";
        private string FWingL2 = "FWingL2";
        private string FWingL3 = "FWingL3";

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        public static float hoverVelocity = -3f;
        public static float hoverAcceleration = 0.5f;

        private XHoverComponent hoverComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            hoverComponent = GetComponent<XHoverComponent>();

            hoverComponent.SetHover(true);

            //hoverComponent.SetChildLocator(base.GetModelTransform().GetComponent<ChildLocator>());

            //if (isAuthority && inputBank && characterDirection)
            //{
            //    if (inputBank.moveVector != Vector3.zero)
            //    {
            //        forwardDirection = aimRay.direction;
            //    }
            //    else
            //    {
            //        // forwardDirection = Vector3.zero;
            //        float num3 = base.characterMotor.velocity.y;
            //        num3 = Mathf.MoveTowards(num3, hoverVelocity, hoverAcceleration * base.GetDeltaTime());
            //        base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num3, base.characterMotor.velocity.z);
            //    }
            //}

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction;
            }

            Vector3 rhs = characterDirection ? characterDirection.forward : forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            float num = Vector3.Dot(forwardDirection, rhs);
            float num2 = Vector3.Dot(forwardDirection, rhs2);

            RecalculateRollSpeed();

            if (characterMotor && characterDirection)
            {
                //characterMotor.velocity.y = 0f;
                characterMotor.velocity = forwardDirection * rollSpeed;
            }

            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;

            base.characterMotor.useGravity = false;


            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, RDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR1, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR2, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR3, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL1, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL2, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL3, true);


            PlayAnimation("FullBody, Override", "DashLoop", "DashLoop.playbackRate", duration);
            AkSoundEngine.PostEvent(XStaticValues.X_Falcon_Dash, this.gameObject);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(XBuffs.armorBuff, 3f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.2f * duration);
            }
        }

        private void RecalculateRollSpeed()
        {
            rollSpeed = moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateRollSpeed();

            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR1, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR2, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR3, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL1, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL2, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL3, true);

            if (characterDirection) characterDirection.forward = forwardDirection;
            if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            Vector3 normalized = (transform.position - previousPosition).normalized;
            if (characterMotor && characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, forwardDirection), 0f);
                vector = forwardDirection * d;

                //if(inputBank.moveVector != Vector3.zero)
                //{
                //   vector = forwardDirection * d;
                //}
                //else
                //{
                //    //vector = Vector3.zero;
                //    // forwardDirection = Vector3.zero;
                //    float num4 = base.characterMotor.velocity.y;
                //    num4 = Mathf.MoveTowards(num4, hoverVelocity, hoverAcceleration * base.GetDeltaTime());
                //    //base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num4, base.characterMotor.velocity.z);
                //    vector = new Vector3(base.characterMotor.velocity.x, num4, base.characterMotor.velocity.z);
                //}

                //vector = forwardDirection * d;
                //vector.y = 0f;

                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;

            if (isAuthority && fixedAge >= duration && base.inputBank.skill3.down && base.skillLocator.utility.stock >= 1)
            {
                FalconDash FD = new FalconDash();
                base.skillLocator.utility.stock--;
                outer.SetNextState(FD);
                return;
            }

            if (isAuthority && fixedAge >= duration)
            {
                base.characterMotor.useGravity = true;
                PlayAnimation("FullBody, Override", "DashEnd", "DashEnd.playbackRate", duration);
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
            base.OnExit();

            characterMotor.disableAirControlUntilCollision = false;
            
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
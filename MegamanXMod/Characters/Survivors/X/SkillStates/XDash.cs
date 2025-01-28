using EntityStates;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XDash : BaseSkillState
    {
        public static float duration = 0.5f;
        public static float initialSpeedCoefficient = 5f;
        public static float finalSpeedCoefficient = 4f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

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

            //if (isAuthority && inputBank && characterDirection)
            //{
            //    forwardDirection = aimRay.direction;
            //}

            Vector3 rhs = characterDirection ? characterDirection.forward : forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            float num = Vector3.Dot(forwardDirection, rhs);
            float num2 = Vector3.Dot(forwardDirection, rhs2);

            RecalculateRollSpeed();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity.y = 0f;
                characterMotor.velocity = forwardDirection * rollSpeed;
            }

            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;


            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, RDashPos, true);
            PlayAnimation("FullBody, Override", "DashLoop", "DashLoop.playbackRate", duration);
            AkSoundEngine.PostEvent(XStaticValues.X_Dash_SFX, this.gameObject);

            //if (NetworkServer.active)
            //{
            //    characterBody.AddTimedBuff(XBuffs.armorBuff, 3f * duration);
            //    characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.2f * duration);
            //}
        }

        private void RecalculateRollSpeed()
        {
            rollSpeed = moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateRollSpeed();

            if (characterDirection) characterDirection.forward = forwardDirection;
            if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            base.characterMotor.Motor.ForceUnground(0.1f);

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
                vector.y = 0f;

                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;

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
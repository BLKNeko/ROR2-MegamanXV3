using EntityStates;
using MegamanXMod.Survivors.X;
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

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            Ray aimRay = GetAimRay();

            if (isAuthority && inputBank && characterDirection)
            {
                if (inputBank.moveVector != Vector3.zero)
                {
                    forwardDirection = aimRay.direction;
                }
                else
                {
                    forwardDirection = Vector3.zero;
                }
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
            

            PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", duration);
            Util.PlaySound(dodgeSoundString, gameObject);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(HenryBuffs.armorBuff, 3f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * duration);
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

            if (characterDirection) characterDirection.forward = forwardDirection;
            if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            Vector3 normalized = (transform.position - previousPosition).normalized;
            if (characterMotor && characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, forwardDirection), 0f);

                if(inputBank.moveVector != Vector3.zero)
                {
                   vector = forwardDirection * d;
                }
                else
                {
                    vector = Vector3.zero;
                }

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
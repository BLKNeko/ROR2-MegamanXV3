using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXV3.SkillStates
{
    public class Dash : BaseState
    {
        public float baseDuration = 0.75f; // 75
        private float duration = 0.9f;
        private Animator animator;


        // Token: 0x06003E1F RID: 15903 RVA: 0x00102CA0 File Offset: 0x00100EA0
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(Sounds.xDash, base.gameObject);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, "LeftJet", false);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, "RightJet", false);
            this.animator = base.GetModelAnimator();
            ChildLocator component = this.animator.GetComponent<ChildLocator>();
            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }
            Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);
            float num = Vector3.Dot(this.forwardDirection, rhs);
            float num2 = Vector3.Dot(this.forwardDirection, rhs2);
            this.animator.SetFloat("forwardSpeed", num, 0.1f, Time.fixedDeltaTime);
            this.animator.SetFloat("rightSpeed", num2, 0.1f, Time.fixedDeltaTime);
            if (Mathf.Abs(num) > Mathf.Abs(num2))
            {
                base.PlayAnimation("FullBody, Override", (num > 0f) ? "Dash" : "Dash", "Dodge.playbackRate", this.duration);
            }
            else
            {
                base.PlayAnimation("FullBody, Override", (num2 > 0f) ? "Dash" : "Dash", "Dodge.playbackRate", this.duration);
            }
            if (Dash.jetEffect)
            {
                Transform transform = component.FindChild("LeftJet");
                Transform transform2 = component.FindChild("RightJet");
                if (transform)
                {
                    UnityEngine.Object.Instantiate<GameObject>(Dash.jetEffect, transform);
                }
                if (transform2)
                {
                    UnityEngine.Object.Instantiate<GameObject>(Dash.jetEffect, transform2);
                }

            }
            this.RecalculateRollSpeed();
            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y = 0f;
                base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
            }
            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;
        }

        // Token: 0x06003E20 RID: 15904 RVA: 0x00102EFD File Offset: 0x001010FD
        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(this.initialSpeedCoefficient, this.finalSpeedCoefficient, base.fixedAge / this.duration);
        }

        // Token: 0x06003E21 RID: 15905 RVA: 0x00102F2C File Offset: 0x0010112C
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.RecalculateRollSpeed();
            if (base.cameraTargetParams)
            {
                //base.cameraTargetParams.fovOverride = Mathf.Lerp(dodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
                base.cameraTargetParams.fovOverride = -2.5f;
            }
            Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
            if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * this.rollSpeed;
                float y = vector.y;
                vector.y = 0f;
                float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                vector = this.forwardDirection * d;
                vector.y += Mathf.Max(y, 0f);
                base.characterMotor.velocity = vector;
            }
            this.previousPosition = base.transform.position;
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06003E22 RID: 15906 RVA: 0x0010305D File Offset: 0x0010125D
        public override void OnExit()
        {
            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }
            base.OnExit();
        }

        // Token: 0x06003E23 RID: 15907 RVA: 0x00103082 File Offset: 0x00101282
        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        // Token: 0x06003E24 RID: 15908 RVA: 0x00103097 File Offset: 0x00101297
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }

        // Token: 0x0400392E RID: 14638

        public float initialSpeedCoefficient = 5.5f;

        // Token: 0x0400392F RID: 14639

        public float finalSpeedCoefficient = 4.5f;

        // Token: 0x04003930 RID: 14640
        public static string dodgeSoundString;

        // Token: 0x04003931 RID: 14641
        public static GameObject jetEffect;

        // Token: 0x04003932 RID: 14642
        public static float dodgeFOV;

        // Token: 0x04003933 RID: 14643
        private float rollSpeed = 5.5f;

        // Token: 0x04003934 RID: 14644
        private Vector3 forwardDirection;

        // Token: 0x04003936 RID: 14646
        private Vector3 previousPosition;
    }
}

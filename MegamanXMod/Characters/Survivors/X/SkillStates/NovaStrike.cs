using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class NovaStrike : BaseMeleeAttack2
    {

        public static float initialSpeedCoefficient = 9f;
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
            damageCoefficient = XStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1.4f * this.attackSpeedStat;            

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.1f;
            attackEndPercentTime = 0.9f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 1f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            swingSoundString = "HenrySwordSwing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = XAssets.swordSwingEffect;
            hitEffectPrefab = XAssets.swordHitImpactEffect;

            impactSound = XAssets.swordHitSoundEvent.index;

            //XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            //SetNextEntityState(xRathalosSlashCombo2);

            SetHitReset(true, 8);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            base.characterMotor.Motor.ForceUnground(0.1f);

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
                characterMotor.velocity = (forwardDirection * rollSpeed) + Vector3.one;
            }

            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;



            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "NovaStrikeStart", "attackSpeed", this.duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        private void RecalculateRollSpeed()
        {
            rollSpeed = (moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / baseDuration)) + 0.5f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //characterMotor.velocity *= 1.5f;

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
                //vector.y = 0f;

                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;

        }

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "NovaStrikeEnd", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
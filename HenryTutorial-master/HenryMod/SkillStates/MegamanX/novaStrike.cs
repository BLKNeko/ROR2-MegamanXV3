using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXV3.SkillStates
{
    public class novaStrike : BaseSkillState
    {
        public static float damageCoefficient = 2f;
        public static float buffDamageCoefficient = 1f;
        public float baseDuration = 0.95f; // 0.75
        public static float attackRecoil = 0.5f;
        public static float hitHopVelocity = 5.5f;
        public static float baseEarlyExit = 0.25f;
        public int swingIndex;

        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactMercAssaulter");

        private float earlyExitDuration;
        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        //private PaladinSwordController swordController;
        //DASH VARIAVEIS ------------------------------------------

        // Token: 0x0400392E RID: 14638

        public float initialSpeedCoefficient = 5f;

        // Token: 0x0400392F RID: 14639

        public float finalSpeedCoefficient = 4f;

        // Token: 0x04003930 RID: 14640
        public static string dodgeSoundString;

        // Token: 0x04003931 RID: 14641
        public static GameObject jetEffect;

        // Token: 0x04003932 RID: 14642
        public static float dodgeFOV;

        // Token: 0x04003933 RID: 14643
        private float rollSpeed = 5f;

        // Token: 0x04003934 RID: 14644
        private Vector3 forwardDirection;

        // Token: 0x04003936 RID: 14646
        private Vector3 previousPosition;

        //FOR DASH STATE


        public override void OnEnter()
        {
            base.OnEnter();

            //DAMAGE PART ----------------------------------------------------------------
            this.duration = this.baseDuration / (this.attackSpeedStat / 1.5f);
            this.earlyExitDuration = novaStrike.baseEarlyExit / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            //this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "NovaDashBox");
            }

            //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            base.PlayAnimation("FullBody, Override", "NovaStrike", "attackSpeed", this.duration / 2);

            EffectManager.SimpleMuzzleFlash(Assets.novaDashFireEffect, base.gameObject, "NovaStrikePoint", true);
            //EffectManager.SimpleMuzzleFlash(ZeroX.Assets.iceeffect, base.gameObject, "Sword", true);
            //EffectManager.SimpleMuzzleFlash(ZeroX.Assets.iceeffect, base.gameObject, "Sword", true);

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.8f);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.FullCrit, 0.7f);
            }

            //DAMAGE PART END ---------------------------------------------------------------------



            //DASH PART --------------------------------------------------------------------------

            Util.PlaySound(Sounds.xDash, base.gameObject);
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
                //base.PlayAnimation("Body", (num > 0f) ? "Dash" : "Dash", "Dodge.playbackRate", this.duration);
            }
            else
            {
                //base.PlayAnimation("Body", (num2 > 0f) ? "Dash" : "Dash", "Dodge.playbackRate", this.duration);
            }
            if (novaStrike.jetEffect)
            {
                Transform transform = component.FindChild("LeftJet");
                Transform transform2 = component.FindChild("RightJet");
                if (transform)
                {
                    UnityEngine.Object.Instantiate<GameObject>(novaStrike.jetEffect, transform);
                }
                if (transform2)
                {
                    UnityEngine.Object.Instantiate<GameObject>(novaStrike.jetEffect, transform2);
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

            //DASH PART END --------------------------------------------------------------------------------------

            float dmg = novaStrike.damageCoefficient;
            //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Freeze2s;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = novaStrike.hitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
           // Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);


        }

        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(this.initialSpeedCoefficient, this.finalSpeedCoefficient, base.fixedAge / this.duration);
        }

        public override void OnExit()
        {
            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                //this.hasFired = true;


                //string muzzleString = null;
                //if (this.swingIndex == 0) muzzleString = "SwingLeft";
                //else muzzleString = "SwingRight";

                // EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, muzzleString, true);

                //EffectManager.SimpleMuzzleFlash(ZeroX.Assets.fireeffect2, base.gameObject, "Sword", true);

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * novaStrike.attackRecoil, -2f * novaStrike.attackRecoil, -0.5f * novaStrike.attackRecoil, 0.5f * novaStrike.attackRecoil);

                    Ray aimRay = base.GetAimRay();

                    //if (this.swordController && this.swordController.swordActive)
                    //{
                    //    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                    // }

                    if (this.attack.Fire())
                    {
                        Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                        //Util.PlaySound(MinerPlugin.Sounds.Hit, base.gameObject);

                        if (!this.hasHopped)
                        {
                            if (base.characterMotor && !base.characterMotor.isGrounded)
                            {
                                base.SmallHop(base.characterMotor, novaStrike.hitHopVelocity);
                            }

                            this.hasHopped = true;
                        }

                        if (!this.inHitPause)
                        {
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "attackSpeed");
                            this.hitPauseTimer = (0.6f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                            this.inHitPause = true;
                        }
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();



            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
                //Vector3 velocity = base.characterDirection.forward * this.moveSpeedStat * Mathf.Lerp(6f, 6f, base.age / this.duration);
                //DASH FIXED UPDATE ----------------------------------------------------------------------------------------------------------------
                this.RecalculateRollSpeed();
                if (base.cameraTargetParams)
                {
                    //base.cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
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
                //DASH FIXED UPDATE END ----------------------------------------------------------------------------------------------------------------
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("attackSpeed", 1f);
            }

            if (this.stopwatch >= this.duration * 0.2f)
            {
                this.FireAttack();
            }



            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                //base.cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
                this.outer.SetNextStateToMain();

                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}

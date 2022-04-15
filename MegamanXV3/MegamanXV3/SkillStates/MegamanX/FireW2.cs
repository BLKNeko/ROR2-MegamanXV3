using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXV3.SkillStates
{
    public class FireW2 : BaseSkillState
    {
        public float damageCoefficient = 1f;
        public float baseDuration = 0.9f;
        public float recoil = 0.5f;
        //public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerToolbotRebar");
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/TracerEmbers");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");

        private int repeat = 0;
        private float duration;
        private float fireDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.25f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Weapon";


            base.PlayAnimation("Gesture, Override", "ShootPose", "attackSpeed", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireW()
        {
            if (!this.hasFired)
            {
                //this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.15f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    if (repeat == 1)
                        Util.PlaySound(Sounds.FireWaveSFX, base.gameObject);

                    if (repeat % 10 == 0 && repeat > 10 && repeat < 90)
                        Util.PlaySound(Sounds.FireWaveSFX, base.gameObject);

                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0.1f,
                        maxSpread = 0.4f,
                        damage = ((damageCoefficient * this.damageStat) / 100),
                        force = 20f,
                        tracerEffectPrefab = FireW2.tracerEffectPrefab,
                        muzzleName = muzzleString,
                        hitEffectPrefab = FireW2.hitEffectPrefab,
                        maxDistance = 35f,
                        smartCollision = true,
                        damageType = (Util.CheckRoll(0.48f, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic)
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                FireW();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                if (repeat <= 100)
                {
                    repeat++;
                    FireW();
                    base.characterBody.SetAimTimer(1f);
                }
                else
                {
                    base.PlayAnimation("Gesture, Override", "ShootBurst", "attackSpeed", this.duration);
                    this.outer.SetNextStateToMain();
                }
                //FireW2 FTR2 = new FireW2();
                //this.outer.SetNextState(FTR2);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

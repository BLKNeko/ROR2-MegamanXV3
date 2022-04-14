using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXV3.SkillStates
{
    public class AcidBurst2 : BaseSkillState
    {
        public float damageCoefficient = 1.45f;
        public float baseDuration = 0.84f;
        public float recoil = 0.7f;


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

        private void FireAB()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireShotgun.effectPrefab, base.gameObject, this.muzzleString, false);
                if (base.isAuthority)
                {
                    base.PlayAnimation("Gesture, Override", "ShootBurst", "attackSpeed", this.duration);
                    Util.PlaySound(Sounds.xBullet, base.gameObject);
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.aBurst, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                }
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (base.fixedAge >= this.fireDuration)
            {
                FireAB();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

    }
}

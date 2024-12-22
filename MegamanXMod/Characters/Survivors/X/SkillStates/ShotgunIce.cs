using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class ShotgunIce : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;
        private Renderer objectRenderer;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);



        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                base.characterBody.AddSpreadBloom(0.75f);
                //Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.IceNova.impactEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo TestProjectile = new FireProjectileInfo();
                    TestProjectile.projectilePrefab = XAssets.shotgunIceprefab;
                    TestProjectile.position = aimRay.origin;
                    TestProjectile.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    TestProjectile.owner = gameObject;
                    TestProjectile.damage = damageCoefficient * damageStat;
                    TestProjectile.force = force;
                    TestProjectile.crit = RollCrit();
                    TestProjectile.speedOverride = 10f;
                    TestProjectile.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(TestProjectile);
                    //ProjectileManager.instance.FireProjectile(XAssets.shotFMJ, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
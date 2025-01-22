using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class SqueezeBomb : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private GameObject muzzleObject;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzzPos";
            muzzleObject = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashLunarSecondary");

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
                FireSimpleBullet();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void FireSimpleBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(1f);
                EffectManager.SimpleMuzzleFlash(muzzleObject, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);
                    //Util.PlaySound(Sounds.xChargeShot, base.gameObject);
                    //ProjectileManager.instance.FireProjectile(XAssets.shurikenProjectilePrefab2, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                    FireProjectileInfo SqueezeBombProjectille = new FireProjectileInfo();
                    SqueezeBombProjectille.projectilePrefab = XAssets.SqueezeBombProjectile;
                    SqueezeBombProjectille.position = aimRay.origin;
                    SqueezeBombProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    SqueezeBombProjectille.owner = gameObject;
                    SqueezeBombProjectille.damage = damageCoefficient * damageStat;
                    SqueezeBombProjectille.force = force;
                    SqueezeBombProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    SqueezeBombProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(SqueezeBombProjectille);


                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
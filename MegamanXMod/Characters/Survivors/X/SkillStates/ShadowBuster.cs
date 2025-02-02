using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class ShadowBuster : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.ShadowBusterDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 1f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private GameObject hitEffectPrefab;
        private GameObject muzzleEffectPrefab;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(1f);
            muzzleString = "BusterMuzzPos";

            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/BackstabSpark");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashBarrage");


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

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    //AkSoundEngine.PostEvent(XStaticValues.X_Simple_Bullet, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();
                    Vector3 raygun1 = new Vector3(aimRay.direction.x + 0.15f, aimRay.direction.y, aimRay.direction.z);
                    Vector3 raygun2 = new Vector3(aimRay.direction.x - 0.15f, aimRay.direction.y, aimRay.direction.z);
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    
                    //Util.PlaySound(Sounds.xChargeShot, base.gameObject);
                    //ProjectileManager.instance.FireProjectile(XAssets.shurikenProjectilePrefab2, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                    FireProjectileInfo ShadowShurikenProjectille = new FireProjectileInfo();
                    ShadowShurikenProjectille.projectilePrefab = XAssets.shurikenProjectilePrefab2;
                    ShadowShurikenProjectille.position = aimRay.origin;
                    ShadowShurikenProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    ShadowShurikenProjectille.owner = gameObject;
                    ShadowShurikenProjectille.damage = damageCoefficient * damageStat;
                    ShadowShurikenProjectille.force = force;
                    ShadowShurikenProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    ShadowShurikenProjectille.damageColorIndex = DamageColorIndex.Default;



                    FireProjectileInfo ShadowShurikenProjectille2 = new FireProjectileInfo();
                    ShadowShurikenProjectille2.projectilePrefab = XAssets.shurikenProjectilePrefab2;
                    ShadowShurikenProjectille2.position = aimRay.origin;
                    ShadowShurikenProjectille2.rotation = Util.QuaternionSafeLookRotation(raygun1.normalized);
                    ShadowShurikenProjectille2.owner = gameObject;
                    ShadowShurikenProjectille2.damage = damageCoefficient * damageStat;
                    ShadowShurikenProjectille2.force = force;
                    ShadowShurikenProjectille2.crit = RollCrit();
                    //ShadowShurikenProjectille2.speedOverride = 20f;
                    ShadowShurikenProjectille2.damageColorIndex = DamageColorIndex.Default;



                    FireProjectileInfo ShadowShurikenProjectille3 = new FireProjectileInfo();
                    ShadowShurikenProjectille3.projectilePrefab = XAssets.shurikenProjectilePrefab2;
                    ShadowShurikenProjectille3.position = aimRay.origin;
                    ShadowShurikenProjectille3.rotation = Util.QuaternionSafeLookRotation(raygun2.normalized);
                    ShadowShurikenProjectille3.owner = gameObject;
                    ShadowShurikenProjectille3.damage = damageCoefficient * damageStat;
                    ShadowShurikenProjectille3.force = force;
                    ShadowShurikenProjectille3.crit = RollCrit();
                    //ShadowShurikenProjectille2.speedOverride = 20f;
                    ShadowShurikenProjectille3.damageColorIndex = DamageColorIndex.Default;



                    ProjectileManager.instance.FireProjectile(ShadowShurikenProjectille);
                    ProjectileManager.instance.FireProjectile(ShadowShurikenProjectille2);
                    ProjectileManager.instance.FireProjectile(ShadowShurikenProjectille3);

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
using EntityStates;
using MegamanXMod.Modules.BaseStates;
using MegamanXMod.Survivors.X;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XLightBuster : BaseChargePrimary
    {


        public override void OnEnter()
        {
            base.OnEnter();
            damageCoefficient = XStaticValues.XBusterDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 1f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;
            muzzleString = "BusterMuzzPos";

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(1f);
            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/HitsparkCommandoFMJ");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashBanditShotgun");

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        protected override void FireSimpleBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XBusterMediumProjectille = new FireProjectileInfo();
                    XBusterMediumProjectille.projectilePrefab = XAssets.xBusterMediumProjectile;
                    XBusterMediumProjectille.position = aimRay.origin;
                    XBusterMediumProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XBusterMediumProjectille.owner = gameObject;
                    XBusterMediumProjectille.damage = damageCoefficient * damageStat;
                    XBusterMediumProjectille.force = force;
                    XBusterMediumProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XBusterMediumProjectille.damageColorIndex = DamageColorIndex.Luminous;

                    ProjectileManager.instance.FireProjectile(XBusterMediumProjectille);
                }
            }
        }

        protected override void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XBusterChargeProjectille = new FireProjectileInfo();
                    XBusterChargeProjectille.projectilePrefab = XAssets.xBusterChargeProjectile;
                    XBusterChargeProjectille.position = aimRay.origin;
                    XBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XBusterChargeProjectille.owner = gameObject;
                    XBusterChargeProjectille.damage = damageCoefficient * damageStat;
                    XBusterChargeProjectille.force = force;
                    XBusterChargeProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    XBusterChargeProjectille.damageColorIndex = DamageColorIndex.Luminous;


                    ProjectileManager.instance.FireProjectile(XBusterChargeProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.75f);
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);

                if (base.isAuthority)
                {

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XLightBusterChargeProjectille = new FireProjectileInfo();
                    XLightBusterChargeProjectille.projectilePrefab = XAssets.xLightBusterChargeProjectile;
                    XLightBusterChargeProjectille.position = aimRay.origin;
                    XLightBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterChargeProjectille.owner = gameObject;
                    XLightBusterChargeProjectille.damage = damageCoefficient * damageStat;
                    XLightBusterChargeProjectille.force = force;
                    XLightBusterChargeProjectille.crit = RollCrit();
                    XLightBusterChargeProjectille.speedOverride = 300f;
                    XLightBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XLightBusterSmallProjectille = new FireProjectileInfo();
                    XLightBusterSmallProjectille.projectilePrefab = XAssets.xLightBusterSmallProjectile;
                    XLightBusterSmallProjectille.position = aimRay.origin;
                    XLightBusterSmallProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterSmallProjectille.owner = gameObject;
                    XLightBusterSmallProjectille.damage = damageCoefficient * damageStat;
                    XLightBusterSmallProjectille.force = force;
                    XLightBusterSmallProjectille.crit = RollCrit();
                    XLightBusterSmallProjectille.speedOverride = 250f;
                    XLightBusterSmallProjectille.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XLightBusterSmallProjectille2 = new FireProjectileInfo();
                    XLightBusterSmallProjectille2.projectilePrefab = XAssets.xLightBusterSmallProjectile;
                    XLightBusterSmallProjectille2.position = new Vector3(aimRay.origin.x + 0.8f, aimRay.origin.y + 0f, aimRay.origin.z + 0f);
                    XLightBusterSmallProjectille2.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterSmallProjectille2.owner = gameObject;
                    XLightBusterSmallProjectille2.damage = damageCoefficient * damageStat;
                    XLightBusterSmallProjectille2.force = force;
                    XLightBusterSmallProjectille2.crit = RollCrit();
                    XLightBusterSmallProjectille2.speedOverride = 250f;
                    XLightBusterSmallProjectille2.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XLightBusterSmallProjectille3 = new FireProjectileInfo();
                    XLightBusterSmallProjectille3.projectilePrefab = XAssets.xLightBusterSmallProjectile;
                    XLightBusterSmallProjectille3.position = new Vector3(aimRay.origin.x - 0.8f, aimRay.origin.y - 0f, aimRay.origin.z - 0f);
                    XLightBusterSmallProjectille3.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterSmallProjectille3.owner = gameObject;
                    XLightBusterSmallProjectille3.damage = damageCoefficient * damageStat;
                    XLightBusterSmallProjectille3.force = force;
                    XLightBusterSmallProjectille3.crit = RollCrit();
                    XLightBusterSmallProjectille3.speedOverride = 250f;
                    XLightBusterSmallProjectille3.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XLightBusterSmallProjectille4 = new FireProjectileInfo();
                    XLightBusterSmallProjectille4.projectilePrefab = XAssets.xLightBusterSmallProjectile;
                    XLightBusterSmallProjectille4.position = new Vector3(aimRay.origin.x + 0f, aimRay.origin.y + 1.5f, aimRay.origin.z + 0f);
                    XLightBusterSmallProjectille4.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterSmallProjectille4.owner = gameObject;
                    XLightBusterSmallProjectille4.damage = damageCoefficient * damageStat;
                    XLightBusterSmallProjectille4.force = force;
                    XLightBusterSmallProjectille4.crit = RollCrit();
                    XLightBusterSmallProjectille4.speedOverride = 250f;
                    XLightBusterSmallProjectille4.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XLightBusterSmallProjectille5 = new FireProjectileInfo();
                    XLightBusterSmallProjectille5.projectilePrefab = XAssets.xLightBusterSmallProjectile;
                    XLightBusterSmallProjectille5.position = new Vector3(aimRay.origin.x - 0f, aimRay.origin.y - 1.5f, aimRay.origin.z - 0f);
                    XLightBusterSmallProjectille5.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterSmallProjectille5.owner = gameObject;
                    XLightBusterSmallProjectille5.damage = damageCoefficient * damageStat;
                    XLightBusterSmallProjectille5.force = force;
                    XLightBusterSmallProjectille5.crit = RollCrit();
                    XLightBusterSmallProjectille5.speedOverride = 250f;
                    XLightBusterSmallProjectille5.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XLightBusterSmallProjectille6 = new FireProjectileInfo();
                    XLightBusterSmallProjectille6.projectilePrefab = XAssets.xLightBusterSmallProjectile;
                    XLightBusterSmallProjectille6.position = new Vector3(aimRay.origin.x + 0f, aimRay.origin.y - 0f, aimRay.origin.z + -1f);
                    XLightBusterSmallProjectille6.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterSmallProjectille6.owner = gameObject;
                    XLightBusterSmallProjectille6.damage = damageCoefficient * damageStat;
                    XLightBusterSmallProjectille6.force = force;
                    XLightBusterSmallProjectille6.crit = RollCrit();
                    XLightBusterSmallProjectille6.speedOverride = 220f;
                    XLightBusterSmallProjectille6.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XLightBusterChargeProjectille);

                    ProjectileManager.instance.FireProjectile(XLightBusterSmallProjectille);

                    ProjectileManager.instance.FireProjectile(XLightBusterSmallProjectille2);

                    ProjectileManager.instance.FireProjectile(XLightBusterSmallProjectille3);

                    ProjectileManager.instance.FireProjectile(XLightBusterSmallProjectille4);

                    ProjectileManager.instance.FireProjectile(XLightBusterSmallProjectille5);

                    ProjectileManager.instance.FireProjectile(XLightBusterSmallProjectille6);

                }
            }
        }

       
    }
}
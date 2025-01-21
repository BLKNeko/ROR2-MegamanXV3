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
    public class XForceBuster : BaseChargePrimary
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
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashFMJ");

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
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XBusterMediumProjectille = new FireProjectileInfo();
                    XBusterMediumProjectille.projectilePrefab = XAssets.xBusterMediumProjectile;
                    XBusterMediumProjectille.position = aimRay.origin;
                    XBusterMediumProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XBusterMediumProjectille.owner = gameObject;
                    XBusterMediumProjectille.damage = 1f;
                    XBusterMediumProjectille.force = force;
                    XBusterMediumProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XBusterMediumProjectille.damageColorIndex = DamageColorIndex.Default;

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
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

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
                    XBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;


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
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                if (base.isAuthority)
                {

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XLightBusterChargeProjectille = new FireProjectileInfo();
                    XLightBusterChargeProjectille.projectilePrefab = XAssets.xForceBusterProjectile;
                    XLightBusterChargeProjectille.position = aimRay.origin;
                    XLightBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterChargeProjectille.owner = gameObject;
                    XLightBusterChargeProjectille.damage = damageCoefficient * damageStat;
                    XLightBusterChargeProjectille.force = force;
                    XLightBusterChargeProjectille.crit = RollCrit();
                    XLightBusterChargeProjectille.speedOverride = 300f;
                    XLightBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XLightBusterChargeProjectille);

                }
            }
        }

        
    }
}
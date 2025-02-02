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
    public class XRathalosBuster : BaseChargePrimary
    {

        public override void OnEnter()
        {
            base.OnEnter();
            damageCoefficient = XStaticValues.XRathalosBusterDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 0.6f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 2f;
            range = 256f;
            muzzleString = "BusterMuzzPos";

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(1f);
            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/HitsparkCommandoFMJ");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashFireMeatBall");

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

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(1f);
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    AkSoundEngine.PostEvent(XStaticValues.X_RathalosBusterSimple_SFX, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XRethalosBusterMediumProjectille = new FireProjectileInfo();
                    XRethalosBusterMediumProjectille.projectilePrefab = XAssets.XRFireProjectile;
                    XRethalosBusterMediumProjectille.position = aimRay.origin;
                    XRethalosBusterMediumProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XRethalosBusterMediumProjectille.owner = gameObject;
                    XRethalosBusterMediumProjectille.damage = damageCoefficient * damageStat;
                    XRethalosBusterMediumProjectille.force = force;
                    XRethalosBusterMediumProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XRethalosBusterMediumProjectille.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(XRethalosBusterMediumProjectille);
                }
            }
        }

        protected override void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(1f);
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    AkSoundEngine.PostEvent(XStaticValues.X_RathalosBusterSimple_SFX, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XRethalosBusterChargeProjectille = new FireProjectileInfo();
                    XRethalosBusterChargeProjectille.projectilePrefab = XAssets.XRFireProjectile;
                    XRethalosBusterChargeProjectille.position = aimRay.origin;
                    XRethalosBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XRethalosBusterChargeProjectille.owner = gameObject;
                    XRethalosBusterChargeProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XRethalosBusterChargeProjectille.force = force;
                    XRethalosBusterChargeProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    XRethalosBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XRethalosBusterChargeProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (base.isAuthority)
                {

                    base.characterBody.AddSpreadBloom(2f);
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    AkSoundEngine.PostEvent(XStaticValues.X_RathalosBusterCharge_SFX, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XRethalosBusterChargeProjectille = new FireProjectileInfo();
                    XRethalosBusterChargeProjectille.projectilePrefab = XAssets.XRFire2Projectile;
                    XRethalosBusterChargeProjectille.position = aimRay.origin;
                    XRethalosBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XRethalosBusterChargeProjectille.owner = gameObject;
                    XRethalosBusterChargeProjectille.damage = (damageCoefficient * XStaticValues.XFullChargeDamageCoefficient) * damageStat;
                    XRethalosBusterChargeProjectille.force = force;
                    XRethalosBusterChargeProjectille.crit = RollCrit();
                    //XGaeaBusterChargeProjectille.speedOverride = 300f;
                    XRethalosBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;

                    


                    ProjectileManager.instance.FireProjectile(XRethalosBusterChargeProjectille);

                }
            }
        }

        
    }
}
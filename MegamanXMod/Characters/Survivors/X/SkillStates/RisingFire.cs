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
    public class RisingFire : BaseChargeSpecial
    {

        public override void OnEnter()
        {
            base.OnEnter();

            damageCoefficient = XStaticValues.RisingFireDamageCoefficient;
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
            hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/FireMeatBallExplosion");



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

                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, gameObject, muzzleString, false);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_Attack_VSFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(XStaticValues.X_RisingFire_SFX, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XRisingFireSimpleProjectille = new FireProjectileInfo();
                    XRisingFireSimpleProjectille.projectilePrefab = XAssets.XRFire2Projectile;
                    XRisingFireSimpleProjectille.position = aimRay.origin;
                    XRisingFireSimpleProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XRisingFireSimpleProjectille.owner = gameObject;
                    XRisingFireSimpleProjectille.damage = damageCoefficient * damageStat;
                    XRisingFireSimpleProjectille.force = force;
                    XRisingFireSimpleProjectille.crit = RollCrit();
                    XRisingFireSimpleProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XRisingFireSimpleProjectille);



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

                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, gameObject, muzzleString, false);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_Attack_VSFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(XStaticValues.X_RisingFire_SFX, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XRisingFireSimpleProjectille = new FireProjectileInfo();
                    XRisingFireSimpleProjectille.projectilePrefab = XAssets.XRFire2Projectile;
                    XRisingFireSimpleProjectille.position = aimRay.origin;
                    XRisingFireSimpleProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XRisingFireSimpleProjectille.owner = gameObject;
                    XRisingFireSimpleProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XRisingFireSimpleProjectille.force = force;
                    XRisingFireSimpleProjectille.crit = RollCrit();
                    XRisingFireSimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;



                    ProjectileManager.instance.FireProjectile(XRisingFireSimpleProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                RisingFireCharge risingFireCharge = new RisingFireCharge();

                SetNextEntityState(risingFireCharge);

            }
        }


    }
}
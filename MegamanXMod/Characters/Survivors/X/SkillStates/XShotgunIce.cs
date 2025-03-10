﻿using EntityStates;
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
    public class XShotgunIce : BaseChargeSecondary
    {

        public override void OnEnter()
        {
            base.OnEnter();
            damageCoefficient = XStaticValues.XShotgunIceDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 1f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 2f;
            range = 256f;
            muzzleString = "BusterMuzzPos";

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(1f);
            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/HitsparkCommandoFMJ");
            //muzzleEffectPrefab = EntityStates.Mage.Weapon.IceNova.impactEffectPrefab;
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

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(0.8f);
                    //EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    EffectManager.SpawnEffect(EntityStates.Mage.Weapon.IceNova.impactEffectPrefab, new EffectData
                    {
                        origin = childLocator.FindChild(muzzleString).transform.position,

                    }, true);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_shotgunIce_VSFX, this.gameObject);
                    }

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XShotgunIceProjectille = new FireProjectileInfo();
                    XShotgunIceProjectille.projectilePrefab = XAssets.shotgunIceprefab;
                    XShotgunIceProjectille.position = aimRay.origin;
                    XShotgunIceProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XShotgunIceProjectille.owner = gameObject;
                    XShotgunIceProjectille.damage = damageCoefficient * damageStat;
                    XShotgunIceProjectille.force = force;
                    XShotgunIceProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XShotgunIceProjectille.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(XShotgunIceProjectille);


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
                    //EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    EffectManager.SpawnEffect(EntityStates.Mage.Weapon.IceNova.impactEffectPrefab, new EffectData
                    {
                        origin = childLocator.FindChild(muzzleString).transform.position,

                    }, true);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_shotgunIce_VSFX, this.gameObject);
                    }

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XShotgunIceProjectille = new FireProjectileInfo();
                    XShotgunIceProjectille.projectilePrefab = XAssets.shotgunIceprefab;
                    XShotgunIceProjectille.position = aimRay.origin;
                    XShotgunIceProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XShotgunIceProjectille.owner = gameObject;
                    XShotgunIceProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XShotgunIceProjectille.force = force;
                    XShotgunIceProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XShotgunIceProjectille.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(XShotgunIceProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (isAuthority)
                {

                    base.characterBody.AddSpreadBloom(0.75f);
                    //EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    EffectManager.SpawnEffect(EntityStates.Mage.Weapon.IceNova.impactEffectPrefab, new EffectData
                    {
                        origin = childLocator.FindChild(muzzleString).transform.position,

                    }, true);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_shotgunIce_VSFX, this.gameObject);
                    }

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XShotgunIceProjectille = new FireProjectileInfo();
                    XShotgunIceProjectille.projectilePrefab = XAssets.shotgunIceprefab;
                    XShotgunIceProjectille.position = aimRay.origin;
                    XShotgunIceProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XShotgunIceProjectille.owner = gameObject;
                    XShotgunIceProjectille.damage = (damageCoefficient * XStaticValues.XFullChargeDamageCoefficient) * damageStat;
                    XShotgunIceProjectille.force = force;
                    XShotgunIceProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XShotgunIceProjectille.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(XShotgunIceProjectille);


                }
            }
        }

        
    }
}
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
    public class ChameleonSting : BaseChargeSpecial
    {

        public override void OnEnter()
        {
            base.OnEnter();

            damageCoefficient = XStaticValues.ChameleonStingDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 1f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzzPos";
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

                if (XConfig.enableVoiceBool.Value)
                {
                    AkSoundEngine.PostEvent(XStaticValues.X_ChameleonSting_VSFX, this.gameObject);
                }
                AkSoundEngine.PostEvent(XStaticValues.X_ChameleonSting_SFX, this.gameObject);

                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XChameleonStingSimpleProjectille = new FireProjectileInfo();
                    XChameleonStingSimpleProjectille.projectilePrefab = XAssets.ChameleonStingProjectile;
                    XChameleonStingSimpleProjectille.position = aimRay.origin;
                    XChameleonStingSimpleProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XChameleonStingSimpleProjectille.owner = gameObject;
                    XChameleonStingSimpleProjectille.damage = damageCoefficient * damageStat;
                    XChameleonStingSimpleProjectille.force = force;
                    XChameleonStingSimpleProjectille.crit = RollCrit();
                    XChameleonStingSimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;

                    FireProjectileInfo XChameleonSting2SimpleProjectille = new FireProjectileInfo();
                    XChameleonSting2SimpleProjectille.projectilePrefab = XAssets.ChameleonStingProjectile;
                    XChameleonSting2SimpleProjectille.position = aimRay.origin;
                    XChameleonSting2SimpleProjectille.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x + 0.2f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XChameleonSting2SimpleProjectille.owner = gameObject;
                    XChameleonSting2SimpleProjectille.damage = damageCoefficient * damageStat;
                    XChameleonSting2SimpleProjectille.force = force;
                    XChameleonSting2SimpleProjectille.crit = RollCrit();
                    XChameleonSting2SimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;

                    FireProjectileInfo XChameleonSting3SimpleProjectille = new FireProjectileInfo();
                    XChameleonSting3SimpleProjectille.projectilePrefab = XAssets.ChameleonStingProjectile;
                    XChameleonSting3SimpleProjectille.position = aimRay.origin;
                    XChameleonSting3SimpleProjectille.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x - 0.2f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XChameleonSting3SimpleProjectille.owner = gameObject;
                    XChameleonSting3SimpleProjectille.damage = damageCoefficient * damageStat;
                    XChameleonSting3SimpleProjectille.force = force;
                    XChameleonSting3SimpleProjectille.crit = RollCrit();
                    XChameleonSting3SimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;



                    ProjectileManager.instance.FireProjectile(XChameleonStingSimpleProjectille);
                    ProjectileManager.instance.FireProjectile(XChameleonSting2SimpleProjectille);
                    ProjectileManager.instance.FireProjectile(XChameleonSting3SimpleProjectille);


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

                if (XConfig.enableVoiceBool.Value)
                {
                    AkSoundEngine.PostEvent(XStaticValues.X_ChameleonSting_VSFX, this.gameObject);
                }
                AkSoundEngine.PostEvent(XStaticValues.X_ChameleonSting_SFX, this.gameObject);

                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XChameleonStingSimpleProjectille = new FireProjectileInfo();
                    XChameleonStingSimpleProjectille.projectilePrefab = XAssets.ChameleonStingProjectile;
                    XChameleonStingSimpleProjectille.position = aimRay.origin;
                    XChameleonStingSimpleProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XChameleonStingSimpleProjectille.owner = gameObject;
                    XChameleonStingSimpleProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XChameleonStingSimpleProjectille.force = force;
                    XChameleonStingSimpleProjectille.crit = RollCrit();
                    XChameleonStingSimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;

                    FireProjectileInfo XChameleonSting2SimpleProjectille = new FireProjectileInfo();
                    XChameleonSting2SimpleProjectille.projectilePrefab = XAssets.ChameleonStingProjectile;
                    XChameleonSting2SimpleProjectille.position = aimRay.origin;
                    XChameleonSting2SimpleProjectille.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x + 0.2f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XChameleonSting2SimpleProjectille.owner = gameObject;
                    XChameleonSting2SimpleProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XChameleonSting2SimpleProjectille.force = force;
                    XChameleonSting2SimpleProjectille.crit = RollCrit();
                    XChameleonSting2SimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;

                    FireProjectileInfo XChameleonSting3SimpleProjectille = new FireProjectileInfo();
                    XChameleonSting3SimpleProjectille.projectilePrefab = XAssets.ChameleonStingProjectile;
                    XChameleonSting3SimpleProjectille.position = aimRay.origin;
                    XChameleonSting3SimpleProjectille.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x - 0.2f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XChameleonSting3SimpleProjectille.owner = gameObject;
                    XChameleonSting3SimpleProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XChameleonSting3SimpleProjectille.force = force;
                    XChameleonSting3SimpleProjectille.crit = RollCrit();
                    XChameleonSting3SimpleProjectille.damageColorIndex = DamageColorIndex.Luminous;



                    ProjectileManager.instance.FireProjectile(XChameleonStingSimpleProjectille);
                    ProjectileManager.instance.FireProjectile(XChameleonSting2SimpleProjectille);
                    ProjectileManager.instance.FireProjectile(XChameleonSting3SimpleProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                AkSoundEngine.PostEvent(XStaticValues.X_Squeezebomb_SFX, this.gameObject);

                PlayAnimation("FullBody, Override", "HyperMode", "HyperMode.playbackRate", duration);

                if (NetworkServer.active)
                {

                    characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 8f);
                    characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 8f);
                    characterBody.AddTimedBuff(RoR2Content.Buffs.Cloak, 8f);
                    characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 8f);

                }

                //if (base.isAuthority)
                //{

                //    if (NetworkServer.active)
                //    {

                //        characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 8f);
                //        characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 8f);
                //        characterBody.AddTimedBuff(RoR2Content.Buffs.Cloak, 8f);
                //        characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 8f);

                //    }


                //}
            }
        }


    }
}
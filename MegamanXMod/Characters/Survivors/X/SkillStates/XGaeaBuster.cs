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
    public class XGaeaBuster : BaseChargePrimary
    {

        public override void OnEnter()
        {
            base.OnEnter();
            damageCoefficient = XStaticValues.XBusterDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 0.6f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;
            muzzleString = "BusterMuzzPos";

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(1f);
            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashVerminSpit");

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

                AkSoundEngine.PostEvent(XStaticValues.X_Mid_Bullet, this.gameObject);

                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XGaeaBusterMediumProjectille = new FireProjectileInfo();
                    XGaeaBusterMediumProjectille.projectilePrefab = XAssets.xGaeaBusterSmallProjectile;
                    XGaeaBusterMediumProjectille.position = aimRay.origin;
                    XGaeaBusterMediumProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XGaeaBusterMediumProjectille.owner = gameObject;
                    XGaeaBusterMediumProjectille.damage = damageCoefficient * damageStat;
                    XGaeaBusterMediumProjectille.force = force;
                    XGaeaBusterMediumProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    XGaeaBusterMediumProjectille.damageColorIndex = DamageColorIndex.Poison;

                    ProjectileManager.instance.FireProjectile(XGaeaBusterMediumProjectille);
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

                AkSoundEngine.PostEvent(XStaticValues.X_Mid_Bullet, this.gameObject);

                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XGaeaBusterChargeProjectille = new FireProjectileInfo();
                    XGaeaBusterChargeProjectille.projectilePrefab = XAssets.xGaeaBusterSmallProjectile;
                    XGaeaBusterChargeProjectille.position = aimRay.origin;
                    XGaeaBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XGaeaBusterChargeProjectille.owner = gameObject;
                    XGaeaBusterChargeProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XGaeaBusterChargeProjectille.force = force;
                    XGaeaBusterChargeProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    XGaeaBusterChargeProjectille.damageColorIndex = DamageColorIndex.Poison;


                    ProjectileManager.instance.FireProjectile(XGaeaBusterChargeProjectille);


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

                AkSoundEngine.PostEvent(XStaticValues.X_Charge_Shot, this.gameObject);

                PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                if (base.isAuthority)
                {

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XGaeaBusterChargeProjectille = new FireProjectileInfo();
                    XGaeaBusterChargeProjectille.projectilePrefab = XAssets.xGaeaBusterChargeProjectile;
                    XGaeaBusterChargeProjectille.position = aimRay.origin;
                    XGaeaBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XGaeaBusterChargeProjectille.owner = gameObject;
                    XGaeaBusterChargeProjectille.damage = (damageCoefficient * XStaticValues.XFullChargeDamageCoefficient) * damageStat;
                    XGaeaBusterChargeProjectille.force = force;
                    XGaeaBusterChargeProjectille.crit = RollCrit();
                    //XGaeaBusterChargeProjectille.speedOverride = 300f;
                    XGaeaBusterChargeProjectille.damageColorIndex = DamageColorIndex.Poison;

                    


                    ProjectileManager.instance.FireProjectile(XGaeaBusterChargeProjectille);

                }
            }
        }

       
    }
}
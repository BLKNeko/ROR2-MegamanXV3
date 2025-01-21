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
    public class XMaxBuster : BaseChargePrimary
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
                    XBusterMediumProjectille.damage = damageCoefficient * damageStat;
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

                    FireProjectileInfo XMaxBusterChargeProjectille = new FireProjectileInfo();
                    XMaxBusterChargeProjectille.projectilePrefab = XAssets.xMaxBusterChargeProjectile;
                    XMaxBusterChargeProjectille.position = aimRay.origin;
                    XMaxBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XMaxBusterChargeProjectille.owner = gameObject;
                    XMaxBusterChargeProjectille.damage = damageCoefficient * damageStat;
                    XMaxBusterChargeProjectille.force = force;
                    XMaxBusterChargeProjectille.crit = RollCrit();
                    XMaxBusterChargeProjectille.speedOverride = 300f;
                    XMaxBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille = new FireProjectileInfo();
                    XMaxBusterSmallProjectille.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille.position = aimRay.origin;
                    XMaxBusterSmallProjectille.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y - 0.2f, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille.owner = gameObject;
                    XMaxBusterSmallProjectille.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille.force = force;
                    XMaxBusterSmallProjectille.crit = RollCrit();
                    XMaxBusterSmallProjectille.speedOverride = 250f;
                    XMaxBusterSmallProjectille.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille2 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille2.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille2.position = aimRay.origin;
                    XMaxBusterSmallProjectille2.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x + 0.1f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille2.owner = gameObject;
                    XMaxBusterSmallProjectille2.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille2.force = force;
                    XMaxBusterSmallProjectille2.crit = RollCrit();
                    XMaxBusterSmallProjectille2.speedOverride = 250f;
                    XMaxBusterSmallProjectille2.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille3 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille3.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille3.position = aimRay.origin;
                    XMaxBusterSmallProjectille3.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x + 0.2f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille3.owner = gameObject;
                    XMaxBusterSmallProjectille3.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille3.force = force;
                    XMaxBusterSmallProjectille3.crit = RollCrit();
                    XMaxBusterSmallProjectille3.speedOverride = 250f;
                    XMaxBusterSmallProjectille3.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille4 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille4.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille4.position = aimRay.origin;
                    XMaxBusterSmallProjectille4.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x - 0.1f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille4.owner = gameObject;
                    XMaxBusterSmallProjectille4.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille4.force = force;
                    XMaxBusterSmallProjectille4.crit = RollCrit();
                    XMaxBusterSmallProjectille4.speedOverride = 250f;
                    XMaxBusterSmallProjectille4.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille5 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille5.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille5.position = aimRay.origin;
                    XMaxBusterSmallProjectille5.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x - 0.2f, aimRay.direction.y, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille5.owner = gameObject;
                    XMaxBusterSmallProjectille5.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille5.force = force;
                    XMaxBusterSmallProjectille5.crit = RollCrit();
                    XMaxBusterSmallProjectille5.speedOverride = 250f;
                    XMaxBusterSmallProjectille5.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille6 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille6.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille6.position = aimRay.origin;
                    XMaxBusterSmallProjectille6.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y + 0.1f, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille6.owner = gameObject;
                    XMaxBusterSmallProjectille6.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille6.force = force;
                    XMaxBusterSmallProjectille6.crit = RollCrit();
                    XMaxBusterSmallProjectille6.speedOverride = 220f;
                    XMaxBusterSmallProjectille6.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille7 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille7.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille7.position = aimRay.origin;
                    XMaxBusterSmallProjectille7.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y + 0.2f, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille7.owner = gameObject;
                    XMaxBusterSmallProjectille7.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille7.force = force;
                    XMaxBusterSmallProjectille7.crit = RollCrit();
                    XMaxBusterSmallProjectille7.speedOverride = 220f;
                    XMaxBusterSmallProjectille7.damageColorIndex = DamageColorIndex.Default;

                    FireProjectileInfo XMaxBusterSmallProjectille8 = new FireProjectileInfo();
                    XMaxBusterSmallProjectille8.projectilePrefab = XAssets.xMaxBusterSmallProjectile;
                    XMaxBusterSmallProjectille8.position = aimRay.origin;
                    XMaxBusterSmallProjectille8.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y - 0.1f, aimRay.direction.z).normalized);
                    XMaxBusterSmallProjectille8.owner = gameObject;
                    XMaxBusterSmallProjectille8.damage = damageCoefficient * damageStat;
                    XMaxBusterSmallProjectille8.force = force;
                    XMaxBusterSmallProjectille8.crit = RollCrit();
                    XMaxBusterSmallProjectille8.speedOverride = 220f;
                    XMaxBusterSmallProjectille8.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XMaxBusterChargeProjectille);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille2);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille3);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille4);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille5);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille6);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille7);

                    ProjectileManager.instance.FireProjectile(XMaxBusterSmallProjectille8);

                }
            }
        }

        
    }
}
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
    public class AcidBurst : BaseChargePrimary
    {

        public override void OnEnter()
        {
            base.OnEnter();

            //Nao precisa dos damagetype aqui de vdd, ja que estao no projetil, mas estao aqui para eu lembrar qqr coisa
            damageType = DamageType.PoisonOnHit;
            damageType = DamageType.WeakOnHit;
            damageType = DamageType.BypassArmor;
            damageType = DamageType.BypassBlock;
            damageType = DamageType.SlowOnHit;
            damageCoefficient = 1f;
            procCoefficient = 1f;
            baseDuration = 1f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";
            hitEffectPrefab = XAssets.swordHitImpactEffect;



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
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XAcidBurstSimpleProjectille = new FireProjectileInfo();
                    XAcidBurstSimpleProjectille.projectilePrefab = XAssets.AcidBurstProjectile;
                    XAcidBurstSimpleProjectille.position = aimRay.origin;
                    XAcidBurstSimpleProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XAcidBurstSimpleProjectille.owner = gameObject;
                    XAcidBurstSimpleProjectille.damage = damageCoefficient * damageStat;
                    XAcidBurstSimpleProjectille.force = force;
                    XAcidBurstSimpleProjectille.crit = RollCrit();
                    XAcidBurstSimpleProjectille.speedOverride = 50f;
                    XAcidBurstSimpleProjectille.damageColorIndex = DamageColorIndex.Poison;

                    ProjectileManager.instance.FireProjectile(XAcidBurstSimpleProjectille);


                }
            }
        }

        protected override void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XAcidBurstMediumProjectille = new FireProjectileInfo();
                    XAcidBurstMediumProjectille.projectilePrefab = XAssets.AcidBurstProjectile;
                    XAcidBurstMediumProjectille.position = aimRay.origin;
                    XAcidBurstMediumProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XAcidBurstMediumProjectille.owner = gameObject;
                    XAcidBurstMediumProjectille.damage = damageCoefficient * damageStat;
                    XAcidBurstMediumProjectille.force = force;
                    XAcidBurstMediumProjectille.crit = RollCrit();
                    XAcidBurstMediumProjectille.speedOverride = 50f;
                    XAcidBurstMediumProjectille.damageColorIndex = DamageColorIndex.Poison;

                    ProjectileManager.instance.FireProjectile(XAcidBurstMediumProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.75f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                if (base.isAuthority)
                {

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XAcidBurstChargeProjectille = new FireProjectileInfo();
                    XAcidBurstChargeProjectille.projectilePrefab = XAssets.AcidBurstProjectile;
                    XAcidBurstChargeProjectille.position = aimRay.origin;
                    XAcidBurstChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XAcidBurstChargeProjectille.owner = gameObject;
                    XAcidBurstChargeProjectille.damage = damageCoefficient * damageStat;
                    XAcidBurstChargeProjectille.force = force;
                    XAcidBurstChargeProjectille.crit = RollCrit();
                    XAcidBurstChargeProjectille.speedOverride = 50f;
                    XAcidBurstChargeProjectille.damageColorIndex = DamageColorIndex.Poison;

                    FireProjectileInfo XAcidBurstChargeProjectille2 = new FireProjectileInfo();
                    XAcidBurstChargeProjectille2.projectilePrefab = XAssets.AcidBurstProjectile;
                    XAcidBurstChargeProjectille2.position = aimRay.origin;
                    XAcidBurstChargeProjectille2.rotation = Util.QuaternionSafeLookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y + 0.25f, aimRay.direction.z).normalized);
                    XAcidBurstChargeProjectille2.owner = gameObject;
                    XAcidBurstChargeProjectille2.damage = damageCoefficient * damageStat;
                    XAcidBurstChargeProjectille2.force = force;
                    XAcidBurstChargeProjectille2.crit = RollCrit();
                    XAcidBurstChargeProjectille2.speedOverride = 50f;
                    XAcidBurstChargeProjectille2.damageColorIndex = DamageColorIndex.Poison;


                    ProjectileManager.instance.FireProjectile(XAcidBurstChargeProjectille);
                    ProjectileManager.instance.FireProjectile(XAcidBurstChargeProjectille2);

                }
            }
        }


    }
}
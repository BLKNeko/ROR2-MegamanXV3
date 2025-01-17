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

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XRisingFireSimpleProjectille = new FireProjectileInfo();
                    XRisingFireSimpleProjectille.projectilePrefab = XAssets.XRFire2Projectile;
                    XRisingFireSimpleProjectille.position = aimRay.origin;
                    XRisingFireSimpleProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XRisingFireSimpleProjectille.owner = gameObject;
                    XRisingFireSimpleProjectille.damage = damageCoefficient * damageStat;
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

                base.characterBody.AddSpreadBloom(0.75f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                //PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                if (base.isAuthority)
                {


                }
            }
        }


    }
}
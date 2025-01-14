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
    public class MeltCreeper : BaseChargeSpecial
    {
        private string muzzleStringB;

        private ChildLocator childLocator;

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
            hitEffectPrefab = XAssets.swordHitImpactEffect;
            muzzleString = "MeltCreeperFrontPos";
            muzzleStringB = "MeltCreeperBackPos";

            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

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
                EffectManager.SimpleMuzzleFlash(XAssets.MeltCreeperVFX, gameObject, muzzleString, true);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XMeltCreeperProjectille = new FireProjectileInfo();
                    XMeltCreeperProjectille.projectilePrefab = XAssets.MeltCreeperProjectile;
                    XMeltCreeperProjectille.position = childLocator.FindChild("MeltCreeperFrontPos").transform.position;
                    XMeltCreeperProjectille.rotation = childLocator.FindChild("MeltCreeperFrontPos").rotation;
                    XMeltCreeperProjectille.owner = gameObject;
                    XMeltCreeperProjectille.damage = 1;
                    XMeltCreeperProjectille.force = force;
                    XMeltCreeperProjectille.crit = RollCrit();
                    XMeltCreeperProjectille.speedOverride = 2f;
                    XMeltCreeperProjectille.damageColorIndex = DamageColorIndex.Default;
                    

                    ProjectileManager.instance.FireProjectile(XMeltCreeperProjectille);


                }
            }
        }

        protected override void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(XAssets.MeltCreeperVFX, gameObject, muzzleString, true);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XMeltCreeperProjectille = new FireProjectileInfo();
                    XMeltCreeperProjectille.projectilePrefab = XAssets.MeltCreeperProjectile;
                    XMeltCreeperProjectille.position = childLocator.FindChild("MeltCreeperFrontPos").transform.position;
                    XMeltCreeperProjectille.rotation = childLocator.FindChild("MeltCreeperFrontPos").rotation;
                    XMeltCreeperProjectille.owner = gameObject;
                    XMeltCreeperProjectille.damage = 1;
                    XMeltCreeperProjectille.force = force;
                    XMeltCreeperProjectille.crit = RollCrit();
                    XMeltCreeperProjectille.speedOverride = 2f;
                    XMeltCreeperProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XMeltCreeperProjectille);


                }
            }
        }

        protected override void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.75f);
                EffectManager.SimpleMuzzleFlash(XAssets.MeltCreeperChargeVFX, gameObject, muzzleString, true);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XMeltCreeperProjectille = new FireProjectileInfo();
                    XMeltCreeperProjectille.projectilePrefab = XAssets.MeltCreeperChargeProjectile;
                    XMeltCreeperProjectille.position = childLocator.FindChild("MeltCreeperFrontPos").transform.position;
                    XMeltCreeperProjectille.rotation = childLocator.FindChild("MeltCreeperFrontPos").rotation;
                    XMeltCreeperProjectille.owner = gameObject;
                    XMeltCreeperProjectille.damage = 1;
                    XMeltCreeperProjectille.force = force;
                    XMeltCreeperProjectille.crit = RollCrit();
                    XMeltCreeperProjectille.speedOverride = 0f;
                    XMeltCreeperProjectille.damageColorIndex = DamageColorIndex.Default;



                    ProjectileManager.instance.FireProjectile(XMeltCreeperProjectille);


                }
            }
        }

        
    }
}
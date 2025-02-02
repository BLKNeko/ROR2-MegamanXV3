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
            damageCoefficient = XStaticValues.MeltCreeperDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 1f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/FireMeatBallExplosion");
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

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(XAssets.MeltCreeperVFX, gameObject, muzzleString, true);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_meltCreeper_VSFX, this.gameObject);
                    }

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XMeltCreeperProjectille = new FireProjectileInfo();
                    XMeltCreeperProjectille.projectilePrefab = XAssets.MeltCreeperProjectile;
                    XMeltCreeperProjectille.position = childLocator.FindChild("MeltCreeperFrontPos").transform.position;
                    XMeltCreeperProjectille.rotation = childLocator.FindChild("MeltCreeperFrontPos").rotation;
                    XMeltCreeperProjectille.owner = gameObject;
                    XMeltCreeperProjectille.damage = damageCoefficient * damageStat;
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

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(XAssets.MeltCreeperVFX, gameObject, muzzleString, true);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_meltCreeper_VSFX, this.gameObject);
                    }

                    PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo XMeltCreeperProjectille = new FireProjectileInfo();
                    XMeltCreeperProjectille.projectilePrefab = XAssets.MeltCreeperProjectile;
                    XMeltCreeperProjectille.position = childLocator.FindChild("MeltCreeperFrontPos").transform.position;
                    XMeltCreeperProjectille.rotation = childLocator.FindChild("MeltCreeperFrontPos").rotation;
                    XMeltCreeperProjectille.owner = gameObject;
                    XMeltCreeperProjectille.damage = damageCoefficient * damageStat;
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

                if (isAuthority)
                {

                    base.characterBody.AddSpreadBloom(0.75f);
                    EffectManager.SimpleMuzzleFlash(XAssets.MeltCreeperChargeVFX, gameObject, muzzleString, true);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_meltCreeper_VSFX, this.gameObject);
                    }

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XMeltCreeperProjectille = new FireProjectileInfo();
                    XMeltCreeperProjectille.projectilePrefab = XAssets.MeltCreeperChargeProjectile;
                    XMeltCreeperProjectille.position = childLocator.FindChild("MeltCreeperFrontPos").transform.position;
                    XMeltCreeperProjectille.rotation = childLocator.FindChild("MeltCreeperFrontPos").rotation;
                    XMeltCreeperProjectille.owner = gameObject;
                    XMeltCreeperProjectille.damage = damageCoefficient * damageStat;
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
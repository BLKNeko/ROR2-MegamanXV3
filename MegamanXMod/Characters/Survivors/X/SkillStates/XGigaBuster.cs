using EntityStates;
using MegamanXMod.Survivors.X;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XGigaBuster : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.XGigaBusterDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 500f;
        public static float recoil = 5f;
        public static float range = 256f;

        private GameObject hitEffectPrefab;
        private GameObject muzzleEffectPrefab;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private const float Level1ChargeTime = 0.5f; // Tempo para ativar o primeiro nível de carregamento
        private const float Level2ChargeTime = 1.8f; // Tempo para ativar o segundo nível de carregamento
        private const float ChargeInterval = 0.68f;  // Intervalo entre efeitos de carregamento completo

        private float chargeTime = 0f;
        private float lastChargeTime = 0f;
        private bool chargeFullSFX = false;
        private bool hasTime = false;
        private int chargeLevel = 0;
        private bool chargingSFX = false;
        private bool playedVSFX = false;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzzPos";

            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/HitsparkCommandoFMJ");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashBanditShotgun");

        }

        public override void OnExit()
        {
            playedVSFX = false;
            chargeTime = 0f;
            chargeLevel = 0;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Carregar o ataque
            if (base.inputBank.skill1.down && !base.characterBody.HasBuff(XBuffs.GigaBusterChargeBuff))
            {
                ChargeShot();
            }
            else // Soltar o ataque
            {
                ReleaseChargeShot();
            }


            //if (fixedAge >= fireTime)
            //{
            //    FireSimpleBullet();
            //}

            //if (fixedAge >= duration && isAuthority)
            //{
            //    outer.SetNextStateToMain();
            //    return;
            //}

            if (((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 3 && hasTime == true) || characterBody.HasBuff(XBuffs.GigaBusterChargeBuff))
            {
                FireChargedBullet();
            } 
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 2 && hasTime == true && !characterBody.HasBuff(XBuffs.GigaBusterChargeBuff))
            {
                FireMediumBullet();
            }
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 1 && hasTime == true && !characterBody.HasBuff(XBuffs.GigaBusterChargeBuff))
            {
                FireSimpleBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;

                if(chargeLevel == 3 || characterBody.HasBuff(XBuffs.GigaBusterChargeBuff))
                {
                    XGigaBusterBuff XGB = new XGigaBusterBuff();
                    this.outer.SetNextState(XGB);
                }
                else
                    this.outer.SetNextStateToMain();

            }

        }

        private void FireSimpleBullet()
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

        private void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                AkSoundEngine.PostEvent(XStaticValues.X_Charge_Shot, this.gameObject);

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
                    XBusterChargeProjectille.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    XBusterChargeProjectille.force = force;
                    XBusterChargeProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    XBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XBusterChargeProjectille);


                }
            }
        }

        private void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;



                if (base.isAuthority)
                {

                    base.characterBody.AddSpreadBloom(0.75f);
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    AkSoundEngine.PostEvent(XStaticValues.X_Charge_Shot, this.gameObject);

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo XLightBusterChargeProjectille = new FireProjectileInfo();
                    XLightBusterChargeProjectille.projectilePrefab = XAssets.xLightBusterChargeProjectile;
                    XLightBusterChargeProjectille.position = aimRay.origin;
                    XLightBusterChargeProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    XLightBusterChargeProjectille.owner = gameObject;
                    XLightBusterChargeProjectille.damage = (damageCoefficient * XStaticValues.XFullChargeDamageCoefficient) * damageStat;
                    XLightBusterChargeProjectille.force = force;
                    XLightBusterChargeProjectille.crit = RollCrit();
                    XLightBusterChargeProjectille.speedOverride = 300f;
                    XLightBusterChargeProjectille.damageColorIndex = DamageColorIndex.Default;


                    ProjectileManager.instance.FireProjectile(XLightBusterChargeProjectille);

                }
            }
        }

        //CHARGE LOGIC

        private void ChargeShot()
        {
            chargeTime += Time.fixedDeltaTime;
            base.characterBody.SetAimTimer(2f);

            if (chargeTime > Level1ChargeTime && chargeTime <= Level2ChargeTime && !chargingSFX)
            {
                PlayChargingEffects(1);
                chargingSFX = true;
            }

            if (chargeTime >= Level2ChargeTime && !chargeFullSFX)
            {
                PlayChargingEffects(2);
                chargeFullSFX = true;
                lastChargeTime = chargeTime;
            }

            if ((chargeTime - lastChargeTime) >= ChargeInterval && chargeFullSFX)
            {
                PlayChargingEffects(2);
                lastChargeTime = chargeTime;
            }
        }

        private void ReleaseChargeShot()
        {
            // Determina o nível de carregamento com base no tempo
            if (chargeTime >= Level2ChargeTime)
            {
                chargeLevel = 3; // Nível máximo de carregamento
            }
            else if (chargeTime >= Level1ChargeTime)
            {
                chargeLevel = 2; // Nível intermediário de carregamento
            }
            else
            {
                chargeLevel = 1; // Nível mínimo de carregamento
            }

            AkSoundEngine.PostEvent(21663534, base.gameObject);

            if (XConfig.enableVoiceBool.Value && !playedVSFX && chargeLevel == 3)
            {
                AkSoundEngine.PostEvent(XStaticValues.X_Attack_VSFX, this.gameObject);
                playedVSFX = true;
            }

            chargingSFX = false;
            chargeFullSFX = false;
            hasTime = true;
            //chargeTime = 0; // Reseta o tempo de carga - movido para o onexit

        }

        private void PlayChargingEffects(int level)
        {
            switch (level)
            {
                case 1:
                    //Util.PlaySound(XStaticValues.charging, base.gameObject);
                    AkSoundEngine.PostEvent(3358936867, this.gameObject);
                    EffectManager.SimpleMuzzleFlash(XAssets.Charge1VFX, base.gameObject, "CorePosition", true);
                    break;

                case 2:
                    //Util.PlaySound(XStaticValues.fullCharge, base.gameObject);
                    AkSoundEngine.PostEvent(992292707, this.gameObject);
                    EffectManager.SimpleMuzzleFlash(XAssets.Charge2VFX, base.gameObject, "CorePosition", true);
                    break;

                default:
                    break;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
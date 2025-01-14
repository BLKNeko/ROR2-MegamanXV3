using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Modules.BaseStates
{
    public abstract class BaseChargePrimary : BaseSkillState
    {
        protected DamageType damageType = DamageType.Generic;
        protected float damageCoefficient = 1f;
        protected float procCoefficient = 1f;
        protected float baseDuration = 1f;
        protected string muzzleString = "";
        protected GameObject hitEffectPrefab;
        
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 400f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerBanditShotgun");

        public float duration;
        public bool hasFired;
        protected float fireTime;

        protected const float Level1ChargeTime = 0.5f; // Tempo para ativar o primeiro nível de carregamento
        protected const float Level2ChargeTime = 1.8f; // Tempo para ativar o segundo nível de carregamento
        protected const float ChargeInterval = 0.68f;  // Intervalo entre efeitos de carregamento completo

        protected float chargeTime = 0f;
        protected float lastChargeTime = 0f;
        protected bool chargeFullSFX = false;
        protected bool hasTime = false;
        protected int chargeLevel = 0;
        protected bool chargingSFX = false;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";



        }

        public override void OnExit()
        {
            //base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);
            chargeTime = 0f;
            chargeLevel = 0;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Carregar o ataque
            if (base.inputBank.skill1.down)
            {
                ChargeShot();
            }
            else // Soltar o ataque
            {
                ReleaseChargeShot();
            }

            if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 3 && hasTime == true)
            {
                FireChargedBullet();
            }
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 2 && hasTime == true)
            {
                FireMediumBullet();
            }
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 1 && hasTime == true)
            {
                FireSimpleBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;

                this.outer.SetNextStateToMain();
            }

        }

        protected virtual void FireSimpleBullet()
        {
            //Apenas para ser sobrescrito
        }

        protected virtual void FireMediumBullet()
        {
            //Apenas para ser sobrescrito
        }

        protected virtual void FireChargedBullet()
        {
            //Apenas para ser sobrescrito
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
                    //Util.PlaySound(Sounds.charging, base.gameObject);
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1C, base.gameObject, "Center", true);
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1W, base.gameObject, "Center", true);
                    EffectManager.SimpleMuzzleFlash(XAssets.Charge1VFX, base.gameObject, "CorePosition", true);
                    break;

                case 2:
                    //Util.PlaySound(Sounds.fullCharge, base.gameObject);
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect2C, base.gameObject, "Center", true);
                    EffectManager.SimpleMuzzleFlash(XAssets.Charge2VFX, base.gameObject, "CorePosition", true);
                    break;

                default:
                    break;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
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
    public class XFireWave : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.XBusterDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 400f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerBanditShotgun");

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
            if (base.inputBank.skill2.down)
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

            if ((!base.inputBank || !base.inputBank.skill2.down) && chargeLevel == 3 && hasTime == true)
            {
                XFireWave3 xFireWave3 = new XFireWave3();
                this.outer.SetNextState(xFireWave3);
            } 
            else if ((!base.inputBank || !base.inputBank.skill2.down) && chargeLevel == 2 && hasTime == true)
            {
                XFireWave2 xFireWave2 = new XFireWave2();
                this.outer.SetNextState(xFireWave2);
            }
            else if ((!base.inputBank || !base.inputBank.skill2.down) && chargeLevel == 1 && hasTime == true)
            {
                XFireWave2 xFireWave2 = new XFireWave2();
                this.outer.SetNextState(xFireWave2);
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
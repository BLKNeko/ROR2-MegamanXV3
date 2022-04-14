using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXV3.SkillStates.BaseStates
{
    public class Unlimited : GenericCharacterMain
    {
        public float MaxHP;
        public float GetHP;
        public double MinHP;
        public float Timer = 0;
        public float baseDuration = 1f;
        public static float ChameleonIvul = 0f;
        private float duration;
        private Animator animator;
        public override void OnEnter()
        {
            base.OnEnter();

        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Timer -= Time.fixedDeltaTime;
            MinHP = 0.3 + (base.characterBody.level / 200);
            if (base.characterBody.healthComponent.combinedHealthFraction < MinHP && Timer < 5f)
            {
                Util.PlaySound(Sounds.XPassive, base.gameObject);
                EffectManager.SimpleMuzzleFlash(Modules.Assets.CrystalEffect, base.gameObject, "Crystal", true);
                base.healthComponent.AddBarrierAuthority(base.characterBody.healthComponent.fullHealth / 2f);
                if (NetworkServer.active)
                {
                    // base.characterBody.AddTimedBuff(BuffIndex.LifeSteal, 5.8f);
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.LifeSteal, 5.8f);
                    // base.characterBody.AddTimedBuff(BuffIndex.FullCrit, 9.5f);
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.FullCrit, 5.8f);
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.SmallArmorBoost, 5.8f);
                }
                Timer = 150f;

            }

            

            if(ChameleonIvul >= 1.1f)
            {
                this.skillLocator.secondary.RemoveAllStocks();
                this.skillLocator.utility.RemoveAllStocks();

                
            }
            if(ChameleonIvul <= 1f && ChameleonIvul >= 0.4f)
            {
                this.skillLocator.secondary.Reset();
                this.skillLocator.utility.Reset();
               // Chat.AddMessage("test");
            }
            if(ChameleonIvul >= 0.2f)
            {
                ChameleonIvul -= Time.deltaTime;
            }


            return;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

using EntityStates;
using MegamanXMod.Modules;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Modules.BaseContent.BaseStates
{
    internal class XHeart : GenericCharacterMain
    {
        private float MaxHP;
        private float GetHP;
        private double MinHP;
        private float Timer = 0;
        private float baseDuration = 1f;
        private static float ChameleonIvul = 0f;
        private float duration;
        private Animator animator;

        private XBaseComponent XBaseComponent;

        public override void OnEnter()
        {
            base.OnEnter();

            XBaseComponent = GetComponent<XBaseComponent>();

            if (!XBaseComponent.GetExtraLife() && characterBody.inventory.GetItemCount(RoR2Content.Items.ExtraLifeConsumed) <= 0)
            {
                XBaseComponent.SetExtraLife(true);
                characterBody.inventory.GiveItem(RoR2Content.Items.ExtraLife, 1);
            }

        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(isAuthority)
            {
                if(Timer > 0)
                    Timer -= Time.fixedDeltaTime;

                MinHP = 0.3 + (base.characterBody.level / 200);

                if (base.characterBody.healthComponent.combinedHealthFraction < MinHP && Timer < 1f)
                {
                    
                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_Passive_VSFX, this.gameObject);
                    }
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.CrystalEffect, base.gameObject, "Crystal", true);
                    base.healthComponent.AddBarrierAuthority(base.characterBody.healthComponent.fullHealth / 2f);
                    if (NetworkServer.active)
                    {
                        // base.characterBody.AddTimedBuff(BuffIndex.LifeSteal, 5.8f);
                        base.characterBody.AddTimedBuff(RoR2Content.Buffs.LifeSteal, 5.8f);
                        // base.characterBody.AddTimedBuff(BuffIndex.FullCrit, 9.5f);
                        base.characterBody.AddTimedBuff(RoR2Content.Buffs.FullCrit, 5.8f);
                        base.characterBody.AddTimedBuff(RoR2Content.Buffs.SmallArmorBoost, 5.8f);
                    }
                    Timer = 200f;

                }

            }
            



            //if (ChameleonIvul >= 1.1f)
            //{
            //    this.skillLocator.secondary.RemoveAllStocks();
            //    this.skillLocator.utility.RemoveAllStocks();


            //}
            //if (ChameleonIvul <= 1f && ChameleonIvul >= 0.4f)
            //{
            //    this.skillLocator.secondary.Reset();
            //    this.skillLocator.utility.Reset();
            //    // Chat.AddMessage("test");
            //}
            //if (ChameleonIvul >= 0.2f)
            //{
            //    ChameleonIvul -= Time.deltaTime;
            //}


            return;

        }

    }
}

using EntityStates;
using MegamanXMod.Modules;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using RoR2.Audio;
using System;
using System.Xml.Linq;
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

            if (!XBaseComponent.GetExtraLife() && characterBody.inventory.GetItemCount(RoR2Content.Items.ExtraLifeConsumed) <= 0 && characterBody.inventory.GetItemCount(RoR2Content.Items.ExtraLife) <= 0)
            {
                XBaseComponent.SetExtraLife(true);
                characterBody.inventory.GiveItem(RoR2Content.Items.ExtraLife, 1);
            }

            //if(XConfig.enableXFootstep.Value)
            //{
            //    footstepHandler.baseFootstepString = "Play_X_Footstep_SFX";
            //    footstepHandler.sprintFootstepOverrideString = "Play_X_Footstep_SFX";
            //}
            //else
            //{
            //    footstepHandler.baseFootstepString = "";
            //    footstepHandler.sprintFootstepOverrideString = "";
            //}

        }



        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // A parte de HP baixo da passiva (escudo + buffs + cooldown) fica no XBaseComponent.
            // Aqui o cooldown se perdia: este state é recriado toda vez que uma skill termina
            // e roda em 3 state machines ao mesmo tempo (Body, Weapon e Weapon2).
            
            


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

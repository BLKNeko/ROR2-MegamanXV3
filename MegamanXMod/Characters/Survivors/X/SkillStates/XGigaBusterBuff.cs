﻿using EntityStates;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XGigaBusterBuff : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;

        private float duration;
        private float fireTime;
        private bool hasFired;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        private XArmorComponent armorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            fireTime = firePercentTime * duration;

        }

        public override void OnExit()
        {
            hasFired = false;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            

            if (fixedAge >= fireTime && !hasFired)
            {
                ApplyBuff();
            }

            if (fixedAge >= duration && isAuthority && hasFired)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void ApplyBuff()
        {

            if (NetworkServer.active)
            {
                if (characterBody.HasBuff(XBuffs.GigaBusterChargeBuff))
                {
                    characterBody.RemoveOldestTimedBuff(XBuffs.GigaBusterChargeBuff);
                }
                else
                {
                    characterBody.AddTimedBuff(XBuffs.GigaBusterChargeBuff, 20f);
                }
            }

            hasFired = true;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
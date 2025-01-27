using EntityStates;
using MegamanXMod.Modules;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Modules.BaseContent.BaseStates
{
    public class SpawnState : GenericCharacterSpawnState
    {
        private float duration;
        public float baseDuration = 1f;
        private Animator animator;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

        }
        public override void OnExit()
        {

            AkSoundEngine.PostEvent(XStaticValues.X_Ready, this.gameObject);

            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();



        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}


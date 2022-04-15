using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXV3.SkillStates.BaseStates
{
    public class DeathState : GenericCharacterDeath
    {

        private float duration;
        public float baseDuration = 1f;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            base.PlayAnimation("FullBody, Override", "Death", "attackSpeed", this.duration);
            Util.PlaySound(Sounds.die, base.gameObject);
        }
        public override void OnExit()
        {
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

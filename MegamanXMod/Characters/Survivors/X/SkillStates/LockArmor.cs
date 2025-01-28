using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class LockArmor : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;

        private float duration;
        private float fireTime;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;

            AkSoundEngine.PostEvent(XStaticValues.X_Error_SFX, this.gameObject);

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
                FireMessage();
            }

            if (fixedAge >= duration && isAuthority && hasFired)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void FireMessage()
        {
            if (!hasFired)
            {
                Chat.AddMessage("X: I Can`t use this yet!");
                hasFired = true;

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
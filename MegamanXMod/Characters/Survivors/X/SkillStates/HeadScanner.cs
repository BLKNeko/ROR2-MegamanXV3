using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HeadScanner : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
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

            AkSoundEngine.PostEvent(XStaticValues.X_Head_SFX, this.gameObject);

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
                FireScanner();
            }

            if (fixedAge >= duration && isAuthority && hasFired)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void FireScanner()
        {
            if (!hasFired)
            {
                NetworkServer.Spawn(UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/ChestScanner"), this.characterBody.corePosition, Quaternion.identity));
                hasFired = true;

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
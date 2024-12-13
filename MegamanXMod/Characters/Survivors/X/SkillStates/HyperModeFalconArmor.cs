using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HyperModeFalconArmor : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);


            //TRANSFORM INTO MAX ARMOR
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                if(this.characterModel != null)
                {
                    childLocator = this.characterModel.GetComponent<ChildLocator>();

                    meshRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
                    meshRenderer.sharedMesh = XAssets.FourthBodyMesh;
                    meshRenderer.sharedMaterial = XAssets.MatFourth;
                    characterModel.baseRendererInfos[0].defaultMaterial = XAssets.MatFourth;

                }
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
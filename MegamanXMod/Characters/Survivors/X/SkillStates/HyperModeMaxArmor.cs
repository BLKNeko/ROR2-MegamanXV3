using EntityStates;
using ExtraSkillSlots;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HyperModeMaxArmor : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private ExtraSkillLocator extraskillLocator;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        private bool setSkills = false;

        private XArmorComponent armorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            armorComponent = GetComponent<XArmorComponent>();
            extraskillLocator = base.GetComponent<ExtraSkillLocator>();

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
                    meshRenderer.sharedMesh = XAssets.MaxBodyMesh;
                    meshRenderer.sharedMaterial = XAssets.MatMax;
                    characterModel.baseRendererInfos[0].defaultMaterial = XAssets.MatMax;

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

            if (!setSkills)
                SetSkills();


            if (fixedAge >= duration && isAuthority && setSkills)
            {
                setSkills = false;
                outer.SetNextStateToMain();
                return;
            }
        }

        private void SetSkills()
        {
            Debug.Log(armorComponent.GetPrimaryArmorSkillDef());
            Debug.Log(armorComponent.GetSecondaryArmorSkillDef());
            Debug.Log(armorComponent.GetThirdArmorSkillDef());
            extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            setSkills = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
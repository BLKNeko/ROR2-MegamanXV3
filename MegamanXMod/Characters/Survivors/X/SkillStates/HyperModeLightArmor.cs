using EntityStates;
using ExtraSkillSlots;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HyperModeLightArmor : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private bool setSkills = false;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        private ExtraSkillLocator extraskillLocator;

        private XArmorComponent armorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);
            }

            EffectManager.SimpleMuzzleFlash(XAssets.HyperModeEffect, base.gameObject, "CorePosition", true);

            armorComponent = GetComponent<XArmorComponent>();
            extraskillLocator = base.GetComponent<ExtraSkillLocator>();


            //TRANSFORM INTO LIGHT ARMOR
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                if(this.characterModel != null)
                {
                    childLocator = this.characterModel.GetComponent<ChildLocator>();

                    meshRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
                    meshRenderer.sharedMesh = XAssets.LightBodyMesh;
                    meshRenderer.sharedMaterial = XAssets.MatLight;
                    characterModel.baseRendererInfos[0].defaultMaterial = XAssets.MatLight;

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

            if (!setSkills && isAuthority)
                SetSkills();


            if (fixedAge >= duration && isAuthority && setSkills)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void SetSkills()
        {

            armorComponent.UnsetAllExtraFirstSkills();
            armorComponent.UnsetAllUtilitySkills();


            //RESET ALL EXTRA SKILLS AND SET FIRST EXTRA TO COOLDOWN X
            extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, armorComponent.GetSecondaryArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, armorComponent.GetThirdArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, armorComponent.GetFourthArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);

            //RESET ALL NORMAL SKILLS AND SET THE PRIMARY AND UTILITY FOR FALCON
            characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, XSurvivor.XFalconDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.secondary.SetSkillOverride(characterBody.skillLocator.secondary, armorComponent.GetSecondaryBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.utility.SetSkillOverride(characterBody.skillLocator.utility, XSurvivor.XFalconDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.special.SetSkillOverride(characterBody.skillLocator.special, armorComponent.GetSpecialBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);

            setSkills = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
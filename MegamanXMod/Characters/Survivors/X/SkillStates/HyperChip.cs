using EntityStates;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HyperChip : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
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
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;

            //PlayAnimation("FullBody, Override", "HyperMode", "HyperMode.playbackRate", duration);

            this.modelTransform = base.GetModelTransform();

            armorComponent = GetComponent<XArmorComponent>();

            //TRANSFORM INTO MAX ARMOR
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                if (this.characterModel != null)
                {
                    childLocator = this.characterModel.GetComponent<ChildLocator>();

                    meshRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
                    meshRenderer.sharedMaterial = XAssets.MatMaxGold;
                    characterModel.baseRendererInfos[0].defaultMaterial = XAssets.MatMaxGold;
                    armorComponent.SetXModel(characterModel);
                    armorComponent.SetXMeshRender(meshRenderer);
                }
            }

            

            //armorComponent = GetComponent<XArmorComponent>();

        }

        public override void OnExit()
        {
            hasFired = false;

            //armorComponent.ApplyGoldTexture(modelTransform.GetComponent<CharacterModel>());

            //if (!this.outer.destroying)
            //{
            //    if (modelTransform)
            //    {
            //        TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
            //        temporaryOverlayInstance.duration = 20f;
            //        temporaryOverlayInstance.animateShaderAlpha = true;
            //        temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 7.5f, 0f);
            //        temporaryOverlayInstance.destroyComponentOnEnd = true;
            //        temporaryOverlayInstance.originalMaterial = XAssets.MatMaxGold;
            //        temporaryOverlayInstance.inspectorCharacterModel = modelTransform.GetComponent<CharacterModel>();
            //        temporaryOverlayInstance.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());

            //        //temporaryOverlayInstance.CleanupEffect();

            //    }
            //}


            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            

            if (fixedAge >= fireTime && !hasFired)
            {
                ApplyChip();
            }

            if (fixedAge >= duration && isAuthority && hasFired)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void ApplyChip()
        {

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(XBuffs.HyperChipBuff, 20f);
            }

            

            hasFired = true;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
using EntityStates;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class FalconDash : BaseSkillState
    {
        public static float duration = 0.7f;
        public static float initialSpeedCoefficient = 8f;
        public static float finalSpeedCoefficient = 5f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";
        private string FWingR1 = "FWingR1";
        private string FWingR2 = "FWingR2";
        private string FWingR3 = "FWingR3";
        private string FWingL1 = "FWingL1";
        private string FWingL2 = "FWingL2";
        private string FWingL3 = "FWingL3";

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        public static float hoverVelocity = -3f;
        public static float hoverAcceleration = 0.5f;

        private float afterImageTimer = 0f;
        private Transform modelTransform;
        private CharacterModel characterModel;
        private ChildLocator childLocator;

        private XHoverComponent hoverComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            hoverComponent = GetComponent<XHoverComponent>();

            hoverComponent.SetHover(true);

            //hoverComponent.SetChildLocator(base.GetModelTransform().GetComponent<ChildLocator>());

            //if (isAuthority && inputBank && characterDirection)
            //{
            //    if (inputBank.moveVector != Vector3.zero)
            //    {
            //        forwardDirection = aimRay.direction;
            //    }
            //    else
            //    {
            //        // forwardDirection = Vector3.zero;
            //        float num3 = base.characterMotor.velocity.y;
            //        num3 = Mathf.MoveTowards(num3, hoverVelocity, hoverAcceleration * base.GetDeltaTime());
            //        base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num3, base.characterMotor.velocity.z);
            //    }
            //}

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction.normalized;
            }

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * initialSpeedCoefficient;
            }

            base.characterMotor.useGravity = false;

            modelTransform = base.GetModelTransform();
            characterModel = characterBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();
            childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            CreateAfterImage();


            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, RDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR1, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR2, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR3, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL1, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL2, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL3, true);


            PlayAnimation("FullBody, Override", "DashLoop", "attackSpeed", duration);
            AkSoundEngine.PostEvent(XStaticValues.X_Falcon_Dash, this.gameObject);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(XBuffs.armorBuff, 3f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.2f * duration);
            }
        }

        private void CreateAfterImage()
        {

            //Debug.LogWarning("ChieldLocator: " + childLocator);
            //Debug.LogWarning("characterModel: " + characterModel);
            //Debug.LogWarning("childLocator.FindChildGameObject(MMZZeroBodyMesh): " + childLocator.FindChildGameObject("MMZZeroBodyMesh"));
            //Debug.LogWarning("childLocator.FindChildGameObject(MMZZeroBodyMesh)SkinnedMeshRenderer: " + childLocator.FindChildGameObject("MMZZeroBodyMesh").GetComponent<SkinnedMeshRenderer>());

            if (modelTransform)
            {
                TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(this.modelTransform.gameObject);
                temporaryOverlayInstance.duration = 0.5f;
                temporaryOverlayInstance.animateShaderAlpha = true;
                temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlayInstance.destroyComponentOnEnd = true;
                temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matGhostEffect");
                temporaryOverlayInstance.inspectorCharacterModel = characterModel;
                temporaryOverlayInstance.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
            }

            if (!characterBody || !characterBody.modelLocator || !characterBody.modelLocator.modelTransform)
            {
                Debug.LogWarning("CreateAfterImage: characterBody or modelLocator is null");
                return;
            }

            var skinnedRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
            if (!skinnedRenderer)
            {
                Debug.LogWarning("CreateAfterImage: SkinnedMeshRenderer not found");
                return;
            }

            var mesh = new Mesh();
            skinnedRenderer.BakeMesh(mesh);

            GameObject ghostObject = new GameObject("AfterImageGhost");
            ghostObject.transform.position = skinnedRenderer.transform.position;
            ghostObject.transform.rotation = skinnedRenderer.transform.rotation;
            ghostObject.transform.localScale = skinnedRenderer.transform.lossyScale;

            var meshFilter = ghostObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            var meshRenderer = ghostObject.AddComponent<MeshRenderer>();
            var ghostMat = LegacyResourcesAPI.Load<Material>("Materials/matGhostEffect");
            if (!ghostMat)
            {
                Debug.LogWarning("CreateAfterImage: matGhostEffect not found");
                return;
            }

            meshRenderer.material = ghostMat;

            // Fade e destruição automática
            ghostObject.AddComponent<DestroyGhost>().Initialize(1f);


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR1, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR2, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingR3, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL1, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL2, true);
            //EffectManager.SimpleMuzzleFlash(EntityStates.Mage.FlyUpState.muzzleflashEffect, gameObject, FWingL3, true);

            if (isAuthority)
            {
                afterImageTimer -= Time.fixedDeltaTime;
                if (afterImageTimer <= 0f)
                {
                    CreateAfterImage();
                    afterImageTimer = 0.045f; // intervalo entre fantasmas
                }
            }

            if (characterDirection) characterDirection.forward = forwardDirection;

            if (cameraTargetParams)
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);


            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
            }

            if (isAuthority && fixedAge >= duration && base.inputBank.skill3.down && base.skillLocator.utility.stock >= 1)
            {
                FalconDash FD = new FalconDash();
                base.skillLocator.utility.stock--;
                outer.SetNextState(FD);
                return;
            }

            if (isAuthority && fixedAge >= duration)
            {
                base.characterMotor.useGravity = true;
                PlayAnimation("FullBody, Override", "DashEnd", "attackSpeed", duration);
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
            base.OnExit();

            characterMotor.disableAirControlUntilCollision = false;

            PlayAnimation("FullBody, Override", "DashEnd", "attackSpeed", duration);

        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
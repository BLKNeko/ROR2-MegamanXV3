using EntityStates;
using MegamanXMod.Modules.BaseStates;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XDash2 : BaseMeleeAttack2
    {

        public static float initialSpeedCoefficient = 5f;
        public static float finalSpeedCoefficient = 4f;
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";

        private float afterImageTimer = 0f;
        private Transform modelTransform;
        private CharacterModel characterModel;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            hitboxGroupName = "";

            //his.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();


            //EffectManager.SimpleMuzzleFlash(XAssets.NovaStrikeVFX, base.gameObject, "NovaDashPos", true);

            //EffectManager.SpawnEffect(XAssets.NovaStrikeVFX, new EffectData
            //{
            //    origin = childLocator.FindChild("NovaDashPos").transform.position,
            //    scale = 8f,
            //    rootObject = characterBody.transform.gameObject,
            //    //rotation = Quaternion.Euler(0, 0, 180),


            //}, true);

            damageType = DamageType.Generic;
            damageCoefficient = 0f;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.5f ;            

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.1f;
            attackEndPercentTime = 0.1f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 1f;

            hitStopDuration = 0;
            attackRecoil = 0f;
            hitHopVelocity = 5f;

            hitSoundString = "";

            playbackRateParam = "Slash.playbackRate";

            //impactSound = XAssets.swordHitSoundEvent.index;

            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, RDashPos, true);
            AkSoundEngine.PostEvent(XStaticValues.X_Dash_SFX, this.gameObject);

            //XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            //SetNextEntityState(xRathalosSlashCombo2);

            SetHitReset(true, 3);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction.normalized;
            }

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * initialSpeedCoefficient;
            }

            modelTransform = base.GetModelTransform();
            characterModel = characterBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();
            childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            CreateAfterImage();

            base.OnEnter();
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
                temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matMercHologram");
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
            var ghostMat = LegacyResourcesAPI.Load<Material>("Materials/matMercHologram");
            if (!ghostMat)
            {
                Debug.LogWarning("CreateAfterImage: matGhostEffect not found");
                return;
            }

            meshRenderer.material = ghostMat;

            // Fade e destruição automática
            ghostObject.AddComponent<DestroyGhost>().Initialize(0.9f);


        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "DashLoop", "attackSpeed", this.duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //characterMotor.velocity *= 1.5f;

            if (isAuthority)
            {
                afterImageTimer -= Time.fixedDeltaTime;
                if (afterImageTimer <= 0f)
                {
                    CreateAfterImage();
                    afterImageTimer = 0.05f; // intervalo entre fantasmas
                }
            }

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (characterDirection) characterDirection.forward = forwardDirection;

            if (cameraTargetParams)
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);


            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
            }

        }

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "DashEnd", "attackSpeed", this.duration);

            base.OnExit();
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
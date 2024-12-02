using MegamanXMod.Modules.BaseStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class SlashCombo : BaseMeleeAttack
    {

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            hitboxGroupName = "SwordGroup";

            damageType = DamageType.Generic;
            damageCoefficient = HenryStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.4f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "HenrySwordSwing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = HenryAssets.swordSwingEffect;
            hitEffectPrefab = HenryAssets.swordHitImpactEffect;

            impactSound = HenryAssets.swordHitSoundEvent.index;

            //TRANSFORM BACK TO X
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                if (this.characterModel != null)
                {
                    childLocator = this.characterModel.GetComponent<ChildLocator>();

                    childLocator.FindChildGameObject("XBusterMesh").active = true;
                    meshRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
                    meshRenderer.sharedMesh = HenryAssets.XBodyMesh;
                    meshRenderer.sharedMaterial = HenryAssets.MatX;
                }
            }

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
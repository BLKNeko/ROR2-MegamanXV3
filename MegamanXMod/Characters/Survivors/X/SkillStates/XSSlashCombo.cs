﻿using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XSSlashCombo : BaseMeleeAttack
    {

        public override void OnEnter()
        {
            hitboxGroupName = "ShadowSaberGroup";

            damageType = DamageType.Generic;
            damageCoefficient = XStaticValues.XSSlashComboDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.5f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.8f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.8f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            swingSoundString = swingIndex % 2 == 0 ? XStaticValues.X_Slash3_SFX : XStaticValues.X_Slash2_SFX;

            hitSoundString = "";
            //muzzleString = "SwordMuzzPos";
            muzzleString = swingIndex % 2 == 0 ? "SwordMuzzLPos" : "SwordMuzzRPos";
            playbackRateParam = "attackSpeed";
            swingEffectPrefab = XAssets.swordSwingEffectY;
            hitEffectPrefab = XAssets.swordHitImpactEffect;

            impactSound = XAssets.swordHitSoundEvent.index;


            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("Gesture, Override", "XSSlash" + (1 + swingIndex), "attackSpeed", this.duration);
        }

        protected virtual void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(swingEffectPrefab, gameObject, muzzleString, true);
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {

            base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Primária da Blade Armor: combo de cortes normais com a Blade Saber.
    public class XBladeSlashCombo : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            // TODO: trocar por um HitBoxGroup próprio da Blade Saber se o tamanho do Shadow Saber não servir
            hitboxGroupName = "ShadowSaberGroup";

            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = XStaticValues.XBladeSlashDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.5f;

            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.6f;
            earlyExitPercentTime = 0.7f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            swingSoundString = swingIndex % 2 == 0 ? XStaticValues.X_Slash3_SFX : XStaticValues.X_Slash2_SFX;
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwordMuzzLPos" : "SwordMuzzRPos";
            playbackRateParam = "attackSpeed";
            swingEffectPrefab = XAssets.swordSwingEffectY;
            hitEffectPrefab = XAssets.swordHitImpactEffect;

            impactSound = XAssets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            // Reaproveita as animações do Shadow Saber (XSSlash1 / XSSlash2)
            PlayAnimation("Gesture, Override", "XSSlash" + (1 + swingIndex % 2), "attackSpeed", duration);
        }

        public override void OnExit()
        {
            PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", duration);
            base.OnExit();
        }
    }
}

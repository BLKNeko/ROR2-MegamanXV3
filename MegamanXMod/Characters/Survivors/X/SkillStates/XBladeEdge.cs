using EntityStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Secundária da Blade Armor: energiza a Blade Saber.
    // O BladeEdgeBuff dá mais dano, roubo de vida e reflete projéteis (XBladeDeflectComponent).
    public class XBladeEdge : BaseSkillState
    {
        public static float baseDuration = 0.4f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;

            if (isAuthority)
            {
                // Funciona no host e no cliente (envia para o servidor quando necessário)
                characterBody.AddTimedBuffAuthority(XBuffs.BladeEdgeBuff.buffIndex, XStaticValues.BladeEdgeDuration);
            }

            AkSoundEngine.PostEvent(XStaticValues.X_Slash2_SFX, gameObject);
            EffectManager.SimpleMuzzleFlash(XAssets.swordSwingEffectY, gameObject, "SwordMuzzPos", false);

            Transform modelTransform = GetModelTransform();
            if (modelTransform)
            {
                CharacterModel characterModel = modelTransform.GetComponent<CharacterModel>();
                if (characterModel)
                {
                    TemporaryOverlayInstance overlay = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                    overlay.duration = 0.6f;
                    overlay.animateShaderAlpha = true;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                    overlay.AddToCharacterModel(characterModel);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

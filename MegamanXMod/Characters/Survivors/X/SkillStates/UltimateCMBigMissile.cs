using EntityStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Secundária da Ultimate Armor CM: um míssil grande que explode em área.
    public class UltimateCMBigMissile : BaseSkillState
    {
        public static float baseDuration = 0.8f;

        private static int launcherIndex;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);

            AkSoundEngine.PostEvent(XStaticValues.X_Charge_Shot, gameObject);

            // Animação acompanha o lançador do disparo (Shoot1 = CMULMuzzR, Shoot2 = CMULMuzzL)
            PlayAnimation("Gesture, Override", UltimateCMMissileHelper.GetShootAnimation(launcherIndex), "ShootGun.playbackRate", duration);

            if (isAuthority)
            {
                HurtBox target = UltimateCMMissileHelper.GetTarget(gameObject);
                UltimateCMMissileHelper.FireMissile(gameObject, GetAimRay(), XAssets.UltimateCMBigMissileProjectile,
                    damageStat * XStaticValues.UltimateCMBigMissileDamageCoefficient, RollCrit(), target, launcherIndex);
            }

            launcherIndex++; // alterna lançador direito/esquerdo a cada disparo (em todos os clientes, para a animação)
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
                outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            PlayCrossfade("Gesture, Override", "BufferEmpty", "ShootGun.playbackRate", duration, 0.03f);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            // Não pode ser cortado pela primária nem por ele mesmo durante o disparo
            return InterruptPriority.PrioritySkill;
        }
    }
}

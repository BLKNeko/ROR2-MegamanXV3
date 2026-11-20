using EntityStates;
using MegamanXMod.Modules.BaseStates;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Estado de carga do Charge Collider (XFire).
    // Segura a tecla para carregar; ao soltar, passa para o ChargeColliderDash em direção ao alvo do HuntressTracker.
    public class ChargeCollider : BaseChargeSpecial
    {
        // Quantidade de hits por nível de carga
        public static int Level1Hits = 3;
        public static int Level2Hits = 5;
        public static int Level3Hits = 8;

        private HuntressTracker huntressTracker;
        private HurtBox initialTarget;

        public override void OnEnter()
        {
            base.OnEnter();

            damageCoefficient = XStaticValues.ChargeColliderDamageCoefficient;
            procCoefficient = 0.5f;
            baseDuration = 0.1f;
            firePercentTime = 0.0f;

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzzPos";

            huntressTracker = GetComponent<HuntressTracker>();

            if (huntressTracker && isAuthority)
            {
                initialTarget = huntressTracker.GetTrackingTarget();
            }
        }

        protected override void FireSimpleBullet()
        {
            StartDash(Level1Hits);
        }

        protected override void FireMediumBullet()
        {
            StartDash(Level2Hits);
        }

        protected override void FireChargedBullet()
        {
            StartDash(Level3Hits);
        }

        private void StartDash(int hitCount)
        {
            if (hasFired)
                return;

            hasFired = true;

            if (!isAuthority)
                return;

            // Pega o alvo atual ao soltar a tecla; se não houver, usa o do início da carga
            HurtBox target = huntressTracker ? huntressTracker.GetTrackingTarget() : null;
            if (!IsValidTarget(target))
                target = initialTarget;

            if (!IsValidTarget(target))
                return; // sem alvo: volta ao estado principal

            ChargeColliderDash dash = new ChargeColliderDash();
            dash.SetParameters(target, hitCount, damageCoefficient, procCoefficient);
            SetNextEntityState(dash);
        }

        private static bool IsValidTarget(HurtBox target)
        {
            return target && target.healthComponent && target.healthComponent.alive;
        }
    }
}

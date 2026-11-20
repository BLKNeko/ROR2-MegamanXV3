using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Dash do Charge Collider: X avança até o alvo, aplica vários blast attacks (ignite + shock)
    // e no último hit empurra o inimigo com mais força e recua.
    public class ChargeColliderDash : BaseSkillState
    {
        // Dash
        public static float dashSpeedCoefficient = 7f;   // multiplicador sobre moveSpeedStat
        public static float minDashSpeed = 40f;
        public static float maxDashDuration = 0.8f;
        public static float contactDistance = 2.5f;      // somado ao raio do alvo

        // Multihit
        public static float hitInterval = 0.1f;
        public static float blastRadius = 5f;
        public static float hitForce = 0f;               // força dos hits normais (baixa para não afastar o alvo)
        public static float finalHitForce = 4000f;       // força do último hit
        public static float finalHitUpForce = 1000f;

        // Recuo
        public static float recoilSpeed = 18f;
        public static float recoilUpSpeed = 10f;
        public static float recoilDuration = 0.3f;

        private enum Phase { Dash, Hits, Recoil }

        private static GameObject hitEffectPrefab;

        private HurtBox target;
        private int hitCount = 3;
        private float damageCoefficient = 1f;
        private float procCoefficient = 0.5f;

        private Phase phase;
        private float phaseTimer;
        private float hitTimer;
        private int hitsDone;
        private Vector3 dashDirection;
        private Vector3 lastTargetPosition;

        public void SetParameters(HurtBox target, int hitCount, float damageCoefficient, float procCoefficient)
        {
            this.target = target;
            this.hitCount = Mathf.Max(1, hitCount);
            this.damageCoefficient = damageCoefficient;
            this.procCoefficient = procCoefficient;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (!hitEffectPrefab)
                hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/IgniteOnKill/IgniteExplosionVFX.prefab").WaitForCompletion();

            phase = Phase.Dash;
            phaseTimer = 0f;
            hitTimer = 0f;
            hitsDone = 0;

            dashDirection = characterDirection ? characterDirection.forward : transform.forward;
            if (target)
                lastTargetPosition = target.transform.position;

            characterBody.SetAimTimer(2f);

            if (NetworkServer.active)
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            if (characterMotor)
                characterMotor.Motor.ForceUnground(0.1f);

            EffectManager.SimpleMuzzleFlash(XAssets.NovaStrikeVFX, gameObject, "NovaDashPos", false);
            AkSoundEngine.PostEvent(XStaticValues.X_Dash_SFX, gameObject);
            PlayAnimation("FullBody, Override", "NovaStrikeStart", "attackSpeed", maxDashDuration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isAuthority)
                return;

            phaseTimer += Time.fixedDeltaTime;

            switch (phase)
            {
                case Phase.Dash:
                    UpdateDash();
                    break;
                case Phase.Hits:
                    UpdateHits();
                    break;
                case Phase.Recoil:
                    UpdateRecoil();
                    break;
            }
        }

        private bool TargetAlive()
        {
            return target && target.healthComponent && target.healthComponent.alive;
        }

        private float TargetRadius()
        {
            CharacterBody body = TargetAlive() ? target.healthComponent.body : null;
            return body ? body.radius : 0f;
        }

        private void UpdateDash()
        {
            if (!TargetAlive())
            {
                outer.SetNextStateToMain();
                return;
            }

            lastTargetPosition = target.transform.position;
            Vector3 toTarget = lastTargetPosition - transform.position;
            float distance = toTarget.magnitude;

            if (distance > 0.01f)
                dashDirection = toTarget / distance;

            if (characterDirection)
                characterDirection.forward = new Vector3(dashDirection.x, 0f, dashDirection.z);

            // Chegou no alvo (ou passou do tempo máximo): começa os hits
            if (distance <= contactDistance + TargetRadius() || phaseTimer >= maxDashDuration)
            {
                EnterPhase(Phase.Hits);
                if (characterMotor)
                    characterMotor.velocity = Vector3.zero;
                return;
            }

            if (characterMotor)
            {
                characterMotor.Motor.ForceUnground(0.1f);
                float speed = Mathf.Max(minDashSpeed, moveSpeedStat * dashSpeedCoefficient);
                characterMotor.velocity = dashDirection * speed;
            }
        }

        private void UpdateHits()
        {
            // Fica parado colado no inimigo durante os hits
            if (characterMotor)
                characterMotor.velocity = Vector3.zero;

            if (TargetAlive())
                lastTargetPosition = target.transform.position;

            hitTimer -= Time.fixedDeltaTime;
            if (hitTimer > 0f)
                return;

            hitTimer = hitInterval / attackSpeedStat;
            hitsDone++;

            bool isFinalHit = hitsDone >= hitCount;
            FireHit(lastTargetPosition, isFinalHit);

            if (isFinalHit)
                EnterPhase(Phase.Recoil);
        }

        private void FireHit(Vector3 position, bool isFinalHit)
        {
            BlastAttack blast = new BlastAttack
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = GetTeam(),
                position = position,
                radius = blastRadius,
                baseDamage = damageStat * damageCoefficient,
                baseForce = hitForce,
                bonusForce = isFinalHit ? dashDirection * finalHitForce + Vector3.up * finalHitUpForce : Vector3.zero,
                crit = RollCrit(),
                procCoefficient = procCoefficient,
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageTypeCombo.GenericSpecial | DamageType.IgniteOnHit | DamageType.Shock5s,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
            };
            blast.Fire();

            if (hitEffectPrefab)
            {
                EffectManager.SpawnEffect(hitEffectPrefab, new EffectData
                {
                    origin = position,
                    scale = isFinalHit ? blastRadius * 2f : blastRadius,
                }, true);
            }

            AkSoundEngine.PostEvent(XStaticValues.X_FireWave_SFX, gameObject);
        }

        private void UpdateRecoil()
        {
            if (phaseTimer >= recoilDuration)
            {
                outer.SetNextStateToMain();
            }
        }

        private void EnterPhase(Phase newPhase)
        {
            phase = newPhase;
            phaseTimer = 0f;

            if (newPhase == Phase.Hits)
            {
                hitTimer = 0f; // primeiro hit imediato
            }
            else if (newPhase == Phase.Recoil)
            {
                PlayAnimation("FullBody, Override", "NovaStrikeEnd", "attackSpeed", recoilDuration);

                if (characterMotor)
                {
                    characterMotor.Motor.ForceUnground(0.1f);
                    Vector3 back = new Vector3(-dashDirection.x, 0f, -dashDirection.z).normalized;
                    characterMotor.velocity = back * recoilSpeed + Vector3.up * recoilUpSpeed;
                }
            }
        }

        public override void OnExit()
        {
            if (NetworkServer.active && characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility))
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);

            // Evita sair "voando" se o estado for interrompido no meio do dash
            if (characterMotor && phase == Phase.Dash)
                characterMotor.velocity *= 0.2f;

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

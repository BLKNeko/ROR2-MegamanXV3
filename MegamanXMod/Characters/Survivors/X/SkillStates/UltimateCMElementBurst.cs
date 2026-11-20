using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MegamanXMod.Survivors.X.SkillStates
{
    // Shift da Ultimate Armor CM: explosão em área multihit em volta do X.
    // Cada pulso tem um elemento (fogo -> gelo -> shock -> veneno) e o último pulso tem todos juntos.
    public class UltimateCMElementBurst : BaseSkillState
    {
        public static float startDelay = 0.2f;
        public static float pulseInterval = 0.25f;
        public static float endDelay = 0.3f;
        public static float finalRadiusMultiplier = 1.5f;
        public static float finalForce = 2500f;

        private static readonly DamageTypeCombo[] elements = new DamageTypeCombo[]
        {
            DamageTypeCombo.GenericUtility | DamageType.IgniteOnHit,
            DamageTypeCombo.GenericUtility | DamageType.Freeze2s,
            DamageTypeCombo.GenericUtility | DamageType.Shock5s,
            DamageTypeCombo.GenericUtility | DamageType.PoisonOnHit,
        };

        private static GameObject[] elementEffects;
        private static GameObject finalEffect;

        private int pulsesDone;
        private float pulseTimer;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

            LoadEffects();

            pulsesDone = 0;
            pulseTimer = startDelay;
            duration = startDelay + pulseInterval * (XStaticValues.UltimateCMBurstPulses - 1) + endDelay;

            characterBody.SetAimTimer(duration);

            // Imune durante a skill (funciona para host e cliente)
            if (isAuthority)
                characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration);

            EffectManager.SimpleMuzzleFlash(XAssets.HyperModeEffect, gameObject, "CorePosition", false);
            AkSoundEngine.PostEvent(XStaticValues.X_Charge_Shot, gameObject);
            PlayAnimation("FullBody, Override", "ULCM_Explosion_Loop", "ShootGun.playbackRate", duration);

        }

        private static void LoadEffects()
        {
            if (elementEffects != null)
                return;

            GameObject fallback = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFX");

            elementEffects = new GameObject[]
            {
                TryLoad("RoR2/Base/IgniteOnKill/IgniteExplosionVFX.prefab") ?? fallback,       // fogo
                TryLoad("RoR2/Base/EliteIce/AffixWhiteExplosion.prefab") ?? fallback,          // gelo
                TryLoad("RoR2/Base/Lightning/LightningStrikeImpact.prefab") ?? fallback,       // shock
                TryLoad("RoR2/Base/Croco/CrocoLeapExplosion.prefab") ?? fallback,              // veneno
            };

            finalEffect = TryLoad("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab") ?? fallback;
        }

        private static GameObject TryLoad(string address)
        {
            try
            {
                GameObject asset = Addressables.LoadAssetAsync<GameObject>(address).WaitForCompletion();
                return asset ? asset : null;
            }
            catch
            {
                return null;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Fica "pairando" enquanto solta a explosão
            if (characterMotor && characterMotor.velocity.y < 0f)
                characterMotor.velocity = new Vector3(characterMotor.velocity.x * 0.5f, 0f, characterMotor.velocity.z * 0.5f);

            if (!isAuthority)
                return;

            if (pulsesDone < XStaticValues.UltimateCMBurstPulses)
            {
                pulseTimer -= Time.fixedDeltaTime;
                if (pulseTimer <= 0f)
                {
                    pulseTimer = pulseInterval;
                    FirePulse(pulsesDone);
                    pulsesDone++;
                }
            }

            if (fixedAge >= duration)
                outer.SetNextStateToMain();
        }

        private void FirePulse(int index)
        {
            bool isFinal = index == XStaticValues.UltimateCMBurstPulses - 1;

            DamageTypeCombo damageType;
            GameObject effect;

            if (isFinal)
            {
                damageType = DamageTypeCombo.GenericUtility | DamageType.IgniteOnHit | DamageType.Freeze2s | DamageType.Shock5s | DamageType.PoisonOnHit;
                effect = finalEffect;
            }
            else
            {
                damageType = elements[index % elements.Length];
                effect = elementEffects[index % elementEffects.Length];
            }

            float radius = XStaticValues.UltimateCMBurstRadius * (isFinal ? finalRadiusMultiplier : 1f);
            Vector3 position = characterBody.corePosition;

            new BlastAttack
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = GetTeam(),
                position = position,
                radius = radius,
                baseDamage = damageStat * XStaticValues.UltimateCMBurstDamageCoefficient,
                baseForce = isFinal ? finalForce : 0f,
                bonusForce = isFinal ? Vector3.up * 800f : Vector3.zero,
                crit = RollCrit(),
                procCoefficient = 1f,
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = DamageColorIndex.Default,
                damageType = damageType,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
            }.Fire();

            if (effect)
            {
                EffectManager.SpawnEffect(effect, new EffectData
                {
                    origin = position,
                    scale = radius,
                }, true);
            }

            AkSoundEngine.PostEvent(XStaticValues.X_FireWave_SFX, gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();

            PlayAnimation("FullBody, Override", "BufferEmpty", "ShootGun.playbackRate", duration);


        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}

using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using RoR2.Projectile;
using MegamanXMod.Modules;
using RoR2.Stats;

namespace MegamanXMod.Survivors.X.Components
{
    public class XMeltCreeperComponent : MonoBehaviour
    {
        private ProjectileOverlapAttack overlapAttack;

        private ProjectileImpactExplosion impactExplosion;

        private ProjectileController projectileController;

        private float timer = 0f;
        private float timeLimit = 0.15f;
        private float damageCoeficient;

        private BlastAttack blastAttack;

        void Awake()
        {
            impactExplosion = GetComponent<ProjectileImpactExplosion>();
            overlapAttack = GetComponent<ProjectileOverlapAttack>();
            projectileController = GetComponent<ProjectileController>();
            damageCoeficient = XStaticValues.MeltCreeperDamageCoefficient;

            //Debug.Log("Wake");

            //EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ExplodeOnDeathVoidExplosionEffect"), new EffectData
            //{
            //    origin = gameObject.transform.position,
            //    scale = 8f,

            //}, true);

        }


        void FixedUpdate()
        {
            if(overlapAttack != null)
            {
                timer += Time.deltaTime;

                blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(projectileController.owner);
                //blastAttack.teamIndex = TeamIndex.Player;
                //blastAttack.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                blastAttack.baseDamage = damageCoeficient * projectileController.owner.GetComponent<CharacterBody>().damage;
                blastAttack.baseForce = 10f;
                blastAttack.position = gameObject.transform.position;
                blastAttack.radius = 8f;
                blastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                blastAttack.damageType = DamageType.IgniteOnHit;
                blastAttack.damageColorIndex = DamageColorIndex.Default;


                if (timer > timeLimit)
                {
                    //overlapAttack.ResetOverlapAttack();
                    blastAttack.Fire();
                    timer = 0f;
                }



            }
        }

    }

}

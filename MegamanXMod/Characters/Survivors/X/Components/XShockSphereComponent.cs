using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using RoR2.Projectile;
using MegamanXMod.Modules;

namespace MegamanXMod.Survivors.X.Components
{
    public class XShockSphereComponent : MonoBehaviour
    {
        private ProjectileOverlapAttack overlapAttack;

        private ProjectileImpactExplosion impactExplosion;

        private float timer = 0f;
        private float timeLimit = 0.2f;

        private BlastAttack blastAttack;

        void Awake()
        {
            impactExplosion = GetComponent<ProjectileImpactExplosion>();
            overlapAttack = GetComponent<ProjectileOverlapAttack>();

            Debug.Log("Wake");

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
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                blastAttack.baseDamage = 1;
                blastAttack.baseForce = 10f;
                blastAttack.position = gameObject.transform.position;
                blastAttack.radius = 5f;
                blastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                blastAttack.damageType = DamageType.Shock5s;
                blastAttack.damageColorIndex = DamageColorIndex.Luminous;


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

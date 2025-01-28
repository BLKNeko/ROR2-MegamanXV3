using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;

namespace MegamanXMod.Survivors.X.Components
{

    internal class XFootstepComponent : MonoBehaviour
    {

        private CharacterMotor characterMotor;

        void Awake()
        {
            characterMotor = GetComponent<CharacterMotor>();
            if (characterMotor != null)
            {
                // Sobrescreve o evento de footstep
                //characterMotor.onFootstep += PlayCustomFootstep;
                //RoR2.FootstepHandler += PlayCustomFootstep;
            }
        }

        void OnDestroy()
        {
            if (characterMotor != null)
            {
                //characterMotor.onFootstep -= PlayCustomFootstep;
            }
        }

        private void PlayCustomFootstep(CharacterMotor motor)
        {
            // Substitua "Play_footstep_grass" pelo evento FMOD correto
            Util.PlaySound("Play_footstep_grass", motor.gameObject);
        }
    }
}
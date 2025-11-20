using UnityEngine;

namespace MegamanXMod.Survivors.X.Components
{
    public class DestroyGhost : MonoBehaviour
    {
        private float duration;
        private float timer;
        private Material matInstance;

        public void Initialize(float duration)
        {
            this.duration = duration;
            var renderer = GetComponent<MeshRenderer>();
            if (renderer)
            {
                matInstance = new Material(renderer.material);
                renderer.material = matInstance;
            }
        }

        void Update()
        {
            if (!matInstance) return;

            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);

            if (matInstance.HasProperty("_Color"))
            {
                Color col = matInstance.color;
                col.a = alpha;
                matInstance.color = col;
            }

            if (timer >= duration)
                Destroy(gameObject);
        }
    }

}
using UnityEngine;

namespace TEGOF.Core
{
    public class AutoDestroyVFX : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 2f;

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }
    }
}

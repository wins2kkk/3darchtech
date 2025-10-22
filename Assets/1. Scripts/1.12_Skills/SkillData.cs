using UnityEngine;

namespace TEGOF.Core
{
    [CreateAssetMenu(fileName = "NewSkillData", menuName = "TEGOF/Skill Data")]
    public class SkillData : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string _id;
        [SerializeField] private float _damage = 50f;
        [SerializeField] private float _speed = 10f;

        [Header("Prefabs")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private GameObject _impactVfxPrefab;
        [SerializeField] private GameObject _castVfxPrefab;

        public string Id => _id;
        public float Damage => _damage;
        public float Speed => _speed;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public GameObject ImpactVfxPrefab => _impactVfxPrefab;
        public GameObject CastVfxPrefab => _castVfxPrefab;
    }
}

using UnityEngine;

namespace TEGOF.Core
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int k_Attack = Animator.StringToHash("Attack");
        private static readonly int k_Hit = Animator.StringToHash("Hit");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        #region Public Methods

        public void PlayAttack()
        {
            _animator.SetTrigger(k_Attack);
        }

        public void PlayHit()
        {
            _animator.SetTrigger(k_Hit);
        }

        #endregion
    }
}

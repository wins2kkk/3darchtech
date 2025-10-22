using UnityEngine;
using UnityEngine.UI;
using TEGOF.Logic;
using TEGOF.Core;

namespace TEGOF.UI
{
    public class UIAttackButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button _attackButton;
        [SerializeField] private OrbCombatController _combatController;
        [SerializeField] private SkillData _skillData;
        [SerializeField] private Transform _enemyTarget;

        private void Start()
        {
            if (_attackButton == null)
                _attackButton = GetComponent<Button>();

            if (_attackButton != null)
                _attackButton.onClick.AddListener(OnAttackPressed);
        }
            
        private void OnAttackPressed()
        {
            if (_combatController == null)
            {
                return;
            }

            if (_skillData == null)
            {
                return;
            }

            if (_enemyTarget == null)
            {
                return;
            }

            // 🔹 Gán Skill + Target trước khi Attack
            _combatController.SetSkill(_skillData);
            _combatController.SetTarget(_enemyTarget.gameObject);

            // 🔹 Gọi animation Attack (sau đó gọi event → SpawnProjectile)
            _combatController.OnAttackButtonPressed();
        }
    }
}

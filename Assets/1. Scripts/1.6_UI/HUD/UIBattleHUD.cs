using UnityEngine;
using UnityEngine.UI;

namespace TEGOF.UI
{
    public class UIBattleHUD : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField] private Image _playerAvatar;
        [SerializeField] private Slider _playerHpBar;

        [Header("Enemy Info")]
        [SerializeField] private Image _enemyAvatar;
        [SerializeField] private Slider _enemyHpBar;

        [Header("Energy Points")]
        [SerializeField] private Slider _epBar;

        [Header("Skill Buttons")]
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _defenseButton;
        [SerializeField] private Button _buffButton;
        [SerializeField] private Button _ultimateButton;
        [SerializeField] private Button _defenseQuickButton;

        private void Start()
        {
    
        }

        // Gọi các hàm này từ Player/Enemy để cập nhật UI
        public void UpdatePlayerHp(float value) => _playerHpBar.value = value;
        public void UpdateEnemyHp(float value) => _enemyHpBar.value = value;
        public void UpdateEp(float value) => _epBar.value = value;
    }
}

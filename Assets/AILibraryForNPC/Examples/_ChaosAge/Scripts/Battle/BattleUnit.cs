using ChaosAge.AI.battle;
using ChaosAge.data;
using UnityEngine;
using UnityEngine.AI;

namespace ChaosAge.Battle
{
    public class BattleUnit : MonoBehaviour
    {
        public EUnitType Type
        {
            get => unitConfig.type;
        }

        public UnitConfigSO unitConfig = null;
        private VisualUnit _visualUnit;
        public float Health => _health;
        private float _health;

        private void Awake()
        {
            _visualUnit = GetComponent<VisualUnit>();
        }

        public void Initialize(Vector2 cell) // todo
        {
            _health = unitConfig.health;
            _visualUnit.SetHealth((int)Health, unitConfig.health);


            //Tránh lỗi khi đặt vị trí ban đầu
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = AIBattleManager.Instance.GetWorldPosition(cell);
            GetComponent<NavMeshAgent>().enabled = true;
            // Debug.LogWarning($"Initialize {transform.position} - cell: {cell}");
        }

        public void TakeDamage(float damage)
        {
            if (_health <= 0) return;

            _health -= damage;
            if (_health < 0) _health = 0;
            _visualUnit.SetHealth((int)_health, unitConfig.health);

            if (_health <= 0)
            {
                AIBattleManager.Instance.RemoveUnit(this);
            }

        }

        public void AddHealth(float hp)
        {
            _health += hp;
            _health = Mathf.Min(_health, 100);
            _visualUnit.SetHealth((int)_health, unitConfig.health);
        }
    }
}

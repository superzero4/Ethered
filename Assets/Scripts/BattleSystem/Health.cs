namespace BattleSystem
{
    public interface IHealth
    {
        int CurrentHealth { get; protected set; }
        int MaxHealth { get; }
        protected void TakeDamageUncapped(int damage);

        public void TakeDamage(int damage)
        {
            TakeDamageUncapped(damage);
            if (CurrentHealth <= 0)
                CurrentHealth = 0;
            else if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        public void Heal(int heal)
        {
            TakeDamage(-heal);
        }
    }
}
namespace BattleSystem
{
    public interface IHealth
    {
        int CurrentHealth { get; protected set; }
        public bool Alive => CurrentHealth > 0;
        int MaxHealth { get; }
        protected void TakeDamageUncapped(int damage, IBattleElement source);

        public void TakeDamage(int damage, IBattleElement source)
        {
            TakeDamageUncapped(damage, source);
            if (CurrentHealth <= 0)
                CurrentHealth = 0;
            else if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        public void Heal(int heal, IBattleElement source)
        {
            TakeDamage(-heal, source);
        }
    }
}
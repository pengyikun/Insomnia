

public interface IStatusEffectable
{
    public void ApplyEffect(StatusEffectData effectData);
    public void RemoveEffect(StatusEffect effect);
    public void HandleEffect(StatusEffect effect);
}

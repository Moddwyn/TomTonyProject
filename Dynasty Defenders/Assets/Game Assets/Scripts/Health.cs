using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Min(0)] public int maxHealth = 1000;
    [Min(0)] public int currentHealth = 1000;
    public TMP_Text healthText;

    [HorizontalLine]
    [ReadOnly] public bool dead;
    public UnityEvent OnDie;

    public static Health Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0) currentHealth = 0;
        if(currentHealth <= 0 && !dead)
        {
            dead = true;
            
            OnDie?.Invoke();
        }

        UpdateHealthText();
    }

    void UpdateHealthText() => healthText.text = currentHealth + "";
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnPlayerAttack(float damage);
    public static event OnPlayerAttack OnAttack;

    public delegate void OnEnemyAttack(float damage);
    public static event OnEnemyAttack OnAIAttack;

    public delegate void OnPlayerDie();
    public static event OnPlayerDie OnDeath;

    public delegate void OnPlayerWin();
    public static event OnPlayerWin OnWin;

    public delegate void OnAIKill();
    public static event OnAIKill Enemykill;

    public static void triggerAttack(float damage)
    {
        OnAttack(damage);
    }
    public static void TriggerEnemyAttack(float damage)
    {
        OnAIAttack(damage);
    }

    public static void TriggerDeath()
    {
        OnDeath();
    }

    public static void TriggerWin()
    {
        OnWin();
    }

    public static void TriggerKill()
    {
        Enemykill();
    }
}

using UnityEngine;
using System;
public class EnemyAttributes : MonoBehaviour
{
    public event Action <int> OnEnemyHealthChange;

    private int _EnemyHealth;
    public int EnemyHealth
    {
        get => _EnemyHealth;
        set 
        {
            if (_EnemyHealth != value)
            {
                _EnemyHealth =value;
                OnEnemyHealthChange?.Invoke(_EnemyHealth);
            }
        }
    }

    public event Action <float> OnEnemyMoveChange;

    private float _EnemyMove;
    public float EnemyMove
    {
        get => _EnemyMove;
        set 
        {
            if (_EnemyMove != value)
            {
                _EnemyMove =value;
                OnEnemyMoveChange?.Invoke(_EnemyMove);
            }
        }
    }

    
}

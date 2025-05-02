using UnityEngine;
using System;

public class PlayerAttributes : MonoBehaviour
{

    void Awake()
    {
        playerhealth = 4;
        playermovespeed = 4f;
    }

    public event Action <int> OnPlayerHealthChange;
    private int _playerHealth; //backing field that stores actual vale of player health (hidden)
    public int playerhealth
    {
        get { return _playerHealth;}
        
        set 
        {
            // prevents unnecessary changes
            if (_playerHealth != value)
            {
                _playerHealth = value; //updates private field
                OnPlayerHealthChange?.Invoke(_playerHealth); //notifies subscribes of change in health
            }
        }
    }

    public event Action <float> OnPlayerMovespeedChange;

    private float _playermovespeed;
    public float playermovespeed
    {
        get => _playermovespeed;
        set 
        {
            if (_playermovespeed != value)
            {
                _playermovespeed = value;
                OnPlayerMovespeedChange?.Invoke(_playermovespeed);
            }
        }
    }

    private event Action <float> OnPlayerHeightChange;

    private float _playerheight;
    public float playerheight
    {
        get{return _playerheight;}
        set
        {
            if (_playerheight != value)
            {
                _playerheight = value;
                OnPlayerHeightChange?.Invoke(_playerheight);
            }
        }
    }
    
}


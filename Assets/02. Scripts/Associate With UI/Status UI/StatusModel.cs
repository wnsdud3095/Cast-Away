using UnityEngine;
using System;

public class StatusModel 
{
    private PlayerStatus m_player_status;

    public Action<float,float> OnUpdatedHP
    {
        get => m_player_status.OnUpdatedHP;
        set => m_player_status.OnUpdatedHP = value;
    }

    public Action<float, float> OnUpdatedThirst
    {
        get => m_player_status.OnUpdatedThirst;
        set => m_player_status.OnUpdatedThirst = value;
    }

    public Action<float, float> OnUpdatedHunger
    {
        get => m_player_status.OnUpdatedHunger;
        set => m_player_status.OnUpdatedHunger = value;
    }

    public StatusModel(PlayerStatus player_status)
    {
        m_player_status = player_status;
    }
    // PlayerStatus를 초기화한다.
    public void Initialize()
    {
        m_player_status.Initialize();
    }
}

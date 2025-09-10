using UnityEngine;

[CreateAssetMenu(fileName = "TimeSettings", menuName = "SO/Settings/Time Setting")]
public class TimeSettings : ScriptableObject
{
    [Header("시간 배율")]
    [SerializeField] private float m_time_multiplier = 2000f;
    public float Multiplier => m_time_multiplier;

    [Header("시작 시간")]
    [SerializeField] private float m_start_hour = 12f;
    public float StartHour => m_start_hour;

    [Header("일출 시간")]
    [SerializeField] private float m_sunrise_hour = 6f;
    public float SunriseHour => m_sunrise_hour;

    [Header("일몰 시간")]
    [SerializeField] private float m_sunset_hour = 18f;
    public float SunsetHour => m_sunset_hour;
}
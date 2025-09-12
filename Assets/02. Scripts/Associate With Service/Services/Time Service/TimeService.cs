using System;
using System.Data;
using UnityEngine;

public class TimeService
{
    private readonly TimeSettings m_settings;

    private DateTime m_current_time;
    private readonly TimeSpan m_sunrise_time;
    private readonly TimeSpan m_sunset_time;

    public event Action OnSunrise;
    public event Action OnSunset;
    public event Action OnHourChanged;

    private readonly Observable<bool> m_is_day_time;
    private readonly Observable<int> m_current_hour;

    public DateTime CurrentTime => m_current_time;

    public TimeService(TimeSettings settings)
    {
        m_settings = settings;

        m_current_time = DateTime.Now.Date + TimeSpan.FromHours(m_settings.StartHour);
        m_sunrise_time = TimeSpan.FromHours(m_settings.SunriseHour);
        m_sunset_time = TimeSpan.FromHours(m_settings.SunsetHour);

        m_is_day_time = new Observable<bool>(IsDayTime());
        m_current_hour = new Observable<int>(m_current_time.Hour);

        m_is_day_time.ValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
        m_current_hour.ValueChanged += _ => OnHourChanged?.Invoke();
    }

    public void UpdateTime(float delta_time)
    {
        m_current_time = m_current_time.AddSeconds(delta_time * m_settings.Multiplier);
        m_is_day_time.Value = IsDayTime();
        m_current_hour.Value = m_current_time.Hour;
    }

    public float CalculateSunAngle()
    {
        var is_day = IsDayTime();
        var start_degree = is_day ? 0f : 180f;

        var start = is_day ? m_sunrise_time : m_sunset_time;
        var end = is_day ? m_sunset_time : m_sunrise_time;

        var total_time = GetTimeDiff(start, end);
        var elapsed_time = GetTimeDiff(start, m_current_time.TimeOfDay);

        var progress = elapsed_time.TotalMinutes / total_time.TotalMinutes;

        return Mathf.Lerp(start_degree, start_degree + 180f, (float)progress);
    }

    public bool IsDayTime() => m_sunrise_time  < m_current_time.TimeOfDay && m_current_time.TimeOfDay < m_sunset_time;

    private TimeSpan GetTimeDiff(TimeSpan from, TimeSpan to)
    {
        var diff = to - from;
        return diff.TotalHours < 0 ? diff + TimeSpan.FromHours(24) : diff;
    }
}

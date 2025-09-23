using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    [Header("시간 텍스트")]
    [SerializeField] private TMP_Text m_time_text;

    [Header("시간 설정")]
    [SerializeField] private TimeSettings m_time_settings;

    [Space(40f)]
    [Header("광원 관련 컴포넌트")]
    [Header("태양 광원")]
    [SerializeField] private Light m_sun;

    [Header("달 광원")]
    [SerializeField] private Light m_moon;

    [Space(20f)]
    [Header("태양 광원 최대 강도")]
    [Range(0f, 1f)][SerializeField] private float m_max_sun_intensity = 1f;

    [Header("달 광원 최대 강도")]
    [Range(0f, 0.5f)][SerializeField] private float m_max_moon_intensity = 0.5f;

    [Header("광원 강도 곡선")]
    [SerializeField] AnimationCurve m_light_intensity_curve;

    [Space(20f)]
    [Header("안개 설정")]
    [SerializeField] private bool m_enable_fog = true;
    [Header("안개 농도 (주/야)")]
    [Range(0f, 0.2f)][SerializeField] private float m_day_fog_density = 0.0025f;
    [Range(0f, 0.2f)][SerializeField] private float m_night_fog_density = 0.02f;

    [Header("안개 색상 (주/야)")]
    [SerializeField] private Color m_day_fog_color = new Color(0.78f, 0.88f, 0.98f, 1f);
    [SerializeField] private Color m_night_fog_color = new Color(0.05f, 0.08f, 0.12f, 1f);
    [Header("안개 전환 곡선")]
    [SerializeField] private AnimationCurve m_fog_blend_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Space(20f)]    
    [Header("전처리 볼륨")]
    [SerializeField] private Volume m_volume;
    [Header("전처리 낮 광원 색상")]
    [SerializeField] private Color m_day_ambient_light;

    [Header("전처리 밤 광원 색상")]
    [SerializeField] private Color m_night_ambient_light;

    ColorAdjustments m_color_adjustments;

    [Space(20f)]
    [SerializeField] private Material m_skybox_material;

    private TimeService m_service;

    public event Action OnSunrise {
        add => m_service.OnSunrise += value;
        remove => m_service.OnSunrise -= value;
    }
    
    public event Action OnSunset {
        add => m_service.OnSunset += value;
        remove => m_service.OnSunset -= value;
    }
    
    public event Action OnHourChanged {
        add => m_service.OnHourChanged += value;
        remove => m_service.OnHourChanged -= value;
    }   

    public bool IsDayTime => m_service.IsDayTime();
    
    private void Awake()
    {
        m_service = new TimeService(m_time_settings);
    }

    private void Start()
    {
        if(m_volume != null && m_volume.profile != null)
        {
            m_volume.profile.TryGet(out m_color_adjustments);
        }

        RenderSettings.fog = m_enable_fog;
        RenderSettings.fogMode = FogMode.ExponentialSquared;

        UpdateFogImmediate();
    }

    private void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        UpdateSkyBlend();
        UpdateFog();
    }

    private void UpdateSkyBlend()
    {
        var dot_product = Vector3.Dot(m_sun.transform.forward, Vector3.up);
        var blend = Mathf.Lerp(0f, 1f, m_light_intensity_curve.Evaluate(dot_product));
        m_skybox_material.SetFloat("_Blend", blend);
    }

    private void UpdateLightSettings()
    {
        var dot_product = Vector3.Dot(m_sun.transform.forward, Vector3.down);

        m_sun.intensity = Mathf.Lerp(0f, m_max_sun_intensity, m_light_intensity_curve.Evaluate(dot_product));
        m_moon.intensity = Mathf.Lerp(m_max_moon_intensity, 0f, m_light_intensity_curve.Evaluate(dot_product));

        if(m_color_adjustments == null)
        {
            return;
        }
        m_color_adjustments.colorFilter.value = Color.Lerp(m_night_ambient_light, m_day_ambient_light, m_light_intensity_curve.Evaluate(dot_product));
    }

    private void UpdateFogImmediate()
    {
        if(!m_enable_fog || m_sun == null)
        {
            return;
        }

        var dot_product = Vector3.Dot(m_sun.transform.forward, Vector3.down);
        var t = Mathf.Clamp01(m_fog_blend_curve.Evaluate(dot_product));
        RenderSettings.fogColor = Color.Lerp(m_night_fog_color, m_day_fog_color, t);
        RenderSettings.fogDensity = Mathf.Lerp(m_night_fog_density, m_day_fog_density, t);
    }

    private void UpdateFog()
    {
        if(!m_enable_fog || m_sun == null)
        {
            return;
        }

        var dot_product = Vector3.Dot(m_sun.transform.forward, Vector3.down);
        var t = Mathf.Clamp01(m_fog_blend_curve.Evaluate(dot_product));
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.Lerp(m_night_fog_color, m_day_fog_color, t), Time.deltaTime * 2f);
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, Mathf.Lerp(m_night_fog_density, m_day_fog_density, t), Time.deltaTime * 2f);
    }

    private void RotateSun()
    {
        var rotation = m_service.CalculateSunAngle();
        m_sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
    }

    private void UpdateTimeOfDay()
    {
        m_service.UpdateTime(Time.deltaTime);

        if(m_time_text != null)
        {
            m_time_text.text = m_service.CurrentTime.ToString(format:"hh:mm");
        }
    }
}

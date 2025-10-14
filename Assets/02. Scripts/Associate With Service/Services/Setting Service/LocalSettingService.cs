using System.IO;
using UnityEngine;

public class LocalSettingService : ISettingService
{
    private readonly string m_local_data_path;
    private SettingData m_setting_data;

    public float MouseSensitivity
    {
        get => m_setting_data.MouseSensitivity;
        set => m_setting_data.MouseSensitivity = value;
    }

    public bool MouseInversion
    {
        get => m_setting_data.MouseInversion;
        set => m_setting_data.MouseInversion = value;
    }

    public bool CameraShaking
    {
        get => m_setting_data.CameraShaking;
        set => m_setting_data.CameraShaking = value;
    }

    public bool BGMPrint
    {
        get => m_setting_data.BGMPrint;
        set => m_setting_data.BGMPrint = value;
    }

    public float BGMRate
    {
        get => m_setting_data.BGMRate;
        set => m_setting_data.BGMRate = value;
    }

    public bool SFXPrint
    {
        get => m_setting_data.SFXPrint;
        set => m_setting_data.SFXPrint = value;
    }
    public float SFXRate
    {
        get => m_setting_data.SFXRate;
        set => m_setting_data.SFXRate = value;
    }

    public LocalSettingService()
    {
        m_local_data_path = Path.Combine(Application.persistentDataPath, "SettingData.json");

        Load();
    }

    public bool Load()
    {
        if(File.Exists(m_local_data_path))
        {
            var json_data = File.ReadAllText(m_local_data_path);
            var setting_data = JsonUtility.FromJson<SettingData>(json_data);

            if(setting_data == null)
            {
                return false;
            }
        }
        else
        {
            m_setting_data = new SettingData();
            Save();
        }

        return true;
    }

    public void Save()
    {
        var json_data = JsonUtility.ToJson(m_setting_data, true);
        File.WriteAllText(m_local_data_path, json_data);
    }
}

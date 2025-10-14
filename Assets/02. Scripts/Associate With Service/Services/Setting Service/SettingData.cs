[System.Serializable]
public class SettingData
{
    public float MouseSensitivity;
    public bool MouseInversion;
    public bool CameraShaking;

    public bool BGMPrint;
    public float BGMRate;
    public bool SFXPrint;
    public float SFXRate;

    public SettingData()
    {
        MouseSensitivity = 1f;
        MouseInversion = true;
        CameraShaking = true;

        BGMPrint = true;
        BGMRate = 0.25f;
        SFXPrint = true;
        SFXRate = 0.5f;
    }

    public SettingData(SettingData setting_data)
    {
        MouseSensitivity = setting_data.MouseSensitivity;
        MouseInversion = setting_data.MouseInversion;
        CameraShaking = setting_data.CameraShaking;

        BGMPrint = setting_data.BGMPrint;
        BGMRate = setting_data.BGMRate;
        SFXPrint = setting_data.SFXPrint;
        SFXRate = setting_data.SFXRate;
    }
}
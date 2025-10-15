namespace SettingService
{
    public interface ISettingService : ISaveable
    {
        SettingData Data { get; }

        float MouseSensitivity { get; set; }
        bool MouseInversion { get; set; }
        bool CameraShaking { get; set;}

        bool BGMPrint { get; set; }
        float BGMRate { get; set; }
        bool SFXPrint { get; set; }
        float SFXRate { get; set; }
    }
}
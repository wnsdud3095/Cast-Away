public interface ISettingService : ISaveable
{
    public float MouseSensitivity { get; set; }
    public bool MouseInversion { get; set; }
    public bool CameraShaking { get; set;}

    public bool BGMPrint { get; set; }
    public float BGMRate { get; set; }
    public bool SFXPrint { get; set; }
    public float SFXRate { get; set; }
}
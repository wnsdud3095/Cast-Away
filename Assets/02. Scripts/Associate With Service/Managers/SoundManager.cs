using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    private AudioSource m_bgm_source;

    [SerializeField] private SoundData[] m_bgm_clips;
    [SerializeField] private SoundData[] m_sfx_clips;

    private Dictionary<string, SoundData> m_bgm_dict;
    private Dictionary<string, SoundData> m_sfx_dict;

    private Dictionary<string, int> m_bgm_channel_dict;
    private Dictionary<string, int> m_sfx_channel_dict;

    private string m_last_bgm_key;

    public AudioSource BGM => m_bgm_source;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        m_bgm_source = GetComponent<AudioSource>();

        m_bgm_dict = new();
        foreach (var bgm_data in m_bgm_clips)
        {
            m_bgm_dict.Add(bgm_data.Clip.name, bgm_data);
        }

        m_sfx_dict = new();
        foreach (var sfx_data in m_sfx_clips)
        {
            m_sfx_dict.Add(sfx_data.Clip.name, sfx_data);
        }

        m_bgm_channel_dict = new();
        m_sfx_channel_dict = new();
    }

    #region BGM
    public void PlayBGM(string bgm_name)
    {
        if (m_last_bgm_key == bgm_name)
        {
            UnityEngine.Debug.Log($"BGM: {bgm_name}이 여기에 빠짐");
            return;
        }

        UnityEngine.Debug.Log($"BGM: {bgm_name}이 정상적 실행됨");

        StartCoroutine(Co_ChangeBGM(bgm_name));
    }

    public void PlayRandBGMLoop()
    {
        StartCoroutine(Co_RandBGMLoop());
    }

    private IEnumerator Co_ChangeBGM(string bgm_name)
    {
        if (m_bgm_dict.TryGetValue(bgm_name, out var bgm_data))
        {
            m_last_bgm_key = bgm_name;
            
            if (BGM.isPlaying)
            {
                if (BGM.clip)
                {
                    if (!string.IsNullOrEmpty(m_last_bgm_key) && m_bgm_channel_dict.ContainsKey(m_last_bgm_key))
                    {
                        m_bgm_channel_dict[m_last_bgm_key] = Mathf.Max(0, m_bgm_channel_dict[m_last_bgm_key] - 1);
                    }

                    m_last_bgm_key = bgm_name;
                }

                yield return StartCoroutine(Co_Fade(BGM, true));
                yield return new WaitForSeconds(0.3f);
            }

            if (m_bgm_channel_dict.TryGetValue(bgm_name, out var channel))
            {
                if (channel < bgm_data.Channel)
                {
                    m_bgm_channel_dict[bgm_name]++;

                    BGM.clip = bgm_data.Clip;
                    BGM.Play();

                    yield return StartCoroutine(Co_Fade(BGM, false));
                }
            }
            else
            {
                m_bgm_channel_dict[bgm_name] = 1;

                BGM.clip = bgm_data.Clip;
                BGM.Play();

                yield return StartCoroutine(Co_Fade(BGM, false));
            }
        }
        else
        {
            yield break;
        }
    }

    private IEnumerator Co_RandBGMLoop()
    {
        while(true)
        {
            var rand_bgm = m_bgm_clips[Random.Range(0, m_bgm_clips.Length)];
            var bgm_name = rand_bgm.Clip.name;

            if(bgm_name == m_last_bgm_key)
            {
                continue;
            }

            if(m_bgm_channel_dict.TryGetValue(bgm_name, out var channel))
            {
                if(channel < rand_bgm.Channel)
                {
                    m_bgm_channel_dict[bgm_name]++;
                }
            }
            else
            {
                m_bgm_channel_dict[bgm_name] = 1;
            }

            m_last_bgm_key = bgm_name;
            BGM.clip = rand_bgm.Clip;
            BGM.Play();

            yield return new WaitForSeconds(BGM.clip.length);
        }
    }

    private IEnumerator Co_Fade(AudioSource bgm_source, bool is_out)
    {
        var elapsed_time = 0f;
        var target_time = 0.4f;

            while (elapsed_time <= target_time)
            {
                var delta = elapsed_time / target_time;
                bgm_source.volume = is_out ? Mathf.Lerp(0.5f, 0f, delta) : Mathf.Lerp(0f, 0.5f, delta);

                elapsed_time += Time.deltaTime;
                yield return null;
            }

            bgm_source.volume = is_out ? 0f : 0.5f;
    }
    #endregion BGM

    #region SFX
    public void PlaySFX(string sfx_name, bool is_spatial, Vector3 positon)
    {        
        if (m_sfx_dict.TryGetValue(sfx_name, out var sfx_data))
        {
            if (m_sfx_channel_dict.TryGetValue(sfx_name, out var channel))
            {
                if (channel >= sfx_data.Channel)
                {
                    return;
                }
                else
                {
                    m_sfx_channel_dict[sfx_name]++;
                }
            }
            else
            {
                m_sfx_channel_dict[sfx_name] = 1;
            }

            var sfx_obj = ObjectManager.Instance.GetObject(ObjectType.SFX);
            sfx_obj.transform.position = positon;

            var sfx_source = sfx_obj.GetComponent<AudioSource>();
            sfx_source.spatialBlend = is_spatial ? 1f : 0f;

            sfx_source.clip = sfx_data.Clip;
            sfx_source.volume = 0.5f;
            sfx_source.Play();

            StartCoroutine(ReturnSFX(sfx_name, sfx_source));
        }
    }

    private IEnumerator ReturnSFX(string sfx_name, AudioSource sfx_source)
    {
        while (sfx_source.isPlaying)
        {
            yield return null;
        }

        m_sfx_channel_dict[sfx_name]--;
        ObjectManager.Instance.ReturnObject(sfx_source.gameObject, ObjectType.SFX);
    }
    #endregion SFX
}
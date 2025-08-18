/*
📝 Hướng dẫn thêm âm thanh mới:

1️ Thêm enum mới vào SFXType hoặc MusicType
    → ví dụ: SFXType.Jump, MusicType.Boss

2️ Thêm AudioClip mới vào AudioManager
    → [SerializeField] private AudioClip jumpSound;

3️ Kéo file âm thanh vào Inspector (tương ứng với biến vừa thêm)

4️ Thêm vào Dictionary trong InitClipDictionaries()
    → sfxClips.Add(SFXType.Jump, jumpSound);

5️ Gọi từ bất kỳ đâu:
    → AudioManager.Instance.PlaySFX(SFXType.Jump);
    → AudioManager.Instance.PlayMusic(MusicType.Boss);
    A
✅ Hoàn tất!  

Trạng thái xử lý & Log  ✅ ❌ 🔄 ⚠️ ☑️ ⏳

Hiệu suất & Tối ưu      ⚡ 🐌 🔥 🧠 🧹

Game logic / UI         🗺️ 🚪 👾 🐉 🎁 💰 ❤️ 💙 💥 🛡️ ⬆️ 📈 ☠️ 🔁

Kỹ thuật & Debug        🧪 📍 🚧 🛠️ 🌀 📝 📜

*/

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SFXType { Click, Win, Lose }
public enum MusicType { Menu, InGame }

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;       // 🔊 Hiệu ứng
    [SerializeField] private AudioSource musicSource;     // 🎵 Nhạc nền

    [Header("Audio Clips - SFX")]
    [SerializeField] private AudioClip clickSound;        // 🖱️ Click
    [SerializeField] private AudioClip winSound;          // 🏆 Thắng
    [SerializeField] private AudioClip loseSound;         // 💀 Thua

    [Header("Audio Clips - Music")]
    [SerializeField] private AudioClip menuMusic;         // 🏠 Menu
    [SerializeField] private AudioClip inGameMusic;       // 🎮 In-game

    // 📦 Dictionary lưu clip
    private Dictionary<SFXType, AudioClip> sfxClips;
    private Dictionary<MusicType, AudioClip> musicClips;

    // ⚙️ Settings
    public bool IsMusicEnabled { get; set; } = true;      // 🎵 Bật nhạc
    public bool IsSFXEnabled { get; set; } = true;        // 🔊 Bật hiệu ứng

    private void Awake()
    {
        InitClipDictionaries(); // 🚀 Khởi tạo clip
    }

    private void InitClipDictionaries()
    {
        sfxClips = new Dictionary<SFXType, AudioClip>
        {
            { SFXType.Click, clickSound },
            { SFXType.Win, winSound },
            { SFXType.Lose, loseSound }
        };

        musicClips = new Dictionary<MusicType, AudioClip>
        {
            { MusicType.Menu, menuMusic },
            { MusicType.InGame, inGameMusic }
        };
    }

    // ========== 🔊 SFX ==========

    /// ▶️ Phát SFX theo loại
    public void PlaySFX(SFXType type, float pitch = 1f)
    {
        if (!IsSFXEnabled || !sfxClips.TryGetValue(type, out var clip) || clip == null) return;
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip);
    }

    /// ▶️ Phát SFX từ clip cụ thể
    public void PlaySFX(AudioClip clip, float pitch = 1f)
    {
        if (!IsSFXEnabled || clip == null) return;
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip);
    }

    // ========== 🎵 MUSIC ==========

    /// 🎶 Phát nhạc theo loại
    public void PlayMusic(MusicType type, float fadeDuration = 1f)
    {
        if (!IsMusicEnabled || !musicClips.TryGetValue(type, out var clip) || clip == null) return;
        CrossfadeMusic(clip, fadeDuration);
    }

    /// 🎶 Phát nhạc từ clip
    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        if (!IsMusicEnabled || clip == null) return;
        CrossfadeMusic(clip, fadeDuration);
    }

    /// ⏹️ Dừng nhạc
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// 🔄 Chuyển tiếp mượt giữa nhạc cũ và mới
    private void CrossfadeMusic(AudioClip newClip, float duration)
    {
        if (musicSource.isPlaying)
        {
            musicSource.DOFade(0f, duration).OnComplete(() =>
            {
                musicSource.clip = newClip;
                musicSource.Play();
                musicSource.DOFade(1f, duration); // 🔊 Tăng dần
            });
        }
        else
        {
            musicSource.clip = newClip;
            musicSource.volume = 0f;
            musicSource.Play();
            musicSource.DOFade(1f, duration);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("🔊 Preview Click")]
    private void PreviewClick() => PlaySFX(SFXType.Click);

    [ContextMenu("🎵 Preview Menu Music")]
    private void PreviewMenuMusic() => PlayMusic(MusicType.Menu);
#endif
}

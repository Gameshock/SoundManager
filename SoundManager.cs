using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Ins { get; private set; }

    public AudioSource BgmAudioSource { get => bgmAudioSource; }
    public AudioSource SeAudioSource { get => seAudioSource; }

    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] List<BGMSoundData> bgmSoundDates;
    [SerializeField] List<SESoundData> seSoundDates;


    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;
    private float pauseTime = 0f;



    [System.Serializable]
    public class BGMSoundData
    {
        public enum BGM
        {
            // これがラベルになる
            Title,
            MainGame,
        }

        public BGM bgm;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1;
    }

    [System.Serializable]
    public class SESoundData
    {
        public enum SE
        {
            // これがラベルになる
            Decision, Cansel, 
            OpenFinishWindow,
            BoxOpen, BoxHover,
            ItemPickup, ItemFailed,
            InDustBox,
            OpenDoor, CloseDoor,
            OpenManual, CloseManual, PegerManual,
            LimitMessage,
            ResultMainSE, ResultSubSE,
        }

        public SE se;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1;
    }

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    #region BGM
    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        if (BgmAudioSource == null) return;

        BGMSoundData data = bgmSoundDates.Find(data => data.bgm == bgm);
        BgmAudioSource.clip = data.audioClip;
        BgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        BgmAudioSource.Play();
    }

    public void PlayBGMFadeIn(BGMSoundData.BGM bgm, float fadeInTime)
    {
        if (BgmAudioSource == null) return;

        BGMSoundData data = bgmSoundDates.Find(data => data.bgm == bgm);
        BgmAudioSource.clip = data.audioClip;
        BgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        BgmAudioSource.DOFade(BgmAudioSource.volume, fadeInTime);
        BgmAudioSource.Play();
    }

    public void PlayBGMNumberFadeIn(int index, float fadeInTime)
    {
        // インデックスがリストの範囲内かどうかを確認
        if (index >= 0 && index < bgmSoundDates.Count)
        {
            // 指定インデックスの要素を取得
            BGMSoundData data = bgmSoundDates[index];

            // BGMの設定と再生
            BgmAudioSource.clip = data.audioClip;
            BgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
            BgmAudioSource.DOFade(BgmAudioSource.volume, fadeInTime); // フェードイン
            BgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("指定されたインデックスがリストの範囲外です");
        }

    }

    public void PlayBGMFadeOut(float fadeInTime)
    {
        if (BgmAudioSource == null) return;

        BgmAudioSource.DOFade(0, fadeInTime).OnComplete(() => StopBGM());
    }

    public void StopBGM()
    {
        if (BgmAudioSource == null) return;

        BgmAudioSource.Stop();
        pauseTime = 0f;
    }

    public void StartBGM()
    {
        if (BgmAudioSource == null) return;

        BgmAudioSource.time = pauseTime;
        BgmAudioSource.Play();
    }

    public void PauseBGM()
    {
        if (BgmAudioSource == null || !BgmAudioSource.isPlaying) return;

        pauseTime = BgmAudioSource.time;
        BgmAudioSource.Pause();
    }
    #endregion


    #region SE
    public void PlaySE(SESoundData.SE se)
    {
        if (SeAudioSource == null) return;

        SESoundData data = seSoundDates.Find(data => data.se == se);
        SeAudioSource.volume = seMasterVolume * masterVolume;
        SeAudioSource.PlayOneShot(data.audioClip);
    }

    public void PlayAndStopSE(SESoundData.SE se, float seDuration)
    {
        if (SeAudioSource == null) return;

        SESoundData data = seSoundDates.Find(data => data.se == se);
        SeAudioSource.volume = seMasterVolume * masterVolume;
        SeAudioSource.PlayOneShot(data.audioClip);

        DOVirtual.DelayedCall(seDuration, () => StopSE());
    }

    public void StopSE()
    {
        if (SeAudioSource == null) return;
        SeAudioSource.Stop();
    }
    #endregion
}



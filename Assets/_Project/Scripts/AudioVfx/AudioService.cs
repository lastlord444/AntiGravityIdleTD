using UnityEngine;
using System.Collections.Generic;

namespace BlockForge.AudioVfx
{
    /// <summary>
    /// Audio servisi. SFX ve müzik yönetimi.
    /// </summary>
    public class AudioService : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        
        private SfxLibrarySO _sfxLibrary;
        private bool _soundEnabled = true;
        private bool _musicEnabled = true;
        
        private void Awake()
        {
            // Audio source'ları oluştur (yoksa)
            if (_sfxSource == null)
            {
                _sfxSource = gameObject.AddComponent<AudioSource>();
                _sfxSource.playOnAwake = false;
            }
            
            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.playOnAwake = false;
                _musicSource.loop = true;
                _musicSource.volume = 0.5f;
            }
        }
        
        /// <summary>
        /// SFX library'sini ayarlar.
        /// </summary>
        public void Initialize(SfxLibrarySO sfxLibrary)
        {
            _sfxLibrary = sfxLibrary;
            Debug.Log("[AudioService] Başlatıldı.");
        }
        
        /// <summary>
        /// SFX çalar.
        /// </summary>
        public void PlaySfx(SfxType sfxType)
        {
            if (!_soundEnabled || _sfxLibrary == null) return;
            
            AudioClip clip = _sfxLibrary.GetClip(sfxType);
            if (clip != null)
            {
                _sfxSource.PlayOneShot(clip);
            }
        }
        
        /// <summary>
        /// Doğrudan AudioClip çalar.
        /// </summary>
        public void PlaySfx(AudioClip clip)
        {
            if (!_soundEnabled || clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }
        
        /// <summary>
        /// SFX açma/kapama.
        /// </summary>
        public void SetSoundEnabled(bool enabled)
        {
            _soundEnabled = enabled;
        }
        
        /// <summary>
        /// Müzik açma/kapama.
        /// </summary>
        public void SetMusicEnabled(bool enabled)
        {
            _musicEnabled = enabled;
            
            if (_musicSource != null)
            {
                if (enabled && !_musicSource.isPlaying)
                {
                    _musicSource.Play();
                }
                else if (!enabled && _musicSource.isPlaying)
                {
                    _musicSource.Stop();
                }
            }
        }
        
        /// <summary>
        /// Müzik çalar.
        /// </summary>
        public void PlayMusic(AudioClip clip)
        {
            if (_musicSource == null || clip == null) return;
            
            _musicSource.clip = clip;
            if (_musicEnabled) _musicSource.Play();
        }
        
        public bool SoundEnabled => _soundEnabled;
        public bool MusicEnabled => _musicEnabled;
    }
    
    /// <summary>
    /// SFX tipleri
    /// </summary>
    public enum SfxType
    {
        Place,           // Yerleştirme
        InvalidPlace,    // Geçersiz yerleştirme
        LineClear,       // Satır/sütun temizleme
        ProductionPop,   // Üretim pop-up
        ContractDeliver, // Kontrat teslimat
        ButtonClick,     // Buton tıklama
        Upgrade,         // Yükseltme
        GameOver         // Oyun sonu
    }
}

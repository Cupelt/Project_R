using UnityEngine;

namespace com.cupelt.sqarebeat.audio
{
    public class AudioSpectrum : MonoBehaviour
    {
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _audioSource.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);

            if (m_audioSpectrum != null && m_audioSpectrum.Length > 0)
            {
                spectrumValue = m_audioSpectrum[0] * 100;
            }
        }

        private void Start()
        {
            /// initialize buffer
            m_audioSpectrum = new float[128];
        }

        // This value served to AudioSyncer for beat extraction
        public static float spectrumValue { get; private set; }

        // Unity fills this up for us
        private float[] m_audioSpectrum;
        private AudioSource _audioSource;
    }
}
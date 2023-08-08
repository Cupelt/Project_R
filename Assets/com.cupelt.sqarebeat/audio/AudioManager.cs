using System;
using System.Collections.Generic;
using com.cupelt.sqarebeat.util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace com.cupelt.sqarebeat.audio
{
    public class AudioManager : MonoBehaviour
    {
        public enum Cycle { Cycle, Repeat, Random }
    
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                return instance;
            }
        }
    
        public AudioSource audio;
    
        public BeatMap nowMusic;
        public List<BeatMap> musicList = new List<BeatMap>();
        public Stack<BeatMap> musicStack = new Stack<BeatMap>();

        //option
        public bool isPause = false;
        public bool isMusicEnd;
        public Cycle nowCycle = Cycle.Cycle;

        void Awake()
        {
            instance = this;
            audio = GetComponent<AudioSource>();
        
            loadMusicList();
            initMusic();
        }

        private void Update()
        {
            if (isPause)
                audio.Pause();
            else
                audio.UnPause();

            if (audio.clip != null && audio.time >= audio.clip.length - 0.1f)
            {
                isMusicEnd = true;
            }
        }

        public void loadMusicList()
        {
            //@todo load music methods
        
            musicList.Sort((x, y) => String.Compare(x.artist, y.artist, StringComparison.Ordinal));
        }

        private void initMusic()
        {
            int track = Random.Range(0, musicList.Count);

            nowMusic = musicList[track];
            audio.clip = nowMusic.clip;
            audio.Play();
        }

        public BeatMap getTrackbyNowBeatMap(int increase)
        {
            int track = musicList.IndexOf(nowMusic);
            if (track == -1)
            {
                return musicList[0];
            }
        
            track = Util.changeCycleValue(musicList.Count, track, increase);

            return musicList[track];
        }

        public BeatMap nextTrack()
        {
            BeatMap music = nowMusic;
        
            switch (nowCycle)
            {
                case Cycle.Random:
                    if (musicList.Count > 2)
                    {
                        BeatMap rand = nowMusic;
                        while (nowMusic.Equals(rand))
                        {
                            rand = musicList[Random.Range(0, musicList.Count)];
                        }

                        music = rand;
                    }
                    break;
                case Cycle.Cycle:
                    music = getTrackbyNowBeatMap(1);
                    break;
            }

            return music;
        }

        public BeatMap beforeTrack()
        {
            return musicStack.Pop();
        }

        public void setTrack(BeatMap music)
        {
            musicStack.Push(music);
        
            isMusicEnd = false;
            nowMusic = music;
            audio.clip = nowMusic.clip;
            audio.time = 0;
            audio.Play();
        }
    
        /*
     * case Cycle.Random:
                musicStack.Push(musicList[track]);
                if (musicList.Count > 2)
                {
                    int rand = track;
                    while (track == rand)
                    {
                        rand = Random.Range(0, musicList.Count);
                    }
                    track = rand;
                }
                break;
     */
    }
}

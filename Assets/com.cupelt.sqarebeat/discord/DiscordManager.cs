using System;
using System.Collections.Generic;
using System.Data;
using com.cupelt.sqarebeat.scenes;
using Discord;
using UnityEngine;

namespace com.cupelt.sqarebeat.discord
{
    public class DiscordManager : MonoBehaviour
    {
        public long applicationID;

        public static Dictionary<string, DiscordState> stateList = new Dictionary<string, DiscordState>
        {
            {
                "idle", new DiscordState
                {
                    state = "Idle",
                    details = "",
                    largeImage = "icon",
                    largeText = "Idle in Lobby",
                    smallImage = "",
                    smallText = ""
                }
            },
            {
                "selecting", new DiscordState
                {
                    state = "Selecting Map..",
                    details = "",
                    largeImage = "icon",
                    largeText = "Selecting Map in Lobby",
                    smallImage = "",
                    smallText = ""
                }
            }
        };

        private Discord.Discord _discord;
        public static DiscordState state;

        void Start()
        {
            _discord = new Discord.Discord(applicationID, (UInt64)CreateFlags.NoRequireDiscord);

            state = stateList["idle"];
        }

        private void LateUpdate()
        {
            try
            {
                _discord.RunCallbacks();

                ActivityManager activityManager = _discord.GetActivityManager();
                Activity activity = new Activity
                {
                    Details = state.details,
                    State = state.state,
                    Assets =
                    {
                        LargeImage = state.largeImage,
                        LargeText = state.largeText,
                        SmallImage = state.smallImage,
                        SmallText = state.smallText
                    },
                    Timestamps =
                    {
                        Start = GlobalManager.startTime
                    }
                };
                
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res != Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
                });
            }
            catch
            {
                // ignored
            }
        }

        private void OnApplicationQuit()
        {
            _discord.Dispose();
        }
    }

    [System.Serializable]
    public struct DiscordState
    {
        public string details;
        public string state;
        public string largeImage;
        public string largeText;
        
        public string smallImage;
        public string smallText;
    }
}

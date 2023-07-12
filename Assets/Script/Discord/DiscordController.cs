using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    public long applicationID;
    
    [Space] public string details = "Test";
    public string state = "null";
    
    [Space] public string largeImage;
    public string largeText = "this is largeImage!";

    [Space] public string smallImage;
    public string smallText = "this is smallImage!";

    private long Time;

    private static bool _instanceExists;
    private static bool _isDiscordRunning;
    public Discord.Discord discord;

    private void Awake()
    {
        // Translation the GameObject between scenes. destroy and duplicates
        if (!_instanceExists)
        {
            _instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // login with the Application Id
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        Time = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        UpdateApplication();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        UpdateApplication();
    }

    void UpdateApplication()
    {
        // Update statue every frame
        try
        {
            ActivityManager activityManager = discord.GetActivityManager();
            Activity activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText,
                    SmallImage = smallImage,
                    SmallText = smallText
                },
                Timestamps =
                {
                    Start = Time
                }
            };
            
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}

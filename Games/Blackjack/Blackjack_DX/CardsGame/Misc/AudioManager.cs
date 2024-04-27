//-----------------------------------------------------------------------------
// AudioManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace Blackjack_DX;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

/// <summary>
/// Component that manages audio playback for all sounds.
/// </summary>
public class AudioManager : GameComponent
{
    /// <summary>
    /// The singleton for this type.
    /// </summary>
    public static AudioManager Instance { get; private set; } = null;

    static readonly string soundAssetLocation = "Sounds/";

    // Audio Data        
    Dictionary<string, SoundEffectInstance> soundBank;
    Dictionary<string, Song> musicBank;

    private AudioManager(Game game)
        : base(game) { }

    /// <summary>
    /// Initialize the static AudioManager functionality.
    /// </summary>
    /// <param name="game">The game that this component will be attached to.</param>
    public static void Initialize(Game game)
    {
        Instance = new AudioManager(game);
        Instance.soundBank = new Dictionary<string, SoundEffectInstance>();
        Instance.musicBank = new Dictionary<string, Song>();

        game.Components.Add(Instance);
    }

    /// <summary>
    /// Loads a single sound into the sound manager, giving it a specified alias.
    /// </summary>
    /// <param name="contentName">The content name of the sound file. Assumes all sounds are located under
    /// the "Sounds" folder in the content project.</param>
    /// <param name="alias">Alias to give the sound. This will be used to identify the sound uniquely.</param>
    /// <remarks>Loading a sound with an alias that is already used will have no effect.</remarks>
    public static void LoadSound(string contentName, string alias)
    {
        SoundEffect soundEffect = Instance.Game.Content.Load<SoundEffect>(soundAssetLocation + contentName);
        SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

        if (!Instance.soundBank.ContainsKey(alias))
        {
            Instance.soundBank.Add(alias, soundEffectInstance);
        }
    }

    /// <summary>
    /// Loads a single song into the sound manager, giving it a specified alias.
    /// </summary>
    /// <param name="contentName">The content name of the sound file containing the song. Assumes all sounds are 
    /// located under the "Sounds" folder in the content project.</param>
    /// <param name="alias">Alias to give the song. This will be used to identify the song uniquely.</param>
    /// /// <remarks>Loading a song with an alias that is already used will have no effect.</remarks>
    public static void LoadSong(string contentName, string alias)
    {
        Song song = Instance.Game.Content.Load<Song>(soundAssetLocation + contentName);

        if (!Instance.musicBank.ContainsKey(alias))
        {
            Instance.musicBank.Add(alias, song);
        }
    }

    /// <summary>
    /// Loads and organizes the sounds used by the game.
    /// </summary>
    public static void LoadSounds()
    {
        LoadSound("Bet", "Bet");
        LoadSound("CardFlip", "Flip");
        LoadSound("CardsShuffle", "Shuffle");
        LoadSound("Deal", "Deal");
    }

    /// <summary>
    /// Loads and organizes the music used by the game.
    /// </summary>
    public static void LoadMusic()
    {
        LoadSong("InGameSong_Loop", "InGameSong_Loop");
        LoadSong("MenuMusic_Loop", "MenuMusic_Loop");
    }

    /// <summary>
    /// Indexer. Return a sound instance by name.
    /// </summary>
    public SoundEffectInstance this[string soundName]
    {
        get
        {
            if (Instance.soundBank.ContainsKey(soundName))
            {
                return Instance.soundBank[soundName];
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Plays a sound by name.
    /// </summary>
    /// <param name="soundName">The name of the sound to play.</param>
    public static void PlaySound(string soundName)
    {
        // If the sound exists, start it
        if (Instance.soundBank.ContainsKey(soundName))
        {
            Instance.soundBank[soundName].Play();
        }
    }

    /// <summary>
    /// Plays a sound by name.
    /// </summary>
    /// <param name="soundName">The name of the sound to play.</param>
    /// <param name="isLooped">Indicates if the sound should loop.</param>
    public static void PlaySound(string soundName, bool isLooped)
    {
        // If the sound exists, start it
        if (Instance.soundBank.ContainsKey(soundName))
        {
            if (Instance.soundBank[soundName].IsLooped != isLooped)
            {
                Instance.soundBank[soundName].IsLooped = isLooped;
            }

            Instance.soundBank[soundName].Play();
        }
    }

    /// <summary>
    /// Plays a sound by name.
    /// </summary>
    /// <param name="soundName">The name of the sound to play.</param>
    /// <param name="isLooped">Indicates if the sound should loop.</param>
    /// <param name="volume">Indicates if the volume</param>
    public static void PlaySound(string soundName, bool isLooped, float volume)
    {
        // If the sound exists, start it
        if (Instance.soundBank.ContainsKey(soundName))
        {
            if (Instance.soundBank[soundName].IsLooped != isLooped)
            {
                Instance.soundBank[soundName].IsLooped = isLooped;
            }

            Instance.soundBank[soundName].Volume = volume;
            Instance.soundBank[soundName].Play();
        }
    }

    /// <summary>
    /// Stops a sound mid-play. If the sound is not playing, this
    /// method does nothing.
    /// </summary>
    /// <param name="soundName">The name of the sound to stop.</param>
    public static void StopSound(string soundName)
    {
        // If the sound exists, stop it
        if (Instance.soundBank.ContainsKey(soundName))
        {
            Instance.soundBank[soundName].Stop();
        }
    }

    /// <summary>
    /// Stops all currently playing sounds.
    /// </summary>
    public static void StopSounds()
    {
        foreach (SoundEffectInstance sound in Instance.soundBank.Values)
        {
            if (sound.State != SoundState.Stopped)
            {
                sound.Stop();
            }
        }
    }

    /// <summary>
    /// Pause or resume all sounds.
    /// </summary>
    /// <param name="resumeSounds">True to resume all paused sounds or false
    /// to pause all playing sounds.</param>
    public static void PauseResumeSounds(bool resumeSounds)
    {
        SoundState state = resumeSounds ? SoundState.Paused : SoundState.Playing;

        foreach (SoundEffectInstance sound in Instance.soundBank.Values)
        {
            if (sound.State == state)
            {
                if (resumeSounds)
                {
                    sound.Resume();
                }
                else
                {
                    sound.Pause();
                }
            }
        }
    }
    /// <summary>
    /// Play music by name. This stops the currently playing music first. Music will loop until stopped.
    /// </summary>
    /// <param name="musicSoundName">The name of the music sound.</param>
    /// <remarks>If the desired music is not in the music bank, nothing will happen.</remarks>
    public static void PlayMusic(string musicSoundName)
    {
        // If the music sound exists
        if (Instance.musicBank.ContainsKey(musicSoundName))
        {
            // Stop the old music sound
            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
            }

            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(Instance.musicBank[musicSoundName]);
        }
    }

    /// <summary>
    /// Stops the currently playing music.
    /// </summary>
    public static void StopMusic()
    {
        if (MediaPlayer.State != MediaState.Stopped)
        {
            MediaPlayer.Stop();
        }
    }

    /// <summary>
    /// Clean up the component when it is disposing.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing)
            {
                foreach (KeyValuePair<string, SoundEffectInstance> item in soundBank)
                {
                    item.Value.Dispose();
                }
                soundBank.Clear();
                soundBank = null;
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace BackgroundAudioWinRT
{
    /// <summary>
    /// The central authority on playback in the application
    /// providing access to the player and active playlist.
    /// </summary>
    [AllowForWeb]
    public sealed class PlaybackService
    {
        static PlaybackService instance;

        public static PlaybackService Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlaybackService();

                return instance;
            }
        }

        public void Play()
        {
            this.Player.Play();
        }
        public void Pause()
        {
            this.Player.Pause();
        }
        public double Volume
        {
            get { return this.Player.Volume; }
            set { this.Player.Volume = value; }
        }
        public void addItem(string uriString)
        {
            Uri s = new Uri(uriString);
            if (this.Player.Source == null)
                this.Player.Source = new MediaPlaybackList();


            MediaPlaybackItem item = new MediaPlaybackItem(MediaSource.CreateFromUri(s));
            (this.Player.Source as MediaPlaybackList).Items.Add(item);
        }

    /// <summary>
    /// This application only requires a single shared MediaPlayer
    /// that all pages have access to. The instance could have 
    /// also been stored in Application.Resources or in an 
    /// application defined data model.
    /// </summary>
    public MediaPlayer Player { get; private set; }

        /// <summary>
        /// The data model of the active playlist. An application might
        /// have a database of items representing a user's media library,
        /// but here we use a simple list loaded from a JSON asset.
        /// </summary>
//        public MediaList CurrentPlaylist { get; set; }

        public PlaybackService()
        {
            // Create the player instance
            Player = new MediaPlayer();
            Player.AutoPlay = false;
        }
    }
}

//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    'use strict';

    var playlist; 

    var playlistElem;
    var albumartElem;
    var songtitleElem;
    
    document.onreadystatechange = (function () {
        if (document.readyState === "complete") {
            playlistElem = document.getElementById('playlist');
            albumartElem = document.getElementById('album-art');
            songtitleElem = document.getElementById('song-title');

            // Wire up event listeners
            //playlist.songChangedEventHandler = playlistItemChanged;
            document.getElementById('play-button').addEventListener('click', () => mediaPlayer.play());
            document.getElementById('pause-button').addEventListener('click', () => mediaPlayer.pause());
            document.getElementById('next-button').addEventListener('click', () => mediaPlayer.moveNext());
            document.getElementById('prev-button').addEventListener('click', () => mediaPlayer.movePrevious());

            loadJSON('ms-appx-web:///assets/playlist.json', function (response) {
                playlist = response;

                mediaPlayer.addPlayList(JSON.stringify(playlist));

                // Create markup from playlist metadata
                buildPlaylistMarkup();
            });

        }
    });

    function playlistItemChanged(e) {
        const { newSong } = e;

        if (newSong) {
            albumartElem.src = newSong.albumArtUri;
            songtitleElem.innerText = newSong.title;
        }

        buildPlaylistMarkup();
    }

    function buildPlaylistMarkup() {
        console.log(playlist);
        var i = 0;
        playlist['mediaList']['items'].forEach(function (song) {
            console.log(song);

            const albumElem = document.createElement('img');
            albumElem.src = song.albumArtUri;

            const titleElem = document.createElement('h4');
            titleElem.innerText = song.title;

            const songElem = document.createElement('div');
            songElem.className = 'song';
            if (mediaPlayer.current === song.mediaUri) {
                songElem.classList.add('active');
            }
            songElem.addEventListener('click', (e) => {
                var index = i;
                mediaPlayer.skipTo(index);
            });

            songElem.appendChild(albumElem);
            songElem.appendChild(titleElem);

            playlistElem.appendChild(songElem);

            i++;
        });
    }

    function loadJSON(uri, callback) {

        callback({
            "mediaList": {
                "title": "Ringtone Mix",
                "items": [
                    {
                        "mediaType": "music",
                        "title": "Ring 1",
                        "mediaUri": "ms-appx:///Assets/media/Ring01.wma",
                        "albumArtUri": "ms-appx:///Assets/media/Ring01.jpg",
                        "albumArtAttribution": "Autumn Yellow Leaves | George Hodan"
                    },
                    {
                        "mediaType": "music",
                        "title": "Ring 2",
                        "mediaUri": "ms-appx:///Assets/media/Ring02.wma",
                        "albumArtUri": "ms-appx:///Assets/media/Ring02.jpg",
                        "albumArtAttribution": "Abstract Background | Larisa Koshkina"
                    },
                    {
                        "mediaType": "music",
                        "title": "Ring 3 Part 1",
                        "mediaUri": "ms-appx:///Assets/media/Ring03Part1.wma",
                        "albumArtUri": "ms-appx:///Assets/media/Ring03Part1.jpg",
                        "albumArtAttribution": "Snow Covered Mountains | Petr Kratochvil"
                    },
                    {
                        "mediaType": "music",
                        "title": "Ring 3 Part 2",
                        "mediaUri": "ms-appx:///Assets/media/Ring03Part2.wma",
                        "albumArtUri": "ms-appx:///Assets/media/Ring03Part2.jpg",
                        "albumArtAttribution": "Tropical Beach With Palm Trees | Petr Kratochvil"
                    },
                    {
                        "mediaType": "music",
                        "title": "Ring 3 Part 3",
                        "mediaUri": "ms-appx:///Assets/media/Ring03Part3.wma",
                        "albumArtUri": "ms-appx:///Assets/media/Ring03Part3.jpg",
                        "albumArtAttribution": "Alyssum Background | Anne Lowe"
                    }
                ]
            }
        });
        return;
    }
})();
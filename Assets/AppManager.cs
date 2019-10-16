using System.IO;
using UnityEngine;
using UnityEngine.Video;
using VideoLibrary;

public class AppManager : MonoBehaviour
{
    private const string UrlVideo2 = "https://www.youtube.com/watch?v=PTSDBLr62qI&t=65s";
    private const string UrlVideo = "https://www.youtube.com/watch?v=Rz1uru_SfpA";
    private const string UrlPdf = "https://steve-parker.org/sh/cheatsheet.pdf";

    private string _videoPath;
    
    private const int Quality = 1080;
    public MediaPlayerCtrl mediaPlayerCtrl;

    private void Start()
    {
//        Application.OpenURL(UrlPdf);
        //Run();
        SaveVideoToDisk(UrlVideo2);
        //PlayVideo();
    }
//    
//    public async void Run()
//    {
//        var videoInfos = await DownloadUrlResolver.GetDownloadUrlsAsync(UrlVideo);
//        var video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == Quality);  
//        
//        print($"video null? {video}");
//        
//        if(video.RequiresDecryption)
//        {
//            DownloadUrlResolver.DecryptDownloadUrl(video);
//        }
//        mediaPlayerCtrl.m_strFileName = video.DownloadUrl;
//        
//        print($"mediaPlayerCtrl.m_strFileName: {mediaPlayerCtrl.m_strFileName}");
//    }

    private YouTubeVideo _video;
    private void SaveVideoToDisk(string link)
    {
        var youTube = YouTube.Default; // starting point for YouTube actions
        _video = youTube.GetVideo(link); // gets a Video object with info about the video
        _videoPath = Path.Combine(Application.streamingAssetsPath, "video.mp4");
        print($"_videoPath: {_videoPath}");
        File.WriteAllBytes(_videoPath, _video.GetBytes());
        var clip = Resources.Load("video") as VideoClip;
    }

    private void PlayVideo()
    {
        // Will attach a VideoPlayer to the main camera.
        var mainCamera = GameObject.Find("Main Camera");

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        var videoPlayer = mainCamera.AddComponent<UnityEngine.Video.VideoPlayer>();

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        // This will cause our Scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 0.5F;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        print("here1");
        videoPlayer.url = _videoPath;
        print("here2");
        // Skip the first 100 frames.
        videoPlayer.frame = 100;

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        // Each time we reach the end, we slow down the playback by a factor of 10.
        videoPlayer.loopPointReached += EndReached;

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        videoPlayer.Play();
    }

    private void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed /= 10.0F;
    }

}

using UnityEngine;
using System.Linq;
using YoutubeExtractor;
public class AppManager : MonoBehaviour
{
    private const string UrlVideo = "https://www.youtube.com/watch?v=PTSDBLr62qI&t=65s";
    private const string UrlPdf = "https://steve-parker.org/sh/cheatsheet.pdf";
    
    private const int Quality = 1080;
    public MediaPlayerCtrl mediaPlayerCtrl;

    private void Start()
    {
        Application.OpenURL(UrlPdf);
        //Run();
    }
    
    public async void Run()
    {
        var videoInfos = await DownloadUrlResolver.GetDownloadUrlsAsync(UrlVideo);
        var video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == Quality);  
        
        print($"video null? {video}");
        
        if(video.RequiresDecryption)
        {
            DownloadUrlResolver.DecryptDownloadUrl(video);
        }
        mediaPlayerCtrl.m_strFileName = video.DownloadUrl;
        
        print($"mediaPlayerCtrl.m_strFileName: {mediaPlayerCtrl.m_strFileName}");
    }
}

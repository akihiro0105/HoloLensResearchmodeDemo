using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace HoloLensResearchmodeDemo
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private MediaFrameReader mediaFrameReader = null;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void SettingSensorData(int deviceNum,int cameraNum)
        {
            if (mediaFrameReader != null)
            {
                await mediaFrameReader.StopAsync();
                mediaFrameReader.FrameArrived -= FrameArrived;
                mediaFrameReader.Dispose();
                mediaFrameReader = null;
            }

            var mediaFrameSourceGroupList = await MediaFrameSourceGroup.FindAllAsync();
            var mediaFrameSourceGroup = mediaFrameSourceGroupList[deviceNum];
            var mediaFrameSourceInfo = mediaFrameSourceGroup.SourceInfos[cameraNum];
            MediaFrameSourceKind kind = mediaFrameSourceInfo.SourceKind;
            var mediaCapture = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings()
            {
                SourceGroup = mediaFrameSourceGroup,
                SharingMode = MediaCaptureSharingMode.SharedReadOnly,
                StreamingCaptureMode = StreamingCaptureMode.Video,
                MemoryPreference = MediaCaptureMemoryPreference.Cpu,
            };
            try
            {
                await mediaCapture.InitializeAsync(settings);
                var mediaFrameSource = mediaCapture.FrameSources[mediaFrameSourceInfo.Id];
                if (kind == MediaFrameSourceKind.Color)
                {
                    mediaFrameReader = await mediaCapture.CreateFrameReaderAsync(mediaFrameSource, MediaEncodingSubtypes.Argb32);
                }
                else
                {
                    mediaFrameReader = await mediaCapture.CreateFrameReaderAsync(mediaFrameSource, mediaFrameSource.CurrentFormat.Subtype);
                }
                mediaFrameReader.FrameArrived += FrameArrived;
                await mediaFrameReader.StartAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FrameArrived(MediaFrameReader sender, MediaFrameArrivedEventArgs args)
        {
            var reader = sender.TryAcquireLatestFrame();
            if (reader != null)
            {
                var videoMediaFrame = reader.VideoMediaFrame;
                var softwareBitmap = videoMediaFrame.SoftwareBitmap;
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                var task = ImageView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                  {
                      SoftwareBitmapSource softwareBitmapSource = new SoftwareBitmapSource();
                      await softwareBitmapSource.SetBitmapAsync(softwareBitmap);
                      ImageView.Source = softwareBitmapSource;
                  });
                reader.Dispose();
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 0.0;
            SettingSensorData(1, 0);
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 0.0;
            SettingSensorData(0, 0);
        }

        private void IR_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 0.0;
            SettingSensorData(0, 1);
        }

        private void Depth_Long_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 0.0;
            SettingSensorData(0, 2);
        }

        private void IR_Long_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 0.0;
            SettingSensorData(0, 3);
        }

        private void Environment1_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 90.0;
            SettingSensorData(0, 4);
        }

        private void Environment2_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 90.0;
            SettingSensorData(0, 5);
        }

        private void Environment3_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 90.0;
            SettingSensorData(0, 6);
        }

        private void Environment4_Click(object sender, RoutedEventArgs e)
        {
            ImageRotation.Rotation = 90.0;
            SettingSensorData(0, 7);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.Kinect;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TARPKinectSupport
{
    class VideoFrameClass
    {
        private KinectSensor kinectSensor;
        
        //フレーム取得用
        private byte[] imagePixelData;

        private int FrameWidth;
        private int FrameHeight;
        
        //2Dイメージ    
        public Color[] VideoColors
        {
            private set;
            get;
        }

        public VideoFrameClass(KinectSensor sensor, ColorImageFormat imgFormat)
        {
            this.kinectSensor = sensor;

            switch (imgFormat) 
            {
                case ColorImageFormat.RgbResolution640x480Fps30:
                    this.FrameWidth = 640;
                    this.FrameHeight = 480;
                    break;
                case ColorImageFormat.RgbResolution1280x960Fps12:
                    this.FrameWidth = 1280;
                    this.FrameHeight = 960;
                    break;
                default:
                    throw new FormatException();
            }

            this.AllocateMemory();
        }
        
        //メモリ割り当て
        private void AllocateMemory()
        {
            this.VideoColors = new Color[this.FrameWidth * this.FrameHeight];
        }


        #region 画像情報の取得
        private bool video_is_exist;    //フレームを取得できたか
        public void getFrame(int timeout_ms)
        {
            // 32-bit per pixel, RGBA image
            lock (this)
            {
                using (ColorImageFrame imageFrame = this.kinectSensor.ColorStream.OpenNextFrame(timeout_ms))
                {
                    if (imageFrame != null)
                    {
                        if (!this.video_is_exist)
                        {
                            this.video_is_exist = true;
                            this.imagePixelData = new byte[imageFrame.PixelDataLength];
                        }

                        imageFrame.CopyPixelDataTo(imagePixelData);

                        //画像取得
                        Parallel.For(0, this.FrameHeight, y =>
                        { //y軸
                            int no = (y * this.FrameWidth) * 4;
                            for (int x = 0; x < this.FrameWidth; x++, no += 4)
                            {
                                this.VideoColors[y * this.FrameWidth + x] = new Color(imagePixelData[no + 2], imagePixelData[no + 1], imagePixelData[no + 0]);
                            }
                        }
                        );

                    }
                    else
                    {
                        // imageFrame is null because the request did not arrive in time
                    }
                }
            }
        }
        #endregion

    }
}

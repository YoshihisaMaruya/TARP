using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.Kinect;

namespace TARPKinectSupport
{
    class DepthFrameClass
    {
        private KinectSensor kinectSensor;
        
        //フレーム取得用
        private Int16[] depthPixelData;
        private int FrameWidth;
        private int FrameHeight;

        public float DepthScale;

        //深度情報およびプレイヤーインデックス　　
        public float[][] KinectDepth
        {
            private set;
            get;
        }

        public byte[][] PlayerIndexes
        {
            private set;
            get;
        }

        public DepthFrameClass(KinectSensor sensor, DepthImageFormat depthFormat, float depthScale)
        {
            switch (depthFormat)
            {
                case DepthImageFormat.Resolution640x480Fps30:
                    this.FrameWidth = 640;
                    this.FrameHeight = 480;
                    break;
                case DepthImageFormat.Resolution320x240Fps30:
                    this.FrameWidth = 320;
                    this.FrameHeight = 240;
                    break;
                case DepthImageFormat.Resolution80x60Fps30:
                    this.FrameWidth = 80;
                    this.FrameHeight = 60;
                    break;
                default:
                    throw new FormatException();
            }

            this.kinectSensor = sensor;
            this.DepthScale = depthScale;

            this.AllocateMemory();
        
        }

        //メモリ割り当て
        private void AllocateMemory()
        {        
            this.KinectDepth = new float[this.FrameWidth][];
            this.PlayerIndexes = new byte[this.FrameWidth][];

            for (int i = 0; i < this.FrameWidth; i++)
            {
                this.KinectDepth[i] = new float[this.FrameHeight];
                this.PlayerIndexes[i] = new byte[this.FrameHeight];
            }
        }

        #region 深度及びプレイヤーインデックスの取得
        private bool depth_is_exist = false;
        public void getFrame(int timeout_ms)
        {
            // 32-bit per pixel, RGBA image
            lock (this)
            {
                using (DepthImageFrame depthFrame = this.kinectSensor.DepthStream.OpenNextFrame(timeout_ms))
                {
                    if (depthFrame != null)
                    {
                        if (!this.depth_is_exist)
                        {
                            this.depth_is_exist = true;
                            this.depthPixelData = new Int16[depthFrame.PixelDataLength];
                        }
                        depthFrame.CopyPixelDataTo(depthPixelData);

                        int width = depthFrame.Width;
                        int height = depthFrame.Height;

                        // 深度情報とプレイヤーインデックスの取り出し
                        Parallel.For(0, width, x =>
                        {
                            for (int y = 0; y < height; y++)
                            {
                                int d = (depthPixelData[y * width + x] >> DepthImageFrame.PlayerIndexBitmaskWidth);

                                //深度不明点は最背面として扱う
                                if (d == kinectSensor.DepthStream.UnknownDepth)
                                {
                                    d = TARPKinectHelper.MAX_DEPTH;
                                }
                                else if (d < TARPKinectHelper.MIX_DEPTH)
                                {
                                    d = TARPKinectHelper.MIX_DEPTH;
                                }
                                else if (d > TARPKinectHelper.MAX_DEPTH)
                                {
                                    d = TARPKinectHelper.MAX_DEPTH;
                                }

                                this.KinectDepth[x][y] = (float)d * this.DepthScale;
                                this.PlayerIndexes[x][y] = (byte)(depthPixelData[y * width + x] & DepthImageFrame.PlayerIndexBitmask);

                            }

                        }
                        );

                    }
                    else
                    {
                        // depthFrame is null because the request did not arrive in time
                    }
                }
            }
        }
        #endregion
    }
}

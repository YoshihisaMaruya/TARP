using System;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace TARPKinectSupport
{
    class TARPKinectHelper
    {
        //for kinect
        private KinectSensor kinectSensor;
        private ColorImageFormat colorImageFormat = ColorImageFormat.RgbResolution640x480Fps30;
        private DepthImageFormat depthImageFormat = DepthImageFormat.Resolution640x480Fps30;

        //for frame
        private DepthFrameClass depthFrameClass;
        private VideoFrameClass videoFrameClass;
        private SkeletonFrameClass skeletonFrameClass;
        private int timeout_ms = 10;
        private float scale;

        //global
        public const int MAX_JOINT_NUM = 20; //関節の数
        public const int MAX_PEOPLE_NUM = 7; //取得できるプレイヤーインデックスの最大数
        public const int MAX_SKELTON_NUM = 2; //取得できる骨格の最大数
        public const int MAX_DEPTH = 5000;
        public const int MIX_DEPTH = 1000;
        public const float Horizontal_Degree = 57.0f / 2.0f;
        public const float Vertical_Degree = 43.0f / 2.0f;
        //public static readonly float H_tang_max = (float)Math.Tan(MathHelper.ToRadians(Horizontal_Degree));
        public const float H_tang_max = 0.5429557f; //Excelで事前計算
        //public static readonly float V_tang_max = (float)Math.Tan(MathHelper.ToRadians(Vertical_Degree));
        public const float V_tang_max = 0.393910476f;   //Excelで事前計算
        public const float xna_view_degree = 38.18917597f;  //Excelで事前計算
        public const float xna_view_radian = 0.666526859f;  //Excelで事前計算
        public const float xna_aspect_ratio = 1.378373344f; //Excelで事前計算
        // Kinect最大のfps
        private int kinectFps = 30;
        private float kinectUpdateMillSec = 1000 / 30.0f;
        private float updateTime;

        public KinectImage KinectImage { private set; get;  }

        public TARPKinectHelper(float scale)
        {
            this.scale = scale;
            this.initialize();
        }

        private void initialize(){
            try
            {
                if (KinectSensor.KinectSensors.Count == 0) throw new Exception("Kinect isn't connected");
                this.kinectSensor = KinectSensor.KinectSensors[0];

                this.KinectImage = new KinectImage();

                //rgb
                this.kinectSensor.ColorStream.Enable(colorImageFormat);
                this.videoFrameClass = new VideoFrameClass(this.kinectSensor, colorImageFormat);
                this.KinectImage.Color = this.videoFrameClass.VideoColors;

                // 深度ストリーム
                this.kinectSensor.DepthStream.Enable(depthImageFormat);
                this.depthFrameClass = new DepthFrameClass(this.kinectSensor, depthImageFormat, this.scale);
                this.KinectImage.Depth = this.depthFrameClass.KinectDepth;
                this.KinectImage.PlayerIndexes = this.depthFrameClass.PlayerIndexes;

                // スケルトンストリーム 
                this.kinectSensor.SkeletonStream.Enable();
                this.skeletonFrameClass = new SkeletonFrameClass(this.kinectSensor, this.scale);
                this.KinectImage.Skeleton = this.skeletonFrameClass.skeletonList;

                //通常モードで使用
                this.kinectSensor.DepthStream.Range = DepthRange.Default;

                // Kinectの動作を開始する
                this.kinectSensor.Start();

                //水平角の修正
                if (this.kinectSensor.ElevationAngle != 0)
                    this.kinectSensor.ElevationAngle = 0;

            }
            catch (Exception e)
            {
                throw new Exception("Kinect internal error");
            }
        }


        //現在保持している情報の更新
        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds;
            if (time - this.updateTime > this.kinectUpdateMillSec)
            {
                this.videoFrameClass.getFrame(this.timeout_ms);
                this.depthFrameClass.getFrame(this.timeout_ms);
                this.skeletonFrameClass.getFrame(this.timeout_ms);
                this.updateTime = time;
            }
        }

        public void Dispose()
        {
            this.kinectSensor.Stop();
        }
    }
}

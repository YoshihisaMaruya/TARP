using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Support.Timer
{
    class MyTimer
    {
        private ArrayList timeList;
        private string timerName;
        private Stopwatch stopWatch;

        private double averageTime;

        public MyTimer(string name) 
        {
            //ファイル名のセット
            timerName = name;

            //時間のリストの初期化
            timeList = new ArrayList();

            //ストップウォッチの初期化
            stopWatch = new Stopwatch();
        }

        //計測開始
        public void startTimer()
        {
            stopWatch.Start();
        }

        //計測終了
        public void stopTimer() 
        {
            stopWatch.Stop();
            
            //ミリ秒単位で時間リストに追加
            timeList.Add((double)stopWatch.ElapsedTicks / (double)Stopwatch.Frequency);

            stopWatch.Reset();
        }

        //時間リストの要素のクリア
        public void clearTimer() 
        {
            timeList.Clear();
        }

        //時間リストの平均値を計算する
        private void calcAverageTime()
        {
            if (timeList.Count <= 0)
            {
                averageTime = -1.0;
            }
            else
            {
                double sum = 0.0;

                for (int i = 0; i < timeList.Count; i++)
                {
                    sum += (double)timeList[i];
                }

                sum /= timeList.Count;

                averageTime = sum;
            }
        }

        //平均値と計測回数を文字列として返す
        public override string ToString()
        {
            string str;

            this.calcAverageTime();

            if (averageTime < 0.0) 
            {
                str = timerName + " average doesn't exist.";
            }
            else
            {
                str = timerName + " average of " + timeList.Count + " times: " + averageTime + "(ms)";
            }

            return str;
        }
    }
}

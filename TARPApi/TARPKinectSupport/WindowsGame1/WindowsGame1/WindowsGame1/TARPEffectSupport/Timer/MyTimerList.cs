using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Support.Timer
{
    class MyTimerList
    {
        private ArrayList TimerList;    //MyTimerクラスのリスト
        private ArrayList NameList;    //MyTimerに対応する名前のリスト

        private string TimerListName;
        //private StreamWriter outStream;

        public MyTimerList(string name)
        {
            TimerList = new ArrayList();
            NameList = new ArrayList();

            TimerListName = name;

            //ファイルを新規作成
            //outStream = new StreamWriter(name, false);
            //outStream.Close();
        }

        //指定された名前を持つストップウォッチを動作させる
        public void startTimerNameOf(string name)
        {
            int index = NameList.IndexOf(name);

            if (index == -1)
            {
                NameList.Add(name);
                TimerList.Add(new MyTimer(name));
                index = TimerList.Count - 1;
            }

            ((MyTimer)TimerList[index]).startTimer();
        }

        //指定された名前を持つストップウォッチを停止させる
        public void stopTimerNameOf(string name)
        {
            int index = NameList.IndexOf(name);

            if (index != -1)
            {
                ((MyTimer)TimerList[index]).stopTimer();
            }

        }

        //指定された名前を持つストップウォッチを初期化させる
        public void clearTimerNameOf(string name)
        {
            int index = NameList.IndexOf(name);

            if (index != -1)
            {
                ((MyTimer)TimerList[index]).clearTimer();
            }

        }


        //リストに持つストップウォッチをすべて初期化させる
        public void clearAllTimer()
        {
            for (int i = 0; i < TimerList.Count; i++)
            {
                ((MyTimer)TimerList[i]).clearTimer();
            }
        }

        //計測結果を出力
        public void printToConsole()
        {
            if (this.TimerList.Count > 0)
            {
                Support.DebugConsole.WriteLine("--average time");
            }
            for (int i = 0; i < TimerList.Count; i++)
            {
                Support.DebugConsole.WriteLine(((MyTimer)TimerList[i]).ToString());
            }
        }

    }
}

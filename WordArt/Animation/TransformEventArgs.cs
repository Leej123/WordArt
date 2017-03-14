using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    public class TransformEventArgs : EventArgs
    {
        public static int IDLE = 0;
        public static int BEGIN = 1;
        public static int RUNNING = 2;
        public static int END = 3;

        private int screenIndex;
        /// <summary>
        /// 当前绘制哪一屏的文字
        /// </summary>
        public int ScreenIndex
        {
            get { return screenIndex; }
        }

        private int state = IDLE;
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenIndex"></param>
        public TransformEventArgs(int screenIndex)
        {
            this.screenIndex = screenIndex;
        }
    }
}

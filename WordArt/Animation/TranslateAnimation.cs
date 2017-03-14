using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 平移动画
    /// </summary>
    public class TranslateAnimation : TextAnimation
    {
        private float fromX, fromY;
        private float toX, toY;

        private int stayCount = 1;

        public TranslateAnimation(PointF from, PointF to) : base()
        {
            fromX = from.X;
            fromY = from.Y;

            toX = to.X;
            toY = to.Y;
        }

        public override Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || isEnd)//动画未开始
            {
                isStillRun = true;
                return transform;
            }
            isStillRun = false;
            ApplyTransformation(transform);
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            if (stayCount != 0)
            {
                stayCount--;
                return;
            }
            transform.Translate(toX - fromX, toY - fromY);
            isEnd = true;
        }

        public override bool IsAnimationEnd()
        {
            return isEnd;
        }
    }
}

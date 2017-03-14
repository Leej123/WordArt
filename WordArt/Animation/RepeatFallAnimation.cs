using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 重复下落动画
    /// </summary>
    public class RepeatFallAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 5;

        private float fromY;
        private float toY;

        private int repeatCount = 0;

        private float endY;

        private float curY;
        private float initY;
        public float InitY
        {
            set { initY = value; curY = initY; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromY">下落的起点位置</param>
        /// <param name="toY">下落的终点位置</param>
        /// <param name="initY">开始位置</param>
        public RepeatFallAnimation(float fromY, float toY, float initY) : base()
        {
            this.fromY = fromY;
            this.toY = toY;
            this.endY = Math.Abs(fromY - toY) / 2;
            this.curY = initY;
            this.initY = initY;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }

            if (isEnd)
            {
                isStillRun = false;
                transform.Reset();
                transform.Translate(0, endY - initY);
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            if (fromY == toY)
            {
                curY = toY;
            }
            else if (toY > fromY)
            {
                if (curY >= toY)
                {
                    curY = fromY;
                    repeatCount++;
                }
                else
                {
                    curY += CHANGE_VALUE;
                    if (curY >= toY)
                    {
                        curY = toY;
                    }
                }
            }
            else
            {
                if (curY <= toY)
                {
                    curY = fromY;
                    repeatCount++;
                }
                else
                {
                    curY -= CHANGE_VALUE;
                    if (curY <= toY)
                    {
                        curY = toY;
                    }
                }
            }

            transform.Reset();
            transform.Translate(0, curY - initY);
        }

        public override void Reset()
        {
            base.Reset();
            curY = initY;
            repeatCount = 0;
        }

        public override void Cancel()
        {
            isEnd = true;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (repeatCount == 20)
                isEnd = true;
            return isEnd;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 前后缩放动画
    /// </summary>
    public class ZoomInOutAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 0.02f;

        //开始时放大倍数
        private float fromScale;

        //结束时的放大倍数
        private float toScale;

        //开始时的放大倍数
        private float beginScale;

        //当前的放放大倍数
        private float curScale;

        //由小到大
        private bool small2Big = true;

        private float pivotX;
        private float pivotY;

        private float offsetX, offsetY;

        private int repeatCount = 0;

        /// <summary>
        /// 放大缩小动画
        /// </summary>
        /// <param name="fromScale"></param>
        /// <param name="toScale"></param>
        /// <param name="beginScale">开始时放大倍数</param>
        /// <param name="pivotX">放大缩小的中心点x坐标</param>
        /// <param name="pivotY">放大缩小的中心点y坐标</param>
        public ZoomInOutAnimation(float fromScale, float toScale, float beginScale, float pivotX, float pivotY) : base()
        {
            //isCycleAnimation = true;
            this.fromScale = fromScale;
            this.toScale = toScale;
            curScale = beginScale;

            this.pivotX = pivotX;
            this.pivotY = pivotY;
            this.beginScale = beginScale;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || IsAnimationEnd())
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            transform.Reset();
            if (fromScale != toScale)
            {
                if (small2Big)
                {
                    curScale += CHANGE_VALUE;
                    if (curScale >= toScale)
                    {
                        curScale = toScale;
                        small2Big = false;
                        repeatCount++;
                    }
                }
                else
                {
                    curScale -= CHANGE_VALUE;
                    if (curScale <= fromScale)
                    {
                        curScale = fromScale;
                        small2Big = true;
                    }
                }

                offsetX = pivotX * curScale - pivotX;
                offsetY = pivotY * curScale - pivotY;

                transform.Translate(-offsetX, -offsetY);
                transform.Scale(curScale, curScale);
            }
        }

        public override void Reset()
        {
            base.Reset();
            curScale = beginScale; 
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (repeatCount >= 2)
            {
                isEnd = true;
                repeatCount = 0;
            }
            return isEnd;
        }
    }
}

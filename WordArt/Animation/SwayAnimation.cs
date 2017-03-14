using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 左右摇摆动画
    /// </summary>
    public class SwayAnimation : TextAnimation
    {
        /// <summary>
        /// 每次位置的变动量
        /// </summary>
        private const float CHANGE_VALUES = 0.05f;

        //x方向切变
        private float leftX;
        private float rightX;

        private float curX;

        //x 方向的当前切变方向
        private bool left2Right = true;

        /// <summary>
        /// y方向中心点的坐标。在该位置进行左右剪切变换
        /// </summary>
        private float pivotY;

        private float offset = 0;

        private int repeatCount = 0;

        public SwayAnimation(float leftX, float rightX, float pivotY) : base()
        {
            if (leftX <= rightX)
            {
                this.leftX = leftX;
                this.rightX = rightX;
                left2Right = true;
            }
            else
            {
                this.leftX = rightX;
                this.rightX = leftX;
                left2Right = false;
            }

            curX = 0;
            this.pivotY = pivotY;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || IsAnimationEnd())
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }
            isStillRun = true;
            ApplyTransformation(transform);
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            transform.Reset();
            if (leftX != rightX)
            {
                if (left2Right)
                {
                    curX += CHANGE_VALUES;
                    if (curX >= rightX)
                    {
                        curX = rightX;
                        left2Right = false;
                        repeatCount++;
                    }
                }
                else
                {
                    curX -= CHANGE_VALUES;
                    if (curX <= leftX)
                    {
                        curX = leftX;
                        left2Right = true;
                    }
                }
            }

            transform.Shear(curX, 0);
            offset = pivotY * curX;
            transform.Translate(-offset, 0);
        }

        public override void Reset()
        {
            base.Reset();
            curX = 0;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (repeatCount >= 3)
            {
                isEnd = true;
                repeatCount = 0;
            }
            return isEnd;
        }
    }
}

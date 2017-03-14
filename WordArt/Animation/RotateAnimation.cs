using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 旋转动画
    /// </summary>
    public class RotateAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 3;

        /// <summary>
        /// 开始角度
        /// </summary>
        private float fromAngle;

        /// <summary>
        /// 结束时角度
        /// </summary>
        private float toAngle;

        private PointF pivotPoint;

        private float curAngle;

        public RotateAnimation(float fromAngle, float toAngle, float pivotX, float pivotY) : base()
        {
            this.fromAngle = fromAngle;
            this.toAngle = toAngle;
            pivotPoint = new PointF();
            pivotPoint.X = pivotX;
            pivotPoint.Y = pivotY;

            this.curAngle = fromAngle;
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
            if (fromAngle == toAngle)
            {
                curAngle = toAngle;
            }
            else
            {
                if (fromAngle < toAngle)
                {
                    curAngle += CHANGE_VALUE;
                    if (curAngle >= toAngle)
                    {
                        curAngle = toAngle;
                    }
                }
                else
                {
                    curAngle -= CHANGE_VALUE;
                    if (curAngle <= toAngle)
                        curAngle = toAngle;
                }
            }

            transform.Reset();
            transform.RotateAt(curAngle, pivotPoint);
        }

        public override void Reset()
        {
            base.Reset();
            curAngle = fromAngle;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (curAngle == toAngle)
                isEnd = true;
            return isEnd;
        }
    }
}

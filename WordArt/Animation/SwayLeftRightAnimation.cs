using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 左右摇晃
    /// </summary>
    public class SwayLeftRightAnimation : TextAnimation
    {
        private const int CHANGE_VALUE = 1;

        /// <summary>
        /// 右边旋转点
        /// </summary>
        private PointF rightPivot;

        /// <summary>
        /// 左边旋转点
        /// </summary>
        private PointF leftPivot;

        /// <summary>
        /// 摇摆的角度
        /// </summary>
        private int angle;

        private int curAngle;

        private int repeatCount;

        private bool right2Left = false;

        public SwayLeftRightAnimation(PointF rightPivot, PointF leftPivot, int angle) : base()
        {
            this.rightPivot = rightPivot;
            this.leftPivot = leftPivot;
            this.angle = angle;
            curAngle = 0;

            if (angle > 0)
                right2Left = false;
            else
                right2Left = true;
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
            if (angle == 0)
            {
                transform.Reset();
                return;
            }

            if (right2Left)
            {
                curAngle -= CHANGE_VALUE;
                if (curAngle <= -angle)
                {
                    curAngle = -angle;
                    right2Left = false;
                }
            }
            else
            {
                curAngle += CHANGE_VALUE;
                if (curAngle >= angle)
                {
                    curAngle = angle;
                    right2Left = true;
                    repeatCount++;
                }
            }

            transform.Reset();
            transform.RotateAt(curAngle, curAngle >= 0 ? rightPivot : leftPivot);
        }

        public override void Reset()
        {
            base.Reset();
            curAngle = 0;

            if (angle > 0)
                right2Left = false;
            else
                right2Left = true;

            repeatCount = 0;
        }

        public override bool IsAnimationEnd()
        {
            //isEnd = false;
            //if (repeatCount == 3)
            //    isEnd = true;
            repeatCount = 0;
            return isEnd;
        }
    }
}

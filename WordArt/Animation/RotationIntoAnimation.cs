using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 旋转挤入
    /// </summary>
    public class RotationIntoAnimation : TextAnimation
    {
        private float moveChangeValue;
        private float rotationChangeValue;
        private float fromX, toX;
        private float curX, curAngle;
        private float steps;
        private PointF pivot;
        private PointF curPivot;
        private float offsetX;

        public RotationIntoAnimation(float fromX, float toX, PointF pivot, int steps) : base()
        {
            this.fromX = fromX;
            this.toX = toX;
            this.pivot = pivot;
            this.steps = steps;

            curX = fromX;
            curAngle = 0;

            moveChangeValue = Math.Abs(fromX - toX) / steps;
            rotationChangeValue = 360.0f / steps;
            curPivot = new PointF(pivot.X, pivot.Y);
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }

            if (IsAnimationEnd())
            {
                isStillRun = false;
                transform.Reset();
                transform.Translate(toX - fromX, 0);
                return transform;
            }

            isStillRun = true;
            ApplyTransformation(transform);
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            if (fromX == toX)
            {
                curX = toX;
            }
            else if (toX > fromX)
            {
                curX += moveChangeValue;
                if (curX >= toX)
                {
                    curX = toX;
                }
            }
            else
            {
                curX -= moveChangeValue;
                if (curX <= toX)
                {
                    curX = toX;
                }
            }

            curAngle += rotationChangeValue;
            if (curAngle >= 360)
            {
                curAngle = 360;
            }

            transform.Reset();

            offsetX = curX - fromX;
            curPivot.X = pivot.X + offsetX;
            transform.RotateAt(-curAngle, curPivot);
            transform.Translate(offsetX, 0);
        }

        public override void Reset()
        {
            base.Reset();
            curX = fromX;
            curAngle = 0;
            curPivot.X = pivot.X;
            curPivot.Y = pivot.Y;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (curAngle == 360)
            {
                isEnd = true;
            }
 
            return isEnd;
        }
    }
}

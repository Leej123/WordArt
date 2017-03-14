using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 旋转滚动动画
    /// </summary>
    public class RollingAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 5;
        private const int MAX_ROTATE_ANGLE = 2 * 360;

        private float fromX;
        private float toX;

        private float curX;

        private PointF pivot = new PointF();

        public RollingAnimation(float fromX, float toX, float pivotX, float pivotY) : base()
        {
            this.fromX = fromX;
            this.toX = toX;

            curX = fromX;

            pivot.X = pivotX;
            pivot.Y = pivotY;
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
            if (toX == fromX)
            {
                curX = toX;
            }
            else if (fromX < toX)
            {
                curX += CHANGE_VALUE;
                if (curX >= toX)
                    curX = toX;
            }
            else
            {
                curX -= CHANGE_VALUE;
                if (curX <= toX)
                    curX = toX;
            }

            transform.Reset();
            
            transform.Translate(curX - fromX, 0);
            transform.RotateAt(-GetRotateAngle(curX), pivot);
        }

        private float GetRotateAngle(float curX)
        {
            if (fromX == toX)
                return 0;
            return (curX - fromX) * MAX_ROTATE_ANGLE / (toX - fromX);
        }

        public override void Cancel()
        {
            
        }

        public override void Reset()
        {
            base.Reset();
            curX = fromX;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (curX == toX)
                isEnd = true;

            return isEnd;
        }
    }
}

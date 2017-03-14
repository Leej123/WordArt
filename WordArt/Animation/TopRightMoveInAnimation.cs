using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 右上方移出动画
    /// </summary>
    public class TopRightMoveInAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 3;
        protected float fromX, fromY;
        protected float toX, toY;

        protected float curX, curY;

        //x,y方向的步长
        private float stepX = 0;
        private float stepY = 0;

        protected float width, height;

        private TopRightMoveInAnimation nextAnimation = null;
        public TopRightMoveInAnimation NextAnimation
        {
            set { nextAnimation = value; }
        }

        public TopRightMoveInAnimation(float fromX, float toX, float fromY, float toY, float width, float height) : base()
        {
            this.fromX = fromX;
            this.fromY = fromY;
            this.toX = toX;
            this.toY = toY;

            curX = fromX;
            curY = fromY;

            this.width = width;
            this.height = height;

            CalcStepLength();
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }

            if (CurAnimationEnd())
            {
                isStillRun = false;
                transform.Reset();
                transform.Translate(toX - fromX, toY - fromY);
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            MoveX();
            MoveY();
            transform.Reset();
            transform.Translate(curX - fromX, curY - fromY);
        }

        protected void MoveX()
        {
            if (curX == toX)
                return;

            if (toX == fromX)
            {
                curX = toX;
            }
            else if (toX > fromX)
            {
                curX += stepX;
                if (curX >= toX)
                    curX = toX;
            }
            else
            {
                curX -= stepX;
                if (curX <= toX)
                    curX = toX;
            }
        }

        protected void MoveY()
        {
            if (curY == toY)
                return;
            if (fromY == toY)
            {
                curY = toY;
            }
            else if (toY > fromY)
            {
                curY += stepY;
                if (curY >= toY)
                    curY = toY;
            }
            else
            {
                curY -= stepY;
                if (curY <= toY)
                    curY = toY;
            }
        }

        /// <summary>
        /// 计算x,y方向的步长
        /// </summary>
        private void CalcStepLength()
        {
            //float deltaX = toX - fromX;
            //float deltaY = toY - fromY;
            float deltaX = width;
            float deltaY = height;
            if (deltaX == 0 && deltaY == 0)
            {
                stepX = 0;
                stepY = 0;
                return;
            }

            if (deltaX == 0)
            {
                stepX = 0;
                stepY = CHANGE_VALUE;
            }
            else if (deltaY == 0)
            {
                stepX = CHANGE_VALUE;
                stepY = 0;
            }
            else
            {
                double x = Math.Abs(deltaX);
                double y = Math.Abs(deltaY);
                double theta = Math.Atan2(y, x);

                stepX = (float)(CHANGE_VALUE * Math.Cos(theta));
                stepY = (float)(CHANGE_VALUE * Math.Sin(theta));
            }
        }

        public override void Cancel()
        {
        }

        public override void Reset()
        {
            base.Reset();
            curX = fromX;
            curY = fromY;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (nextAnimation == null)
            {
                isEnd = CurAnimationEnd();
            }
            else
            {
                isEnd = nextAnimation.IsAnimationEnd();
            }

            return isEnd;
        }

        private bool CurAnimationEnd()
        {
            if (curX == toX && curY == toY)
                return true;
            return false;
        }
    }
}

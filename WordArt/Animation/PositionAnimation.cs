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
    /// 位置动画。文字由一个位置移动到另一个位置
    /// </summary>
    public class PositionAnimation : TextAnimation
    {
        /// <summary>
        /// 每次位置的变动量
        /// </summary>
        private const float CHANGE_VALUES = 10;

        //开始位置
        private PointF from;
        //结束位置
        private PointF to;

        //当前位置
        private float curX;
        private float curY;

        //x,y方向的步长
        private float stepX = 0;
        private float stepY = 0;

        /// <summary>
        /// 线性的由一个点移动到另一个点
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">结束点</param>
        public PositionAnimation(PointF from, PointF to) : base()
        {
            this.from = from;
            this.to = to;
            curX = from.X;
            curY = from.Y;
            CalcStepLength();
        }

        public override Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || isEnd)//动画未开始
            {
                return transform;
            }
            if (IsAnimationEnd())
                isStillRun = false;
            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(Matrix transform)
        {
            float offsetX = 0;
            if (to.X > from.X)
            {
                if (curX + stepX > to.X)
                {
                    offsetX = to.X - curX;
                }
                else
                {
                    offsetX = stepX;
                }
            }
            else if (to.X < from.X)
            {
                if (curX + stepX < to.X)
                {
                    offsetX = to.X - curX;
                }
                else
                {
                    offsetX = stepX;
                }
            }
            curX += offsetX;

            float offsetY = 0;
            if (to.Y != from.Y)
            {
                if (((to.Y > from.Y) && (curY + stepY > to.Y))
                    || ((to.Y < from.Y) && (curY + stepY < to.Y)))
                {
                    offsetY = to.Y - curY;
                }
                else
                {
                    offsetY = stepY;
                }
            }
            curY += offsetY;

            transform.Translate(offsetX, offsetY);
        }

        public override void Reset()
        {
            curX = from.X;
            curY = from.Y;
            base.Reset();
        }

        public override bool IsAnimationEnd()
        {
          
            if (curX == to.X && curY == to.Y)
            {
                isEnd = true;
                return true;
            }
                
            return false;
        }

        /// <summary>
        /// 计算x,y方向的步长
        /// </summary>
        private void CalcStepLength()
        {
            float deltaX = to.X - from.X;
            float deltaY = to.Y - from.Y;
            if (deltaX == 0 && deltaY == 0)
            {
                stepX = 0;
                stepY = 0;
                return;
            }

            if (deltaX == 0)
            {
                stepX = 0;
                stepY = CHANGE_VALUES;
            }
            else if (deltaY == 0)
            {
                stepX = CHANGE_VALUES;
                stepY = 0;
            }
            else
            {
                double x = Math.Abs(deltaX);
                double y = Math.Abs(deltaY);
                double theta = Math.Atan2(y, x);

                stepX = (float)(CHANGE_VALUES * Math.Cos(theta));
                stepY = (float)(CHANGE_VALUES * Math.Sin(theta));
            }

            if (deltaX < 0)
                stepX = -stepX;

            if (deltaY < 0)
                stepY = -stepY;
        }
    }
}

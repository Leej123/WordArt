using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordArt.Config;

namespace WordArt.Animation
{
    /// <summary>
    /// 连续移动动画
    /// </summary>
    class ContinuouseMovementAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 3;

        private PointF initPos;
        private PointF fromPoint;
        private PointF toPoint;

        private float curX, curY;

        private int count = 0;
        private bool firstTurn = true;

        private float excess;//超过的量

        public ContinuouseMovementAnimation(PointF initPos, PointF fromPoint, PointF toPoint) : base()
        {
            this.initPos = initPos;
            this.fromPoint = fromPoint;
            this.toPoint = toPoint;

            curX = initPos.X;
            curY = initPos.Y;
            count = 0;
            firstTurn = true;
            excess = 0;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || IsAnimationEnd())
            {
                transform.Reset();
                isStillRun = false;
                return transform;
            }

            isStillRun = true;
            ApplyTransformation(transform);
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            transform.Reset();
            if (firstTurn)
            {
                Move(initPos, toPoint);
            }
            else
            {
                Move(fromPoint, toPoint);
            }
            transform.Translate(curX - initPos.X, curY - initPos.Y);
            if (curX == toPoint.X)
            { 
                curX = fromPoint.X + excess; 
            }
            if (curY == toPoint.Y)
            {
                curY = fromPoint.Y + excess;
            }
        }

        private void Move(PointF from, PointF to)
        {
            Move(from.X, to.X, ref curX);
            Move(from.Y, to.Y, ref curY);
        }

        private void Move(float from, float to, ref float curPos)
        {
            if (to == from)
            {
                curPos = to;
            }
            else if (to > from)
            {
                excess = 0;
                curPos += CHANGE_VALUE;
                if (curPos >= to)
                {
                    excess = curPos - to;
                    curPos = to;
                    if (!firstTurn) count++;
                    firstTurn = false;
                }
            }
            else
            {
                excess = 0;
                curPos -= CHANGE_VALUE;
                if (curPos <= to)
                {
                    excess = curPos - to;
                    curPos = to;
                    if (!firstTurn) count++;
                    firstTurn = false;
                }
            }
        }

        public override bool IsAnimationEnd()
        {
            if (!AnimationConfig.IsUseGif)//如果不是使用gif图像作为结果
            {
                isEnd = false;
                return false;
            }

            isEnd = false;
            if (count == 5)
            {
                isEnd = true;
            }
            return isEnd;
        }
    }
}

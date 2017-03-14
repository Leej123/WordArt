using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 水平旋转
    /// </summary>
    public class HorizontalRotationAnimation : TextAnimation
    {
        private float initX;
        private float fromX;
        private float toX;

        private float initScale;
        private float fromScale;
        private float toScale;
        private float pivotY;

        private int steps;

        private float offsetY;

        private float curX;
        private float curScale;

        private float moveStepLength;
        private float scaleStepLength;

        private bool reverse = false;

        private int stepCount = 0;

        public HorizontalRotationAnimation(float fromX, float toX, float fromScale, float toScale, float pivotY, int steps) : base()
        {
            this.initX = fromX;
            this.fromX = fromX;
            this.toX = toX;
            this.initScale = fromScale;
            this.fromScale = fromScale;
            this.toScale = toScale;
            this.steps = steps;
            this.pivotY = pivotY;

            curX = fromX;
            curScale = fromScale;
            reverse = false;
            CalcStepLength();
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || isEnd)
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
            if (fromX == toX)
            {
                curX = toX;
                stepCount++;
                if (stepCount >= steps * 4)
                {
                    stepCount = 0;
                    isEnd = true;
                }

                if (stepCount == steps * 2)///校正
                {
                    curScale = toScale;
                }
            }
            else if (toX > fromX)
            {
                curX += moveStepLength;
                if (curX >= toX)
                {
                    curX = toX;
                    if (reverse)
                    {
                        reverse = false;
                        isEnd = true;
                    }
                    else
                    {
                        reverse = true;
                        curScale = toScale;///校正
                    }
                    toX = fromX;
                    fromX = curX;
                }
            }
            else
            {
                curX -= moveStepLength;
                if (curX <= toX)
                {
                    curX = toX;
                    if (reverse)
                    {
                        reverse = false;
                        isEnd = true;
                    }
                    else
                    {
                        reverse = true;
                        curScale = toScale;///校正
                    }
                    toX = fromX;
                    fromX = curX;
                }
            }

            if (fromScale == toScale)
            {
                curScale = toScale;
            }
            else if (toScale > fromScale)
            {
                curScale += scaleStepLength;
                if (curScale >= toScale)
                {
                    curScale = toScale;
                    toScale = fromScale;
                    fromScale = curScale;
                }
            }
            else
            {
                curScale -= scaleStepLength;
                if (curScale <= toScale)
                {
                    curScale = toScale;
                    toScale = fromScale;
                    fromScale = curScale;
                }
            }
            

            transform.Reset();
            if (!isEnd)
            {
                offsetY = pivotY * curScale - pivotY;
                transform.Translate(curX - initX, -offsetY);
                transform.Scale(1, curScale);
            }
        }

        public override void Reset()
        {
            base.Reset();
            curX = initX;
            curScale = initScale;
            reverse = false;
            stepCount = 0;
        }

        public override bool IsAnimationEnd()
        {
            return isEnd;
        }

        private void CalcStepLength()
        {
            float mid = Math.Abs(fromX - toX) / 2.0f;
            moveStepLength = 0;
            if (mid != 0)
            {
                moveStepLength = mid / steps;
            }

            scaleStepLength = 0;
            if (fromScale != toScale)
            {
                scaleStepLength = Math.Abs(fromScale - toScale) / steps;
            }
        }
    }
}

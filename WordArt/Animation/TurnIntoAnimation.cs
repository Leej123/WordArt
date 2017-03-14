using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 翻转挤入动画
    /// </summary>
    public class TurnIntoAnimation : TextAnimation
    {
        private float moveChangeValue = 3;
        private float scaleChangeValue = 0.01f;

        private float fromX;
        private float toX;
        private float shearY;
        private float shearPivot;
        private float scaleX;
        private float scalePivot;

        private float offsetX;

        private float curX, curShear, curScale;

        private bool beginShear = false;
        private int stepCount = 0;
        private int steps;

        public TurnIntoAnimation(float fromX, float toX, float shearY, float shearPivot, float minScaleX, float scalePivot, int steps) : base()
        {
            this.fromX = fromX;
            this.toX = toX;
            this.shearY = shearY;
            this.shearPivot = shearPivot;
            this.scaleX = minScaleX;
            this.scalePivot = scalePivot;
            this.steps = steps;

            curX = fromX;
            curShear = shearY;
            curScale = 1;

            moveChangeValue = Math.Abs(fromX - toX) / steps;
            scaleChangeValue = (1 - minScaleX) * 2 / steps;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                beginShear = false;
                isStillRun = false;
                SetTransform(transform, fromX, 1, shearY);
                return transform;
            }

            if (IsAnimationEnd())
            {
                transform.Reset();
                isStillRun = false;
                transform.Translate(toX - fromX, 0);
                return transform;
            }

            isStillRun = true;
            ApplyTransformation(transform);
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            stepCount++;
            if (fromX == toX)
            {
                curX = toX;
            }
            else if (toX > fromX)
            {
                curX += moveChangeValue;
                if (curX >= toX)
                    curX = toX;
            }
            else
            {
                curX -= moveChangeValue;
                if (curX <= toX)
                    curX = toX;
            }

            if (beginShear)
            {
                curScale += scaleChangeValue;
                if (curScale >= 1)
                {
                    curScale = 1;
                    beginShear = false; 
                }
                curShear = GetShear(curScale);
            }
            else
            {
                curScale -= scaleChangeValue;
                if (curScale <= scaleX)
                {
                    curScale = scaleX;
                    beginShear = true;
                }
                curShear = shearY;
            }

            SetTransform(transform, curX, curScale, curShear);
        }

        private void SetTransform(Matrix transform, float x, float scale, float shear)
        {
            transform.Reset();
            //x平移量
            offsetX = x - fromX;

            //最后剪切shear
            float tempPivot = shearPivot + offsetX;
            transform.Shear(0, shear);
            transform.Translate(0, -tempPivot * shear);

            //其次平移
            transform.Translate(offsetX, 0);

            //先缩放
            offsetX = scalePivot * scale - scalePivot;
            transform.Translate(-offsetX, 0);
            transform.Scale(scale, 1);
        }

        private float GetShear(float scale)
        {
            //将[scale, 1]映射到[shearY, 0]
            float shear = 0;
            if (scaleX != 1)
            {
                shear = (1 - scale) * shearY / (1 - scaleX);
            }
            return shear;
        }

        public override void Reset()
        {
            base.Reset();
            curX = fromX;
            curShear = shearY;
            curScale = 1;
            beginShear = false;
            stepCount = 0;
        }

        public override void Cancel()
        {
            
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (stepCount == steps)
                isEnd = true;
            return isEnd;
        }
    }
}

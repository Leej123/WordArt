using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 文字缩放
    /// </summary>
    class TextScalingAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 0.05f;

        private float fromX;
        private float toX;

        private float curX;
        private float curValue = 0;//其值在[-1,1]中
        private bool add = true;

        private float pivotX, pivotY;

        private float offsetX;
        private float offsetY;

        public TextScalingAnimation(float fromX, float toX, float pivotX, float pivotY) : base()
        {
            this.fromX = fromX;
            this.toX = toX;

            this.pivotX = pivotX;
            this.pivotY = pivotY;

            curX = fromX;
            curValue = 0;
            add = true;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                transform.Scale(0, 0);
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
            if (add)
            {
                curValue += CHANGE_VALUE;
                if (curValue >= 1)
                {
                    curValue = 1;
                    add = false;
                }
            }
            else
            {
                curValue -= CHANGE_VALUE;
                if (curValue <= -1)
                {
                    curValue = -1;
                    add = true;
                }
            }

            SetTransform(transform, Math.Abs(curValue));
        }

        private void SetTransform(Matrix transform, float scale)
        {
            transform.Reset();
            float curX = GetX(scale);
            transform.Translate(curX - fromX, 0);

            offsetX = pivotX * scale - pivotX;
            offsetY = pivotY * scale - pivotY;
            transform.Translate(-offsetX, -offsetY);
            transform.Scale(scale, scale);
        }

        private float GetX(float scale)
        {
            return scale * (toX - fromX) + fromX;
        }

        public override void Reset()
        {
            base.Reset();
            curX = fromX;
            curValue = 0;
            add = true;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (curValue == -1)
                isEnd = true;
            return isEnd;
        }
    }
}

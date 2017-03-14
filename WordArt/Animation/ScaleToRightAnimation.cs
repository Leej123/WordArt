using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 向右放大动画
    /// </summary>
    public class ScaleToRightAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 0.03f;

        private float fromScale;
        private float toScale;

        private float curScale;

        private float pivotX;
        private float offsetX;

        public ScaleToRightAnimation(float fromScale, float toScale, float pivotX) : base()
        {
            this.fromScale = fromScale;
            this.toScale = toScale;
            this.pivotX = pivotX;
            this.offsetX = 0;
            this.curScale = fromScale;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (IsAnimationEnd())
            {
                isStillRun = false;
                SetTransform(toScale);
                return transform;
            }

            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                transform.Scale(0, 0);
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            if (toScale != fromScale)
            {
                if (fromScale < toScale)
                {
                    curScale += CHANGE_VALUE;
                    if (curScale > toScale)
                    {
                        curScale = toScale;
                    }
                }
                else
                {
                    curScale -= CHANGE_VALUE;
                    if (curScale < toScale)
                    {
                        curScale = toScale;
                    }
                }
            }
            else
            {
                curScale = toScale;
            }

            SetTransform(curScale);
        }

        protected virtual void SetTransform(float scale)
        {
            transform.Reset();
            offsetX = pivotX * scale - pivotX;

            transform.Translate(-offsetX, 0);
            transform.Scale(scale, 1);
        }

        public override void Reset()
        {
            base.Reset();
            curScale = fromScale;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (curScale == toScale)
                isEnd = true;
            return isEnd;
        }
    }
}

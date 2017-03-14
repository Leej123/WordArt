using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 放大动画
    /// </summary>
    public class ScaleAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 0.05f;

        /// <summary>
        /// 开始时放大倍数
        /// </summary>
        protected float fromScale;

        /// <summary>
        /// 结束时的放大倍数
        /// </summary>
        protected float toScale;

        private float pivotX;
        private float pivotY;

        private float offsetX;
        private float offsetY;

        protected float curScale;

        public ScaleAnimation(float from, float to, float pivotX, float pivotY) : base()
        {
            this.fromScale = from;
            this.toScale = to;
            this.pivotX = pivotX;
            this.pivotY = pivotY;
            curScale = fromScale;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                SetTransform(transform, fromScale);
                return transform;
            }
            if (IsAnimationEnd())
            {
                isStillRun = false;
                transform.Reset();
                SetTransform(transform, toScale);
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            transform.Reset();
            if (fromScale == toScale)
            {
                curScale = fromScale;
            }
            else if (fromScale < toScale)
            {
                curScale += CHANGE_VALUE;
                if (curScale >= toScale)
                {
                    curScale = toScale;
                }
            }
            else
            {
                curScale -= CHANGE_VALUE;
                if (curScale <= toScale)
                {
                    curScale = toScale;
                }
            }
            SetTransform(transform, curScale);
        }

        protected virtual void SetTransform(System.Drawing.Drawing2D.Matrix transform, float scale)
        {
            offsetX = pivotX * scale - pivotX;
            offsetY = pivotY * scale - pivotY;

            transform.Translate(-offsetX, -offsetY);
            transform.Scale(scale, scale);
        }

        public override void Start()
        {
            base.Start();
            curScale = fromScale;
        }

        public override void Reset()
        {
            base.Reset();
            curScale = fromScale;
        }

        public override void Cancel()
        {
            curScale = toScale;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (curScale == toScale)
            {
                isEnd = true;
            }
            return isEnd;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 黑洞动画
    /// </summary>
    public class BlackHoleAnimation : ScaleAnimation
    {
        private BlackHoleAnimation nextAnimation;
        public BlackHoleAnimation NextAnimation
        {
            set { nextAnimation = value; }
        }

        public BlackHoleAnimation(float from, float to, float pivotX, float pivotY)
            : base(from, to, pivotX, pivotY)
        { 
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
            if (curScale == toScale)
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

        protected override void SetTransform(System.Drawing.Drawing2D.Matrix transform, float scale)
        {
            if (scale < 0) scale = 0;
            base.SetTransform(transform, scale);
        }

        public override bool IsAnimationEnd()
        {
            if (curScale != toScale)
            {
                isEnd = false;
                return false;
            }
               
            if (nextAnimation == null)
            {
                isEnd = true;
            }
            else
            {
                isEnd = nextAnimation.IsAnimationEnd();
            }

            return isEnd;
        }
    }
}

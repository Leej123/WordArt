using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 蛙跳配合的动画
    /// </summary>
    public class FrogAnimation : TextAnimation
    {
        private const int STEPS = 5;
        private float pivotY;
        private bool show = true;//开始时显示还是不显示
        
        private int stepCount = 0;

        public FrogAnimation(float pivotY, bool show) : base()
        {
            this.pivotY = pivotY;
            this.show = show;
            stepCount = 0;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                if (!show)
                {
                    transform.Scale(0, 0);
                }
                return transform;
            }

            if (IsAnimationEnd())
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }

            isStillRun = true;
            ApplyTransformation(transform);
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            stepCount++;

            float scale = 1 - 0.2f * (stepCount - 1);
            float offset = pivotY * scale - pivotY;
            transform.Reset();
            transform.Translate(0, -offset);
            transform.Scale(1, scale);
        }

        public override void Reset()
        {
            base.Reset();
            stepCount = 0;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (stepCount == STEPS)
                isEnd = true;
            return isEnd;
        }
    }
}

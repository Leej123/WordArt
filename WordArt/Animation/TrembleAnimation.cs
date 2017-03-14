using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 颤抖
    /// </summary>
    public class TrembleAnimation : TextAnimation
    {
        private Random random;
        private int repeatCount = 0;

        private int frameCount = 2;

        private float offsetX, offsetY;

        public TrembleAnimation(Random random) : base()
        {
            this.random = random;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || IsAnimationEnd())
            {
                transform.Reset();
                transform.Translate(0, 0);
                isStillRun = false;
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            if (frameCount < 2)
            {
                frameCount++;
                return;
            }

            offsetX = random.Next(-3, 3);
            offsetY = random.Next(-3, 3);

            transform.Reset();
            transform.Translate(offsetX, offsetY);
            repeatCount++;
            frameCount = 0;
        }

        public override void Reset()
        {
            base.Reset();
            frameCount = 0;
            repeatCount = 0;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (repeatCount == 20)
                isEnd = true;
            return isEnd;
        }
    }
}

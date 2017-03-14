using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 依次翻转动画
    /// </summary>
    public class FlipAnimation : TextAnimation
    {
        private static float SPEED = (float)(Math.PI / 30);

        /// <summary>
        /// 初始相位
        /// </summary>
        private float initPhase;

        /// <summary>
        /// 延迟值
        /// </summary>
        private int delay;

        /// <summary>
        /// 设置y方向放大时的基点
        /// </summary>
        private float pivotY;
        private float offsetY;

        private int delayCount = 0;
        private float phase = 0;

        private float curScale = -1;

        private FlipAnimation nextAnimation = null;
        public FlipAnimation NextAnimation
        {
            set { nextAnimation = value; }
        }

        public FlipAnimation(float initPhase, float pivotY, int delay) : base()
        {
            this.initPhase = initPhase;
            this.pivotY = pivotY;
            this.phase = initPhase;
            this.delay = delay;
            this.delayCount = 0;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (curScale == 1)//动画结束
            {
                isStillRun = false;
                transform.Reset();
                transform.Scale(1, 1);
                return transform;
            }

            if (delayCount < delay)
            {
                transform.Reset();
                transform.Scale(0, 0);
                delayCount++;
            }
            else
            {
                ApplyTransformation(transform);

            }
            isStillRun = true;

            
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            curScale = (float) Math.Sin(phase);
            if (curScale >= 0.95) curScale = 1;
            SetTransform(curScale);
            phase -= SPEED;
        }

        private void SetTransform(float scale)
        {
            transform.Reset();
            offsetY = pivotY * scale - pivotY;

            transform.Translate(0, -offsetY);
            transform.Scale(1, scale);
        }

        public override void Reset()
        {
            base.Reset();
            phase = initPhase;
            delayCount = 0;
            curScale = -1;
        }

        public override bool IsAnimationEnd()
        {
            //isEnd = false;
            //if (preAnimation == null && curScale == 1)
            //{
            //    isEnd = true;
            //}

            //if (preAnimation != null && preAnimation.IsAnimationEnd() && curScale == 1)
            //{
            //    isEnd = true;
            //}
            
            //结束条件
            isEnd = false;
            if (nextAnimation == null)
            {
                if (curScale == 1)
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

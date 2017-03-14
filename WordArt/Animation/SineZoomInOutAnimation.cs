using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 向上放大缩小动画
    /// </summary>
    class SineZoomInOutAnimation : TextAnimation
    {
        private static float SPEED = (float)(Math.PI / 30);

        /// <summary>
        /// 初始相位
        /// </summary>
        private float initPhase = 0;

        /// <summary>
        /// 最大缩放值
        /// </summary>
        private float maxScale;

        /// <summary>
        /// 最小缩放值
        /// </summary>
        private float minScale;

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

        private bool isCancel = false;

        public SineZoomInOutAnimation(float initPhase, float maxScale, float minScale, float pivotY, int delay) : base()
        {
            this.initPhase = initPhase;
            this.maxScale = maxScale;
            this.minScale = minScale;
            this.delay = delay;
            this.pivotY = pivotY;

            offsetY = 0;

            phase = initPhase;
            delayCount = 0;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                transform.Reset();
                transform.Scale(0, 0);
                isStillRun = false;
                return transform;
            }

            if (IsAnimationEnd() || isCancel)
            {
                isStillRun = false;
                SetTransform(1);
                return transform;
            }

            if (delayCount < delay)
            {
                delayCount++;
                isStillRun = true;
                transform.Reset();
                transform.Scale(0, 0);
            }
            else
            {
                ApplyTransformation(transform);
                isStillRun = true;
            }
            phase -= SPEED;
            //phase = (float) (phase %  (2 * Math.PI));
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            float value = (float) Math.Sin(phase);
            float curScale = (value + 1) * (maxScale - minScale) / 2 + minScale;//将[-1, 1]的值，映射到[minScale, maxScale]
            SetTransform(curScale);
        }

        private void SetTransform(float curScale)
        {
            transform.Reset();
            offsetY = pivotY * curScale - pivotY;

            transform.Translate(0, -offsetY);
            transform.Scale(1, curScale);
        }

        public override void Cancel()
        {
            isCancel = true;
        }

        public override void Start()
        {
            base.Start();
            isCancel = false;
            delayCount = 0;
            phase = initPhase;
        }

        public override void Reset()
        {
            base.Reset();
            isCancel = false;
            phase = initPhase;
            delayCount = 0;
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (phase >= 6 * Math.PI || phase <= -6 * Math.PI)
            {
                isEnd = true;
            }

            return isEnd;
        }
    }
}

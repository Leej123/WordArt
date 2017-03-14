using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 波浪动画
    /// </summary>
    public class WaveAnimation : TextAnimation
    {
        private static float SPEED = (float) (Math.PI / 30);

        /// <summary>
        /// 初始位置
        /// </summary>
        private float initPos;

        /// <summary>
        /// 初始相位
        /// </summary>
        private float initPhase;

        /// <summary>
        /// 当前相位
        /// </summary>
        private float currentPhase;

        /// <summary>
        /// 振幅
        /// </summary>
        private float amplitude;

        private float value;
        private float sineValue;

        public WaveAnimation(float pos, float amplitude, float phase) : base()
        {
            this.initPos = pos;
            this.amplitude = amplitude;
            this.initPhase = phase;
            currentPhase = initPhase;
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
            transform.Reset();
            if (IsAnimationEnd())
            {
                return;
            }

            sineValue = (float) Math.Sin(currentPhase);
            value = sineValue * amplitude;
            currentPhase -= SPEED;
            //currentPhase = (float)(currentPhase % (2 * Math.PI));
            transform.Translate(0, value);
        }

        public override void Reset()
        {
            transform.Reset();
            currentPhase = initPhase;
            base.Reset();
            
        }

        public override bool IsAnimationEnd()
        {
            isEnd = false;
            if (currentPhase >= 4 * Math.PI || currentPhase <= -4 * Math.PI)
                isEnd = true;
            return isEnd;
        }
    }
}

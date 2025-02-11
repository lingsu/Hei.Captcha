﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.Captcha
{
    public class Options
    {
        /// <summary>
        /// 指定字体目录。若为空，从操作系统中随机获取4个字体
        /// </summary>
        public string FontPath { get; set; } = "fonts";

        /// <summary>
        /// 浅色线条数量
        /// </summary>
        public int LightGrids { get; set; } = 8;

        /// <summary>
        /// 字符上面的线条折点数量。若小于等于0，字符上面不画线条
        /// </summary>
        public int Inflection { get; set; } = 6;

        /// <summary>
        /// 字符上面的线条粗度
        /// </summary>
        public float GridThickness { get; set; } = 1F;

        /// <summary>
        /// 字符上面的线条透明度
        /// </summary>
        public float GridAlpha { get; set; } = 1F;

        /// <summary>
        /// 气泡数量
        /// </summary>
        public int Circles { get; set; } = 15;

        /// <summary>
        /// 高斯模糊
        /// </summary>
        public float GaussianBlur { get; set; } = 0.4F;

        /// <summary>
        /// 文字旋转的最大角度
        /// </summary>
        public int Rotate { get; set; } = 45;

        /// <summary>
        /// 字符大小浮动范围
        /// </summary>
        public float FontSize { get; set; } = 0.1F;
    }
}

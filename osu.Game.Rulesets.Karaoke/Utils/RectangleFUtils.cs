﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics.Primitives;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class RectangleFUtils
    {
        /// <summary>Creates the smallest possible third rectangle that can contain both of multi rectangles that form a union.</summary>
        /// <returns>A third <see cref="rectangles"/> structure that contains both of the multi rectangles that form the union.</returns>
        /// <filterpriority>1</filterpriority>
        public static RectangleF Union(RectangleF[] rectangles)
        {
            if (rectangles == null || rectangles.Length == 0)
                return new RectangleF();

            var result = rectangles.FirstOrDefault();

            foreach (var rectangle in rectangles)
            {
                result = RectangleF.Union(result, rectangle);
            }

            return result;
        }
    }
}

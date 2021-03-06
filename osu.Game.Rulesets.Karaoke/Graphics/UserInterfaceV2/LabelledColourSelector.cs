﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    public class LabelledColourSelector : LabelledComponent<BasicHSVColourPicker, Colour4>
    {
        public LabelledColourSelector()
            : base(true)
        {
        }

        protected override BasicHSVColourPicker CreateComponent()
            => new BasicHSVColourPicker();
    }
}

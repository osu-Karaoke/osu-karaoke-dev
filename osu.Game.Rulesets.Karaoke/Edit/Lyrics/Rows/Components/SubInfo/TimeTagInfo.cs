﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.SubInfo
{
    public class TimeTagInfo : SubInfo
    {
        public TimeTagInfo(Lyric lyric)
            : base(lyric)
        {
            lyric.TimeTagsBindable.BindValueChanged(value =>
            {
                BadgeText = LyricUtils.TimeTagTimeFormattedString(Lyric);
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.Green;
        }
    }
}

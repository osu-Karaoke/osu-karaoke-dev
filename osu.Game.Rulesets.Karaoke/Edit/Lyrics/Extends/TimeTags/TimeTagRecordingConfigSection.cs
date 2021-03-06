﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagRecordingConfigSection : Section
    {
        protected override string Title => "Config";

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            Children = new[]
            {
                new LabelledDropdown<RecordingMovingCaretMode>
                {
                    Label = "Record tag",
                    Description = "Only record time with start/end time-tag while recording.",
                    Current = state.BindableRecordingMovingCaretMode,
                    Items = EnumUtils.GetValues<RecordingMovingCaretMode>(),
                }
            };
        }
    }
}

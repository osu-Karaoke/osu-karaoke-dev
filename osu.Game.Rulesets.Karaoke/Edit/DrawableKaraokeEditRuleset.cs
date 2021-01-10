﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class DrawableKaraokeEditRuleset : DrawableKaraokeRuleset
    {
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();

        public new IScrollingInfo ScrollingInfo => base.ScrollingInfo;

        public override bool DisplayNotePlayfield => true;

        public DrawableKaraokeEditRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
            bindableEditMode.BindValueChanged(e =>
            {
                if (e.NewValue == EditMode.LyricEditor)
                    Playfield.Hide();
                else
                    Playfield.Show();
            }, true);
        }

        protected override void InitialOverlay()
        {
        }

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h) => null;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key != Key.S)
                return base.OnKeyDown(e);

            string directory = Path.Combine(Path.GetTempPath(), @"osu!");
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "karaoke.txt");

            using (var sw = new StreamWriter(path))
            {
                var encoder = new KaraokeLegacyBeatmapEncoder();
                sw.WriteLine(encoder.Encode(new Beatmap
                {
                    HitObjects = Beatmap.HitObjects.OfType<HitObject>().ToList()
                }));
            }

            return true;
        }

        protected override Playfield CreatePlayfield() => new KaraokeEditPlayfield();

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetEditConfigManager editConfigManager)
        {
            editConfigManager.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
        }
    }
}

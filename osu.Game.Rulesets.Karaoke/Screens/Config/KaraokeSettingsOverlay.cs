﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Config.Sections;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class KaraokeSettingsOverlay : SettingsPanel
    {
        protected override IEnumerable<SettingsSection> CreateSections() => new SettingsSection[]
        {
            new ConfigSection(),
            new StyleSection(),
            new ScoringSection()
        };

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            var config = dependencies.Get<RulesetConfigCache>().GetConfigFor(new KaraokeRuleset());
            if (config != null)
                dependencies.Cache(config);

            return dependencies;
        }
    }
}
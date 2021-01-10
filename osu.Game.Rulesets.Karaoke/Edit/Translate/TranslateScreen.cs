﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class TranslateScreen : EditorSubScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly TranslateManager TranslateManager;

        public TranslateScreen()
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Green);
            Content.Add(TranslateManager = new TranslateManager());
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(50),
                Child = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = colours.GreySeafoamDark,
                            RelativeSizeAxes = Axes.Both,
                        },
                        new SectionsContainer<Container>
                        {
                            FixedHeader = new TranslateScreenHeader(),
                            RelativeSizeAxes = Axes.Both,
                            Children = new Container[]
                            {
                                new TranslateEditSection
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                },
                            }
                        },
                    }
                }
            };
        }

        internal class TranslateScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new TranslateScreenTitle();

            private class TranslateScreenTitle : OverlayTitle
            {
                public TranslateScreenTitle()
                {
                    Title = "translate";
                    Description = "create translate of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}

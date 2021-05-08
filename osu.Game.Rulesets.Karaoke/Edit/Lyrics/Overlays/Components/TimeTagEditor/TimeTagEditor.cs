﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components.Timeline;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    [Cached]
    public class TimeTagEditor : ZoomableScrollContainer
    {
        private const float timeline_height = 48;

        private readonly IBindable<TimeTag[]> timeTags = new Bindable<TimeTag[]>();

        [Resolved]
        private EditorClock editorClock { get; set; }

        public readonly Lyric HitObject;

        public TimeTagEditor(Lyric lyric)
        {
            HitObject = lyric;

            RelativeSizeAxes = Axes.X;
            Height = timeline_height;

            ZoomDuration = 200;
            ZoomEasing = Easing.OutQuint;
            ScrollbarVisible = false;

            timeTags.BindTo(lyric.TimeTagsBindable);
        }

        private Container mainContent;

        private CurrentTimeMarker currentTimeMarker;

        private TimelineTickDisplay ticks;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddRange(new Drawable[]
            {
                mainContent = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = timeline_height,
                    Depth = float.MaxValue,
                    Children = new Drawable[]
                    {
                        ticks = new TimelineTickDisplay(),
                        new TimeTagEditorBlueprintContainer(HitObject),
                        currentTimeMarker = new CurrentTimeMarker(),
                    }
                },
            });

            // initialize time-tag if value changed.
            timeTags.BindValueChanged(e =>
            {
                var timeTags = e.NewValue;
            });

            // initialize scroll zone.
            MaxZoom = getZoomLevelForVisibleMilliseconds(500);
            MinZoom = getZoomLevelForVisibleMilliseconds(10000);
            Zoom = getZoomLevelForVisibleMilliseconds(2000);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var position = getPositionFromTime(HitObject.LyricStartTime);
            ScrollTo(position, false);
        }

        private float getZoomLevelForVisibleMilliseconds(double milliseconds) => Math.Max(1, (float)(editorClock.TrackLength / milliseconds));

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            var position = getPositionFromTime(editorClock.CurrentTime);
            currentTimeMarker.MoveToX(position);
        }

        private float getPositionFromTime(double time)
            => (float)(time / editorClock.TrackLength) * Content.DrawWidth;
    }
}
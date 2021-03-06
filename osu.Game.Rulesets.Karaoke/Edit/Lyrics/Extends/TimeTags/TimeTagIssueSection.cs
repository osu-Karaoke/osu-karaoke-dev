﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagIssueSection : Section
    {
        protected override string Title => "Invalid time-tag";

        private BindableDictionary<Lyric, Issue[]> bindableReports;

        private TimeTagIssueTable table;

        [BackgroundDependencyLoader]
        private void load(LyricCheckerManager lyricCheckerManager)
        {
            Children = new[]
            {
                table = new TimeTagIssueTable(),
            };

            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((a, b) =>
            {
                // todo : might have filter in here.
                var issues = bindableReports.Values.SelectMany(x => x);
                table.Issues = issues.OfType<TimeTagIssue>();
            }, true);
        }

        public class TimeTagIssueTable : IssueTableContainer
        {
            [Resolved]
            private OsuColour colour { get; set; }

            public IEnumerable<TimeTagIssue> Issues
            {
                set
                {
                    Content = null;
                    BackgroundFlow.Clear();

                    if (value == null)
                        return;

                    Content = value.Select((g, i) =>
                    {
                        var lyric = g.HitObjects.FirstOrDefault() as Lyric;

                        var rows = new List<Drawable[]>();

                        if (g.MissingStartTimeTag)
                            rows.Add(createMissingStartOrEndTimeTagContent(lyric));

                        if (g.MissingEndTimeTag)
                            rows.Add(createMissingStartOrEndTimeTagContent(lyric));

                        foreach (var (invalidReason, timeTags) in g.InvalidTimeTags)
                        {
                            rows.AddRange(timeTags.Select(timeTag => createContent(lyric, timeTag, invalidReason)));
                        }

                        return rows;
                    }).SelectMany(x => x).ToArray().ToRectangular();

                    BackgroundFlow.Children = value.Select((g, i) =>
                    {
                        var lyric = g.HitObjects.FirstOrDefault() as Lyric;

                        var rows = new List<RowBackground>();

                        if (g.MissingStartTimeTag)
                            rows.Add(new TimeTagRowBackground(lyric, null));

                        if (g.MissingEndTimeTag)
                            rows.Add(new TimeTagRowBackground(lyric, null));

                        foreach (var (_, timeTags) in g.InvalidTimeTags)
                        {
                            rows.AddRange(timeTags.Select(timeTag => new TimeTagRowBackground(lyric, timeTag)).Cast<RowBackground>());
                        }

                        return rows;
                    }).SelectMany(x => x).ToArray();
                }
            }

            protected override TableColumn[] CreateHeaders() => new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Lyric", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
                new TableColumn("Position", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 60)),
                new TableColumn("Message", Anchor.CentreLeft),
            };

            private Drawable[] createContent(Lyric lyric, TimeTag timeTag, TimeTagInvalid invalid) => new Drawable[]
            {
                new RightTriangle
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(10),
                    Colour = getInvalidColour(invalid),
                    Margin = new MarginPadding { Left = 10 },
                },
                new OsuSpriteText
                {
                    Text = $"#{lyric.Order}",
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Text = TextIndexUtils.PositionFormattedString(timeTag.Index),
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Text = getInvalidReason(invalid),
                    Truncate = true,
                    RelativeSizeAxes = Axes.X,
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium)
                },
            };

            private Drawable[] createMissingStartOrEndTimeTagContent(Lyric lyric) => new Drawable[]
            {
                new SpriteIcon
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(10),
                    Colour = colour.Red,
                    Margin = new MarginPadding { Left = 10 },
                    Icon = FontAwesome.Solid.AlignLeft
                },
                new OsuSpriteText
                {
                    Text = $"#{lyric.Order}",
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Text = "Missing end time-tag in lyric.",
                    Truncate = true,
                    RelativeSizeAxes = Axes.X,
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium)
                },
            };

            private Color4 getInvalidColour(TimeTagInvalid invalid)
            {
                switch (invalid)
                {
                    case TimeTagInvalid.OutOfRange:
                        return colour.Red;

                    case TimeTagInvalid.Overlapping:
                        return colour.Red;

                    case TimeTagInvalid.EmptyTime:
                        return colour.Yellow;

                    default:
                        throw new IndexOutOfRangeException(nameof(invalid));
                }
            }

            private string getInvalidReason(TimeTagInvalid invalid)
            {
                switch (invalid)
                {
                    case TimeTagInvalid.OutOfRange:
                        return "Time-tag out of range.";

                    case TimeTagInvalid.Overlapping:
                        return "Time-tag overlapping.";

                    case TimeTagInvalid.EmptyTime:
                        return "Time-tag has no time.";

                    default:
                        throw new IndexOutOfRangeException(nameof(invalid));
                }
            }

            public class TimeTagRowBackground : RowBackground
            {
                private readonly Lyric lyric;
                private readonly TimeTag timeTag;

                [Resolved]
                private EditorClock clock { get; set; }

                public TimeTagRowBackground(Lyric lyric, TimeTag timeTag)
                {
                    this.lyric = lyric;
                    this.timeTag = timeTag;
                }

                private BindableList<TimeTag> selectedTimeTags;

                [BackgroundDependencyLoader]
                private void load(ILyricEditorState state, LyricCaretState lyricCaretState, BlueprintSelectionState blueprintSelectionState)
                {
                    // update selected state by bindable.
                    selectedTimeTags = blueprintSelectionState.SelectedTimeTags.GetBoundCopy();
                    selectedTimeTags.BindCollectionChanged((a, b) =>
                    {
                        var selected = selectedTimeTags.Contains(timeTag);
                        UpdateState(selected);
                    });

                    Action = () =>
                    {
                        // navigate to current lyric.
                        switch (state.Mode)
                        {
                            case LyricEditorMode.CreateTimeTag:
                                lyricCaretState.BindableCaretPosition.Value = new TimeTagIndexCaretPosition(lyric, timeTag?.Index ?? new TextIndex());
                                break;

                            case LyricEditorMode.RecordTimeTag:
                                lyricCaretState.BindableCaretPosition.Value = new TimeTagCaretPosition(lyric, timeTag);
                                break;

                            case LyricEditorMode.AdjustTimeTag:
                                lyricCaretState.BindableCaretPosition.Value = new NavigateCaretPosition(lyric);
                                break;

                            default:
                                throw new IndexOutOfRangeException(nameof(state.Mode));
                        }

                        // set current time-tag as selected.
                        selectedTimeTags.Clear();
                        if (timeTag == null)
                            return;

                        // select time-tag is not null.
                        selectedTimeTags.Add(timeTag);
                        if (timeTag.Time == null)
                            return;

                        // seek to target time-tag time if time-tag has time.
                        clock.Seek(timeTag.Time.Value);
                    };
                }
            }
        }
    }
}

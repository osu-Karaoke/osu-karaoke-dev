﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Export
{
    public class ExportLyricManager : Component
    {
        [Resolved]
        private Storage storage { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public void ExportToLrc()
        {
            var exportStorage = storage.GetStorageForDirectory("lrc");
            using (var outputStream = exportStorage.GetStream($"{beatmap.Name}.lrc", FileAccess.Write, FileMode.Create))
            using (var sw = new StreamWriter(outputStream))
            {
                var encoder = new LrcEncoder();
                sw.WriteLine(encoder.Encode(new Beatmap
                {
                    HitObjects = beatmap.HitObjects.OfType<HitObject>().ToList()
                }));
            }
            exportStorage.OpenInNativeExplorer();
        }

        public void ExportToText()
        {
            var exportStorage = storage.GetStorageForDirectory("text");
            using (var outputStream = exportStorage.GetStream($"{beatmap.Name}.txt", FileAccess.Write, FileMode.Create))
            using (var sw = new StreamWriter(outputStream))
            {
                var encoder = new LyricTextEncoder();
                sw.WriteLine(encoder.Encode(new Beatmap
                {
                    HitObjects = beatmap.HitObjects.OfType<HitObject>().ToList()
                }));
            }
            exportStorage.OpenInNativeExplorer();
        }
    }
}
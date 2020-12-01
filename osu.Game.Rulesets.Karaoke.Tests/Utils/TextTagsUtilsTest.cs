﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    [TestFixture]
    public class TextTagsUtilsTest
    {
        private const string lyric = "Test lyric";

        [TestCase(nameof(ValidTextTagWithSorted), TextTagsUtils.Sorting.Asc, new int[] { 0, 1, 1, 2, 2, 3 })]
        [TestCase(nameof(ValidTextTagWithSorted), TextTagsUtils.Sorting.Desc, new int[] { 2, 3, 1, 2, 0, 1 })]
        [TestCase(nameof(ValidTextTagWithUnsorted), TextTagsUtils.Sorting.Asc, new int[] { 0, 1, 1, 2, 2, 3 })]
        [TestCase(nameof(ValidTextTagWithUnsorted), TextTagsUtils.Sorting.Desc, new int[] { 2, 3, 1, 2, 0, 1 })]
        public void TestSort(string testCase, TextTagsUtils.Sorting sorting, int[] results)
        {
            var textTags = getValueByMethodName(testCase);

            var sortedTextTags = TextTagsUtils.Sort(textTags, sorting);
            for (int i = 0; i < sortedTextTags.Length; i++)
            {
                // result would be start, end, start, end...
                Assert.AreEqual(sortedTextTags[i].StartIndex, results[i * 2]);
                Assert.AreEqual(sortedTextTags[i].EndIndex, results[i * 2 + 1]);
            }
        }

        [TestCase(nameof(ValidTextTagWithSorted), TextTagsUtils.Sorting.Asc, new int[] { })]
        [TestCase(nameof(ValidTextTagWithUnsorted), TextTagsUtils.Sorting.Asc, new int[] { })]
        [TestCase(nameof(InvalidTextTagWithSameStartAndEndIndex), TextTagsUtils.Sorting.Asc, new int[] {0 })]
        [TestCase(nameof(InvalidTextTagWithWrongIndex), TextTagsUtils.Sorting.Asc, new int[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithNegativeIndex), TextTagsUtils.Sorting.Asc, new int[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithEndLargerThenNextStart), TextTagsUtils.Sorting.Asc, new int[] { 1 })]
        [TestCase(nameof(InvalidTextTagWithEndLargerThenNextStart), TextTagsUtils.Sorting.Desc, new int[] { 0 })]
        [TestCase(nameof(InvalidTextTagWithWrapNextTextTag), TextTagsUtils.Sorting.Asc, new int[] { 1 })]
        [TestCase(nameof(InvalidTextTagWithWrapNextTextTag), TextTagsUtils.Sorting.Desc, new int[] { 0 })]

        public void TestFindInvalid(string testCase, TextTagsUtils.Sorting sorting, int[] errorIndex)
        {
            var textTags = getValueByMethodName(testCase);

            // run all and find invalid indexes.
            var invalidTextTag = TextTagsUtils.FindInvalid(textTags, lyric, sorting);
            var invalidIndexes = invalidTextTag.Select(v => textTags.IndexOf(v)).ToArray();
            Assert.AreEqual(invalidIndexes, errorIndex);
        }

        private RubyTag[] getValueByMethodName(string methodName)
        {
            Type thisType = GetType();
            var theMethod = thisType.GetMethod(methodName);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.Invoke(this, null) as RubyTag[];
        }

        #region valid source

        public static RubyTag[] ValidTextTagWithSorted()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 1 },
                new RubyTag { StartIndex = 1, EndIndex = 2 },
                new RubyTag { StartIndex = 2, EndIndex = 3 }
            };

        public static RubyTag[] ValidTextTagWithUnsorted()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 1 },
                new RubyTag { StartIndex = 2, EndIndex = 3 },
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        #endregion

        #region invalid source

        public static RubyTag[] InvalidTextTagWithWrongIndex()
            => new[]
            {
                new RubyTag { StartIndex = 1, EndIndex = 0 },
            };

        public static RubyTag[] InvalidTextTagWithNegativeIndex()
            => new[]
            {
                new RubyTag { StartIndex = -1, EndIndex = 0 },
            };

        public static RubyTag[] InvalidTextTagWithSameStartAndEndIndex()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 0 }, // Same number.
            };

        public static RubyTag[] InvalidTextTagWithEndLargerThenNextStart()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 2 }, // End is larger than second start.
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        public static RubyTag[] InvalidTextTagWithWrapNextTextTag()
            => new[]
            {
                new RubyTag { StartIndex = 0, EndIndex = 3 }, // Wrap second text tag.
                new RubyTag { StartIndex = 1, EndIndex = 2 }
            };

        #endregion
    }
}
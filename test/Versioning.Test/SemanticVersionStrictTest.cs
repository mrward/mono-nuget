﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Moq;
using NuGet.Test;
using NuGet.Test.Mocks;
using Xunit;
using NuGet.Versioning;
using Xunit.Extensions;

namespace NuGet.Test
{
    public class SemanticVersionStrictTest
    {
        [Theory]
        [InlineData("1.0.0")]
        [InlineData("0.0.1")]
        [InlineData("1.2.3")]
        [InlineData("1.2.3-alpha")]
        [InlineData("1.2.3-X.yZ.3.234.243.32423423.4.23423.4324.234.234.3242")]
        [InlineData("1.2.3-X.yZ.3.234.243.32423423.4.23423+METADATA")]
        [InlineData("1.2.3-X.y3+0")]
        [InlineData("1.2.3-X+0")]
        [InlineData("1.2.3+0")]
        [InlineData("1.2.3-0")]
        public void ParseSemanticVersionStrict(string versionString)
        {
            // Act
            SemanticVersionStrict semVer = null;
            SemanticVersionStrict.TryParse(versionString, out semVer);

            // Assert
            Assert.Equal<string>(versionString, semVer.ToNormalizedString());
            Assert.Equal<string>(versionString, semVer.ToString());
        }

        [Theory]
        [InlineData("1.2.3")]
        [InlineData("1.2.3+0")]
        [InlineData("1.2.3+321")]
        [InlineData("1.2.3+XYZ")]
        public void SemanticVersionStrictEquality(string versionString)
        {
            // Act
            SemanticVersionStrict main = null;
            SemanticVersionStrict.TryParse("1.2.3", out main);

            SemanticVersionStrict semVer = null;
            SemanticVersionStrict.TryParse(versionString, out semVer);

            // Assert
            Assert.True(main == semVer);
            Assert.True(main.Equals(semVer));
            Assert.True(main <= (semVer));
            Assert.True(main >= (semVer));
            Assert.True(main.CompareTo(semVer) == 0);
            Assert.False(main != (semVer));
            Assert.False(main < (semVer));
            Assert.False(main > (semVer));

            Assert.True(semVer == main);
            Assert.True(semVer.Equals(main));
            Assert.True(semVer <= (main));
            Assert.True(semVer >= (main));
            Assert.True(semVer.CompareTo(main) == 0);
            Assert.False(semVer != (main));
            Assert.False(semVer < (main));
            Assert.False(semVer > (main));

            Assert.True(main.GetHashCode() == semVer.GetHashCode());
        }

        [Theory]
        [InlineData("1.2.3-alpha")]
        [InlineData("1.2.3-alpha+0")]
        [InlineData("1.2.3-alpha+10")]
        [InlineData("1.2.3-alpha+beta")]
        public void SemanticVersionStrictEqualityPreRelease(string versionString)
        {
            // Act
            SemanticVersionStrict main = null;
            SemanticVersionStrict.TryParse("1.2.3-alpha", out main);

            SemanticVersionStrict semVer = null;
            SemanticVersionStrict.TryParse(versionString, out semVer);

            // Assert
            Assert.True(main == semVer);
            Assert.True(main.Equals(semVer));
            Assert.True(main <= (semVer));
            Assert.True(main >= (semVer));
            Assert.True(main.CompareTo(semVer) == 0);
            Assert.False(main != (semVer));
            Assert.False(main < (semVer));
            Assert.False(main > (semVer));

            Assert.True(semVer == main);
            Assert.True(semVer.Equals(main));
            Assert.True(semVer <= (main));
            Assert.True(semVer >= (main));
            Assert.True(semVer.CompareTo(main) == 0);
            Assert.False(semVer != (main));
            Assert.False(semVer < (main));
            Assert.False(semVer > (main));

            Assert.True(main.GetHashCode() == semVer.GetHashCode());
        }

        [Theory]
        [InlineData("2.7")]
        [InlineData("1.3.4.5")]
        [InlineData("1.3-alpha")]
        [InlineData("1.3 .4")]
        [InlineData("2.3.18.2-a")]
        [InlineData("1.2.3-A..B")]
        [InlineData("01.2.3")]
        [InlineData("1.02.3")]
        [InlineData("1.2.03")]
        [InlineData(".2.03")]
        [InlineData("1.2.")]
        [InlineData("1.2.3-a.b.c+0.0")]
        [InlineData("1.2.3-a$b")]
        [InlineData("a.b.c")]
        //[InlineData("1.2.3-00")] // TODO: fix the semver regex so these are invalid
        //[InlineData("1.2.3-A.00.B")]
        public void TryParseStrictReturnsFalseIfVersionIsNotStrictSemVer(string version)
        {
            // Act 
            SemanticVersionStrict semanticVersion;
            bool result = SemanticVersionStrict.TryParse(version, out semanticVersion);

            // Assert
            Assert.False(result);
            Assert.Null(semanticVersion);
        }
    }
}

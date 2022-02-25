using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Core.Maybe.Tests;

internal class Examples
{
    [Test]
    public void ShouldXXXXXXXXXXXXX() //bug
    {
        //GIVEN
        var x = GetList();

        //WHEN

        //THEN
        x.Should().Be(Maybe<List<int>>.Nothing);
    }

    private Maybe<List<int>> GetList()
    {
        var maybe = true.Then(null as List<int>);
        return maybe;
    }
}
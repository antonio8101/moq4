﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Moq.Tests.Linq
{
	public class SupportedQuerying
	{
		public class GivenABooleanProperty
		{
			[Fact]
			public void WhenImplicitlyQueryingTrueOneOf_ThenSetsPropertyToTrue()
			{
				var target = Mocks.OneOf<IFoo>(x => x.IsValid);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenImplicitlyQueryingTrueWhere_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().Where(x => x.IsValid);

				Assert.True(target.First().IsValid);
			}

			[Fact]
			public void WhenImplicitlyQueryingTrueFirst_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().First(x => x.IsValid);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenImplicitlyQueryingTrueFirstOrDefault_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().FirstOrDefault(x => x.IsValid);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenExplicitlyQueryingTrueOneOf_ThenSetsPropertyToTrue()
			{
				var target = Mocks.OneOf<IFoo>(x => x.IsValid == true);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenExplicitlyQueryingTrueWhere_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().Where(x => x.IsValid == true);

				Assert.True(target.First().IsValid);
			}

			[Fact]
			public void WhenExplicitlyQueryingTrueFirst_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().First(x => x.IsValid == true);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenExplicitlyQueryingTrueFirstOrDefault_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFoo>().FirstOrDefault(x => x.IsValid == true);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenQueryingOnFluent_ThenSetsPropertyToTrue()
			{
				var target = Mocks.Of<IFluent>().FirstOrDefault(x => x.Foo.IsValid == true);

				Assert.True(target.Foo.IsValid);
			}

			[Fact]
			public void WhenQueryingWithFalse_ThenSetsProperty()
			{
				var target = Mocks.OneOf<FooDefaultIsValid>(x => x.IsValid == false);

				Assert.False(target.IsValid);
			}

			[Fact]
			public void WhenQueryingTrueEquals_ThenSetsProperty()
			{
				var target = Mocks.OneOf<IFoo>(x => true == x.IsValid);

				Assert.True(target.IsValid);
			}

			[Fact]
			public void WhenQueryingFalseEquals_ThenSetsProperty()
			{
				var target = Mocks.OneOf<FooDefaultIsValid>(x => false == x.IsValid);

				Assert.False(target.IsValid);
			}

			[Fact]
			public void WhenQueryingNegatedProperty_ThenSetsProperty()
			{
				var target = Mocks.OneOf<FooDefaultIsValid>(x => !x.IsValid);

				Assert.False(target.IsValid);
			}

			[Fact]
			public void WhenQueryingWithNoValue_ThenAlwaysHasPropertyStubBehavior()
			{
				var foo = Mocks.OneOf<IFoo>();

				foo.IsValid = true;

				Assert.True(foo.IsValid);

				foo.IsValid = false;

				Assert.False(foo.IsValid);
			}

			public class FooDefaultIsValid : IFoo
			{
				public FooDefaultIsValid()
				{
					this.IsValid = true;
				}

				public virtual bool IsValid { get; set; }
			}

			public interface IFoo
			{
				bool IsValid { get; set; }
			}

			public interface IFluent
			{
				IFoo Foo { get; set; }
			}
		}

		public class GivenAnEnumProperty
		{
			[Fact]
			public void WhenQueryingWithEnumValue_ThenSetsPropertyValue()
			{
				var target = Mocks.Of<IFoo>().First(f => f.Targets == AttributeTargets.Class);

				Assert.Equal(AttributeTargets.Class, target.Targets);
			}

			[Fact(Skip = "Not implemented yet. Need to refactor old matcher stuff to the new one, require the MatcherAttribute on matchers, and verify it.")]
			public void WhenQueryingWithItIsAny_ThenThrowsNotSupportedException()
			{
				var target = Mocks.Of<IFoo>().First(f => f.Targets == It.IsAny<AttributeTargets>());

				Assert.Equal(AttributeTargets.Class, target.Targets);
			}

			public interface IFoo
			{
				AttributeTargets Targets { get; set; }
			}
		}

		public class GivenTwoProperties
		{
			[Fact]
			public void WhenCombiningQueryingWithImplicitBoolean_ThenSetsBothProperties()
			{
				var target = Mocks.OneOf<IFoo>(x => x.IsValid && x.Value == "foo");

				Assert.True(target.IsValid);
				Assert.Equal("foo", target.Value);
			}

			[Fact]
			public void WhenCombiningQueryingWithExplicitBoolean_ThenSetsBothProperties()
			{
				var target = Mocks.OneOf<IFoo>(x => x.IsValid == true && x.Value == "foo");

				Assert.True(target.IsValid);
				Assert.Equal("foo", target.Value);
			}

			public interface IFoo
			{
				string Value { get; set; }
				bool IsValid { get; set; }
			}
		}

		public class GivenAMethodWithOneParameter
		{
			[Fact]
			public void WhenUsingSpecificArgumentValue_ThenSetsReturnValue()
			{
				var foo = Mocks.OneOf<IFoo>(x => x.Do(5) == "foo");

				Assert.Equal("foo", foo.Do(5));
			}

			[Fact]
			public void WhenUsingItIsAnyForArgument_ThenSetsReturnValue()
			{
				var foo = Mocks.OneOf<IFoo>(x => x.Do(It.IsAny<int>()) == "foo");

				Assert.Equal("foo", foo.Do(5));
			}

			[Fact]
			public void WhenUsingItIsForArgument_ThenSetsReturnValue()
			{
				var foo = Mocks.OneOf<IFoo>(x => x.Do(It.Is<int>(i => i > 0)) == "foo");

				Assert.Equal("foo", foo.Do(5));
				Assert.Equal(default(string), foo.Do(-5));
			}

			[Fact]
			public void WhenUsingCustomMatcherForArgument_ThenSetsReturnValue()
			{
				var foo = Mocks.OneOf<IFoo>(x => x.Do(Any<int>()) == "foo");

				Assert.Equal("foo", foo.Do(5));
			}

			public TValue Any<TValue>()
			{
				return Match.Create<TValue>(v => true);
			}

			public interface IFoo
			{
				string Do(int value);
			}
		}
	}
}
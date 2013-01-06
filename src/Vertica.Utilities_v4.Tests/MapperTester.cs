﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testing.Commons;
using Testing.Commons.NUnit.Constraints;

namespace Vertica.Utilities_v4.Tests
{
	[TestFixture]
	public class InheritorClassMapperTester
	{
		#region subjects

		internal class InvalidToArgument : ClassMapper<InvalidOperationException, ArgumentException>
		{
			public override ArgumentException MapOne(InvalidOperationException from)
			{
				return new ArgumentException(from.Message);
			}
		}

		#endregion

		[Test]
		public void Map_SingleNotNull_Transformation()
		{
			string message = "message";
			var notNull = new InvalidOperationException(message);
			var subject = new InvalidToArgument();
			ArgumentException to = subject.Map(notNull);

			Assert.That(to, Is.Not.Null.And.With.Message.EqualTo(message));
		}

		[Test]
		public void Map_SingleNotNullWithDefault_Transformation()
		{
			string message = "message";
			var notNull = new InvalidOperationException(message);
			var @default = new ArgumentException(string.Empty);

			var subject = new InvalidToArgument();
			ArgumentException to = subject.Map(notNull, @default);

			Assert.That(to, Is.Not.Null.And.With.Message.EqualTo(message));
		}

		[Test]
		public void Map_NullSingle_Null()
		{
			InvalidOperationException @null = null;
			var subject = new InvalidToArgument();
			ArgumentException to = subject.Map(@null);

			Assert.That(to, Is.Null);
		}

		[Test]
		public void Map_NullSingleWithDefault_Default()
		{
			InvalidOperationException @null = null;
			string message = "message";
			var @default = new ArgumentException(message);

			var subject = new InvalidToArgument();
			ArgumentException to = subject.Map(@null, @default);

			Assert.That(to, Is.SameAs(@default));
		}

		[Test]
		public void Map_SeveralNotNull_Transformation()
		{
			var from = new[]
			{
				new InvalidOperationException("1"),
				new InvalidOperationException("2")
			};
			var subject = new InvalidToArgument();
			IEnumerable<ArgumentException> to = subject.Map(from);

			Assert.That(to, Must.Be.Constrained(
				Has.Message.EqualTo("1"),
				Has.Message.EqualTo("2")));
		}

		[Test]
		public void Map_NullSeveral_Empty()
		{
			IEnumerable<InvalidOperationException> from = null;
			var subject = new InvalidToArgument();
			IEnumerable<ArgumentException> to = subject.Map(from);

			Assert.That(to, Is.InstanceOf<IEnumerable<ArgumentException>>()
				.And.Empty);
		}

		[Test]
		public void Map_EmptySeveral_Empty()
		{
			IEnumerable<InvalidOperationException> from = Enumerable.Empty<InvalidOperationException>();
			var subject = new InvalidToArgument();
			IEnumerable<ArgumentException> to = subject.Map(from);

			Assert.That(to, Is.InstanceOf<IEnumerable<ArgumentException>>()
				.And.Empty);
		}

		[Test]
		public void Map_SeveralWithNulls_NullsIgnored()
		{
			var from = new[]
			{
				new InvalidOperationException("1"),
				null,
				new InvalidOperationException("2")
			};
			var subject = new InvalidToArgument();
			IEnumerable<ArgumentException> to = subject.Map(from);

			Assert.That(to, Must.Be.Constrained(
				Has.Message.EqualTo("1"),
				Has.Message.EqualTo("2")));
		}
	}

	[TestFixture]
	public class StaticClassMapperTester
	{
		[Test]
		public void Map_SingleNotNull_Transformation()
		{
			string message = "message";
			var notNull = new InvalidOperationException(message);
			ArgumentException to = ClassMapper.MapIfNotNull(notNull,
				() => new ArgumentException(notNull.Message));

			Assert.That(to, Is.Not.Null.And.With.Message.EqualTo(message));
		}
	}
}

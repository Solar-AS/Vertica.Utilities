﻿using System;

namespace Vertica.Utilities.Extensions.Infrastructure
{
	public interface IExtensionPoint
	{
		object ExtendedValue { get; }
		Type ExtendedType { get; }
	}

	public interface IExtensionPoint<out T> : IExtensionPoint
	{
		new T ExtendedValue { get; }
	}
}
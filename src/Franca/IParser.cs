using System;
using System.Collections.Generic;

namespace Franca
{
    public interface IParser<T>
	{
		IEnumerable<T> Parse(Token token);
	}
}

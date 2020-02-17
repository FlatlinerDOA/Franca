using System.Collections.Generic;

namespace Franca
{
    public interface IParser<T>
	{
		Result<T> Parse(Token token);
	}
}

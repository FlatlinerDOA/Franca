using System;

namespace Franca
{
    public interface ITokenizer
	{
		Token Parse(ReadOnlySpan<char> span);
	}
}

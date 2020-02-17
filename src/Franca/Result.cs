using System;

namespace Franca
{
    public struct Result<T>
	{
		public static readonly Result<T> Fail = new Result<T>();

		public Result(T value)
		{
			this.Value = value;
			this.IsSuccess = true;
		}

		public bool IsSuccess;

		public T Value;

		public static Result<T> Success(T value)
		{
			return new Result<T>(value);
		}
	}
}

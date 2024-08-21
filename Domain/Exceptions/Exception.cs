namespace Domain.Exceptions;

public class FailWhileLoadingDataException : Exception
{
	public FailWhileLoadingDataException()
	{
	}

	public FailWhileLoadingDataException(string message) : base(message)
	{
	}
}
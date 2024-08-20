namespace Domain.Exceptions;

public class FailWhileLoadingFileException : Exception
{
	public FailWhileLoadingFileException()
	{
	}

	public FailWhileLoadingFileException(string message) : base(message)
	{
	}
}
using AutoMapper;
using Domain.Entities;
using Domain.Repository;

namespace Application.Service;

public sealed class BookService(IBookRepository bookRepository, IMapper mapper)
{
	private readonly IMapper _mapper = mapper;
	private readonly IBookRepository _bookRepository =
		bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

	public GetBookDto Add(AddBookDto book)
	{
		return _mapper.Map<GetBookDto>(_bookRepository.Add(_mapper.Map<Book>(book)));
	}

	public GetBookDto Update(UpdateBookDto book)
	{
		Book? updatedBook = _bookRepository.GetById(book.Id);
		
		if(updatedBook == null)
			throw new KeyNotFoundException("No Books found with Id : " + book.Id);
		
		return _mapper.Map<GetBookDto>(_bookRepository.Update(_mapper.Map<Book>(book)));
	}

	public bool Delete(int bookId)
	{
		if (bookId <= 0)
			throw new IndexOutOfRangeException("Book Id cannot be negative.");

		return _bookRepository.Delete(bookId);
	}

	public List<GetBookDto> Get()
	{
		return _mapper.Map<List<GetBookDto>>(_bookRepository.Get());
	}

	public GetBookDto? GetById(int bookId)
	{
		if (bookId <= 0)
			throw new IndexOutOfRangeException("Book Id cannot be negative.");
		
		return _mapper.Map<GetBookDto>(_bookRepository.GetById(bookId));
	}
}
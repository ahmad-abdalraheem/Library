using Domain.Entities;

namespace Infrastructure.DataHandler
{
	public class BookDbHandler<TBook>(LibraryContext context) : IDataHandler<TBook> where TBook : Book
	{
		public bool Add(TBook book)
		{
			try
			{
				if (book == null)
					throw new ArgumentNullException();
				
				context.Books.Add(book);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				return false;
			}
		}

		public bool Update(TBook book)
		{
			try
			{
				if(book == null)
					throw new ArgumentNullException();
				
				context.Books.Update(book);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				return false;
			}
		}

		public bool Delete(int bookId)
		{
			try
			{
				Book? book = context.Books?.FirstOrDefault(b => b.Id == bookId);
				if (book == null)
					throw new ArgumentNullException();
				
				context.Books?.Remove(book);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				return false;
			}
		}

		public List<TBook>? Get()
		{
			try
			{
				var query = from book in context.Books
					join member in context.Members
						on book.BorrowedBy equals member.Id into bookMembers
					from member in bookMembers.DefaultIfEmpty()
					select new Book
					{
						Id = book.Id,
						Title = book.Title,
						Author = book.Author,
						IsBorrowed = book.IsBorrowed,
						BorrowedDate = book.BorrowedDate,
						BorrowedBy = book.BorrowedBy,
						Borrower = member,
						MemberName = member != null ? member.Name : null
					};

				return query.ToList() as List<TBook>;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}

		public TBook? GetById(int memberId)
		{
			try
			{
				Book? query = (from book in context.Books
					where book.Id == memberId
					join member in context.Members
						on book.BorrowedBy equals member.Id into bookMembers
					from member in bookMembers.DefaultIfEmpty()
					select new Book
					{
						Id = book.Id,
						Title = book.Title,
						Author = book.Author,
						IsBorrowed = book.IsBorrowed,
						BorrowedDate = book.BorrowedDate,
						BorrowedBy = book.BorrowedBy,
						MemberName = member != null ? member.Name : null
					}).FirstOrDefault();

				return query as TBook;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
	}
}
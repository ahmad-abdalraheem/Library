using Domain.Entities;

namespace Infrastructure.DataHandler
{
	public class MemberDbHandler<TMember>(LibraryContext context) : IDataHandler<TMember> where TMember : Member
	{
		public bool Add(TMember member)
		{
			try
			{
				if (member == null)
					throw new ArgumentNullException();
				
				context.Members.Add(member);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("" + e.Message);
				return false;
			}
		}

		public bool Update(TMember member)
		{
			try
			{
				if(member == null)
					throw new ArgumentNullException();
				
				context.Members.Update(member);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				return false;
			}
		}

		public bool Delete(int memberId)
		{
			try
			{
				Member? member = context.Members.Find(memberId);
				if (member == null)
					return false;
				
				context.UpdateIsBorrowedStatus(member);
				context.Members.Remove(member);
				context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				return false;
			}
		}

		public List<TMember>? Get()
		{
			try
			{
				return context.Members.ToList() as List<TMember>;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}

		public TMember? GetById(int memberId)
		{
			try
			{
				return (TMember?) context.Members.Find(memberId);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
	}
}
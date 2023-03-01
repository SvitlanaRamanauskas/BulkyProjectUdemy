using BulkyBook.DataAccess.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDBContext _db;
		public UnitOfWork(ApplicationDBContext db)
		{
			_db = db;
			Category = new CategoryRepository(_db);// created objs of reposes
			CoverType = new CoverTypeRepository(_db);
			Product = new ProductRepository(_db);
			Company = new CompanyRepository(_db);
			ShoppingCart = new ShoppingCartRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
        }
		public ICategoryRepository Category { get; private set; }
		public ICoverTypeRepository CoverType { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }

		public void Save()
		{
			_db.SaveChanges();
		}


	}
}

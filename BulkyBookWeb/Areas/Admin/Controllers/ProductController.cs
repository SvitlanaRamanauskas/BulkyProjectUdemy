using BulkyBook.DataAccess;
using BulkyBook.DataAccess.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}
		public IActionResult Index() 
		{
			return View();
		}

		//UpSert
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				Product = new(),
				CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}), //projection using select FOR dropdowns
				CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
			};
			if (id == null || id == 0)
			{
				//create
				//ViewBag.CategoryList = categoryList;
				//ViewData["CoverTypeList"] = coverTypeList;
				return View(productVM);
			}
			else
			{
				//update
			}
			return View(productVM);
	}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM obj, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _hostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(wwwRootPath, @"images/products");
					var extenshion = Path.GetExtension(file.FileName);
					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName+extenshion), FileMode.Create))
					{
						file.CopyTo(fileStreams);
					}
					obj.Product.ImageURL = @"\images\products\" + fileName + extenshion;
				}
				if (obj.Product.Id == 0)
				{
					_unitOfWork.Product.Add(obj.Product);
				}
				else
				{
					_unitOfWork.Product.Update(obj.Product);
				}
				_unitOfWork.Save();
				TempData["success"] = "Product added successfully";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//DELETE
		public IActionResult Delete(int? id)
		{
			if(id==null|| id == 0)
			{
				return NotFound();
			}
			var coverTypeFirstOrDefault = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
			if (coverTypeFirstOrDefault == null)
			{
				return NotFound();
			}
			return View(coverTypeFirstOrDefault);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{
			var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
			if(obj == null)
			{
				return NotFound();
			}
			_unitOfWork.CoverType.Remove(obj);
			TempData["success"] = "CoverType deleted successfully";
			_unitOfWork.Save();
			return RedirectToAction("Index");
		}


		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
			return Json(new { data = productList });
		}

		#endregion

	}
}

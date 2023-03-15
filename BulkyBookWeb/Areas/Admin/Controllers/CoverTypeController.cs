using BulkyBook.DataAccess.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CoverTypeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public CoverTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index() // what includes Index page = all CoverTypes
		{
			IEnumerable<CoverType> objListOfCoverTypes = _unitOfWork.CoverType.GetAll();
			return View(objListOfCoverTypes);
		}

		//CREATE
		public IActionResult Create() 
		{
			return View();
		}

		[HttpPost] 
		[ValidateAntiForgeryToken]
		public IActionResult Create(CoverType obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.CoverType.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "CoverType created successfully";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//EDIT
		public IActionResult Edit(int? id) 
		{
			if(id == null|| id == 0)
			{
				return NotFound();
			}
			var coverTypeFirstFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
			if(coverTypeFirstFromDb == null)
			{
				return NotFound();
			}
			return View(coverTypeFirstFromDb);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CoverType obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.CoverType.Update(obj);
				TempData["success"] = "CoverType updated successfully";
				_unitOfWork.Save();
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
			_unitOfWork.Save();
			TempData["success"] = "CoverType deleted successfully";
			return RedirectToAction("Index");
		}

	}
}

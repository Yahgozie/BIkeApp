using Bikes.Data;
using Bikes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Controllers
{
	[Authorize(Roles = "Admin,Executive")]
	public class MakeController : Controller
	{
		private readonly ApplicationDbContext _db;

		public MakeController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			var AllMakes = _db.Makes.ToList();
			return View(AllMakes);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Make make)
		{
			if (ModelState.IsValid)
			{
				_db.Add(make);
				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			return View(make);
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var make = _db.Makes.Find(id);
			if (make == null)
			{
				return NotFound();
			}
			_db.Remove(make);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var make = _db.Makes.Find(id);
			if (make == null)
			{
				return NotFound();
			}
			return View(make);
		}

		[HttpPost]
		public IActionResult Edit(Make make)
		{
			if (ModelState.IsValid)
			{
				_db.Update(make);
				_db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			return View(make);
		}
	}
}

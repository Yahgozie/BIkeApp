using AutoMapper;
using Bikes.Controllers.Resources;
using Bikes.Data;
using Bikes.Models;
using Bikes.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Controllers
{
	[Authorize(Roles = "Admin,Executive")]
	public class ModelController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		[BindProperty]
		public ModelViewModel ModelVM { get; set; }

		public ModelController(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
			ModelVM = new ModelViewModel()
			{
				Makes = _db.Makes.ToList(),
				Model = new Models.Model()
			};
		}

		public IActionResult Index()
		{
			var model = _db.Models.Include(m => m.Make);
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(ModelVM);
		}

		[HttpPost, ActionName("Create")]
	//	[ValidateAntiForgeryToken]
		public IActionResult CreatePost()
		{
			if (!ModelState.IsValid)
			{
				return View(ModelVM);
			}
			_db.Add(ModelVM.Model);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
			if (ModelVM.Model == null)
			{
				return NotFound();
			}
			return View(ModelVM);
		}

		[HttpPost, ActionName("Edit")]
		public IActionResult EditPost()
		{
			if (!ModelState.IsValid)
			{
				return View(ModelVM);
			}
			_db.Update(ModelVM.Model);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			Model model = _db.Models.Find(id);
			if (model == null)
			{
				return NotFound();
			}
			_db.Models.Remove(model);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		[AllowAnonymous]
		[HttpGet("api/models/{MakeID}")]
		public IEnumerable<Model> Models(int MakeID)
		{
			return _db.Models.ToList()
				.Where(m => m.MakeId == MakeID);
		}

		[AllowAnonymous]
		[HttpGet("api/models")]
		public IEnumerable<ModelResources> Models()
		{
			var model = _db.Models.ToList();
			var modelResources = _mapper.Map<List<Model>, List<ModelResources>>(model);
			return modelResources;
		}
	}
}

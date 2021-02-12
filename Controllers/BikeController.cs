using AutoMapper;
using Bikes.Controllers.Resources;
using Bikes.Data;
using Bikes.Models;
using Bikes.Models.ViewModels;
using Bikes.RoleMembers;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Controllers
{
	[Authorize(Roles = Roles.Admin+","+Roles.Executive)]
	public class BikeController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _webhostEnv;

		[BindProperty]
		public BikeViewModel BikeVM { get; set; }

		public BikeController(ApplicationDbContext db, IWebHostEnvironment webhostEnv)
		{
			_db = db;
			_webhostEnv = webhostEnv;
			BikeVM = new BikeViewModel()
			{
				Makes = _db.Makes.ToList(),
				Models = _db.Models.ToList(),  //changed to list of models
				Bike = new Models.Bike()
			};
		}

		[AllowAnonymous]
		public IActionResult Index(string searchString, string sortOrder, int pageNumber = 1, int pageSize = 2)
		{
			ViewBag.CurrentSortOrder = sortOrder;
			ViewBag.CurrentFilter = searchString;
			ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "Price_desc": "";
			int ExcludeRecords = (pageSize * pageNumber) - pageSize;
			var bikes = from b in _db.Bikes.Include(m => m.Make).Include(m => m.Model)
						select b;
			var bikeCount = bikes.Count();
			if (!String.IsNullOrEmpty(searchString))
			{
				bikes = bikes.Where(b => b.Make.Name.Contains(searchString));
				bikeCount = bikes.Count();
			}
			//Sorting Logic
			switch (sortOrder)
			{
				case "Price_desc":
					bikes = bikes.OrderByDescending(b => b.Price);
					break;
				default:
					bikes = bikes.OrderBy(b => b.Price);
					break;
			}
			bikes = bikes
				.Skip(ExcludeRecords)
				.Take(pageSize);
			var result = new PagedResult<Bike>
			{
				Data = bikes.AsNoTracking().ToList(),
				TotalItems = bikeCount,
				PageNumber = pageNumber,
				PageSize = pageSize
			};
			return View(result);
		}
		
		[HttpGet]
		public IActionResult Create()
		{
			return View(BikeVM);
		}

		[HttpPost, ActionName("Create")]
		//	[ValidateAntiForgeryToken]
		public IActionResult CreatePost()
		{
			if (!ModelState.IsValid)
			{
				BikeVM.Makes = _db.Makes.ToList();
				BikeVM.Models = _db.Models.ToList();
				return View(BikeVM);
			}
			_db.Add(BikeVM.Bike);			
			_db.SaveChanges();
			UploadImage();
			return RedirectToAction(nameof(Index));
		}

		private void UploadImage()
		{
			//Save bike logic
			//Get bike id we have saved in database
			var BikeID = BikeVM.Bike.Id;
			//Get the www root folder to save the file in server
			string wwwRootPath = _webhostEnv.WebRootPath;
			//Get the uploaded files
			var files = HttpContext.Request.Form.Files;
			//Get the reference of dbset for the bike we just saved to the database
			var SavedBike = _db.Bikes.Find(BikeID);
			//upload the files on server and save the image path if user have uploaded any file.
			if (files.Count != 0)
			{
				var imagePath = @"images\bike";
				var Extension = Path.GetExtension(files[0].FileName);
				var RelativeImagePath = imagePath + BikeID + Extension;
				var AbsImagePath = Path.Combine(wwwRootPath, RelativeImagePath);
				//upload the file on server
				using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
				{
					files[0].CopyTo(fileStream);
				}
				//set the image path on the database
				SavedBike.ImagePath = RelativeImagePath;
				_db.SaveChanges();
			}
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			BikeVM.Bike = _db.Bikes.SingleOrDefault(m => m.Id == id);
			//filter the models associated with the make
			BikeVM.Models = _db.Models.Where(m => m.MakeId == BikeVM.Bike.MakeId);
			if (BikeVM.Bike == null)
			{
				return NotFound();
			}
			return View(BikeVM);
		}

		[HttpPost, ActionName("Edit")]
		public IActionResult EditPost()
		{
			if (!ModelState.IsValid)
			{
				BikeVM.Makes = _db.Makes.ToList();
				BikeVM.Models = _db.Models.ToList();
				return View(BikeVM);
			}
			_db.Update(BikeVM.Bike);			
			_db.SaveChanges();
			UploadImage();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			Bike bike = _db.Bikes.Find(id);
			if (bike == null)
			{
				return NotFound();
			}
			_db.Bikes.Remove(bike);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		[AllowAnonymous]
		public IActionResult Details(int id)
		{
			BikeVM.Bike = _db.Bikes.SingleOrDefault(m => m.Id == id);
			if (BikeVM.Bike == null)
			{
				return NotFound();
			}
			return View(BikeVM);
		}


	}
}

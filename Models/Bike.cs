using Bikes.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Models
{
	public class Bike
	{
		public int Id { get; set; }

		public Make Make { get; set; }
		[RegularExpression("^[1-9]*$",ErrorMessage = "Please Select Make")]
		public int MakeId { get; set; }
		public Model Model { get; set; }
		[RegularExpression("^[1-9]*$", ErrorMessage = "Please Select Model")]
		public int ModelId { get; set; }
		[Required(ErrorMessage = "Provide Year Name")]
		[AnnotationExtension(2000, ErrorMessage = "Invalid Year")]
		public int Year { get; set; }
		[Required]
		[Range(1, int.MaxValue,ErrorMessage = "Provide Mileage Amount")]
		public int Mileage { get; set; }
		public string Features { get; set; }
		[Required(ErrorMessage = "Provide Seller Name")]
		public string SellerName { get; set; }
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string SellerEmail{ get; set; }
		[Required(ErrorMessage = "Provide Seller Phone Number")]
		public string SellerPhone { get; set; }
		[Required(ErrorMessage = "Provide Selling Price")]
		public decimal  Price { get; set; }
		[Required]
		[RegularExpression("^[A-Za-z]*$", ErrorMessage = "Please Select Currency")]
		public string Currency { get; set; }
		[Display(Name = "Photo")]
		public string ImagePath { get; set; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Models
{
	public class Model
	{
		public int Id { get; set; }
		[Required]
		[StringLength(255)]
		public string Name { get; set; }
		[ForeignKey("Make")]
		public int MakeId { get; set; }
		public Make Make { get; set; }
	}
}

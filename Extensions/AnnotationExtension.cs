using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Extensions
{
	public class AnnotationExtension : RangeAttribute
	{
		public AnnotationExtension(int StartYear) : base(StartYear, DateTime.Today.Year)
		{

		}
	}
}

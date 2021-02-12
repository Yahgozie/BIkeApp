using AutoMapper;
using Bikes.Controllers.Resources;
using Bikes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.MappingProfiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Model, ModelResources>();
		}
	}
}

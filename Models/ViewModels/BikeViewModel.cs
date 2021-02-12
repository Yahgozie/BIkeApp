using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bikes.Models.ViewModels
{
	public class BikeViewModel
	{
		public Bike Bike { get; set; }
		public IEnumerable<Make> Makes { get; set; }
		public IEnumerable<Model> Models { get; set; }
		public IEnumerable<Currency> Currencies { get; set; }
		public List<Model> Model { get; internal set; }

		private List<Currency> CList = new List<Currency>();
		private List<Currency> CreateList()
		{
			CList.Add(new Currency("USD", "USD"));
			CList.Add(new Currency("EUR", "EUR"));
			CList.Add(new Currency("NAIRA", "NAIRA"));
			return CList;
		}
		public BikeViewModel()//Contructor to immediately populate the bikeviewmodel
		{
			Currencies = CreateList();
		}
	}
	public class Currency
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public Currency(string id, string name)
		{
			Id = id;
			Name = name;
		}
	}

}

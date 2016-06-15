using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parliament.Api.SGNS.ViewModels
{
	public class ActViewModel
	{
		public string Naziv { get; set; }

		public string Status { get; set; }

		public string DatumVremePredlaganja { get; set; }

		public string DatumVremeUsvajanja { get; set; }

		public string Text { get; set; }

		public ActViewModel()
		{
			Naziv = "";
			Status = "";
			DatumVremePredlaganja = "";
			DatumVremeUsvajanja = "";
			Text = "";
		}
	}
}
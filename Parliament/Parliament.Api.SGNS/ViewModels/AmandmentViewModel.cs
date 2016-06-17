using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parliament.Api.SGNS.ViewModels
{
	public class AmandmentViewModel
	{
		public string IdPropisa { get; set; }

		public string DatumVremePredlaganja { get; set; }

		public string DatumVremeUsvajanja { get; set; }

		public string Text { get; set; }

		public string ImeNadleznogOrgana { get; set; }

		public string PrezimeNadleznogOrgana { get; set; }

		public string EmailNadleznogOrgana { get; set; }

		public AmandmentViewModel()
		{
			IdPropisa = "";
			DatumVremePredlaganja = "";
			DatumVremeUsvajanja = "";
			Text = "";
			ImeNadleznogOrgana = "";
			PrezimeNadleznogOrgana = "";
			EmailNadleznogOrgana = "";
		}
	}
}
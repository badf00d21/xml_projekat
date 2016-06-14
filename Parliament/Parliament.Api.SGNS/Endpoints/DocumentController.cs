using Marklogic.Xcc;
using Parliament.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace Parliament.Api.SGNS.Endpoints
{
	[Authorize]
	public class DocumentController : ApiController
	{
		[HttpPost]
		[Route("api/documents/propose/act", Name = "ProposeAct")]
		public async Task<IHttpActionResult> ProposeAct()
		{
			XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
			XmlDocument document = new XmlDocument();
			document.LoadXml(doc.ToString());

			Uri uri = new Uri(WebConfigurationManager.AppSettings["MarkLogicConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			using (Session session = contentSource.NewSession())
			{
				var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
				getActQuery.SetNewStringVariable("datum_vreme_predlaganja", document.DocumentElement.Attributes["parliament:DatumVremePredlaganja"].Value);
				getActQuery.SetNewStringVariable("serijski_broj",  document.DocumentElement.Attributes["SerijskiBroj"].Value);

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = new XmlDocument();
				xmlResult.LoadXml(getActQueryResult.AsString());

				if (xmlResult.SelectSingleNode("Propisi").HasChildNodes)
					return BadRequest("Xml document with same date and serial number already exists!");

				X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
				store.Open(OpenFlags.ReadOnly);

				var certificate = store.Certificates.Find(X509FindType.FindBySubjectName, "vladimirdjurdjevic93@gmail.com", true)[0];
				store.Close();

				if (certificate == null)
					return BadRequest("Could not find users certificate!");
				
				XMLUtils.SignXmlDocument(document, certificate);

				var addActQuery = session.NewModuleInvoke("AddActQuery.xqy");
				addActQuery.SetNewStringVariable("act_string", document.ToString());

				ResultSequence addActQueryResult = session.SubmitRequest(addActQuery);

				if (addActQueryResult.AsString() == "NOT OK")
					return BadRequest("Invalid XML");

				return Created(addActQueryResult.AsString(), doc);
			}
		}
	}
}
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

				getActQuery.SetNewStringVariable("text", "");
				getActQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
				getActQuery.SetNewStringVariable("naslov_propisa", "");
				getActQuery.SetNewStringVariable("status", "");

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = new XmlDocument();
				xmlResult.LoadXml(getActQueryResult.AsString());

				
				if (xmlResult.SelectSingleNode("Propisi").HasChildNodes)
					return BadRequest("Xml document with same date and serial number already exists!");

				X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
				store.Open(OpenFlags.ReadOnly);

				var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, "vladimirdjurdjevic93@gmail.com", false);
				var targetCertificate = certificates.Count > 0 ? certificates[0] : null;

				store.Close();

				if (targetCertificate == null)
					return BadRequest("Could not find users certificate!");
				
				XMLUtils.SignXmlDocument(document, targetCertificate);

				var addActQuery = session.NewModuleInvoke("/AddActQuery.xqy");
				addActQuery.SetNewStringVariable("act_string", document.InnerXml);

				ResultSequence addActQueryResult = session.SubmitRequest(addActQuery);

				if (addActQueryResult.AsString() != "OK")
					return BadRequest(addActQueryResult.AsString());

				return Created(addActQueryResult.AsString(), doc);
			}
		}
	}
}
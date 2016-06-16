using Marklogic.Xcc;
using Parliament.Api.SGNS.ViewModels;
using Parliament.Security;
using System;
using System.Collections.Generic;
using System.IO;
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

			Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			using (Session session = contentSource.NewSession())
			{
				var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
				getActQuery.SetNewStringVariable("datum_vreme_predlaganja", document.DocumentElement.Attributes["parliament:DatumVremePredlaganja"].Value);
				getActQuery.SetNewStringVariable("serijski_broj",  document.DocumentElement.Attributes["SerijskiBroj"].Value);

				getActQuery.SetNewStringVariable("text", "");
				getActQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
				getActQuery.SetNewStringVariable("naziv_propisa", "");
				getActQuery.SetNewStringVariable("status", "");

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = new XmlDocument();
				xmlResult.LoadXml(getActQueryResult.AsString());

				
				if (xmlResult.SelectSingleNode("Propisi").HasChildNodes)
					return BadRequest("Xml document with same date and serial number already exists!");

				X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
				store.Open(OpenFlags.ReadOnly);

				var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, User.Identity.Name, false);
				var targetCertificate = certificates.Count > 0 ? certificates[0] : null;

				store.Close();

				if (targetCertificate == null)
					return BadRequest("Could not find users certificate!");
				
				XMLUtils.SignXmlDocument(document, targetCertificate);

				var addActQuery = session.NewModuleInvoke("/AddActQuery.xqy");
				addActQuery.SetNewStringVariable("act_string", document.InnerXml);

				ResultSequence addActQueryResult = session.SubmitRequest(addActQuery);

				if (addActQueryResult.AsString().Contains("Error"))
					return BadRequest(addActQueryResult.AsString());

				return Created(addActQueryResult.AsString(), document.DocumentElement);
			}
		}

		[HttpGet]
        [AllowAnonymous]
		[Route("api/documents/schema/{name}", Name = "GetSchema")]
		public IHttpActionResult GetSchema(string name)
		{
			Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlSchemaDbConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			if (!name.Contains(".xsd"))
				name = name + ".xsd";

			using (Session session = contentSource.NewSession())
			{
				var getSchemaQuery = session.NewAdhocQuery(string.Format("doc('{0}')", name));
				ResultSequence getSchemaQueryResult = session.SubmitRequest(getSchemaQuery);

				if (getSchemaQueryResult.AsString() == "")
					return BadRequest("Schema '" + name + "' does not exist!");

				return Ok(XElement.Parse( getSchemaQueryResult.AsString()));
			}
		}

		[HttpGet]
        [AllowAnonymous]
        [Route("api/documents/acts", Name = "GetAllActs")]
		public IHttpActionResult GetAllActs()
		{
			Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			using (Session session = contentSource.NewSession())
			{
				var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
				getActQuery.SetNewStringVariable("datum_vreme_predlaganja", "");
				getActQuery.SetNewStringVariable("serijski_broj", "");

				getActQuery.SetNewStringVariable("text", "");
				getActQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
				getActQuery.SetNewStringVariable("naziv_propisa", "");
				getActQuery.SetNewStringVariable("status", "");

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = XElement.Parse(getActQueryResult.AsString());

				return Ok(xmlResult);
			}
		}

		[HttpGet]
        [AllowAnonymous]
        [Route("api/documents/acts/proposed", Name = "GetAllProposedActs")]
		public IHttpActionResult GetAllProposedActs()
		{
			Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			using (Session session = contentSource.NewSession())
			{
				var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
				getActQuery.SetNewStringVariable("datum_vreme_predlaganja", "");
				getActQuery.SetNewStringVariable("serijski_broj", "");

				getActQuery.SetNewStringVariable("text", "");
				getActQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
				getActQuery.SetNewStringVariable("naziv_propisa", "");
				getActQuery.SetNewStringVariable("status", "Predlozen");

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = XElement.Parse(getActQueryResult.AsString());

				return Ok(xmlResult);
			}
		}

		[HttpGet]
        [AllowAnonymous]
        [Route("api/documents/acts/adopted", Name = "GetAllAdoptedActs")]
		public IHttpActionResult GetAllAdoptedActs()
		{
			Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			using (Session session = contentSource.NewSession())
			{
				var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
				getActQuery.SetNewStringVariable("datum_vreme_predlaganja", "");
				getActQuery.SetNewStringVariable("serijski_broj", "");

				getActQuery.SetNewStringVariable("text", "");
				getActQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
				getActQuery.SetNewStringVariable("naziv_propisa", "");
				getActQuery.SetNewStringVariable("status", "Usvojen");

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = XElement.Parse(getActQueryResult.AsString());

				return Ok(xmlResult);
			}
		}

		[HttpPost]
		[Route("api/documents/acts/filter", Name = "FindActs")]
		public IHttpActionResult FindActs(ActViewModel act)
		{
			if (act == null)
				act = new ActViewModel();

			Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
			ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

			using (Session session = contentSource.NewSession())
			{
				var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
				getActQuery.SetNewStringVariable("datum_vreme_predlaganja", act.DatumVremePredlaganja);
				getActQuery.SetNewStringVariable("serijski_broj", "");

				getActQuery.SetNewStringVariable("text", act.Text);
				getActQuery.SetNewStringVariable("datum_vreme_usvajanja", act.DatumVremeUsvajanja);
				getActQuery.SetNewStringVariable("naziv_propisa", act.Naziv);
				getActQuery.SetNewStringVariable("status", act.Status);

				ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
				var xmlResult = XElement.Parse(getActQueryResult.AsString());

				return Ok(xmlResult);
			}
		}
	}
}
using Marklogic.Xcc;
using Parliament.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Parliament.Api.IAGNS.Endpoints
{
    public class ArchiveController : System.Web.Http.ApiController
    {
        [HttpPost]
        //[Authorize(Roles = "Alderman")]
        [Route("api/documents/save/act", Name = "SaveAct")]
        public async Task<IHttpActionResult> ProposeAct()
        {
            XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
            XmlDocument document = new XmlDocument();
            document.LoadXml(doc.ToString());

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, "sgns.parliament.rs", false);
            var targetCertificate = certificates.Count > 0 ? certificates[0] : null;

            store.Close();

            if (targetCertificate == null)
                return BadRequest("Could not find users certificate!");

            XMLUtils.DecryptXmlDocument(document, targetCertificate);

            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var insertQuery = session.NewAdhocQuery(string.Format("'xdmp:document-insert(http://www.parliament.rs/archive/documents/acts/{0}.xml)'", Guid.NewGuid().ToString()));

                return Ok();
            }
        }
    }
}
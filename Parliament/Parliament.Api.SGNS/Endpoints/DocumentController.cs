﻿using Marklogic.Xcc;
using Parliament.Api.SGNS.ViewModels;
using Parliament.DataAccess.Database;
using Parliament.DataAccess.Utils;
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
using static javax.sound.sampled.AudioFormat;

namespace Parliament.Api.SGNS.Endpoints
{
    [Authorize]
    public class DocumentController : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "Alderman")]
        [Route("api/documents/propose/act", Name = "ProposeAct")]
        public async Task<IHttpActionResult> ProposeAct()
        {
            XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
            XmlDocument document = new XmlDocument();
            document.LoadXml(doc.ToString());

            XMLUtils.AddTimeAndSerialNumber(document);

            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getActQuery = session.NewModuleInvoke("/GetActQuery.xqy");
                getActQuery.SetNewStringVariable("datum_vreme_predlaganja", document.DocumentElement.Attributes["parliament:DatumVremePredlaganja"].Value);
                getActQuery.SetNewStringVariable("serijski_broj", document.DocumentElement.Attributes["SerijskiBroj"].Value);

                getActQuery.SetNewStringVariable("text", "");
                getActQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
                getActQuery.SetNewStringVariable("naziv_propisa", "");
                getActQuery.SetNewStringVariable("status", "");

                getActQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("email_nadleznog_organa", "");

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

                XMLUtils.FillAutoGeneratedData(document);

                using (var dbContext = new ParliamentDbContext())
                {
                    using (var userManager = new ParliamentUserManager(dbContext))
                    {
                        var user = await userManager.FindByNameAsync(User.Identity.Name);
                        string role = user.Role == ParliamentRole.Citizen ? "Gradjanin" : user.Role == ParliamentRole.Chairman ? "Odbornik" : "Predsednik skupstine";
                        XMLUtils.AddActUserInfo(document, user.FirstName, user.LastName, user.Email, role);
                    }
                }

                string documentId = XMLUtils.AddDocumentId(document);

                var addActQuery = session.NewModuleInvoke("/AddActQuery.xqy");
                addActQuery.SetNewStringVariable("act_string", document.InnerXml);
                addActQuery.SetNewStringVariable("id", documentId);

                ResultSequence addActQueryResult = session.SubmitRequest(addActQuery);

                if (addActQueryResult.AsString().Contains("Error"))
                    return BadRequest(addActQueryResult.AsString());

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                return Created(addActQueryResult.AsString(), document.DocumentElement);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Alderman")]
        [Route("api/documents/propose/amandment", Name = "ProposeAmandment")]
        public async Task<IHttpActionResult> ProposeAmandment()
        {
            XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
            XmlDocument document = new XmlDocument();
            document.LoadXml(doc.ToString());

            XMLUtils.AddTimeAndSerialNumber(document);

            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getAmandmentQuery = session.NewModuleInvoke("/GetAmandmentQuery.xqy");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_predlaganja", document.DocumentElement.Attributes["parliament:DatumVremePredlaganja"].Value);
                getAmandmentQuery.SetNewStringVariable("serijski_broj", document.DocumentElement.Attributes["SerijskiBroj"].Value);

                getAmandmentQuery.SetNewStringVariable("text", "");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
                getAmandmentQuery.SetNewStringVariable("id_propisa", "");

                getAmandmentQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getAmandmentQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getAmandmentQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getActQueryResult = session.SubmitRequest(getAmandmentQuery);
                var xmlResult = new XmlDocument();
                xmlResult.LoadXml(getActQueryResult.AsString());


                if (xmlResult.SelectSingleNode("Amandmani").HasChildNodes)
                    return BadRequest("Xml document with same date and serial number already exists!");

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);

                var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, User.Identity.Name, false);
                var targetCertificate = certificates.Count > 0 ? certificates[0] : null;

                store.Close();

                if (targetCertificate == null)
                    return BadRequest("Could not find users certificate!");

                XMLUtils.SignXmlDocument(document, targetCertificate);

                using (var dbContext = new ParliamentDbContext())
                {
                    using (var userManager = new ParliamentUserManager(dbContext))
                    {
                        var user = await userManager.FindByNameAsync(User.Identity.Name);
                        string role = user.Role == ParliamentRole.Citizen ? "Gradjanin" : user.Role == ParliamentRole.Chairman ? "Odbornik" : "Predsednik skupstine";
                        XMLUtils.AddAmandmentUserInfo(document, user.FirstName, user.LastName, user.Email, role);
                    }
                }

                string documentId = XMLUtils.AddDocumentId(document);

                var addAmandmentQuery = session.NewModuleInvoke("/AddAmandmentQuery.xqy");
                addAmandmentQuery.SetNewStringVariable("amandment_string", document.InnerXml);
                addAmandmentQuery.SetNewStringVariable("id", documentId);

                ResultSequence addAmandmentQueryResult = session.SubmitRequest(addAmandmentQuery);

                if (addAmandmentQueryResult.AsString().Contains("Error"))
                    return BadRequest(addAmandmentQueryResult.AsString());

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                return Created(addAmandmentQueryResult.AsString(), document.DocumentElement);
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

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                return Ok(XElement.Parse(getSchemaQueryResult.AsString()));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/documents/schema/{name}/{referenced}", Name = "GetReferencedSchema")]
        public IHttpActionResult GetReferencedSchema(string name, string referenced)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlSchemaDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            if (!referenced.Contains(".xsd"))
                referenced = referenced + ".xsd";

            using (Session session = contentSource.NewSession())
            {
                var getSchemaQuery = session.NewAdhocQuery(string.Format("doc('{0}')", referenced));
                ResultSequence getSchemaQueryResult = session.SubmitRequest(getSchemaQuery);

                if (getSchemaQueryResult.AsString() == "")
                    return BadRequest("Schema '" + referenced + "' does not exist!");

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                return Ok(XElement.Parse(getSchemaQueryResult.AsString()));
            }
        }

        [HttpGet]
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

                getActQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Alderman")]
        [Route("api/documents/amandments", Name = "GetAllAmandments")]
        public IHttpActionResult GetAllAmandments()
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getAmandmentQuery = session.NewModuleInvoke("/GetAmandmentQuery.xqy");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_predlaganja", "");
                getAmandmentQuery.SetNewStringVariable("serijski_broj", "");

                getAmandmentQuery.SetNewStringVariable("text", "");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
                getAmandmentQuery.SetNewStringVariable("id_propisa", "");

                getAmandmentQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getAmandmentQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getAmandmentQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getActQueryResult = session.SubmitRequest(getAmandmentQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpGet]
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

                getActQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpGet]
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

                getActQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpGet]
        [Route("api/documents/acts/adopted/inprinciple", Name = "GetAllAdoptedInPrincipleActs")]
        public IHttpActionResult GetAllAdoptedInPrincipleActs()
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
                getActQuery.SetNewStringVariable("status", "Usvojen u nacelu");

                getActQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getActQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpGet]
        [Route("api/documents/acts/{id}", Name = "GetActById")]
        public IHttpActionResult GetActById(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getActQuery = session.NewAdhocQuery(string.Format("doc('http://www.parliament.rs/documents/acts/{0}.xml')", id));

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);

                if (getActQueryResult.AsString() == "")
                    return BadRequest(string.Format("Document '{0}' does not exist!", id));

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                return Ok(XElement.Parse(getActQueryResult.AsString()));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Alderman")]
        [Route("api/documents/amandments/revoke/{id}", Name = "RevokeAmandment")]
        public IHttpActionResult RevokeAmandment(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var deleteAmandmentQuery = session.NewAdhocQuery(string.Format("xdmp:document-delete('http://www.parliament.rs/documents/amandments/{0}.xml')", id));

                ResultSequence deleteAmandmentQueryResult = session.SubmitRequest(deleteAmandmentQuery);

                if (deleteAmandmentQueryResult.AsString() != "")
                    return BadRequest(string.Format("Error deleting document '{0}'!", id));

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                return Ok();
            }
        }

        [HttpGet]
        [Route("api/documents/acts/{id}/html", Name = "GetActHtmlById")]
        public IHttpActionResult GetActHtmlById(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getActQuery = session.NewAdhocQuery(string.Format("doc('http://www.parliament.rs/documents/acts/{0}.xml')", id));

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);

                if (getActQueryResult.AsString() == "")
                    return BadRequest(string.Format("Document '{0}' does not exist!", id));

                var xmlResult = XDocument.Parse(getActQueryResult.AsString());

                XslCompiledTransform transform = new XslCompiledTransform();
                string xslPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/propis-html.xsl");
                transform.Load(xslPath);

                Request.Headers.Accept.Clear();
                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                using (StringWriter resultWriter = new StringWriter())
                {
                    transform.Transform(xmlResult.CreateNavigator(), null, resultWriter);
                    return Ok(resultWriter.ToString());
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/documents/acts/{id}/pdf", Name = "GetActPdfById")]
        public IHttpActionResult GetActPdfById(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getActQuery = session.NewAdhocQuery(string.Format("doc('http://www.parliament.rs/documents/acts/{0}.xml')", id));

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);

                if (getActQueryResult.AsString() == "")
                    return BadRequest(string.Format("Document '{0}' does not exist!", id));

                XmlDocument xmlResult = new XmlDocument();
                xmlResult.LoadXml(getActQueryResult.AsString());

                XslCompiledTransform transform = new XslCompiledTransform();
                string xslPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/propis-fo.xsl");
                transform.Load(xslPath);

                var pdfGenerator = new Aspose.Pdf.Generator.Pdf();
                pdfGenerator.BindXML(xmlResult, xslPath);

                pdfGenerator.Save(string.Format("{0}.pdf", id), Aspose.Pdf.Generator.SaveType.OpenInBrowser, HttpContext.Current.Response);

                return Ok();
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

                getActQuery.SetNewStringVariable("ime_nadleznog_organa", act.ImeNadleznogOrgana);
                getActQuery.SetNewStringVariable("prezime_nadleznog_organa", act.PrezimeNadleznogOrgana);
                getActQuery.SetNewStringVariable("email_nadleznog_organa", act.EmailNadleznogOrgana);

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Alderman")]
        [Route("api/documents/amandments/filter", Name = "FindAmandments")]
        public IHttpActionResult FindAmandments(AmandmentViewModel amandment)
        {
            if (amandment == null)
                amandment = new AmandmentViewModel();

            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getAmandmentQuery = session.NewModuleInvoke("/GetAmandmentQuery.xqy");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_predlaganja", amandment.DatumVremePredlaganja);
                getAmandmentQuery.SetNewStringVariable("serijski_broj", "");

                getAmandmentQuery.SetNewStringVariable("text", amandment.Text);
                getAmandmentQuery.SetNewStringVariable("datum_vreme_usvajanja", amandment.DatumVremeUsvajanja);
                getAmandmentQuery.SetNewStringVariable("id_propisa", amandment.IdPropisa);

                getAmandmentQuery.SetNewStringVariable("ime_nadleznog_organa", amandment.ImeNadleznogOrgana);
                getAmandmentQuery.SetNewStringVariable("prezime_nadleznog_organa", amandment.PrezimeNadleznogOrgana);
                getAmandmentQuery.SetNewStringVariable("email_nadleznog_organa", amandment.EmailNadleznogOrgana);

                ResultSequence getActQueryResult = session.SubmitRequest(getAmandmentQuery);
                var xmlResult = XElement.Parse(getActQueryResult.AsString());

                return Ok(xmlResult);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Chairman")]
        [Route("api/documents/acts/adopt/inprinciple/{id}", Name = "AdoptActInPrinciple")]
        public IHttpActionResult AdoptActInPrinciple(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var setPropertyQuery = session.NewModuleInvoke("/ChangeActStatusQuery.xqy");
                setPropertyQuery.SetNewStringVariable("document", string.Format("http://www.parliament.rs/documents/acts/{0}.xml", id));
                setPropertyQuery.SetNewStringVariable("status", "Usvojen u nacelu");

                ResultSequence setPropertyQueryResult = session.SubmitRequest(setPropertyQuery);

                if (setPropertyQueryResult.AsString() != "")
                    return BadRequest(string.Format("Document with '{0}' id could not be adopted", id));

                return Ok();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Chairman")]
        [Route("api/documents/acts/adopt/{id}", Name = "AdoptAct")]
        public IHttpActionResult AdoptAct(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getActQuery = session.NewAdhocQuery(string.Format("doc('http://www.parliament.rs/documents/acts/{0}.xml')", id));

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);

                if (getActQueryResult.AsString() == "")
                    return BadRequest(string.Format("Document '{0}' does not exist!", id));

                XmlDocument document = new XmlDocument();
                document.LoadXml(getActQueryResult.AsString());

                var getAmandmentQuery = session.NewModuleInvoke("/GetAmandmentQuery.xqy");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_predlaganja", "");
                getAmandmentQuery.SetNewStringVariable("serijski_broj", "");

                getAmandmentQuery.SetNewStringVariable("text", "");
                getAmandmentQuery.SetNewStringVariable("datum_vreme_usvajanja", "");
                getAmandmentQuery.SetNewStringVariable("id_propisa", id);

                getAmandmentQuery.SetNewStringVariable("ime_nadleznog_organa", "");
                getAmandmentQuery.SetNewStringVariable("prezime_nadleznog_organa", "");
                getAmandmentQuery.SetNewStringVariable("email_nadleznog_organa", "");

                ResultSequence getAmandmentQueryResult = session.SubmitRequest(getAmandmentQuery);
                var xmlResult = XElement.Parse(getAmandmentQueryResult.AsString());

                foreach (var amandman in xmlResult.Elements("Amandman"))
                {
                    var getAmandmentQuery1 = session.NewAdhocQuery(string.Format("doc('http://www.parliament.rs/documents/amandments/{0}.xml')", amandman.CreateNavigator().SelectSingleNode("Id").Value));
                    ResultSequence getAmandmentQueryResult1 = session.SubmitRequest(getAmandmentQuery1);

                    if (getAmandmentQueryResult1.AsString() == "")
                        return BadRequest(string.Format("Document '{0}' does not exist!", id));

                    XmlDocument amandmanDoc = new XmlDocument();
                    amandmanDoc.LoadXml(getAmandmentQueryResult1.AsString());

                    XMLUtils.Merge(document, amandmanDoc);

					
                }

                var deleteQuery = session.NewAdhocQuery(string.Format("xdmp:document-delete('http://www.parliament.rs/documents/acts/{0}.xml')", id));
                session.SubmitRequest(deleteQuery);

                var updateDocQuery = session.NewModuleInvoke("/AddActQuery.xqy");
                updateDocQuery.SetNewStringVariable("act_string", document.InnerXml);
                updateDocQuery.SetNewStringVariable("id", id);

                var setPropertyQuery = session.NewModuleInvoke("/ChangeActStatusQuery.xqy");
                setPropertyQuery.SetNewStringVariable("document", string.Format("http://www.parliament.rs/documents/acts/{0}.xml", id));
                setPropertyQuery.SetNewStringVariable("status", "Usvojen");

                ResultSequence updateDocQueryResult = session.SubmitRequest(updateDocQuery);

                if (updateDocQueryResult.AsString().Contains("Error"))
                    return BadRequest(updateDocQueryResult.AsString());

                ResultSequence setPropertyQueryResult = session.SubmitRequest(setPropertyQuery);

                if (setPropertyQueryResult.AsString() != "")
                    return BadRequest(string.Format("Document with '{0}' id could not be adopted", id));

               

                return Ok();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Chairman")]
        [Route("api/documents/acts/sendtoarchive/{id}", Name = "SendActToArchive")]
        public IHttpActionResult SendActToArchive(string id)
        {
            Uri uri = new Uri(WebConfigurationManager.AppSettings["ParliamentXmlDbConnectionString"]);
            ContentSource contentSource = ContentSourceFactory.NewContentSource(uri);

            using (Session session = contentSource.NewSession())
            {
                var getActQuery = session.NewAdhocQuery(string.Format("doc('http://www.parliament.rs/documents/acts/{0}.xml')", id));

                ResultSequence getActQueryResult = session.SubmitRequest(getActQuery);

                if (getActQueryResult.AsString() == "")
                    return BadRequest(string.Format("Document '{0}' does not exist!", id));

                XmlDocument document = new XmlDocument();
                document.LoadXml(getActQueryResult.AsString());

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);

                var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, "parliament.sgns", false);
                var targetCertificate = certificates.Count > 0 ? certificates[0] : null;

                store.Close();

                if (targetCertificate == null)
                    return BadRequest("Could not find users certificate!");

                XMLUtils.EncryptXmlDocument(document, targetCertificate);

                WebRequest request = WebRequest.Create("http://localhost:8904/api/auth/token");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                string body = "grant_type=password&username=sgns@parliament.rs&password=SuperPassword123@&client_id=f360404661eb4520b96f8a226792caf7";

                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] byte1 = encoding.GetBytes(body);

                request.ContentLength = byte1.Length;

                Stream newStream = request.GetRequestStream();

                newStream.Write(byte1, 0, byte1.Length);

                var response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    var token = sr.ReadToEnd().Split(',')[0];

                    WebRequest request1 = WebRequest.Create("http://localhost:9009/api/documents/save/act");
                    request1.Method = "POST";
                    request1.ContentType = "text/xml";
                    request1.Headers.Add("Authorization", "Bearer " + token);

                    byte[] byte2 = encoding.GetBytes(document.InnerXml);

                    Stream newStream1 = request1.GetRequestStream();

                    newStream1.Write(byte2, 0, byte2.Length);

                    var response1 = (HttpWebResponse)request1.GetResponse();
                }

                return Ok();
            }
        }
    }
}
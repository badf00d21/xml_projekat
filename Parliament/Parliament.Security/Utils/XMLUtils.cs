﻿using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parliament.Security
{
	public static class XMLUtils
	{
		public static void SignXmlDocument(XmlDocument document, X509Certificate2 certificate)
		{
			SignedXml signedXml = new SignedXml(document);

			signedXml.SigningKey = certificate.PrivateKey;

			KeyInfo keyInfo = new KeyInfo();
			KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
			keyInfo.AddClause(keyInfoData);

			signedXml.KeyInfo = keyInfo;

			Reference reference = new Reference();
			reference.Uri = "";

			XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
			reference.AddTransform(env);

			signedXml.AddReference(reference);
			signedXml.ComputeSignature();

			XmlElement xmlDigitalSignature = signedXml.GetXml();
			document.DocumentElement.AppendChild(document.ImportNode(xmlDigitalSignature, true));
		}

		public static void EncryptXmlDocument(XmlDocument document, X509Certificate2 certificate)
		{
			EncryptedXml eXml = new EncryptedXml();
			EncryptedData edElement = eXml.Encrypt(document.DocumentElement, certificate);

			EncryptedXml.ReplaceElement(document.DocumentElement, edElement, false);
		}

		public static void DecryptXmlDocument(XmlDocument document, X509Certificate2 certificate)
		{
			EncryptedXml eXml = new EncryptedXml(document);
			eXml.AddKeyNameMapping("rsaKey", certificate.PrivateKey);

			eXml.DecryptDocument();
		}

		public static void FillAutoGeneratedData(XmlDocument document)
		{
			var nsmgr = new XmlNamespaceManager(document.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			var deoNodes = document.GetElementsByTagName("Deo", "http://www.parliament.rs/schema");

			for (int i = 0; i < deoNodes.Count; i++)
			{
				if (deoNodes[i].Attributes["parliament:RedniBroj"] == null)
					deoNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (deoNodes[i].Attributes["parliament:Id"] == null)
					deoNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				deoNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				deoNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var glavaNodes = document.GetElementsByTagName("Glava", "http://www.parliament.rs/schema");

			for (int i = 0; i < glavaNodes.Count; i++)
			{
				if (glavaNodes[i].Attributes["parliament:RedniBroj"] == null)
					glavaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (glavaNodes[i].Attributes["parliament:Id"] == null)
					glavaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				glavaNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				glavaNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var odeljakNodes = document.GetElementsByTagName("Odeljak", "http://www.parliament.rs/schema");

			for (int i = 0; i < odeljakNodes.Count; i++)
			{
				if (odeljakNodes[i].Attributes["parliament:RedniBroj"] == null)
					odeljakNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (odeljakNodes[i].Attributes["parliament:Id"] == null)
					odeljakNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				odeljakNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				odeljakNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var pododeljakNodes = document.GetElementsByTagName("Pododeljak", "http://www.parliament.rs/schema");

			for (int i = 0; i < pododeljakNodes.Count; i++)
			{
				if (pododeljakNodes[i].Attributes["parliament:RedniBroj"] == null)
					pododeljakNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (pododeljakNodes[i].Attributes["parliament:Id"] == null)
					pododeljakNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				pododeljakNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				pododeljakNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var clanNodes = document.GetElementsByTagName("Clan", "http://www.parliament.rs/schema");

			for (int i = 0; i < clanNodes.Count; i++)
			{
				if (clanNodes[i].Attributes["parliament:RedniBroj"] == null)
					clanNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (clanNodes[i].Attributes["parliament:Id"] == null)
					clanNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				clanNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				clanNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var stavNodes = document.GetElementsByTagName("Stav", "http://www.parliament.rs/schema");

			for (int i = 0; i < stavNodes.Count; i++)
			{
				if (stavNodes[i].Attributes["parliament:RedniBroj"] == null)
					stavNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (stavNodes[i].Attributes["parliament:Id"] == null)
					stavNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				stavNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				stavNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var tackaNodes = document.GetElementsByTagName("Tacka", "http://www.parliament.rs/schema");

			for (int i = 0; i < tackaNodes.Count; i++)
			{
				if (tackaNodes[i].Attributes["parliament:RedniBroj"] == null)
					tackaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (tackaNodes[i].Attributes["parliament:Id"] == null)
					tackaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				tackaNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				tackaNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var podtackaNodes = document.GetElementsByTagName("Podtacka", "http://www.parliament.rs/schema");

			for (int i = 0; i < podtackaNodes.Count; i++)
			{
				if (podtackaNodes[i].Attributes["parliament:RedniBroj"] == null)
					podtackaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "RedniBroj", "http://www.parliament.rs/schema"));

				if (podtackaNodes[i].Attributes["parliament:Id"] == null)
					podtackaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				podtackaNodes[i].Attributes["parliament:RedniBroj"].Value = (i + 1).ToString();
				podtackaNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}

			var alinejaNodes = document.GetElementsByTagName("Alineja", "http://www.parliament.rs/schema");

			for (int i = 0; i < alinejaNodes.Count; i++)
			{
				if (alinejaNodes[i].Attributes["parliament:Id"] == null)
					alinejaNodes[i].Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

				alinejaNodes[i].Attributes["parliament:Id"].Value = Guid.NewGuid().ToString();
			}
		}

		public static void AddTimeAndSerialNumber(XmlDocument document)
		{
			var nsmgr = new XmlNamespaceManager(document.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			Random random = new Random();
			var rootNode = document.DocumentElement;

			if (rootNode.Attributes["SerijskiBroj"] == null)
				rootNode.Attributes.Append(document.CreateAttribute("SerijskiBroj"));

			if (rootNode.Attributes["parliament:DatumVremePredlaganja"] == null)
				rootNode.Attributes.Append(document.CreateAttribute("parliament", "DatumVremePredlaganja", "http://www.parliament.rs/schema"));

			rootNode.Attributes["SerijskiBroj"].Value = random.Next(100000).ToString();
			rootNode.Attributes["parliament:DatumVremePredlaganja"].Value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
		}

		public static string AddDocumentId(XmlDocument document)
		{
			string id = Guid.NewGuid().ToString();

			var nsmgr = new XmlNamespaceManager(document.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			var rootNode = document.DocumentElement;

			if (rootNode.Attributes["parliament:Id"] == null)
				rootNode.Attributes.Append(document.CreateAttribute("parliament", "Id", "http://www.parliament.rs/schema"));

			rootNode.Attributes["parliament:Id"].Value = id;

			return id;
		}

		public static void AddActUserInfo(XmlDocument document, string firstName, string lastName, string email, string role)
		{
			var nsmgr = new XmlNamespaceManager(document.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			document.SelectSingleNode("/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan/parliament:Ime", nsmgr).InnerText = firstName;
			document.SelectSingleNode("/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan/parliament:Prezime", nsmgr).InnerText = lastName;
			document.SelectSingleNode("/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan/parliament:Email", nsmgr).InnerText = email;
			document.SelectSingleNode("/parliament:Propis/parliament:Preambula/parliament:NadlezniOrgan", nsmgr).Attributes["Uloga"].Value = role;
		}

		public static void AddAmandmentUserInfo(XmlDocument document, string firstName, string lastName, string email, string role)
		{
			var nsmgr = new XmlNamespaceManager(document.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			document.SelectSingleNode("/parliament:Amandman/parliament:NadlezniOrgan/parliament:Ime", nsmgr).InnerText = firstName;
			document.SelectSingleNode("/parliament:Amandman/parliament:NadlezniOrgan/parliament:Prezime", nsmgr).InnerText = lastName;
			document.SelectSingleNode("/parliament:Amandman/parliament:NadlezniOrgan/parliament:Email", nsmgr).InnerText = email;
			document.SelectSingleNode("/parliament:Amandman/parliament:NadlezniOrgan", nsmgr).Attributes["Uloga"].Value = role;
		}

        public static void Merge(XmlDocument act, XmlDocument amandment)
        {
			var nsmgr = new XmlNamespaceManager(amandment.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			var nsmgr1 = new XmlNamespaceManager(act.NameTable);
			nsmgr.AddNamespace("parliament", "http://www.parliament.rs/schema");

			var modifications = amandment.SelectNodes("/parliament:Amandman/parliament:Modifikacija", nsmgr);

			for (int i = 0; i < modifications.Count; i++)
			{
				if (modifications[i].Attributes["PredmetModifikacije"] == null || modifications[i].Attributes["TipModifikacije"] == null)
					continue;

				string predmetModifikacije = modifications[i].Attributes["PredmetModifikacije"].Value;
				string tipModifikacije = modifications[i].Attributes["TipModifikacije"].Value;

				if (tipModifikacije == "Dodavanje")
				{
					var clanNodes = act.GetElementsByTagName("parliament:Clan");

					for (int j = 0; j < clanNodes.Count; j++)
					{
						if (clanNodes[j].Attributes["parliament:Id"].Value == predmetModifikacije)
						{
							if (clanNodes[j].SelectNodes("parliament:Stav", nsmgr).Count > 0)
								break;

							clanNodes[j].SelectSingleNode("parliament:TekstualniSadrzaj", nsmgr).InnerText += modifications[i].InnerText;
							break;
						}
					}
				}
				else if (tipModifikacije == "Izmena")
				{
					var clanNodes = act.GetElementsByTagName("parliament:Clan");

					for (int j = 0; j < clanNodes.Count; j++)
					{
						if (clanNodes[j].Attributes["parliament:Id"].Value == predmetModifikacije)
						{
							if (clanNodes[j].SelectNodes("parliament:Stav", nsmgr).Count > 0)
								break;

							clanNodes[j].SelectSingleNode("parliament:TekstualniSadrzaj", nsmgr).InnerText = modifications[i].InnerText;
							break;
						}
					}
				}
				else if (tipModifikacije == "Brisanje")
				{
					var clanNodes = act.GetElementsByTagName("parliament:Clan");

					for (int j = 0; j < clanNodes.Count; j++)
					{
						if (clanNodes[j].Attributes["parliament:Id"].Value == predmetModifikacije)
						{
							clanNodes[j].ParentNode.RemoveChild(clanNodes[i]);
							break;
						}
					}
				}
				
			}
			
        }

	}
}

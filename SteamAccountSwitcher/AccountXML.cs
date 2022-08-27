using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SteamAccountSwitcher
{
	public class AccountXML
	{
		public AccountSwitcher Parent { get; set; }

		private XmlDocument _xmlDoc = new XmlDocument();
		private XmlElement _xmlDocBody;
		private string _xmlDocContent;

		private string _xmlPath, _backupPath;
		private bool _purgeOnRootFailure, _performBackups;

		private bool _xmlLock;

		public AccountXML(string settingsPath, AccountSwitcher accountSwitcher, bool deleteFileIfRootMissing = true, bool performBackups = true)
		{
			Parent = accountSwitcher;

			_xmlPath = settingsPath;
			_purgeOnRootFailure = deleteFileIfRootMissing;
			_performBackups = performBackups;
			_backupPath = Path.ChangeExtension(settingsPath, Path.GetExtension(settingsPath) + ".bkup");

			InitXML();
		}

		public bool AddAccount(Account account)
		{
			string nodeName = "uid_" + account.UniqueID;

			if (!DoesDuplicateExist(nodeName))
			{
				XmlNode accountsNode = AddToHeadingNode("accounts");
				XmlNode accNode = _xmlDoc.CreateElement(nodeName);

				XmlNode uidNode = _xmlDoc.CreateElement("uid");
				uidNode.InnerText = account.UniqueID.ToString();
				accNode.AppendChild(uidNode);

				XmlNode userNameNode = _xmlDoc.CreateElement("userName");
				userNameNode.InnerText = account.Username;
				accNode.AppendChild(userNameNode);

				XmlNode displayNameNode = _xmlDoc.CreateElement("displayName");
				displayNameNode.InnerText = account.CustomDisplayName;
				accNode.AppendChild(displayNameNode);

				XmlNode descriptionNode = _xmlDoc.CreateElement("description");
				descriptionNode.InnerText = account.Description;
				accNode.AppendChild(descriptionNode);

				XmlNode posNode = _xmlDoc.CreateElement("uiPos");
				posNode.InnerText = account.Position.ToString();
				accNode.AppendChild(posNode);

				accountsNode.AppendChild(accNode);

				SaveXML();
				return true;
			}
			return false;
		}

		public bool EditAccount(Account account)
		{
			string nodeName = "uid_" + account.UniqueID;

			LoadXML();
			XmlNodeList elemList = _xmlDoc.GetElementsByTagName(nodeName);

			if (elemList[0].SelectSingleNode("displayName") == null)
			{
				XmlNode displayNameNode = _xmlDoc.CreateElement("displayName");
				displayNameNode.InnerText = account.CustomDisplayName;
				elemList[0].AppendChild(displayNameNode);
			}
			if (elemList[0].SelectSingleNode("description") == null)
			{
				XmlNode descriptionNode = _xmlDoc.CreateElement("description");
				descriptionNode.InnerText = account.Description;
				elemList[0].AppendChild(descriptionNode);
			}
			if (elemList[0].SelectSingleNode("uiPos") == null)
			{
				XmlNode posNode = _xmlDoc.CreateElement("uiPos");
				posNode.InnerText = account.Position.ToString();
				elemList[0].AppendChild(posNode);
			}

			if (elemList.Count == 1)
			{
				foreach (XmlNode node in elemList[0].ChildNodes)
				{
					switch (node.Name)
					{
						case "uid":
							node.InnerText = account.UniqueID.ToString();
							break;
						case "userName":
							node.InnerText = account.Username;
							break;
						case "displayName":
							node.InnerText = account.CustomDisplayName;
							break;
						case "description":
							node.InnerText = account.Description;
							break;
						case "uiPos":
							node.InnerText = account.Position.ToString();
							break;
					}
				}

				SaveXML();
				return true;
			}
			return false;
		}

		public Account LoadAccount(Int64 uniqueID)
		{
			try
			{
				LoadXML();
				XmlNodeList elemList = _xmlDoc.GetElementsByTagName("uid_" + uniqueID);

				if (elemList.Count > 0)
				{
					Account account = new Account(uniqueID);
					foreach (XmlNode node in elemList[0].ChildNodes)
					{
						if (node.Name == "userName")
							account.Username = node.InnerText;
						else if (node.Name == "displayName")
							account.CustomDisplayName = node.InnerText;
						else if (node.Name == "description")
							account.Description = node.InnerText;
						else if (node.Name == "uiPos")
							account.Position = Convert.ToInt32(node.InnerText);
					}
					return account;
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		public List<Account> LoadAllAccounts()
		{
			XmlNodeList elemList = _xmlDoc.GetElementsByTagName("accounts");

			if (elemList.Count == 1)
			{
				List<Account> accounts = new List<Account>();
				foreach (XmlNode node in elemList[0].ChildNodes)
					accounts.Add(LoadAccount(Convert.ToInt64(node.SelectSingleNode("uid").InnerText)));
				return accounts;
			}
			return null;
		}

		public bool RemoveAccount(Account account)
		{
			return RemoveXmlElement(account.UniqueID.ToString());
		}

		private XmlNode AddToHeadingNode(string headingName)
		{
			XmlNode node;

			if (_xmlDoc.DocumentElement[headingName] != null)
				node = _xmlDoc.DocumentElement[headingName];
			else
			{
				node = _xmlDoc.CreateElement(headingName);
				_xmlDocBody.AppendChild(node);
			}
			return node;
		}

		private void InitXML()
		{
			if (!File.Exists(_xmlPath))
			{
				_xmlLock = true;
				CreateXMLFile();
			}
			else LoadXML();
		}

		public void ReloadXMLSettings()
		{
			_xmlDoc = new XmlDocument();
			_xmlDocBody = null;
			_xmlDocContent = null;
			_xmlLock = false;

			InitXML();
		}

		public void CreateXMLFile()
		{
			if (_xmlLock)
			{
				XmlDeclaration xmlDeclaration = _xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
				XmlElement root = _xmlDoc.DocumentElement;

				XmlElement pi = _xmlDoc.CreateElement("SteamAccountSwitcherVersion");
				pi.InnerText = Program.GetVersion();

				_xmlDoc.InsertBefore(xmlDeclaration, root);

				_xmlDocBody = _xmlDoc.CreateElement(string.Empty, "SAS_Accounts", string.Empty);
				_xmlDoc.AppendChild(_xmlDocBody);
				_xmlDocBody.AppendChild(pi);

				_xmlLock = false;

				SaveXML();
			}
		}

		private void RecreateXMLFile(bool deleteBackups = true, bool skipBackup = false)
		{
			if (_performBackups && !deleteBackups && !skipBackup) PerformXMLBackup();
			DeleteXMLFile(deleteBackups);
			CreateXMLFile();
			LoadXML();
		}

		public void PerformXMLBackup()
		{
			if (File.Exists(_xmlPath))
			{
				if (File.Exists(_backupPath)) File.Delete(_backupPath);
				File.Copy(_xmlPath, _backupPath);
			}
		}

		public void DeleteXMLBackups()
		{
			File.Delete(_backupPath);
		}

		public void DeleteXMLFile(bool deleteBackups = true)
		{
			LockXML();

			if (deleteBackups) DeleteXMLBackups();
			File.Delete(_xmlPath);
		}

		private void LoadXML()
		{
			try
			{
				_xmlDoc.Load(_xmlPath);
				_xmlDocContent = _xmlDoc.InnerXml;
				_xmlDocBody = _xmlDoc.DocumentElement;
			}
			catch (Exception)
			{
				if (_purgeOnRootFailure)
				{
					Parent.Log("AccountXML -> 'account.xml' file is corrupted! A backup is being created and the file is being recreated.", Logging.Severity.ERROR);
					RecreateXMLFile(false);
				}
				else Parent.Log("AccountXML -> 'account.xml' file is corrupted! Either enable purgeOnRootFailure or delete the file manually!", Logging.Severity.ERROR);
			}
		}

		private void SaveXML()
		{
			_xmlDoc.Save(_xmlPath);
		}

		private bool RemoveXmlElement(string varName)
		{
			try
			{
				LoadXML();
				XmlNodeList elemList = _xmlDoc.GetElementsByTagName(varName);
				if (elemList.Count > 0)
				{
					elemList[0].ParentNode.RemoveAll();
					SaveXML();
					return true;
				}
			}
			catch
			{ }
			return false;
		}

		private bool DoesDuplicateExist(string varName)
		{
			LoadXML();
			XmlNodeList elemList = _xmlDoc.GetElementsByTagName(varName);
			return elemList.Count > 0;
		}

		private void LockXML()
		{
			_xmlDoc = new XmlDocument();
			_xmlDocBody = null;
			_xmlDocContent = null;
			_xmlLock = true;
		}
	}
}

using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SalesforceMetadata.MetadataWSDL;

namespace SalesforceMetadata
{
    public class SalesforceCredentials
    {
        public Dictionary<String, String> usernamePartnerUrl;
        public Dictionary<String, String> usernameMetadataUrl;
        public Dictionary<String, String> usernameToolingWsdlUrl;
        public Dictionary<String, List<String>> defaultWsdlObjects;
        public Dictionary<String, Boolean> isProduction;

        public SalesforceMetadata.PartnerWSDL.LoginResult fromOrgLR;
        public SalesforceMetadata.PartnerWSDL.SforceService fromOrgSS;
        public SalesforceMetadata.MetadataWSDL.MetadataService fromOrgMS;
        public SalesforceMetadata.ToolingWSDL.SforceServiceService fromOrgToolingSvc;
        public SalesforceMetadata.ToolingWSDL.LoginResult fromOrgToolingLR;

        public SalesforceMetadata.PartnerWSDL.LoginResult toOrgLR;
        public SalesforceMetadata.PartnerWSDL.SforceService toOrgSS;
        public SalesforceMetadata.MetadataWSDL.MetadataService toOrgMS;
        public SalesforceMetadata.ToolingWSDL.SforceServiceService toOrgToolingSvc;
        public SalesforceMetadata.ToolingWSDL.LoginResult toOrgToolingLR;

        public Boolean loginSuccess = false;
        public Boolean toolingLoginSuccess = false;

        public SalesforceCredentials()
        {
            populateUsernameMaps();
        }

        public void salesforceLogin(UtilityClass.REQUESTINGORG reqOrg, String userName)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            loginSuccess = false;

            if (this.fromOrgSS != null)
            {
                try
                {
                    this.fromOrgSS.logout();
                    this.fromOrgLR = null;
                    this.fromOrgMS = null;
                }
                catch (Exception e)
                {
                    // don't care about the error
                }
            }

            if (this.toOrgSS != null)
            {
                try
                {
                    this.toOrgSS.logout();
                    this.toOrgLR = null;
                    this.toOrgMS = null;
                }
                catch (Exception e)
                {
                    // don't care about the error
                }
            }

            // Create a new Salesforce login object
            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                this.fromOrgSS = new SalesforceMetadata.PartnerWSDL.SforceService();
                this.fromOrgLR = new SalesforceMetadata.PartnerWSDL.LoginResult();
                this.fromOrgMS = new SalesforceMetadata.MetadataWSDL.MetadataService();

                try
                {
                    this.fromOrgSS.Timeout = 15000;
                    this.fromOrgSS.Url = this.usernamePartnerUrl[userName];

                    if (this.isProduction[userName] == false)
                    {
                        this.fromOrgLR.sandbox = true;
                    }
                    
                    Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();
                    passwordSecurityToken = getUsernameCredentials(userName);

                    if (passwordSecurityToken.ContainsKey("password") && passwordSecurityToken.ContainsKey("securitytoken"))
                    {
                        String pwdSecToken = passwordSecurityToken["password"] + passwordSecurityToken["securitytoken"];

                        this.fromOrgLR = this.fromOrgSS.login(userName, pwdSecToken);
                        if (this.fromOrgLR.sessionId != null && this.fromOrgLR.passwordExpired == false)
                        {
                            this.loginSuccess = true;
                        }
                    }
                    else
                    {
                        this.fromOrgLR = this.fromOrgSS.login(userName, passwordSecurityToken["password"]);
                        if (this.fromOrgLR.sessionId != null && this.fromOrgLR.passwordExpired == false)
                        {
                            this.loginSuccess = true;
                        }
                    }

                    // Get the login result and update the Salesforce Service object
                    this.fromOrgSS.Url = this.fromOrgLR.serverUrl;
                    this.fromOrgSS.SessionHeaderValue = new PartnerWSDL.SessionHeader();
                    this.fromOrgSS.SessionHeaderValue.sessionId = this.fromOrgLR.sessionId;

                    // Set up the Metadata Service connection including the All Or None Header
                    this.fromOrgMS.Url = this.fromOrgLR.metadataServerUrl;
                    this.fromOrgMS.SessionHeaderValue = new MetadataWSDL.SessionHeader();
                    this.fromOrgMS.SessionHeaderValue.sessionId = this.fromOrgLR.sessionId;
                }
                catch (Exception loginFromExc1)
                {
                    Console.WriteLine("loginFromExc1: " + loginFromExc1.Message);
                }
            }


            ///* TOORG Login Credentials */
            if (reqOrg == UtilityClass.REQUESTINGORG.TOORG
               && String.IsNullOrEmpty(userName) == false)
            {
                this.toOrgSS = new SalesforceMetadata.PartnerWSDL.SforceService();
                this.toOrgLR = new SalesforceMetadata.PartnerWSDL.LoginResult();
                this.toOrgMS = new SalesforceMetadata.MetadataWSDL.MetadataService();

                try
                {
                    this.toOrgSS.Timeout = 15000;
                    this.toOrgSS.Url = this.usernamePartnerUrl[userName];

                    if (this.isProduction[userName] == false)
                    {
                        this.toOrgLR.sandbox = true;
                    }

                    Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();
                    passwordSecurityToken = getUsernameCredentials(userName);

                    if (passwordSecurityToken.ContainsKey("password") && passwordSecurityToken.ContainsKey("securitytoken"))
                    {
                        String pwdSecToken = passwordSecurityToken["password"] + passwordSecurityToken["securitytoken"];

                        this.toOrgLR = this.toOrgSS.login(userName, pwdSecToken);
                        if (this.toOrgLR.sessionId != null && this.toOrgLR.passwordExpired == false)
                        {
                            this.loginSuccess = true;
                        }
                    }
                    else
                    {
                        this.toOrgLR = this.toOrgSS.login(userName, passwordSecurityToken["password"]);
                        if (this.toOrgLR.sessionId != null && this.toOrgLR.passwordExpired == false)
                        {
                            this.loginSuccess = true;
                        }
                    }

                    // Get the login result and update the Salesforce Service object
                    this.toOrgSS.Url = toOrgLR.serverUrl;
                    this.toOrgSS.SessionHeaderValue = new PartnerWSDL.SessionHeader();
                    this.toOrgSS.SessionHeaderValue.sessionId = toOrgLR.sessionId;

                    // Set up the Metadata Service connection including the All Or None Header
                    this.toOrgMS.Url = toOrgLR.metadataServerUrl;
                    this.toOrgMS.SessionHeaderValue = new MetadataWSDL.SessionHeader();
                    this.toOrgMS.SessionHeaderValue.sessionId = toOrgLR.sessionId;
                }
                catch (Exception loginToExc2)
                {
                    Console.WriteLine("loginToExc2: " + loginToExc2.Message);
                }
            }
        }

        public void salesforceToolingLogin(UtilityClass.REQUESTINGORG reqOrg, String userName)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            this.toolingLoginSuccess = false;

            this.fromOrgToolingSvc = new SalesforceMetadata.ToolingWSDL.SforceServiceService();
            this.fromOrgToolingLR = new SalesforceMetadata.ToolingWSDL.LoginResult();

            try
            {
                if (this.isProduction[userName] == false)
                {
                    this.fromOrgToolingSvc.Url = this.usernameToolingWsdlUrl[userName];
                    this.fromOrgToolingLR.sandbox = true;
                }

                Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();
                passwordSecurityToken = getUsernameCredentials(userName);

                if (passwordSecurityToken.ContainsKey("password") && passwordSecurityToken.ContainsKey("securitytoken"))
                {
                    this.fromOrgToolingLR = this.fromOrgToolingSvc.login(userName, passwordSecurityToken["password"] + passwordSecurityToken["securitytoken"]);
                    if (this.fromOrgToolingLR.sessionId != null && this.fromOrgToolingLR.passwordExpired == false)
                    {
                        this.toolingLoginSuccess = true;
                    }
                }
                else
                {
                    this.fromOrgToolingLR = this.fromOrgToolingSvc.login(userName, passwordSecurityToken["password"]);
                    if (this.fromOrgToolingLR.sessionId != null && this.fromOrgToolingLR.passwordExpired == false)
                    {
                        toolingLoginSuccess = true;
                    }
                }

                // Get the login result and update the Salesforce Service object
                this.fromOrgToolingSvc.Url = fromOrgToolingLR.serverUrl;
                this.fromOrgToolingSvc.SessionHeaderValue = new ToolingWSDL.SessionHeader();
                this.fromOrgToolingSvc.SessionHeaderValue.sessionId = fromOrgToolingLR.sessionId;
            }
            catch (Exception loginFromExc2)
            {
            }
        }

        public void salesforceLogout()
        {
            try
            {
                if(this.fromOrgSS != null) this.fromOrgSS.logout();
                
            }
            catch (Exception e) { }

            try
            {
                if (toOrgSS != null) this.toOrgSS.logout();
            }
            catch (Exception e) { }
        }

        public SalesforceMetadata.PartnerWSDL.DescribeGlobalResult getDescribeGlobalResult(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.PartnerWSDL.DescribeGlobalResult dgr = new SalesforceMetadata.PartnerWSDL.DescribeGlobalResult();
            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                dgr = this.fromOrgSS.describeGlobal();
            }
            else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
            {
                dgr = this.toOrgSS.describeGlobal();
            }

            return dgr;
        }

        //public SalesforceMetadata.PartnerWSDL.DescribeLayout[] getDescribeLayout(UtilityClass.REQUESTINGORG reqOrg, String[] sobjectType)
        //{
        //    SalesforceMetadata.PartnerWSDL.DescribeLayout[] descrLayout;

        //    foreach (String sobjType in sobjectType)
        //    {
        //        if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
        //        {
        //            descrLayout = this.fromOrgSS.describeLayout();
        //        }
        //    }

        //    return descrTabs;

        //}
        public SalesforceMetadata.PartnerWSDL.DescribeTab[] getDescribeTab(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.PartnerWSDL.DescribeTab[] descrTabs;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                descrTabs = this.fromOrgSS.describeAllTabs();
            }
            else
            {
                descrTabs = new SalesforceMetadata.PartnerWSDL.DescribeTab[1];
            }

            return descrTabs;
        }

        public SalesforceMetadata.MetadataWSDL.DescribeMetadataResult getDescribeMetadataResult(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.MetadataWSDL.DescribeMetadataResult dmd = new SalesforceMetadata.MetadataWSDL.DescribeMetadataResult();
            try
            {
                if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
                {
                    dmd = this.fromOrgMS.describeMetadata(Convert.ToDouble(Properties.Settings.Default.DefaultAPI));
                }
                else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
                {
                    dmd = this.toOrgMS.describeMetadata(Convert.ToDouble(Properties.Settings.Default.DefaultAPI));
                }
            }
            catch (Exception exc)
            {

                SalesforceMetadata.MetadataWSDL.DescribeMetadataObject dmo = new SalesforceMetadata.MetadataWSDL.DescribeMetadataObject();
                dmo.xmlName = exc.Message;
                dmo.directoryName = exc.Message;

                SalesforceMetadata.MetadataWSDL.DescribeMetadataObject[] dmoArray = new SalesforceMetadata.MetadataWSDL.DescribeMetadataObject[1];
                dmoArray[0] = dmo;

                dmd.metadataObjects = dmoArray;
            }

            return dmd;
        }

        public SalesforceMetadata.MetadataWSDL.MetadataService getMetadataService(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.MetadataWSDL.MetadataService ms = new SalesforceMetadata.MetadataWSDL.MetadataService();
            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                ms = this.fromOrgMS;
            }
            else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
            {
                ms = this.toOrgMS;
            }

            return ms;
        }

        public SalesforceMetadata.MetadataWSDL.FileProperties[] listMetadata(String metadataType, UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.MetadataWSDL.FileProperties[] fp;

            try
            {
                SalesforceMetadata.MetadataWSDL.ListMetadataQuery query = new SalesforceMetadata.MetadataWSDL.ListMetadataQuery();
                query.type = metadataType;

                //query.setFolder(null);
                Double asOfVersion = Convert.ToDouble(Properties.Settings.Default.DefaultAPI);
                // Assuming that the SOAP binding has already been established.
                SalesforceMetadata.MetadataWSDL.FileProperties[] fpList = this.fromOrgMS.listMetadata( new ListMetadataQuery[] { query }, asOfVersion);
                if (fpList != null)
                {
                    fp = fpList;
                }
                else
                {
                    fp = new SalesforceMetadata.MetadataWSDL.FileProperties[1];
                }
            }
            catch (Exception exc)
            {
                // Return an empty list
                fp = new SalesforceMetadata.MetadataWSDL.FileProperties[1];
            }

            return fp;
        }


        public void populateUsernameMaps()
        {
            this.usernamePartnerUrl = new Dictionary<String, String>();
            this.usernameMetadataUrl = new Dictionary<String, String>();
            this.usernameToolingWsdlUrl = new Dictionary<String, String>();
            this.isProduction = new Dictionary<String, Boolean>();
            this.defaultWsdlObjects = new Dictionary<String, List<String>>();

            // Decrypt the contents of the file and place in an XML Document format
            StreamReader encryptedContents = new StreamReader(Properties.Settings.Default.UserAndAPIFileLocation);
            StreamReader sharedSecret = new StreamReader(Properties.Settings.Default.SharedSecretLocation);
            String decryptedContents = Crypto.DecryptString(encryptedContents.ReadToEnd(),
                                                            sharedSecret.ReadToEnd(),
                                                            Properties.Settings.Default.Salt);

            encryptedContents.Close();
            sharedSecret.Close();

            XmlDocument sfUser = new XmlDocument();
            sfUser.LoadXml(decryptedContents);

            XmlNodeList documentNodes = sfUser.GetElementsByTagName("usersetting");

            for (int i = 0; i < documentNodes.Count; i++)
            {
                String username = "";
                String partnerWsdlUrl = "";
                String metadataWdldUrl = "";
                String toolingWsdlUrl = "";
                Boolean isProd = false;
                List<String> defaultWsdlObjectList = new List<String>();
                foreach (XmlNode childNode in documentNodes[i].ChildNodes)
                {
                    if (childNode.Name == "username")
                    {
                        username = childNode.InnerText;
                    }

                    if (childNode.Name == "isproduction")
                    {
                        isProd = Convert.ToBoolean(childNode.InnerText);
                    }

                    if (childNode.Name == "partnerwsdlurl")
                    {
                        partnerWsdlUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "metadatawsdlurl")
                    {
                        metadataWdldUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "toolingwsdlurl")
                    {
                        toolingWsdlUrl = childNode.InnerText;
                    }

                    if (childNode.Name == "defaultpackages" && childNode.HasChildNodes)
                    {
                        XmlNodeList defObjects = childNode.ChildNodes;
                        foreach (XmlNode obj in defObjects)
                        {
                            defaultWsdlObjectList.Add(obj.InnerText);
                        }
                    }
                }

                this.usernamePartnerUrl.Add(username, partnerWsdlUrl);
                this.usernameMetadataUrl.Add(username, metadataWdldUrl);
                this.isProduction.Add(username, isProd);

                if (defaultWsdlObjectList.Count > 0)
                {
                    this.defaultWsdlObjects.Add(username, defaultWsdlObjectList);
                }

                if (toolingWsdlUrl != "")
                {
                    this.usernameToolingWsdlUrl.Add(username, toolingWsdlUrl);
                }
            }
        }

        private Dictionary<String, String> getUsernameCredentials(String userName)
        {
            Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();

            StreamReader encryptedContents = new StreamReader(Properties.Settings.Default.UserAndAPIFileLocation);
            StreamReader sharedSecret = new StreamReader(Properties.Settings.Default.SharedSecretLocation);
            String decryptedContents = Crypto.DecryptString(encryptedContents.ReadToEnd(),
                                                            sharedSecret.ReadToEnd(),
                                                            Properties.Settings.Default.Salt);

            encryptedContents.Close();
            sharedSecret.Close();

            XmlDocument sfUser = new XmlDocument();
            sfUser.LoadXml(decryptedContents);

            XmlNodeList documentNodes = sfUser.GetElementsByTagName("usersetting");

            Boolean usernameFound = false;
            for (int i = 0; i < documentNodes.Count; i++)
            {
                foreach (XmlNode childNode in documentNodes[i].ChildNodes)
                {
                    if (childNode.Name == "username")
                    {
                        if (childNode.InnerText == userName)
                        {
                            usernameFound = true;
                        }
                    }

                    if (usernameFound == true && childNode.Name == "password")
                    {
                        passwordSecurityToken.Add("password", childNode.InnerText);
                    }

                    if (usernameFound == true && childNode.Name == "securitytoken")
                    {
                        passwordSecurityToken.Add("securitytoken", childNode.InnerText);
                    }
                }

                if (usernameFound == true) break;
            }

            return passwordSecurityToken;
        }
    }

}

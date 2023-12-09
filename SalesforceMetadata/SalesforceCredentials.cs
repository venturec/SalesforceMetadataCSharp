using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SalesforceMetadata
{
    class SalesforceCredentials
    {
        public static Dictionary<String, String> usernamePartnerUrl;
        public static Dictionary<String, String> usernameMetadataUrl;
        public static Dictionary<String, String> usernameToolingWsdlUrl;
        public static Dictionary<String, List<String>> defaultWsdlObjects;
        public static Dictionary<String, Boolean> isProduction;

        public static SalesforceMetadata.PartnerWSDL.LoginResult fromOrgLR;
        public static SalesforceMetadata.PartnerWSDL.SforceService fromOrgSS;
        public static SalesforceMetadata.MetadataWSDL.MetadataService fromOrgMS;
        public static SalesforceMetadata.ToolingWSDL.SforceServiceService fromOrgToolingSvc;
        public static SalesforceMetadata.ToolingWSDL.LoginResult fromOrgToolingLR;

        public static SalesforceMetadata.PartnerWSDL.LoginResult toOrgLR;
        public static SalesforceMetadata.PartnerWSDL.SforceService toOrgSS;
        public static SalesforceMetadata.MetadataWSDL.MetadataService toOrgMS;
        public static SalesforceMetadata.ToolingWSDL.SforceServiceService toOrgToolingSvc;
        public static SalesforceMetadata.ToolingWSDL.LoginResult toOrgToolingLR;

        private static int ONE_SECOND = 1000;
        private static int MAX_NUM_POLL_REQUESTS = 50;
        public static Boolean salesforceLogin(UtilityClass.REQUESTINGORG reqOrg, String userName)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Boolean loginSuccess = false;
            int poll = 0;
            int waitTimeMilliSecs = ONE_SECOND;

            if (fromOrgSS != null)
            {
                try
                {
                    fromOrgSS.logout();
                    fromOrgLR = null;
                    fromOrgMS = null;
                }
                catch (Exception e)
                {
                    // don't care about the error
                }
            }

            if (toOrgSS != null)
            {
                try
                {
                    toOrgSS.logout();
                    toOrgLR = null;
                    toOrgMS = null;
                }
                catch (Exception e)
                {
                    // don't care about the error
                }
            }

            // Create a new Salesforce login object
            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                fromOrgSS = new SalesforceMetadata.PartnerWSDL.SforceService();
                fromOrgLR = new SalesforceMetadata.PartnerWSDL.LoginResult();
                fromOrgMS = new SalesforceMetadata.MetadataWSDL.MetadataService();

                try
                {
                    fromOrgSS.Timeout = 5000;

                    if (isProduction[userName] == false)
                    {
                        fromOrgSS.Url = usernamePartnerUrl[userName];
                        fromOrgLR.sandbox = true;
                    }
                    
                    Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();
                    passwordSecurityToken = getUsernameCredentials(userName);

                    if (passwordSecurityToken.ContainsKey("password") && passwordSecurityToken.ContainsKey("securitytoken"))
                    {
                        String pwdSecToken = passwordSecurityToken["password"] + passwordSecurityToken["securitytoken"];

                        fromOrgLR = fromOrgSS.login(userName, pwdSecToken);
                        if (fromOrgLR.sessionId != null && fromOrgLR.passwordExpired == false)
                        {
                            loginSuccess = true;
                        }
                    }
                    else
                    {
                        fromOrgLR = fromOrgSS.login(userName, passwordSecurityToken["password"]);
                        if (fromOrgLR.sessionId != null && fromOrgLR.passwordExpired == false)
                        {
                            loginSuccess = true;
                        }
                    }

                    // Get the login result and update the Salesforce Service object
                    fromOrgSS.Url = fromOrgLR.serverUrl;
                    fromOrgSS.SessionHeaderValue = new PartnerWSDL.SessionHeader();
                    fromOrgSS.SessionHeaderValue.sessionId = fromOrgLR.sessionId;

                    // Set up the Metadata Service connection including the All Or None Header
                    fromOrgMS.Url = fromOrgLR.metadataServerUrl;
                    fromOrgMS.SessionHeaderValue = new MetadataWSDL.SessionHeader();
                    fromOrgMS.SessionHeaderValue.sessionId = fromOrgLR.sessionId;
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
                toOrgSS = new SalesforceMetadata.PartnerWSDL.SforceService();
                toOrgLR = new SalesforceMetadata.PartnerWSDL.LoginResult();
                toOrgMS = new SalesforceMetadata.MetadataWSDL.MetadataService();

                try
                {
                    toOrgLR.metadataServerUrl = usernamePartnerUrl[userName];

                    if (isProduction[userName] == false)
                    {
                        toOrgSS.Url = usernamePartnerUrl[userName];
                        toOrgLR.sandbox = true;
                    }

                    Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();
                    passwordSecurityToken = getUsernameCredentials(userName);

                    if (passwordSecurityToken.ContainsKey("password") && passwordSecurityToken.ContainsKey("securitytoken"))
                    {
                        toOrgLR = toOrgSS.login(userName, passwordSecurityToken["password"] + passwordSecurityToken["securitytoken"]);
                        if (toOrgLR.sessionId != null && toOrgLR.passwordExpired == false)
                        {
                            loginSuccess = true;
                        }
                    }
                    else
                    {
                        toOrgLR = toOrgSS.login(userName, passwordSecurityToken["password"]);
                        if (toOrgLR.sessionId != null && toOrgLR.passwordExpired == false)
                        {
                            loginSuccess = true;
                        }
                    }

                    // Get the login result and update the Salesforce Service object
                    toOrgSS.Url = toOrgLR.serverUrl;
                    toOrgSS.SessionHeaderValue = new PartnerWSDL.SessionHeader();
                    toOrgSS.SessionHeaderValue.sessionId = toOrgLR.sessionId;

                    // Set up the Metadata Service connection including the All Or None Header
                    toOrgMS.Url = toOrgLR.metadataServerUrl;
                    toOrgMS.SessionHeaderValue = new MetadataWSDL.SessionHeader();
                    toOrgMS.SessionHeaderValue.sessionId = toOrgLR.sessionId;
                }
                catch (Exception loginToExc2)
                {
                    Console.WriteLine("loginToExc2: " + loginToExc2.Message);
                }
            }

            return loginSuccess;
        }

        public static Boolean salesforceToolingLogin(UtilityClass.REQUESTINGORG reqOrg, String userName)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Boolean loginSuccess = false;

            fromOrgToolingSvc = new SalesforceMetadata.ToolingWSDL.SforceServiceService();
            fromOrgToolingLR = new SalesforceMetadata.ToolingWSDL.LoginResult();

            try
            {
                if (isProduction[userName] == false)
                {
                    fromOrgToolingSvc.Url = usernameToolingWsdlUrl[userName];
                    fromOrgToolingLR.sandbox = true;
                }

                Dictionary<String, String> passwordSecurityToken = new Dictionary<String, String>();
                passwordSecurityToken = getUsernameCredentials(userName);

                if (passwordSecurityToken.ContainsKey("password") && passwordSecurityToken.ContainsKey("securitytoken"))
                {
                    fromOrgToolingLR = fromOrgToolingSvc.login(userName, passwordSecurityToken["password"] + passwordSecurityToken["securitytoken"]);
                    if (fromOrgToolingLR.sessionId != null && fromOrgToolingLR.passwordExpired == false)
                    {
                        loginSuccess = true;
                    }
                }
                else
                {
                    fromOrgToolingLR = fromOrgToolingSvc.login(userName, passwordSecurityToken["password"]);
                    if (fromOrgToolingLR.sessionId != null && fromOrgToolingLR.passwordExpired == false)
                    {
                        loginSuccess = true;
                    }
                }

                // Get the login result and update the Salesforce Service object
                fromOrgToolingSvc.Url = fromOrgToolingLR.serverUrl;
                fromOrgToolingSvc.SessionHeaderValue = new ToolingWSDL.SessionHeader();
                fromOrgToolingSvc.SessionHeaderValue.sessionId = fromOrgToolingLR.sessionId;
            }
            catch (Exception loginFromExc2)
            {
            }

            return loginSuccess;
        }

        public static void salesforceLogout()
        {
            try
            {
                if(fromOrgSS != null) fromOrgSS.logout();
                
            }
            catch (Exception e) { }

            try
            {
                if (toOrgSS != null) toOrgSS.logout();
            }
            catch (Exception e) { }
        }

        public static SalesforceMetadata.PartnerWSDL.DescribeGlobalResult getDescribeGlobalResult(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.PartnerWSDL.DescribeGlobalResult dgr = new SalesforceMetadata.PartnerWSDL.DescribeGlobalResult();
            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                dgr = SalesforceCredentials.fromOrgSS.describeGlobal();
            }
            else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
            {
                dgr = SalesforceCredentials.toOrgSS.describeGlobal();
            }

            return dgr;
        }

        public static SalesforceMetadata.PartnerWSDL.DescribeTab[] getDescribeTab(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.PartnerWSDL.DescribeTab[] descrTabs;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                descrTabs = fromOrgSS.describeAllTabs();
            }
            else
            {
                descrTabs = new SalesforceMetadata.PartnerWSDL.DescribeTab[1];
            }

            return descrTabs;
        }
        
        public static SalesforceMetadata.MetadataWSDL.DescribeMetadataResult getDescribeMetadataResult(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.MetadataWSDL.DescribeMetadataResult dmd = new SalesforceMetadata.MetadataWSDL.DescribeMetadataResult();
            try
            {
                if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
                {
                    dmd = SalesforceCredentials.fromOrgMS.describeMetadata(Convert.ToDouble(Properties.Settings.Default.DefaultAPI));
                }
                else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
                {
                    dmd = SalesforceCredentials.toOrgMS.describeMetadata(Convert.ToDouble(Properties.Settings.Default.DefaultAPI));
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

        public static SalesforceMetadata.MetadataWSDL.MetadataService getMetadataService(UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.MetadataWSDL.MetadataService ms = new SalesforceMetadata.MetadataWSDL.MetadataService();
            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                ms = fromOrgMS;
            }
            else if (reqOrg == UtilityClass.REQUESTINGORG.TOORG)
            {
                ms = toOrgMS;
            }

            return ms;
        }

        public static void populateUsernameMaps()
        {
            usernamePartnerUrl = new Dictionary<String, String>();
            usernameMetadataUrl = new Dictionary<String, String>();
            usernameToolingWsdlUrl = new Dictionary<String, String>();
            isProduction = new Dictionary<String, Boolean>();
            defaultWsdlObjects = new Dictionary<String, List<String>>();

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

                SalesforceCredentials.usernamePartnerUrl.Add(username, partnerWsdlUrl);
                SalesforceCredentials.usernameMetadataUrl.Add(username, metadataWdldUrl);
                SalesforceCredentials.isProduction.Add(username, isProd);

                if (defaultWsdlObjectList.Count > 0)
                {
                    SalesforceCredentials.defaultWsdlObjects.Add(username, defaultWsdlObjectList);
                }

                if (toolingWsdlUrl != "")
                {
                    SalesforceCredentials.usernameToolingWsdlUrl.Add(username, toolingWsdlUrl);
                }
            }
        }

        private static Dictionary<String, String> getUsernameCredentials(String userName)
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

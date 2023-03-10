using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;
using System.Runtime.CompilerServices;

namespace SalesforceMetadata
{
    class SalesforceCredentials
    {
        public static Dictionary<String, String> usernamePartnerUrl;
        public static Dictionary<String, String> usernameMetadataUrl;
        public static Dictionary<String, String> usernameToolingWsdlUrl;
        public static Dictionary<String, List<String>> defaultWsdlObjects;
        public static Dictionary<String, Boolean> isProduction;

        public static String fromOrgUsername;
        public static String fromOrgPassword;
        public static String fromOrgSecurityToken;
        public static SforceService fromOrgSS;
        public static SalesforceMetadata.PartnerWSDL.LoginResult fromOrgLR;
        public static MetadataService fromOrgMS;

        public static SalesforceMetadata.ToolingWSDL.SforceServiceService fromOrgToolingSvc;
        public static SalesforceMetadata.ToolingWSDL.LoginResult fromOrgToolingLR;

        public static String toOrgUsername;
        public static String toOrgPassword;
        public static String toOrgSecurityToken;
        public static SforceService toOrgSS;
        public static SalesforceMetadata.PartnerWSDL.LoginResult toOrgLR;
        public static MetadataService toOrgMS;

        public static Boolean salesforceLogin(UtilityClass.REQUESTINGORG reqOrg)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Boolean loginSuccess = false;

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
            if(reqOrg == UtilityClass.REQUESTINGORG.FROMORG
               && fromOrgUsername != null
               && fromOrgUsername != "")
            {
                fromOrgSS = new SforceService();
                fromOrgLR = new SalesforceMetadata.PartnerWSDL.LoginResult();
                fromOrgMS = new MetadataService();

                try
                {
                    if (isProduction[fromOrgUsername] == false)
                    {
                        fromOrgSS.Url = usernamePartnerUrl[fromOrgUsername];
                        fromOrgLR.sandbox = true;
                    }

                    if (fromOrgSecurityToken != null && fromOrgSecurityToken != "")
                    {
                        fromOrgLR = fromOrgSS.login(fromOrgUsername, fromOrgPassword + fromOrgSecurityToken);
                        if (fromOrgLR.sessionId != null && fromOrgLR.passwordExpired == false)
                        {
                            loginSuccess = true;
                        }
                    }
                    else
                    {
                        fromOrgLR = fromOrgSS.login(fromOrgUsername, fromOrgPassword);
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
                    //fromOrgMS.Url = usernameMetadataUrl[fromOrgUsername];
                    fromOrgMS.SessionHeaderValue = new MetadataWSDL.SessionHeader();
                    fromOrgMS.SessionHeaderValue.sessionId = fromOrgLR.sessionId;
                }
                catch (Exception loginFromExc1)
                {

                }
            }


            ///* TOORG Login Credentials */
            if (reqOrg == UtilityClass.REQUESTINGORG.TOORG
               && toOrgUsername != null
               && toOrgUsername != "")
            {
                toOrgSS = new SforceService();
                toOrgLR = new SalesforceMetadata.PartnerWSDL.LoginResult();
                toOrgMS = new MetadataService();

                try
                {
                    toOrgLR.metadataServerUrl = usernamePartnerUrl[toOrgUsername];

                    if (isProduction[toOrgUsername] == false)
                    {
                        toOrgSS.Url = usernamePartnerUrl[toOrgUsername];
                        toOrgLR.sandbox = true;
                    }

                    if (toOrgSecurityToken != null && toOrgSecurityToken != "")
                    {
                        toOrgLR = toOrgSS.login(toOrgUsername, toOrgPassword + toOrgSecurityToken);
                        if (toOrgLR.sessionId != null && toOrgLR.passwordExpired == false)
                        {
                            loginSuccess = true;
                        }
                    }
                    else
                    {
                        toOrgLR = toOrgSS.login(toOrgUsername, toOrgPassword);
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
                catch (Exception loginFromExc2)
                {
                }
            }
            
            return loginSuccess;
        }

        public static Boolean salesforceToolingLogin()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Boolean loginSuccess = false;

            fromOrgToolingSvc = new SalesforceMetadata.ToolingWSDL.SforceServiceService();
            fromOrgToolingLR = new SalesforceMetadata.ToolingWSDL.LoginResult();

            try
            {
                if (isProduction[fromOrgUsername] == false)
                {
                    fromOrgToolingSvc.Url = usernameToolingWsdlUrl[fromOrgUsername];
                    fromOrgToolingLR.sandbox = true;
                }

                if (fromOrgSecurityToken != null 
                    && fromOrgSecurityToken != "")
                {
                    fromOrgToolingLR = fromOrgToolingSvc.login(fromOrgUsername, fromOrgPassword + fromOrgSecurityToken);
                    if (fromOrgToolingLR.sessionId != null && fromOrgToolingLR.passwordExpired == false)
                    {
                        loginSuccess = true;
                    }
                }
                else
                {
                    fromOrgToolingLR = fromOrgToolingSvc.login(fromOrgUsername, fromOrgPassword);
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
        public static DescribeMetadataResult getDescribeMetadataResult(UtilityClass.REQUESTINGORG reqOrg)
        {
            DescribeMetadataResult dmd = new DescribeMetadataResult();
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

                DescribeMetadataObject dmo = new DescribeMetadataObject();
                dmo.xmlName = exc.Message;
                dmo.directoryName = exc.Message;

                DescribeMetadataObject[] dmoArray = new DescribeMetadataObject[1];
                dmoArray[0] = dmo;

                dmd.metadataObjects = dmoArray;
            }

            return dmd;
        }

        public static MetadataService getMetadataService(UtilityClass.REQUESTINGORG reqOrg)
        {
            MetadataService ms = new MetadataService();
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


    }

}

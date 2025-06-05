using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesforceMetadata.PartnerWSDL;
using SalesforceMetadata.MetadataWSDL;
using SalesforceMetadata.ToolingWSDL;
using System.Security.Policy;
using static SalesforceMetadata.AutomationReporter;


namespace SalesforceMetadata
{
    class ToolingApiHelper
    {
        // QueryResult of RecordTypes will remain here in a static variable for use later
        public static Dictionary<String, PartnerWSDL.sObject> recordTypes;

        public static String buildSoqlSelect(SalesforceMetadata.PartnerWSDL.DescribeSObjectResult dsr, List<String> lstFields)
        {
            StringBuilder selectStatement = new StringBuilder();
            selectStatement.Append("SELECT ");

            for (Int32 i = 0; i < lstFields.Count; i++)
            {
                if (i == 0)
                {
                    selectStatement.Append(lstFields[i]);
                }
                else
                {
                    selectStatement.Append("," + lstFields[i]);
                }
            }

            selectStatement.Append(" FROM " + dsr.name);

            return selectStatement.ToString();
        }


        public static String sObjectQuery(SalesforceMetadata.PartnerWSDL.Field[] dsrFields, String sObject)
        {
            String queryString = "SELECT ";

            foreach (SalesforceMetadata.PartnerWSDL.Field fld in dsrFields)
            {
                queryString = queryString + fld.name + ',';
            }

            queryString = queryString.Substring(0, queryString.Length - 1);

            queryString = queryString + " FROM " + sObject;

            return queryString;
        }


        public static void getAllRecordTypes(SalesforceCredentials sc)
        {
            recordTypes = new Dictionary<String, PartnerWSDL.sObject>();

            PartnerWSDL.QueryResult recordTypeQueryResults = sc.fromOrgSS.query("SELECT Id, Name, SobjectType, DeveloperName FROM RecordType");

            foreach (PartnerWSDL.sObject s in recordTypeQueryResults.records)
            {
                System.Xml.XmlElement[] rtFields = new System.Xml.XmlElement[3];
                rtFields = s.Any;

                String rtKey = rtFields[2].InnerText + " -> " + rtFields[1].InnerText;

                PartnerWSDL.sObject rtObj = new PartnerWSDL.sObject();

                rtObj.Id = s.Id;
                recordTypes.Add(rtKey, rtObj);
            }
        }

        public static String AnimationRuleQuery(String fullName)
        {
            String queryString = "";
            if (fullName == "")
            {

                queryString = "SELECT Id, AnimationFrequency, DeveloperName, IsActive, Language, MasterLabel, RecordTypeContext, RecordTypeId, SobjectType, TargetField, TargetFieldChangeToValues, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate,LastModifiedDate " +
                        "FROM AnimationRule";
            }
            else
            {
                queryString = "SELECT Id, AnimationFrequency, DeveloperName, IsActive, Language, MasterLabel, RecordTypeContext, RecordTypeId, SobjectType, TargetField, TargetFieldChangeToValues, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate,LastModifiedDate " +
                        "FROM AnimationRule " +
                        "WHERE FullName = '" + fullName + "'";
            }

            return queryString;
        }


        public static String ApexClassQuery(String fullName)
        {
            String queryString = "";
            if (fullName == "")
            {
                queryString = "SELECT Id, ApiVersion, BodyCrc, IsValid, LengthWithoutComments, ManageableState, Name, NamespacePrefix, Status, SymbolTable, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                        "FROM ApexClass";
            }
            else
            {
                queryString = "SELECT Id, ApiVersion, BodyCrc, IsValid, LengthWithoutComments, ManageableState, Name, NamespacePrefix, Status, SymbolTable, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                        "FROM ApexClass " +
                        "WHERE FullName = '" + fullName + "'";

            }

            return queryString;
        }


        public static String ApexClassMemberQuery(String fullName, String contentEntityId)
        {
            String queryString = "";

            if (fullName == "" && contentEntityId == "")
            {
                queryString = "ERROR: Either the FullName or ContentEntityId is required";
            }
            else if (fullName != "" && contentEntityId == "")
            {
                queryString = "SELECT Id, Content, ContentEntityId, FullName, LastSyncDate, SymbolTable, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                        "FROM ApexClassMember " +
                        "WHERE FullName = '" + fullName + "'";
            }
            else if (fullName == "" && contentEntityId != "")
            {
                queryString = "SELECT Id, Content, ContentEntityId, FullName, LastSyncDate, SymbolTable, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                        "FROM ApexClassMember " +
                        "WHERE ContentEntityId = '" + contentEntityId + "'";
            }

            return queryString;
        }

        public static String ApexCodeCoverageQuery(String apexClassOrTriggerId)
        {
            String queryString = "SELECT ApexTestClassId,TestMethodName,ApexClassorTriggerId,NumLinesCovered,NumLinesUncovered,Coverage " +
                           "FROM ApexCodeCoverage " +
                           "WHERE ApexClassorTriggerId = '" + apexClassOrTriggerId + "'";

            return queryString;
        }

        public static String ApexCodeCoverageAggregateQuery(String apexClassOrTriggerId)
        {
            String queryString = "SELECT ApexClassorTriggerId,NumLinesCovered,NumLinesUncovered,Coverage " +
                           "FROM ApexCodeCoverageAggregate " +
                           "WHERE ApexClassorTriggerId = '" + apexClassOrTriggerId + "'";

            return queryString;
        }


        public static String ApexComponentQuery()
        {
            String queryString = "SELECT Id, Name, MasterLabel, NamespacePrefix, ApiVersion, ControllerKey, ControllerType, ManageableState, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexComponent";

            return queryString;
        }

        public static String ApexComponentMemberQuery(String fullName, String contentEntityId)
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexComponentMember";

            return queryString;
        }

        public static String ApexEmailNotificationQuery()
        {
            String queryString = "SELECT Id, Email, UserId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexEmailNotification";

            return queryString;
        }

        public static String ApexExecutionOverlayActionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexExecutionOverlayAction";

            return queryString;
        }

        public static String ApexExecutionOverlayResultQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexExecutionOverlayResult";

            return queryString;
        }

        public static String ApexLogQuery()
        {
            String queryString = "SELECT Id, Application, DurationMilliseconds, Location, LogLength, LogUserId, Operation, " +
                           "Request, RequestIdentifier, StartTime, Status, " +
                           "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                           "FROM ApexLog ";

            return queryString;
        }

        public static String ApexOrgWideCoverageQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexOrgWideCoverage";

            return queryString;
        }

        public static String ApexPageQuery()
        {
            String queryString = "SELECT Id, Name, MasterLabel, NamespacePrefix, ApiVersion, ControllerKey, ControllerType, ManageableState, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexPage";

            return queryString;
        }

        public static String ApexPageInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexPageInfo";

            return queryString;
        }

        public static String ApexPageMemberQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexPageMember";

            return queryString;
        }

        public static String ApexResultQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexResult";

            return queryString;
        }

        public static String ApexTestQueueItemQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexTestQueueItem";

            return queryString;
        }

        public static String ApexTestResultQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexTestResult";

            return queryString;
        }

        public static String ApexTestResultLimitsQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexTestResultLimits";

            return queryString;
        }

        public static String ApexTestRunResultQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexTestRunResult";

            return queryString;
        }

        public static String ApexTestSuiteQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ApexTestSuite";

            return queryString;
        }

        public static String ApexTriggerQuery(String fullName)
        {
            String queryString = "";
            if (fullName == "")
            {
                queryString = "SELECT Id, Name, NamespacePrefix, ApiVersion, BodyCrc, EntityDefinitionId, IsValid, LengthWithoutComments, ManageableState, Status, TableEnumOrId, " +
                               "UsageAfterDelete, UsageAfterInsert, UsageAfterUndelete, UsageAfterUpdate, UsageBeforeDelete, UsageBeforeInsert, UsageBeforeUpdate, UsageIsBulk, " +
                               "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                         "FROM ApexTrigger";
            }
            else
            {
                queryString = "SELECT Id, Name, NamespacePrefix, ApiVersion, BodyCrc, EntityDefinitionId, IsValid, LengthWithoutComments, ManageableState, Status, TableEnumOrId, " +
                               "UsageAfterDelete, UsageAfterInsert, UsageAfterUndelete, UsageAfterUpdate, UsageBeforeDelete, UsageBeforeInsert, UsageBeforeUpdate, UsageIsBulk, " +
                               "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                         "FROM ApexTrigger " +
                         "WHERE Name = '" + fullName + "'";
            }

            return queryString;
        }

        public static String ApexTriggerMemberQuery(String fullName)
        {
            String queryString = "";
            if (fullName == "")
            {
                queryString = "SELECT Id, ContentEntityId, LastSyncDate, MetadataContainerId, SymbolTable, " +
                               "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                         "FROM ApexTriggerMember";
            }
            else
            {
                queryString = "SELECT Id, FullName, ContentEntityId, LastSyncDate, MetadataContainerId, SymbolTable, " +
                               "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                          "FROM ApexTriggerMember " +
                         "WHERE FullName = '" + fullName + "'";
            }

            return queryString;
        }

        public static String AssignmentRuleQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM AssignmentRule";

            return queryString;
        }

        public static String AuraDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM AuraDefinition";

            return queryString;
        }

        public static String AuraDefinitionBundleQuery()
        {
            String queryString = "SELECT Id, DeveloperName, NamespacePrefix, ApiVersion, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM AuraDefinitionBundle";

            return queryString;
        }

        public static String AutoResponseRuleQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM AutoResponseRule";

            return queryString;
        }

        public static String BrandingSetQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BrandingSet";

            return queryString;
        }

        public static String BrandingSetPropertyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BrandingSetProperty";

            return queryString;
        }

        public static String BriefcaseDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BriefcaseDefinition";

            return queryString;
        }

        public static String BusinessProcessQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BusinessProcess";

            return queryString;
        }

        public static String BusinessProcessDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BusinessProcessDefinition";

            return queryString;
        }

        public static String BusinessProcessFeedbackQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BusinessProcessFeedback";

            return queryString;
        }

        public static String BusinessProcessGroupQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BusinessProcessGroup";

            return queryString;
        }

        public static String BusProcessFeedbackConfigQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM BusProcessFeedbackConfig";

            return queryString;
        }

        public static String CertificateQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Certificate";

            return queryString;
        }

        public static String CleanDataServiceQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CleanDataService";

            return queryString;
        }

        public static String CleanRuleQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CleanRule";

            return queryString;
        }

        public static String ColorDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ColorDefinition";

            return queryString;
        }

        public static String CommunityWorkspacesNodeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CommunityWorkspacesNode";

            return queryString;
        }

        public static String CompactLayoutQuery()
        {
            String queryString = "SELECT Id, DeveloperName, ManageableState, MasterLabel, NamespacePrefix, SobjectType, " +
            "LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CompactLayout";

            return queryString;
        }

        public static String CompactLayoutInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CompactLayoutInfo";

            return queryString;
        }

        public static String CompactLayoutItemInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CompactLayoutItemInfo";

            return queryString;
        }

        public static String ContainerAsyncRequestQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ContainerAsyncRequest";

            return queryString;
        }

        public static String CspTrustedSiteQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CspTrustedSite";

            return queryString;
        }

        public static String CustomApplicationQuery()
        {
            String queryString = "SELECT Id, Label, DeveloperName, NamespacePrefix, NavType, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomApplication";

            return queryString;
        }

        public static String CustomFieldQuery()
        {
            String queryString = "SELECT Id, TableEnumOrId, DeveloperName, RelationshipLabel, ManageableState, NamespacePrefix, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomField";

            return queryString;
        }

        public static String CustomFieldMemberQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomFieldMember";

            return queryString;
        }

        public static String CustomHelpMenuSectionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomHelpMenuSection";

            return queryString;
        }

        public static String CustomHttpHeaderQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomHttpHeader";

            return queryString;
        }

        public static String CustomNotificationTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomNotificationType";

            return queryString;
        }

        public static String CustomObjectQuery()
        {
            String queryString = "SELECT Id, DeveloperName, NamespacePrefix, ExternalName, ExternalRepository, ManageableState, SharingModel, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomObject";

            return queryString;
        }

        public static String CustomTabQuery()
        {
            String queryString = "SELECT Id, DeveloperName, NamespacePrefix, MasterLabel, MotifName, Type, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM CustomTab";

            return queryString;
        }

        public static String DataAssessmentConfigItemQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DataAssessmentConfigItem";

            return queryString;
        }

        public static String DataIntegrationRecordPurchasePermissionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DataIntegrationRecordPurchasePermission";

            return queryString;
        }

        public static String DataTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DataType";

            return queryString;
        }

        public static String DebugLevelQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DebugLevel";

            return queryString;
        }

        public static String DeployDetailsQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DeployDetails";

            return queryString;
        }

        public static String DocumentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Document";

            return queryString;
        }

        public static String DuplicateJobDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DuplicateJobDefinition";

            return queryString;
        }

        public static String DuplicateJobMatchingRuleDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM DuplicateJobMatchingRuleDefinition";

            return queryString;
        }

        public static String EmailTemplateQuery()
        {
            String queryString = "SELECT Id, Name, Subject, ApiVersion, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmailTemplate";

            return queryString;
        }

        public static String EmbeddedServiceBrandingQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceBranding";

            return queryString;
        }

        public static String EmbeddedServiceConfigQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceConfig";

            return queryString;
        }

        public static String EmbeddedServiceCustomComponentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceCustomComponent";

            return queryString;
        }

        public static String EmbeddedServiceCustomLabelQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceCustomLabel";

            return queryString;
        }

        public static String EmbeddedServiceFieldServiceQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceFieldService";

            return queryString;
        }

        public static String EmbeddedServiceFlowQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceFlow";

            return queryString;
        }

        public static String EmbeddedServiceFlowConfigQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceFlowConfig";

            return queryString;
        }

        public static String EmbeddedServiceLiveAgentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceLiveAgent";

            return queryString;
        }

        public static String EmbeddedServiceMenuItemQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceMenuItem";

            return queryString;
        }

        public static String EmbeddedServiceMenuSettingsQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceMenuSettings";

            return queryString;
        }

        public static String EmbeddedServiceQuickActionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EmbeddedServiceQuickAction";

            return queryString;
        }

        public static String EnrichedFieldQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EnrichedField";

            return queryString;
        }

        public static String EntityLimitQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EntityLimit";

            return queryString;
        }

        public static String EntityParticleQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EntityParticle";

            return queryString;
        }

        public static String EventDeliveryQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EventDelivery";

            return queryString;
        }

        public static String EventSubscriptionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM EventSubscription";

            return queryString;
        }

        public static String ExternalDataSourceQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ExternalDataSource";

            return queryString;
        }

        public static String ExternalServiceRegistrationQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ExternalServiceRegistration";

            return queryString;
        }

        public static String ExternalStringQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ExternalString";

            return queryString;
        }

        public static String ExternalStringLocalizationQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ExternalStringLocalization";

            return queryString;
        }

        public static String FieldDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FieldDefinition";

            return queryString;
        }

        public static String FieldMappingQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FieldMapping";

            return queryString;
        }

        public static String FieldMappingFieldQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FieldMappingField";

            return queryString;
        }

        public static String FieldMappingRowQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FieldMappingRow";

            return queryString;
        }

        public static String FieldSetQuery()
        {
            String queryString = "SELECT Id, MasterLabel, DeveloperName, NamespacePrefix, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FieldSet";

            return queryString;
        }

        public static String FlexiPageQuery(String fullName)
        {
            String queryString = "";

            if (fullName == "")
            {
                queryString = "SELECT Id, Description, DeveloperName, EntityDefinitionId, ManageableState, MasterLabel, NamespacePrefix, ParentFlexiPage, Type, " +
                        "CreatedDate, LastModifiedDate " +
                        "FROM FlexiPage";
            }
            else
            {
                queryString = "SELECT Id, Description, DeveloperName, EntityDefinitionId, ManageableState, MasterLabel, NamespacePrefix, ParentFlexiPage, Type, " +
                        "CreatedDate, LastModifiedDate " +
                        "FROM FlexiPage " +
                        "WHERE FullName = '" + fullName + "'";
            }

            return queryString;
        }

        public static String FlowQuery()
        {
            String queryString = "SELECT Id, MasterLabel, DefinitionId, IsTemplate, ManageableState, ProcessType, RunInMode, Status, VersionNumber, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Flow " + 
            "WHERE Status = 'Active'";

            return queryString;
        }

        public static String FlowDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FlowDefinition";

            return queryString;
        }

        public static String FlowElementTestCoverageQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FlowElementTestCoverage";

            return queryString;
        }

        public static String FlowTestCoverageQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FlowTestCoverage";

            return queryString;
        }

        public static String ForecastingDisplayedFamilyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ForecastingDisplayedFamily";

            return queryString;
        }

        public static String FormulaFunctionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FormulaFunction";

            return queryString;
        }

        public static String FormulaFunctionAllowedTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FormulaFunctionAllowedType";

            return queryString;
        }

        public static String FormulaOperatorQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM FormulaOperator";

            return queryString;
        }

        public static String GlobalValueSetQuery()
        {
            String queryString = "SELECT Id, MasterLabel, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM GlobalValueSet";

            return queryString;
        }

        public static String GroupQuery()
        {
            String queryString = "SELECT Id, Name, DeveloperName, Type, DoesIncludeBosses, OwnerId, RelatedId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Group";

            return queryString;
        }

        public static String HeapDumpQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM HeapDump";

            return queryString;
        }

        public static String HistoryRetentionJobQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM HistoryRetentionJob";

            return queryString;
        }

        public static String HomePageComponentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM HomePageComponent";

            return queryString;
        }

        public static String HomePageLayoutQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM HomePageLayout";

            return queryString;
        }

        public static String IconDefinitionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM IconDefinition";

            return queryString;
        }

        public static String InboundNetworkConnectionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM InboundNetworkConnection";

            return queryString;
        }

        public static String InboundNetworkConnPropertyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM InboundNetworkConnProperty";

            return queryString;
        }

        public static String IndexQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Index";

            return queryString;
        }

        public static String IndexFieldQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM IndexField";

            return queryString;
        }

        public static String InstalledSubscriberPackageQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM InstalledSubscriberPackage";

            return queryString;
        }

        public static String InstalledSubscriberPackageVersionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM InstalledSubscriberPackageVersion";

            return queryString;
        }

        public static String KeywordListQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM KeywordList";

            return queryString;
        }

        public static String LayoutQuery(String fullName)
        {
            String queryString = "";

            if (fullName == "")
            {

                queryString = "SELECT Id, EntityDefinitionId, LayoutType, ManageableState, Name, NamespacePrefix, ShowSubmitAndAttachButton, TableEnumOrId, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                        "FROM Layout";
            }
            else
            {
                queryString = "SELECT Id, EntityDefinitionId, FullName, LayoutType, ManageableState, Name, NamespacePrefix, ShowSubmitAndAttachButton, TableEnumOrId, " +
                        "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                        "FROM Layout " +
                        "WHERE FullName = '" + fullName + "'";
            }

            return queryString;
        }

        public static String LightningComponentBundleQuery()
        {
            String queryString = "SELECT Id, DeveloperName, ApiVersion, IsExposed, TargetConfigs, NamespacePrefix, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM LightningComponentBundle";

            return queryString;
        }

        public static String LightningComponentResourceQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM LightningComponentResource";

            return queryString;
        }

        public static String LookupFilterQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM LookupFilter";

            return queryString;
        }

        public static String ManagedContentNodeTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ManagedContentNodeType";

            return queryString;
        }

        public static String ManagedContentTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ManagedContentType";

            return queryString;
        }

        public static String MatchingRuleQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM MatchingRule";

            return queryString;
        }

        public static String MenuItemQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM MenuItem";

            return queryString;
        }

        public static String MetadataContainerQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM MetadataContainer";

            return queryString;
        }

        public static String MetadataPackageQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM MetadataPackage";

            return queryString;
        }

        public static String MetadataPackageVersionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM MetadataPackageVersion";

            return queryString;
        }

        public static String ModerationRuleQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ModerationRule";

            return queryString;
        }

        public static String MyDomainLogQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM MyDomainLog";

            return queryString;
        }

        public static String NamedCredentialQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM NamedCredential";

            return queryString;
        }

        public static String OperationLogQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM OperationLog";

            return queryString;
        }

        public static String OpportunitySplitTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM OpportunitySplitType";

            return queryString;
        }

        public static String OutboundNetworkConnectionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM OutboundNetworkConnection";

            return queryString;
        }

        public static String OutboundNetworkConnPropertyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM OutboundNetworkConnProperty";

            return queryString;
        }

        public static String OwnerChangeOptionInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM OwnerChangeOptionInfo";

            return queryString;
        }

        public static String PackageInstallRequestQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PackageInstallRequest";

            return queryString;
        }

        public static String PackageUploadRequestQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PackageUploadRequest";

            return queryString;
        }

        public static String PackageVersionUninstallRequestErrorQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PackageVersionUninstallRequestError";

            return queryString;
        }

        public static String PathAssistantQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PathAssistant";

            return queryString;
        }

        public static String Package2Query()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Package2";

            return queryString;
        }

        public static String Package2MemberQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Package2Member";

            return queryString;
        }

        public static String Package2VersionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Package2Version";

            return queryString;
        }

        public static String Package2VersionCreateRequestQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Package2VersionCreateRequest";

            return queryString;
        }

        public static String Package2VersionCreateRequestErrorQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Package2VersionCreateRequestError";

            return queryString;
        }

        public static String PathAssistantStepInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PathAssistantStepInfo";

            return queryString;
        }

        public static String PathAssistantStepItemQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PathAssistantStepItem";

            return queryString;
        }

        public static String PermissionDependencyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PermissionDependency";

            return queryString;
        }

        public static String PermissionSetQuery()
        {
            String queryString = "SELECT Id, Label, Name, NamespacePrefix, PermissionSetGroupId, ProfileId, Type, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PermissionSet";

            return queryString;
        }

        public static String PermissionSetGroupQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PermissionSetGroup";

            return queryString;
        }

        public static String PermissionSetGroupComponentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PermissionSetGroupComponent";

            return queryString;
        }

        public static String PermissionSetTabSettingQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PermissionSetTabSetting";

            return queryString;
        }

        public static String PlatformEventChannelQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PlatformEventChannel";

            return queryString;
        }

        public static String PlatformEventChannelMemberQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PlatformEventChannelMember";

            return queryString;
        }

        public static String PlatformEventSubscriberConfigQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PlatformEventSubscriberConfig";

            return queryString;
        }

        public static String PostTemplateQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM PostTemplate";

            return queryString;
        }

        public static String ProfileQuery()
        {
            String queryString = "SELECT Id, Name, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Profile";

            return queryString;
        }

        public static String ProfileLayoutQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ProfileLayout";

            return queryString;
        }

        public static String PublisherQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Publisher";

            return queryString;
        }

        public static String QueryResultQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM QueryResult";

            return queryString;
        }

        public static String QuickActionDefinitionQuery()
        {
            String queryString = "SELECT Id, Description, DeveloperName, Height, IconId, Label, Language, ManageableState, MasterLabel, NamespacePrefix, OptionsCreateFeedItem, SobjectType, " + 
                            "StandardLabel, SuccessMessage, TargetField, TargetRecordTypeId, TargetSobjectType, Type, Width, " +
                           "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                           "FROM QuickActionDefinition";

            return queryString;
        }

        public static String QuickActionListQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM QuickActionList";

            return queryString;
        }

        public static String QuickActionListItemQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM QuickActionListItem";

            return queryString;
        }

        public static String RecentlyViewedQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM RecentlyViewed";

            return queryString;
        }

        public static String RecommendationStrategyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM RecommendationStrategy";

            return queryString;
        }

        public static String RecordActionDeploymentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM RecordActionDeployment";

            return queryString;
        }

        public static String RecordTypeQuery()
        {
            String queryString = "SELECT Id, BusinessProcessId, Description, EntityDefinitionId, IsActive, ManageableState, Name, NamespacePrefix, SobjectType, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM RecordType";

            return queryString;
        }

        public static String RelationshipDomainQuery(String parentObject, String fieldName)
        {
            String queryString = "SELECT Id, ChildSobjectId, ChildSobject.DeveloperName, DurableId, FieldId, Field.DeveloperName, " + 
                           "IsCascadeDelete, IsDeprecatedAndHidden, IsRestrictedDelete, JunctionIdListNames, ParentSobjectId, " + 
                           "ParentSobject.DeveloperName, RelationshipInfoId, RelationshipName " +
                           "FROM RelationshipDomain " +
                           "WHERE ParentSobject.DeveloperName = '" + parentObject + "'";

            return queryString;
        }

        public static String RelationshipInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM RelationshipInfo";

            return queryString;
        }

        public static String ReleaseUpdateQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ReleaseUpdate";

            return queryString;
        }

        public static String RemoteProxyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM RemoteProxy";

            return queryString;
        }

        public static String SandboxInfoQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SandboxInfo";

            return queryString;
        }

        public static String SandboxProcessQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SandboxProcess";

            return queryString;
        }

        public static String SearchLayoutQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SearchLayout";

            return queryString;
        }

        public static String SecurityHealthCheckQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SecurityHealthCheck";

            return queryString;
        }

        public static String SecurityHealthCheckRisksQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SecurityHealthCheckRisks";

            return queryString;
        }

        public static String ServiceFieldDataTypeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM ServiceFieldDataType";

            return queryString;
        }

        public static String ScontrolQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM Scontrol";

            return queryString;
        }

        public static String SiteDetailQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SiteDetail";

            return queryString;
        }

        public static String SOQLResultQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SOQLResult";

            return queryString;
        }

        public static String SourceMemberQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SourceMember";

            return queryString;
        }

        public static String StandardActionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM StandardAction";

            return queryString;
        }

        public static String StandardValueSetQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM StandardValueSet";

            return queryString;
        }

        public static String StaticResourceQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM StaticResource";

            return queryString;
        }

        public static String SubscriberPackageQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SubscriberPackage";

            return queryString;
        }

        public static String SubscriberPackageVersionQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SubscriberPackageVersion";

            return queryString;
        }

        public static String SubscriberPackageVersionUninstallRequestQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SubscriberPackageVersionUninstallRequest";

            return queryString;
        }

        public static String SymbolTableQuery()
        {
            String queryString = "SELECT Id, namespace, name, parentClass, constructors, properties, externalReferences, innerClasses, interfaces, methods, tableDeclaration, variables, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM SymbolTable";

            return queryString;
        }

        public static String TabDefinitionQuery()
        {
            String queryString = "SELECT Id, Name, Label, IsCustom, IsAvailableInDesktop, IsAvailableInLightning, IsAvailableInMobile " +
            "FROM TabDefinition";

            return queryString;
        }

        public static String TestSuiteMembershipQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM TestSuiteMembership";

            return queryString;
        }

        public static String TimeSheetTemplateQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM TimeSheetTemplate";

            return queryString;
        }

        public static String TimeSheetTemplateAssignmentQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM TimeSheetTemplateAssignment";

            return queryString;
        }

        public static String TraceFlagQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM TraceFlag";

            return queryString;
        }

        public static String TransactionSecurityPolicyQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM TransactionSecurityPolicy";

            return queryString;
        }

        public static String UserQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM User";

            return queryString;
        }

        public static String UserCriteriaQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM UserCriteria";

            return queryString;
        }

        public static String UserEntityAccessQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM UserEntityAccess";

            return queryString;
        }

        public static String UserFieldAccessQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM UserFieldAccess";

            return queryString;
        }

        public static String ValidationRuleQuery(String fullName, String entityDefinitionId)
        {
            String queryString = "";

            if (fullName == "")
            {
                queryString = "SELECT Id, NamespacePrefix, ValidationName, Active, EntityDefinitionId, ErrorDisplayField, " +
                               "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                          "FROM ValidationRule";
            }
            else
            {
                queryString = "SELECT Id, NamespacePrefix, ValidationName, Active, EntityDefinitionId, ErrorDisplayField, Metadata, " +
                               "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
                          "FROM ValidationRule " +
                          "WHERE ValidationName = '" + fullName + "' AND EntityDefinitionId = '" + entityDefinitionId + "'";
            }

            return queryString;
        }

        public static String WebLinkQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WebLink";

            return queryString;
        }

        public static String WorkflowAlertQuery()
        {
            String queryString = "SELECT Id, DeveloperName, EntityDefinitionId, SenderType, CcEmails, TemplateId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkflowAlert";

            return queryString;
        }

        public static String WorkflowFieldUpdateQuery()
        {
            String queryString = "SELECT Id, Name, SourceTableEnumOrId, EntityDefinitionId, FieldDefinitionId, LiteralValue, LookupValueId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkflowFieldUpdate";

            return queryString;
        }

        public static String WorkflowOutboundMessageQuery()
        {
            String queryString = "SELECT Id, Name, NamespacePrefix, EntityDefinitionId, IntegrationUserId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkflowOutboundMessage";

            return queryString;
        }

        public static String WorkflowRuleQuery()
        {
            String queryString = "SELECT Id, Name, NamespacePrefix, TableEnumOrId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkflowRule";

            return queryString;
        }

        public static String WorkflowTaskQuery()
        {
            String queryString = "SELECT Id, Subject, NamespacePrefix, Priority, Status, EntityDefinitionId, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkflowTask";

            return queryString;
        }

        public static String WorkSkillRoutingQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkSkillRouting";

            return queryString;
        }

        public static String WorkSkillRoutingAttributeQuery()
        {
            String queryString = "SELECT Id, " +
            "CreatedById, CreatedBy.Name, LastModifiedById, LastModifiedBy.Name, CreatedDate, LastModifiedDate " +
            "FROM WorkSkillRoutingAttribute";

            return queryString;
        }

        /*
        public static String apexTriggerQuery(String triggerName)
        {
            String queryString = "SELECT ApiVersion,BodyCrc,EntityDefinitionId,IsValid,LengthWithoutComments,ManageableState,Metadata,Status," +
                                   "UsageAfterDelete,UsageAfterInsert,UsageAfterUndelete,UsageAfterUpdate,UsageBeforeDelete,UsageBeforeInsert," +
                                   "UsageBeforeUpdate,UsageIsBulk " +
                                   "FROM ApexTrigger " +
                                   "WHERE Name = '" + triggerName + "'" +
                                   "AND NamespacePrefix = null";

            return queryString;
        }

        public static String apexClassMemberQuery(String classTriggerId)
        {
            String queryString = "SELECT Content,ContentEntityId,FullName,LastSyncDate,Metadata,MetadataContainerId,SymbolTable " +
                           "FROM ApexClassMember " +
                           "WHERE ContentEntityId = '" + classTriggerId + "'";

            return queryString;
        }
        */


        public static List<String> accountRelationshipShareRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("accessLevel");
            toolingFieldsAndTypes.Add("accountToCriteriaField");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("staticFormulaCriteria");
            toolingFieldsAndTypes.Add("type");

            return toolingFieldsAndTypes;
        }

        public static List<String> actionLinkGroupTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionLinkTemplates");
            toolingFieldsAndTypes.Add("category");
            toolingFieldsAndTypes.Add("executionsAllowed");
            toolingFieldsAndTypes.Add("hoursUntilExpiration");
            toolingFieldsAndTypes.Add("isPublished");
            toolingFieldsAndTypes.Add("name");

            return toolingFieldsAndTypes;
        }

        public static List<String> actionLinkTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionUrl");
            toolingFieldsAndTypes.Add("headers");
            toolingFieldsAndTypes.Add("isConfirmationRequired");
            toolingFieldsAndTypes.Add("isGroupDefault");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("labelKey");
            toolingFieldsAndTypes.Add("linkType");
            toolingFieldsAndTypes.Add("method");
            toolingFieldsAndTypes.Add("position");
            toolingFieldsAndTypes.Add("requestBody");
            toolingFieldsAndTypes.Add("userAlias");
            toolingFieldsAndTypes.Add("userVisibility");

            return toolingFieldsAndTypes;
        }

        public static List<String> actionPlanTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionPlanTemplateItem");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("targetEntityType");
            toolingFieldsAndTypes.Add("uniqueName");

            return toolingFieldsAndTypes;
        }

        public static List<String> actionPlanTemplateItemFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionPlanTemplateItemValue");
            toolingFieldsAndTypes.Add("displayOrder");
            toolingFieldsAndTypes.Add("isRequired");
            toolingFieldsAndTypes.Add("itemEntityType");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("uniqueName");

            return toolingFieldsAndTypes;
        }

        public static List<String> actionPlanTemplateItemValueFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("itemEntityType");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("valueFormula");
            toolingFieldsAndTypes.Add("valueLiteral");

            return toolingFieldsAndTypes;
        }

        public static List<String> analyticSnapshotFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("groupColumn");
            toolingFieldsAndTypes.Add("mappings");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("runningUser");
            toolingFieldsAndTypes.Add("sourceReport");
            toolingFieldsAndTypes.Add("targetObject");

            return toolingFieldsAndTypes;
        }

        public static List<String> analyticSnapshotMappingFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("aggregateType");
            toolingFieldsAndTypes.Add("sourceField");
            toolingFieldsAndTypes.Add("sourceType");
            toolingFieldsAndTypes.Add("targetField");

            return toolingFieldsAndTypes;
        }

        public static List<String> animationRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("AnimationFrequency");
            toolingFieldsAndTypes.Add("DeveloperName");
            toolingFieldsAndTypes.Add("IsActive");
            toolingFieldsAndTypes.Add("Language");
            toolingFieldsAndTypes.Add("MasterLabel");
            toolingFieldsAndTypes.Add("Metadata");
            toolingFieldsAndTypes.Add("RecordTypeContext");
            toolingFieldsAndTypes.Add("RecordTypeId");
            toolingFieldsAndTypes.Add("SobjectType");
            toolingFieldsAndTypes.Add("TargetField");
            toolingFieldsAndTypes.Add("TargetFieldChangeToValues");

            return toolingFieldsAndTypes;
        }

        public static List<String> apexClassFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("apiVersion");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("packageVersions");
            toolingFieldsAndTypes.Add("status");

            return toolingFieldsAndTypes;
        }

        public static List<String> apexComponentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("apiVersion");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("packageVersions");

            return toolingFieldsAndTypes;
        }

        public static List<String> apexPageFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("apiVersion");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("availableInTouch");
            toolingFieldsAndTypes.Add("confirmationTokenRequired");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("packageVersions");

            return toolingFieldsAndTypes;
        }

        public static List<String> apexTriggerFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("apiVersion");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("packageVersions");
            toolingFieldsAndTypes.Add("status");

            return toolingFieldsAndTypes;
        }

        public static List<String> appMenuFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("appMenuItems");
            return toolingFieldsAndTypes;
        }

        public static List<String> appMenuItemFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("type");

            return toolingFieldsAndTypes;
        }

        public static List<String> appointmentSchedulingPolicyFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("appointmentStartTimeInterval");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("shouldConsiderCalendarEvents");
            toolingFieldsAndTypes.Add("shouldEnforceExcludedResource");
            toolingFieldsAndTypes.Add("shouldEnforceRequiredResource");
            toolingFieldsAndTypes.Add("shouldMatchSkill");
            toolingFieldsAndTypes.Add("shouldMatchSkillLevel");
            toolingFieldsAndTypes.Add("shouldRespectVisitingHours");
            toolingFieldsAndTypes.Add("shouldUsePrimaryMembers");
            toolingFieldsAndTypes.Add("shouldUseSecondaryMembers");
            return toolingFieldsAndTypes;
        }

        public static List<String> approvalProcessFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("allowRecall");
            toolingFieldsAndTypes.Add("allowedSubmitters");
            toolingFieldsAndTypes.Add("approvalPageFields");
            toolingFieldsAndTypes.Add("approvalStep");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("emailTemplate");
            toolingFieldsAndTypes.Add("entryCriteria");
            toolingFieldsAndTypes.Add("finalApprovalActions");
            toolingFieldsAndTypes.Add("finalApprovalRecordLock");
            toolingFieldsAndTypes.Add("finalRejectionActions");
            toolingFieldsAndTypes.Add("finalRejectionRecordLock");
            toolingFieldsAndTypes.Add("initialSubmissionActions");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("nextAutomatedApprover");
            toolingFieldsAndTypes.Add("postTemplate");
            toolingFieldsAndTypes.Add("recallActions");
            toolingFieldsAndTypes.Add("recordEditability");
            toolingFieldsAndTypes.Add("showApprovalHistory");

            return toolingFieldsAndTypes;
        }

        public static List<String> articleTypeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("articleTypeChannel​Display");
            toolingFieldsAndTypes.Add("deploymentStatus");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fields");
            toolingFieldsAndTypes.Add("gender");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("pluralLabel");
            toolingFieldsAndTypes.Add("startsWith");

            return toolingFieldsAndTypes;
        }

        public static List<String> assignmentRulesFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("assignmentRule");
            return toolingFieldsAndTypes;
        }

        public static List<String> audienceFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("audienceName");
            toolingFieldsAndTypes.Add("container");
            toolingFieldsAndTypes.Add("criteria");
            toolingFieldsAndTypes.Add("criterion");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("formula");
            toolingFieldsAndTypes.Add("formulaFilterType");
            toolingFieldsAndTypes.Add("isDefaultAudience");
            toolingFieldsAndTypes.Add("targets");
            return toolingFieldsAndTypes;
        }

        public static List<String> auraDefinitionBundleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("");

            return toolingFieldsAndTypes;
        }

        public static List<String> authProviderFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("authorizeUrl");
            toolingFieldsAndTypes.Add("consumerKey");
            toolingFieldsAndTypes.Add("consumerSecret");
            toolingFieldsAndTypes.Add("customMetadataTypeRecord");
            toolingFieldsAndTypes.Add("defaultScopes");
            toolingFieldsAndTypes.Add("errorUrl");
            toolingFieldsAndTypes.Add("executionUser");
            toolingFieldsAndTypes.Add("friendlyName");
            toolingFieldsAndTypes.Add("iconUrl");
            toolingFieldsAndTypes.Add("idTokenIssuer");
            toolingFieldsAndTypes.Add("includeOrgIdInIdentifier");
            toolingFieldsAndTypes.Add("LinkKickoffUrl");
            toolingFieldsAndTypes.Add("logoutUrl");
            toolingFieldsAndTypes.Add("oauthKickoffUrl");
            toolingFieldsAndTypes.Add("plugin");
            toolingFieldsAndTypes.Add("portal");
            toolingFieldsAndTypes.Add("providerType");
            toolingFieldsAndTypes.Add("registrationHandler");
            toolingFieldsAndTypes.Add("sendAccessTokenInHeader");
            toolingFieldsAndTypes.Add("sendClientCredentialsInHeader");
            toolingFieldsAndTypes.Add("sendSecretInApis");
            toolingFieldsAndTypes.Add("SsoKickoffUrl");
            toolingFieldsAndTypes.Add("tokenUrl");
            toolingFieldsAndTypes.Add("userInfoUrl");
            return toolingFieldsAndTypes;
        }

        public static List<String> autoResponseRulesFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("autoresponseRule");

            return toolingFieldsAndTypes;
        }

        public static List<String> blacklistedComsumersFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("blockedByApiWhitelisting");
            toolingFieldsAndTypes.Add("consumerKey");
            toolingFieldsAndTypes.Add("consumerName");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> botFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("botMlDomain");
            toolingFieldsAndTypes.Add("botUser");
            toolingFieldsAndTypes.Add("botVersions");
            toolingFieldsAndTypes.Add("contextVariables");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            return toolingFieldsAndTypes;
        }

        public static List<String> botVersionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("botDialogGroups");
            toolingFieldsAndTypes.Add("botDialogs");
            toolingFieldsAndTypes.Add("conversationVariables");
            toolingFieldsAndTypes.Add("entryDialog");
            toolingFieldsAndTypes.Add("mainMenuDialog");
            toolingFieldsAndTypes.Add("responseDelayMilliseconds");
            return toolingFieldsAndTypes;
        }

        public static List<String> brandingSetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("brandingSetProperty");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> callCenterFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("adapterUrl");
            toolingFieldsAndTypes.Add("displayName");
            toolingFieldsAndTypes.Add("displayNameLabel");
            toolingFieldsAndTypes.Add("internalNameLabel");
            toolingFieldsAndTypes.Add("version");
            toolingFieldsAndTypes.Add("sections");
            return toolingFieldsAndTypes;
        }

        public static List<String> campaignInfluenceModelFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("isDefaultModel");
            toolingFieldsAndTypes.Add("isModelLocked");
            toolingFieldsAndTypes.Add("modelDescription");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("recordPreference");
            return toolingFieldsAndTypes;
        }

        public static List<String> caseSubjectParticleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("index");
            toolingFieldsAndTypes.Add("textField");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> certificateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("caSigned");
            toolingFieldsAndTypes.Add("encryptedWithPlatformEncryption");
            toolingFieldsAndTypes.Add("expirationDate");
            toolingFieldsAndTypes.Add("keySize");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("privateKeyExportable");
            return toolingFieldsAndTypes;
        }

        public static List<String> chatterExtensionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("extensionName");
            toolingFieldsAndTypes.Add("compositionComponent");
            toolingFieldsAndTypes.Add("headerText");
            toolingFieldsAndTypes.Add("hoverText");
            toolingFieldsAndTypes.Add("icon");
            toolingFieldsAndTypes.Add("isProtected");
            toolingFieldsAndTypes.Add("renderComponent");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> cleanDataServiceFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("cleanRules");
            toolingFieldsAndTypes.Add("matchEngine");
            return toolingFieldsAndTypes;
        }

        public static List<String> CMSConnectSourceFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("cmsConnectAsset");
            toolingFieldsAndTypes.Add("cmsConnectLanguage");
            toolingFieldsAndTypes.Add("cmsConnectPersonalization");
            toolingFieldsAndTypes.Add("cmsConnectResourceType");
            toolingFieldsAndTypes.Add("connectionType");
            toolingFieldsAndTypes.Add("cssScope");
            toolingFieldsAndTypes.Add("developerName");
            toolingFieldsAndTypes.Add("languageEnabled");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("namedCredential");
            toolingFieldsAndTypes.Add("personalizationEnabled");
            toolingFieldsAndTypes.Add("rootPath");
            toolingFieldsAndTypes.Add("sortOrder");
            toolingFieldsAndTypes.Add("status");
            toolingFieldsAndTypes.Add("type");
            toolingFieldsAndTypes.Add("websiteUrl");
            return toolingFieldsAndTypes;
        }

        public static List<String> communityFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("chatterAnswersFacebookSsoUrl");
            toolingFieldsAndTypes.Add("communityFeedPage");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("emailFooterDocument");
            toolingFieldsAndTypes.Add("emailHeaderDocument");
            toolingFieldsAndTypes.Add("emailNotificationUrl");
            toolingFieldsAndTypes.Add("enableChatterAnswers");
            toolingFieldsAndTypes.Add("enablePrivateQuestions");
            toolingFieldsAndTypes.Add("expertsGroup");
            toolingFieldsAndTypes.Add("portal");
            toolingFieldsAndTypes.Add("portalEmailNotificationUrl");
            toolingFieldsAndTypes.Add("reputationLevels");
            toolingFieldsAndTypes.Add("showInPortal");
            toolingFieldsAndTypes.Add("site");
            return toolingFieldsAndTypes;
        }

        public static List<String> communityTemplateDefinitionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("chatterAnswersFacebookSsoUrl");
            toolingFieldsAndTypes.Add("communityFeedPage");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("emailFooterDocument");
            toolingFieldsAndTypes.Add("emailHeaderDocument");
            toolingFieldsAndTypes.Add("emailNotificationUrl");
            toolingFieldsAndTypes.Add("enableChatterAnswers");
            toolingFieldsAndTypes.Add("enablePrivateQuestions");
            toolingFieldsAndTypes.Add("expertsGroup");
            toolingFieldsAndTypes.Add("portal");
            toolingFieldsAndTypes.Add("portalEmailNotificationUrl");
            toolingFieldsAndTypes.Add("reputationLevels");
            toolingFieldsAndTypes.Add("showInPortal");
            toolingFieldsAndTypes.Add("site");
            return toolingFieldsAndTypes;
        }

        public static List<String> communityThemeDefinitionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("bundlesInfo");
            toolingFieldsAndTypes.Add("customThemeLayoutType");
            toolingFieldsAndTypes.Add("defaultBrandingSet");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("enableExtendedCleanUp");
            toolingFieldsAndTypes.Add("OnDelete");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("publisher");
            toolingFieldsAndTypes.Add("themeRouteOverride");
            toolingFieldsAndTypes.Add("themeSetting");
            return toolingFieldsAndTypes;
        }

        public static List<String> connectedAppFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("canvasConfig");
            toolingFieldsAndTypes.Add("contactEmail");
            toolingFieldsAndTypes.Add("contactPhone");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("profileName");
            toolingFieldsAndTypes.Add("attributes");
            toolingFieldsAndTypes.Add("startUrl");
            toolingFieldsAndTypes.Add("infoUrl");
            toolingFieldsAndTypes.Add("logoUrl");
            toolingFieldsAndTypes.Add("iconUrl");
            toolingFieldsAndTypes.Add("mobileStartUrl");
            toolingFieldsAndTypes.Add("ipRanges");
            toolingFieldsAndTypes.Add("oauthConfig");
            toolingFieldsAndTypes.Add("permissionSetName");
            toolingFieldsAndTypes.Add("plugin");
            toolingFieldsAndTypes.Add("pluginExecutionUser");
            toolingFieldsAndTypes.Add("samlConfig");
            return toolingFieldsAndTypes;
        }

        public static List<String> contentAssetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("format");
            toolingFieldsAndTypes.Add("isVisibleByExternalUsers");
            toolingFieldsAndTypes.Add("language");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("originNetwork");
            toolingFieldsAndTypes.Add("relationships");
            toolingFieldsAndTypes.Add("versions");
            return toolingFieldsAndTypes;
        }

        public static List<String> corsWhitelistOriginFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("urlPattern");

            return toolingFieldsAndTypes;
        }

        public static List<String> cspTrustedSiteFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("context");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("endpointUrl");
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("isApplicableToConnectSrc");
            toolingFieldsAndTypes.Add("isApplicableToFontSrc");
            toolingFieldsAndTypes.Add("isApplicableToFrameSrc");
            toolingFieldsAndTypes.Add("isApplicableToImgSrc");
            toolingFieldsAndTypes.Add("isApplicableToMediaSrc");
            toolingFieldsAndTypes.Add("isApplicableToStyleSrc");
            toolingFieldsAndTypes.Add("mobileExtension");

            return toolingFieldsAndTypes;
        }

        public static List<String> customApplicationFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionOverrides");
            toolingFieldsAndTypes.Add("brand");
            toolingFieldsAndTypes.Add("consoleConfig");
            toolingFieldsAndTypes.Add("defaultLandingTab");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("formFactors");
            toolingFieldsAndTypes.Add("isNavAutoTempTabsDisabled");
            toolingFieldsAndTypes.Add("isNavPersonalizationDisabled");
            toolingFieldsAndTypes.Add("isServiceCloudConsole");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("navType");
            toolingFieldsAndTypes.Add("preferences");
            toolingFieldsAndTypes.Add("profileActionOverrides");
            toolingFieldsAndTypes.Add("setupExperience");
            toolingFieldsAndTypes.Add("subscriberTabs");
            toolingFieldsAndTypes.Add("tabs");
            toolingFieldsAndTypes.Add("uiType");
            toolingFieldsAndTypes.Add("utilityBar");
            toolingFieldsAndTypes.Add("workspaceConfig");

            return toolingFieldsAndTypes;
        }

        public static List<String> customApplicationComponentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("buttonIconUrl");
            toolingFieldsAndTypes.Add("buttonStyle");
            toolingFieldsAndTypes.Add("buttonText");
            toolingFieldsAndTypes.Add("buttonWidth");
            toolingFieldsAndTypes.Add("height");
            toolingFieldsAndTypes.Add("isHeightFixed");
            toolingFieldsAndTypes.Add("isHidden");
            toolingFieldsAndTypes.Add("isWidthFixed");
            toolingFieldsAndTypes.Add("visualforcePage");
            toolingFieldsAndTypes.Add("width");
            return toolingFieldsAndTypes;
        }

        public static List<String> customFeedFilterFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("criteria");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("isProtected");
            return toolingFieldsAndTypes;
        }

        public static List<String> customHelpMenuSectionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("customHelpMenuItems");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> customLabelsFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("labels");

            return toolingFieldsAndTypes;
        }

        public static List<String> customMetadataFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("protected");
            toolingFieldsAndTypes.Add("values");

            return toolingFieldsAndTypes;
        }

        public static List<String> customNotificationTypeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("customNotifTypeName");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("desktop");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("mobile");
            return toolingFieldsAndTypes;
        }

        public static List<String> customObjectFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionOverrides");
            toolingFieldsAndTypes.Add("allowInChatterGroups");
            toolingFieldsAndTypes.Add("businessProcesses");
            toolingFieldsAndTypes.Add("compactLayoutAssignment");
            toolingFieldsAndTypes.Add("compactLayouts");
            toolingFieldsAndTypes.Add("customHelp");
            toolingFieldsAndTypes.Add("customHelpPage");
            toolingFieldsAndTypes.Add("customSettingsType");
            toolingFieldsAndTypes.Add("customSettingsVisibility");
            toolingFieldsAndTypes.Add("dataStewardGroup");
            toolingFieldsAndTypes.Add("dataStewardUser");
            toolingFieldsAndTypes.Add("deploymentStatus");
            toolingFieldsAndTypes.Add("deprecated");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("enableActivities");
            toolingFieldsAndTypes.Add("enableBulkApi");
            toolingFieldsAndTypes.Add("enableDivisions");
            toolingFieldsAndTypes.Add("enableEnhancedLookup");
            toolingFieldsAndTypes.Add("enableFeeds");
            toolingFieldsAndTypes.Add("enableHistory");
            toolingFieldsAndTypes.Add("enableReports");
            toolingFieldsAndTypes.Add("enableSearch");
            toolingFieldsAndTypes.Add("enableSharing");
            toolingFieldsAndTypes.Add("enableStreamingApi");
            toolingFieldsAndTypes.Add("eventType");
            toolingFieldsAndTypes.Add("externalDataSource");
            toolingFieldsAndTypes.Add("externalName");
            toolingFieldsAndTypes.Add("externalRepository");
            toolingFieldsAndTypes.Add("externalSharingModel");
            toolingFieldsAndTypes.Add("fields");
            toolingFieldsAndTypes.Add("fieldSets");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("gender");
            toolingFieldsAndTypes.Add("household");
            toolingFieldsAndTypes.Add("historyRetentionPolicy");
            toolingFieldsAndTypes.Add("indexes");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("listViews");
            toolingFieldsAndTypes.Add("namedFilter");
            toolingFieldsAndTypes.Add("nameField");
            toolingFieldsAndTypes.Add("pluralLabel");
            toolingFieldsAndTypes.Add("profileSearchLayouts");
            toolingFieldsAndTypes.Add("publishBehavior");
            toolingFieldsAndTypes.Add("recordTypes");
            toolingFieldsAndTypes.Add("recordTypeTrackFeedHistory");
            toolingFieldsAndTypes.Add("recordTypeTrackHistory");
            toolingFieldsAndTypes.Add("searchLayouts");
            toolingFieldsAndTypes.Add("sharingModel");
            toolingFieldsAndTypes.Add("sharingReasons");
            toolingFieldsAndTypes.Add("sharingRecalculations");
            toolingFieldsAndTypes.Add("startsWith");
            toolingFieldsAndTypes.Add("validationRules");
            toolingFieldsAndTypes.Add("visibility");
            toolingFieldsAndTypes.Add("webLinks");

            return toolingFieldsAndTypes;
        }

        public static List<String> customObjectTranslationFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("caseValues");
            toolingFieldsAndTypes.Add("fields");
            toolingFieldsAndTypes.Add("fieldSets");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("gender");
            toolingFieldsAndTypes.Add("layouts");
            toolingFieldsAndTypes.Add("nameFieldLabel");
            toolingFieldsAndTypes.Add("namedFilters");
            toolingFieldsAndTypes.Add("quickActions");
            toolingFieldsAndTypes.Add("recordTypes");
            toolingFieldsAndTypes.Add("sharingReasons");
            toolingFieldsAndTypes.Add("startsWith");
            toolingFieldsAndTypes.Add("validationRules");
            toolingFieldsAndTypes.Add("webLinks");
            toolingFieldsAndTypes.Add("workflowTasks");
            return toolingFieldsAndTypes;
        }

        public static List<String> customPageWebLinkFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("availability");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("displayType");
            toolingFieldsAndTypes.Add("encodingKey");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("hasMenubar");
            toolingFieldsAndTypes.Add("hasScrollbars");
            toolingFieldsAndTypes.Add("hasToolbar");
            toolingFieldsAndTypes.Add("height");
            toolingFieldsAndTypes.Add("isResizable");
            toolingFieldsAndTypes.Add("linkType");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("openType");
            toolingFieldsAndTypes.Add("page");
            toolingFieldsAndTypes.Add("position");
            toolingFieldsAndTypes.Add("protected");
            toolingFieldsAndTypes.Add("requireRowSelection");
            toolingFieldsAndTypes.Add("scontrol");
            toolingFieldsAndTypes.Add("showsLocation");
            toolingFieldsAndTypes.Add("showsStatus");
            toolingFieldsAndTypes.Add("url");
            toolingFieldsAndTypes.Add("width");

            return toolingFieldsAndTypes;
        }

        public static List<String> customPermissionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("connectedApp");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("requiredPermission");
            return toolingFieldsAndTypes;
        }

        public static List<String> customSiteFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("allowHomePage");
            toolingFieldsAndTypes.Add("allowStandardAnswersPages");
            toolingFieldsAndTypes.Add("allowStandardIdeasPages");
            toolingFieldsAndTypes.Add("allowStandardLookups");
            toolingFieldsAndTypes.Add("allowStandardPortalPages");
            toolingFieldsAndTypes.Add("allowStandardSearch");
            toolingFieldsAndTypes.Add("analyticsTrackingCode");
            toolingFieldsAndTypes.Add("authorizationRequiredPage");
            toolingFieldsAndTypes.Add("bandwidthExceededPage");
            toolingFieldsAndTypes.Add("browserXssProtection");
            toolingFieldsAndTypes.Add("changePasswordPage");
            toolingFieldsAndTypes.Add("chatterAnswersForgotPasswordConfirmPage");
            toolingFieldsAndTypes.Add("chatterAnswersForgotPasswordPage");
            toolingFieldsAndTypes.Add("chatterAnswersHelpPage");
            toolingFieldsAndTypes.Add("chatterAnswersLoginPage");
            toolingFieldsAndTypes.Add("chatterAnswersRegistrationPage");
            toolingFieldsAndTypes.Add("clickjackProtectionLevel");
            toolingFieldsAndTypes.Add("contentSniffingProtection");
            toolingFieldsAndTypes.Add("cspUpgradeInsecureRequests");
            toolingFieldsAndTypes.Add("customWebAddresses");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("enableAuraRequests");
            toolingFieldsAndTypes.Add("favoriteIcon");
            toolingFieldsAndTypes.Add("fileNotFoundPage");
            toolingFieldsAndTypes.Add("forgotPasswordPage");
            toolingFieldsAndTypes.Add("genericErrorPage");
            toolingFieldsAndTypes.Add("guestProfile");
            toolingFieldsAndTypes.Add("inMaintenancePage");
            toolingFieldsAndTypes.Add("inactiveIndexPage");
            toolingFieldsAndTypes.Add("indexPage");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("portal");
            toolingFieldsAndTypes.Add("referrerPolicyOriginWhenCrossOrigin");
            toolingFieldsAndTypes.Add("requireHttps");
            toolingFieldsAndTypes.Add("requireInsecurePortalAccess");
            toolingFieldsAndTypes.Add("robotsTxtPage");
            toolingFieldsAndTypes.Add("serverIsDown");
            toolingFieldsAndTypes.Add("siteAdmin");
            toolingFieldsAndTypes.Add("siteRedirectMappings");
            toolingFieldsAndTypes.Add("siteTemplate");
            toolingFieldsAndTypes.Add("siteType");
            toolingFieldsAndTypes.Add("subdomain");
            toolingFieldsAndTypes.Add("urlPathPrefix");

            return toolingFieldsAndTypes;
        }

        public static List<String> customTabFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionOverrides");
            toolingFieldsAndTypes.Add("auraComponent");
            toolingFieldsAndTypes.Add("customObject");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("flexiPage");
            toolingFieldsAndTypes.Add("frameHeight");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("hasSidebar");
            toolingFieldsAndTypes.Add("icon");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("lwcComponent");
            toolingFieldsAndTypes.Add("motif");
            toolingFieldsAndTypes.Add("page");
            toolingFieldsAndTypes.Add("scontrol");
            toolingFieldsAndTypes.Add("splashPageLink");
            toolingFieldsAndTypes.Add("url");
            toolingFieldsAndTypes.Add("urlEncodingKey");

            return toolingFieldsAndTypes;
        }

        public static List<String> customValueFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("color");
            toolingFieldsAndTypes.Add("default");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("label");
            return toolingFieldsAndTypes;
        }

        public static List<String> dashboardFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("backgroundEndColor");
            toolingFieldsAndTypes.Add("backgroundFadeDirection");
            toolingFieldsAndTypes.Add("backgroundStartColor");
            toolingFieldsAndTypes.Add("chartTheme");
            toolingFieldsAndTypes.Add("colorPalette");
            toolingFieldsAndTypes.Add("dashboardChartTheme");
            toolingFieldsAndTypes.Add("dashboardColorPalette");
            toolingFieldsAndTypes.Add("dashboardFilters");
            toolingFieldsAndTypes.Add("dashboardGridLayout");
            toolingFieldsAndTypes.Add("dashboardType");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("folderName");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("isGridLayout");
            toolingFieldsAndTypes.Add("dashboardResultRefreshedDate");
            toolingFieldsAndTypes.Add("dashboardResultRunningUser");
            toolingFieldsAndTypes.Add("leftSection");
            toolingFieldsAndTypes.Add("middleSection");
            toolingFieldsAndTypes.Add("numSubscriptions");
            toolingFieldsAndTypes.Add("rightSection");
            toolingFieldsAndTypes.Add("runningUser");
            toolingFieldsAndTypes.Add("textColor");
            toolingFieldsAndTypes.Add("title");
            toolingFieldsAndTypes.Add("titleColor");
            toolingFieldsAndTypes.Add("titleSize");
            return toolingFieldsAndTypes;
        }

        public static List<String> dataCategoryGroupFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("dataCategory");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("objectUsage");
            return toolingFieldsAndTypes;
        }

        public static List<String> delegateGroupFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("customObjects");
            toolingFieldsAndTypes.Add("groups");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("loginAccess");
            toolingFieldsAndTypes.Add("permissionSets");
            toolingFieldsAndTypes.Add("profiles");
            toolingFieldsAndTypes.Add("roles");
            return toolingFieldsAndTypes;
        }

        public static List<String> documentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("content");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("internalUseOnly");
            toolingFieldsAndTypes.Add("keywords");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("public");
            return toolingFieldsAndTypes;
        }

        public static List<String> duplicateRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("actionOnInsert");
            toolingFieldsAndTypes.Add("actionOnUpdate");
            toolingFieldsAndTypes.Add("alertText");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("duplicateRuleFilter");
            toolingFieldsAndTypes.Add("duplicateRuleMatchRules");
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("operationsOnInsert");
            toolingFieldsAndTypes.Add("operationsOnUpdate");
            toolingFieldsAndTypes.Add("securityOption");
            toolingFieldsAndTypes.Add("sortOrder");
            return toolingFieldsAndTypes;
        }

        public static List<String> eclairGeoDataFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("maps");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> emailServicesFunctionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("apexClass");
            toolingFieldsAndTypes.Add("attachmentOption");
            toolingFieldsAndTypes.Add("authenticationFailureAction");
            toolingFieldsAndTypes.Add("authorizationFailureAction");
            toolingFieldsAndTypes.Add("authorizedSenders");
            toolingFieldsAndTypes.Add("emailServicesAddresses");
            toolingFieldsAndTypes.Add("errorRoutingAddress");
            toolingFieldsAndTypes.Add("functionInactiveAction");
            toolingFieldsAndTypes.Add("functionName");
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("isAuthenticationRequired");
            toolingFieldsAndTypes.Add("isErrorRoutingEnabled");
            toolingFieldsAndTypes.Add("isTextAttachmentsAsBinary");
            toolingFieldsAndTypes.Add("isTlsRequired");
            toolingFieldsAndTypes.Add("overLimitAction");

            return toolingFieldsAndTypes;
        }

        public static List<String> emailTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("apiVersion");
            toolingFieldsAndTypes.Add("attachedDocuments");
            toolingFieldsAndTypes.Add("attachments");
            toolingFieldsAndTypes.Add("available");
            toolingFieldsAndTypes.Add("content");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("encodingKey");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("letterhead");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("packageVersions");
            toolingFieldsAndTypes.Add("relatedEntityType");
            toolingFieldsAndTypes.Add("style");
            toolingFieldsAndTypes.Add("subject");
            toolingFieldsAndTypes.Add("textOnly");
            toolingFieldsAndTypes.Add("type");
            toolingFieldsAndTypes.Add("UiType");

            return toolingFieldsAndTypes;
        }

        public static List<String> embeddedServiceBrandingFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("contrastInvertedColor");
            toolingFieldsAndTypes.Add("contrastPrimaryColor");
            toolingFieldsAndTypes.Add("embeddedServiceConfig");
            toolingFieldsAndTypes.Add("font");
            toolingFieldsAndTypes.Add("height");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("navBarColor");
            toolingFieldsAndTypes.Add("primaryColor");
            toolingFieldsAndTypes.Add("secondaryColor");
            toolingFieldsAndTypes.Add("width");

            return toolingFieldsAndTypes;
        }

        public static List<String> embeddedServiceConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("areGuestUsersAllowed");
            toolingFieldsAndTypes.Add("authMethod");
            toolingFieldsAndTypes.Add("customMinimizedComponent");
            toolingFieldsAndTypes.Add("embeddedServiceCustomComponents");
            toolingFieldsAndTypes.Add("embeddedServiceCustomLabels");
            toolingFieldsAndTypes.Add("embeddedServiceFlowConfig");
            toolingFieldsAndTypes.Add("embeddedServiceFlows");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("shouldHideAuthDialog");
            toolingFieldsAndTypes.Add("site");

            return toolingFieldsAndTypes;
        }

        public static List<String> embeddedServiceFieldServiceFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("appointmentBookingFlowName");
            toolingFieldsAndTypes.Add("cancelApptBookingFlowName");
            toolingFieldsAndTypes.Add("embeddedServiceConfig");
            toolingFieldsAndTypes.Add("enabled");
            toolingFieldsAndTypes.Add("fieldServiceConfirmCardImg");
            toolingFieldsAndTypes.Add("fieldServiceHomeImg");
            toolingFieldsAndTypes.Add("fieldServiceLogoImg");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("modifyApptBookingFlowName");
            toolingFieldsAndTypes.Add("shouldShowExistingAppointment");
            toolingFieldsAndTypes.Add("shouldShowNewAppointment");

            return toolingFieldsAndTypes;
        }

        public static List<String> embeddedServiceFlowConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("enabled");

            return toolingFieldsAndTypes;
        }

        public static List<String> embeddedServiceLiveAgentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("avatarImg");
            toolingFieldsAndTypes.Add("customPrechatComponent");
            toolingFieldsAndTypes.Add("embeddedServiceConfig");
            toolingFieldsAndTypes.Add("embeddedServiceQuickActions");
            toolingFieldsAndTypes.Add("enabled");
            toolingFieldsAndTypes.Add("fontSize");
            toolingFieldsAndTypes.Add("headerBackgroundImg");
            toolingFieldsAndTypes.Add("isOfflineCaseEnabled");
            toolingFieldsAndTypes.Add("isQueuePositionEnabled");
            toolingFieldsAndTypes.Add("liveAgentChatUrl");
            toolingFieldsAndTypes.Add("liveAgentContentUrl");
            toolingFieldsAndTypes.Add("liveChatButton");
            toolingFieldsAndTypes.Add("liveChatDeployment");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("offlineCaseBackgroundImg");
            toolingFieldsAndTypes.Add("prechatBackgroundImg");
            toolingFieldsAndTypes.Add("prechatEnabled");
            toolingFieldsAndTypes.Add("prechatJson");
            toolingFieldsAndTypes.Add("scenario");
            toolingFieldsAndTypes.Add("smallCompanyLogoImg");
            toolingFieldsAndTypes.Add("waitingStateBackgroundImg");

            return toolingFieldsAndTypes;
        }

        public static List<String> embeddedServiceMenuSettingsFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("branding");
            toolingFieldsAndTypes.Add("embeddedServiceCustomLabels");
            toolingFieldsAndTypes.Add("embeddedServiceMenuItems");
            toolingFieldsAndTypes.Add("isEnabled");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("site");

            return toolingFieldsAndTypes;
        }

        public static List<String> entitlementProcessFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("businessHours");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("entryStartDateField");
            toolingFieldsAndTypes.Add("exitCriteriaBooleanFilter");
            toolingFieldsAndTypes.Add("exitCriteriaFilterItems");
            toolingFieldsAndTypes.Add("exitCriteriaFormula");
            toolingFieldsAndTypes.Add("isVersionDefault");
            toolingFieldsAndTypes.Add("milestones");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("SObjectType");
            toolingFieldsAndTypes.Add("versionMaster");
            toolingFieldsAndTypes.Add("versionNotes");
            toolingFieldsAndTypes.Add("versionNumber");
            return toolingFieldsAndTypes;
        }

        public static List<String> entitlementTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("businessHours");
            toolingFieldsAndTypes.Add("casesPerEntitlement");
            toolingFieldsAndTypes.Add("entitlementProcess");
            toolingFieldsAndTypes.Add("isPerIncident");
            toolingFieldsAndTypes.Add("term");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> escalationRulesFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("escalationRule");

            return toolingFieldsAndTypes;
        }

        public static List<String> eventDeliveryFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("eventParameters");
            toolingFieldsAndTypes.Add("eventSubscription");
            toolingFieldsAndTypes.Add("referenceData");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> eventSubscriptionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("eventParameters");
            toolingFieldsAndTypes.Add("eventType");
            toolingFieldsAndTypes.Add("referenceData");
            return toolingFieldsAndTypes;
        }

        public static List<String> experienceBundleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("experienceResources");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> externalDataSourceFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("authProvider");
            toolingFieldsAndTypes.Add("certificate");
            toolingFieldsAndTypes.Add("customConfiguration");
            toolingFieldsAndTypes.Add("customHttpHeader");
            toolingFieldsAndTypes.Add("endpoint");
            toolingFieldsAndTypes.Add("isWritable");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("oauthRefreshToken");
            toolingFieldsAndTypes.Add("oauthScope");
            toolingFieldsAndTypes.Add("oauthToken");
            toolingFieldsAndTypes.Add("password");
            toolingFieldsAndTypes.Add("principalType");
            toolingFieldsAndTypes.Add("protocol");
            toolingFieldsAndTypes.Add("repository");
            toolingFieldsAndTypes.Add("type");
            toolingFieldsAndTypes.Add("username");
            toolingFieldsAndTypes.Add("version");
            return toolingFieldsAndTypes;
        }

        public static List<String> externalServiceRegistrationFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("namedCredential");
            toolingFieldsAndTypes.Add("schema");
            toolingFieldsAndTypes.Add("schemaType");
            toolingFieldsAndTypes.Add("schemaUrl");
            toolingFieldsAndTypes.Add("status");
            return toolingFieldsAndTypes;
        }

        public static List<String> featureParameterFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("dataFlowDirection");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("value");
            return toolingFieldsAndTypes;
        }

        /*
        public static List<String> featureParameterBooleanFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("dataFlowDirection");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("value");
            return toolingFieldsAndTypes;
        }

        public static List<String> featureParameterDateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("dataFlowDirection");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("value");
            return toolingFieldsAndTypes;
        }

        public static List<String> featureParameterIntegerFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("dataFlowDirection");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("value");
            return toolingFieldsAndTypes;
        }
        */

        public static List<String> flexiPageFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("flexiPageRegions");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("pageTemplate");
            toolingFieldsAndTypes.Add("parentFlexiPage");
            toolingFieldsAndTypes.Add("platformActionList");
            toolingFieldsAndTypes.Add("quickActionList");
            toolingFieldsAndTypes.Add("sobjectType");
            toolingFieldsAndTypes.Add("template");
            toolingFieldsAndTypes.Add("type");

            return toolingFieldsAndTypes;
        }

        public static List<String> flowFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("actionCalls");
            toolingFieldsAndTypes.Add("apexPluginCalls");
            toolingFieldsAndTypes.Add("assignments");
            toolingFieldsAndTypes.Add("choices");
            toolingFieldsAndTypes.Add("constants");
            toolingFieldsAndTypes.Add("decisions");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("dynamicChoiceSets");
            toolingFieldsAndTypes.Add("formulas");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("interviewLabel");
            toolingFieldsAndTypes.Add("isAdditionalPermissionRequiredToRun");
            toolingFieldsAndTypes.Add("isTemplate");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("loops");
            toolingFieldsAndTypes.Add("processMetadataValues");
            toolingFieldsAndTypes.Add("processType");
            toolingFieldsAndTypes.Add("recordCreates");
            toolingFieldsAndTypes.Add("recordDeletes");
            toolingFieldsAndTypes.Add("recordLookups");
            toolingFieldsAndTypes.Add("recordUpdates");
            toolingFieldsAndTypes.Add("runInMode");
            toolingFieldsAndTypes.Add("screens");
            toolingFieldsAndTypes.Add("stages");
            toolingFieldsAndTypes.Add("start");
            toolingFieldsAndTypes.Add("startElementReference");
            toolingFieldsAndTypes.Add("status");
            toolingFieldsAndTypes.Add("steps");
            toolingFieldsAndTypes.Add("subflows");
            toolingFieldsAndTypes.Add("textTemplates");
            toolingFieldsAndTypes.Add("variables");
            toolingFieldsAndTypes.Add("waits");

            return toolingFieldsAndTypes;
        }

        public static List<String> flowCategoryFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("flowCategoryItems");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> flowDefinitionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("activeVersionNumber");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("masterLabel");

            return toolingFieldsAndTypes;
        }

        public static List<String> folderFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("accessType");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("publicFolderAccess");
            toolingFieldsAndTypes.Add("sharedTo");
            return toolingFieldsAndTypes;
        }

        public static List<String> globalPicklistFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("globalPicklistValues");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("sorted");

            return toolingFieldsAndTypes;
        }

        public static List<String> globalPicklistValueFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("color");
            toolingFieldsAndTypes.Add("default");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("isActive");
            return toolingFieldsAndTypes;
        }

        public static List<String> globalValueSetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("sorted");
            toolingFieldsAndTypes.Add("customValue");
            return toolingFieldsAndTypes;
        }

        public static List<String> globalValueSetTranslationFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("translation");
            return toolingFieldsAndTypes;
        }

        public static List<String> groupFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("doesIncludeBosses");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("name");
            return toolingFieldsAndTypes;
        }

        public static List<String> homePageComponentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("body");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("height");
            toolingFieldsAndTypes.Add("links");
            toolingFieldsAndTypes.Add("page");
            toolingFieldsAndTypes.Add("pageComponentType");
            toolingFieldsAndTypes.Add("showLabel");
            toolingFieldsAndTypes.Add("showScrollbars");
            toolingFieldsAndTypes.Add("width");
            return toolingFieldsAndTypes;
        }

        public static List<String> homePageLayoutFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("narrowComponents");
            toolingFieldsAndTypes.Add("wideComponents");
            return toolingFieldsAndTypes;
        }

        public static List<String> installedPackageFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("activateRSS");
            toolingFieldsAndTypes.Add("password");
            toolingFieldsAndTypes.Add("versionNumber");

            return toolingFieldsAndTypes;
        }

        public static List<String> keywordListFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("keywords");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> layoutFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("customButtons");
            toolingFieldsAndTypes.Add("customConsoleComponents");
            toolingFieldsAndTypes.Add("emailDefault");
            toolingFieldsAndTypes.Add("excludeButtons");
            toolingFieldsAndTypes.Add("feedLayout");
            toolingFieldsAndTypes.Add("headers");
            toolingFieldsAndTypes.Add("layoutSections");
            toolingFieldsAndTypes.Add("miniLayout");
            toolingFieldsAndTypes.Add("multilineLayoutFields");
            toolingFieldsAndTypes.Add("platformActionList");
            toolingFieldsAndTypes.Add("quickActionList");
            toolingFieldsAndTypes.Add("relatedContent");
            toolingFieldsAndTypes.Add("relatedLists");
            toolingFieldsAndTypes.Add("relatedObjects");
            toolingFieldsAndTypes.Add("runAssignmentRulesDefault");
            toolingFieldsAndTypes.Add("showEmailCheckbox");
            toolingFieldsAndTypes.Add("showHighlightsPanel");
            toolingFieldsAndTypes.Add("showInteractionLogPanel");
            toolingFieldsAndTypes.Add("showKnowledgeComponent");
            toolingFieldsAndTypes.Add("showRunAssignmentRulesCheckbox");
            toolingFieldsAndTypes.Add("showSolutionSection");
            toolingFieldsAndTypes.Add("showSubmitAndAttachButton");
            toolingFieldsAndTypes.Add("summaryLayout");

            return toolingFieldsAndTypes;
        }

        public static List<String> letterheadFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("available");
            toolingFieldsAndTypes.Add("backgroundColor");
            toolingFieldsAndTypes.Add("bodyColor");
            toolingFieldsAndTypes.Add("bottomLine");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("footer");
            toolingFieldsAndTypes.Add("header");
            toolingFieldsAndTypes.Add("middleLine");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("topLine");
            return toolingFieldsAndTypes;
        }

        public static List<String> lightningBoltFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("category");
            toolingFieldsAndTypes.Add("lightningBoltFeatures");
            toolingFieldsAndTypes.Add("lightningBoltImages");
            toolingFieldsAndTypes.Add("lightningBoltItems");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("publisher");
            toolingFieldsAndTypes.Add("summary");
            return toolingFieldsAndTypes;
        }

        public static List<String> lightningComponentBundleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("apiVersion");
            toolingFieldsAndTypes.Add("capabilities");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("isExplicitImport");
            toolingFieldsAndTypes.Add("isExposed");
            toolingFieldsAndTypes.Add("lwcResources");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("targetConfigs");
            toolingFieldsAndTypes.Add("targets");
            return toolingFieldsAndTypes;
        }

        public static List<String> lightningExperienceThemeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("defaultBrandingSet");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("shouldOverrideLoadingImage");
            return toolingFieldsAndTypes;
        }

        public static List<String> lightningMessageChannelFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("isExposed");
            toolingFieldsAndTypes.Add("lightningMessageFields");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> liveChatAgentConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("assignments");
            toolingFieldsAndTypes.Add("autoGreeting");
            toolingFieldsAndTypes.Add("capacity");
            toolingFieldsAndTypes.Add("criticalWaitTime");
            toolingFieldsAndTypes.Add("customAgentName");
            toolingFieldsAndTypes.Add("enableAgentFileTransfer");
            toolingFieldsAndTypes.Add("enableAgentSneakPeek");
            toolingFieldsAndTypes.Add("enableAssistanceFlag");
            toolingFieldsAndTypes.Add("enableAutoAwayOnDecline");
            toolingFieldsAndTypes.Add("enableAutoAwayOnPushTimeout");
            toolingFieldsAndTypes.Add("enableChatConferencing");
            toolingFieldsAndTypes.Add("enableChatMonitoring");
            toolingFieldsAndTypes.Add("enableChatTransferToAgent");
            toolingFieldsAndTypes.Add("enableChatTransferToButton");
            toolingFieldsAndTypes.Add("enableChatTransferToSkill");
            toolingFieldsAndTypes.Add("enableLogoutSound");
            toolingFieldsAndTypes.Add("enableNotifications");
            toolingFieldsAndTypes.Add("enableRequestSound");
            toolingFieldsAndTypes.Add("enableSneakPeek");
            toolingFieldsAndTypes.Add("enableVisitorBlocking");
            toolingFieldsAndTypes.Add("enableWhisperMessage");
            toolingFieldsAndTypes.Add("supervisorDefaultAgentStatusFilter");
            toolingFieldsAndTypes.Add("supervisorDefaultButtonFilter");
            toolingFieldsAndTypes.Add("supervisorDefaultSkillFilter");
            toolingFieldsAndTypes.Add("supervisorSkills");
            toolingFieldsAndTypes.Add("transferableButtons");
            toolingFieldsAndTypes.Add("transferableSkills");

            return toolingFieldsAndTypes;
        }

        public static List<String> liveChatButtonFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("animation");
            toolingFieldsAndTypes.Add("autoGreeting");
            toolingFieldsAndTypes.Add("chasitorIdleTimeout");
            toolingFieldsAndTypes.Add("chasitorIdleTimeoutWarning");
            toolingFieldsAndTypes.Add("chatPage");
            toolingFieldsAndTypes.Add("customAgentName");
            toolingFieldsAndTypes.Add("deployments");
            toolingFieldsAndTypes.Add("enableQueue");
            toolingFieldsAndTypes.Add("inviteEndPosition");
            toolingFieldsAndTypes.Add("inviteImage");
            toolingFieldsAndTypes.Add("inviteStartPosition");
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("numberOfReroutingAttempts");
            toolingFieldsAndTypes.Add("offlineImage");
            toolingFieldsAndTypes.Add("onlineImage");
            toolingFieldsAndTypes.Add("optionsCustomRoutingIsEnabled");
            toolingFieldsAndTypes.Add("optionsHasChasitorIdleTimeout");
            toolingFieldsAndTypes.Add("optionsHasInviteAfterAccept");
            toolingFieldsAndTypes.Add("optionsHasInviteAfterReject");
            toolingFieldsAndTypes.Add("optionsHasRerouteDeclinedRequest");
            toolingFieldsAndTypes.Add("optionsIsAutoAccept");
            toolingFieldsAndTypes.Add("optionsIsInviteAutoRemove");
            toolingFieldsAndTypes.Add("overallQueueLength");
            toolingFieldsAndTypes.Add("perAgentQueueLength");
            toolingFieldsAndTypes.Add("postChatPage");
            toolingFieldsAndTypes.Add("postChatUrl");
            toolingFieldsAndTypes.Add("preChatFormPage");
            toolingFieldsAndTypes.Add("preChatFormUrl");
            toolingFieldsAndTypes.Add("pushTimeOut");
            toolingFieldsAndTypes.Add("routingType");
            toolingFieldsAndTypes.Add("site");
            toolingFieldsAndTypes.Add("skills");
            toolingFieldsAndTypes.Add("timeToRemoveInvite");
            toolingFieldsAndTypes.Add("type");
            toolingFieldsAndTypes.Add("windowLanguage");

            return toolingFieldsAndTypes;
        }

        public static List<String> liveChatDeploymentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("brandingImage");
            toolingFieldsAndTypes.Add("connectionTimeoutDuration");
            toolingFieldsAndTypes.Add("ConnectionWarningDuration");
            toolingFieldsAndTypes.Add("displayQueuePosition");
            toolingFieldsAndTypes.Add("domainWhiteList");
            toolingFieldsAndTypes.Add("enablePrechatApi");
            toolingFieldsAndTypes.Add("enableTranscriptSave");
            toolingFieldsAndTypes.Add("mobileBrandingImage");
            toolingFieldsAndTypes.Add("site");
            toolingFieldsAndTypes.Add("windowTitle");

            return toolingFieldsAndTypes;
        }

        public static List<String> liveChatSensitiveDataRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("actionType");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("enforceOn");
            toolingFieldsAndTypes.Add("isEnabled");
            toolingFieldsAndTypes.Add("pattern");
            toolingFieldsAndTypes.Add("replacement");

            return toolingFieldsAndTypes;
        }

        public static List<String> managedContentTypeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("developerName");
            toolingFieldsAndTypes.Add("managedContentNodeTypes");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> managedTopicsFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("managedTopicType");
            toolingFieldsAndTypes.Add("topicDescription");
            toolingFieldsAndTypes.Add("parentName");
            toolingFieldsAndTypes.Add("position");
            return toolingFieldsAndTypes;
        }

        public static List<String> matchingRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("booleanFilter");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("matchingRuleItems");
            toolingFieldsAndTypes.Add("ruleStatus");
            return toolingFieldsAndTypes;
        }

        public static List<String> metadataFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("fullName");
            return toolingFieldsAndTypes;
        }

        public static List<String> metadataWithContentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("content");
            toolingFieldsAndTypes.Add("fullName");
            return toolingFieldsAndTypes;
        }

        public static List<String> milestoneTypeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("RecurrenceType");
            return toolingFieldsAndTypes;
        }

        public static List<String> mlDomainFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("mlIntents");
            toolingFieldsAndTypes.Add("mlSlotClasses");
            return toolingFieldsAndTypes;
        }

        public static List<String> mobileApplicationDetailFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("applicationBinaryFile");
            toolingFieldsAndTypes.Add("applicationBinaryFileName");
            toolingFieldsAndTypes.Add("applicationBundleIdentifier");
            toolingFieldsAndTypes.Add("applicationFileLength");
            toolingFieldsAndTypes.Add("applicationIconFile");
            toolingFieldsAndTypes.Add("applicationIconFileName");
            toolingFieldsAndTypes.Add("applicationInstallUrl");
            toolingFieldsAndTypes.Add("devicePlatform");
            toolingFieldsAndTypes.Add("deviceType");
            toolingFieldsAndTypes.Add("minimumOsVersion");
            toolingFieldsAndTypes.Add("privateApp");
            toolingFieldsAndTypes.Add("version");
            return toolingFieldsAndTypes;
        }

        public static List<String> moderationRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("action");
            toolingFieldsAndTypes.Add("actionLimit");
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("entitiesAndFields");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("notifyLimit");
            toolingFieldsAndTypes.Add("userCriteria");
            toolingFieldsAndTypes.Add("userMessage");
            return toolingFieldsAndTypes;
        }

        public static List<String> myDomainDiscoverableLoginFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("apexHandler");
            toolingFieldsAndTypes.Add("executeApexHandlerAs");
            toolingFieldsAndTypes.Add("usernameLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> namedCredentialFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("awsAccessKey");
            toolingFieldsAndTypes.Add("awsAccessSecret");
            toolingFieldsAndTypes.Add("awsRegion");
            toolingFieldsAndTypes.Add("awsService");
            toolingFieldsAndTypes.Add("allowMergeFieldsInBody");
            toolingFieldsAndTypes.Add("allowMergeFieldsInHeader");
            toolingFieldsAndTypes.Add("authProvider");
            toolingFieldsAndTypes.Add("authTokenEndpointUrl");
            toolingFieldsAndTypes.Add("certificate");
            toolingFieldsAndTypes.Add("endpoint");
            toolingFieldsAndTypes.Add("generateAuthorizationHeader");
            toolingFieldsAndTypes.Add("jwtAudience");
            toolingFieldsAndTypes.Add("jwtFormulaSubject");
            toolingFieldsAndTypes.Add("jwtIssuer");
            toolingFieldsAndTypes.Add("jwtSigningCertificate");
            toolingFieldsAndTypes.Add("jwtTextSubject");
            toolingFieldsAndTypes.Add("jwtValidityPeriodSeconds");
            toolingFieldsAndTypes.Add("oauthRefreshToken");
            toolingFieldsAndTypes.Add("oauthScope");
            toolingFieldsAndTypes.Add("oauthToken");
            toolingFieldsAndTypes.Add("password");
            toolingFieldsAndTypes.Add("principalType");
            toolingFieldsAndTypes.Add("protocol");
            toolingFieldsAndTypes.Add("username");

            return toolingFieldsAndTypes;
        }

        public static List<String> navigationMenuFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("container");
            toolingFieldsAndTypes.Add("containerType");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("navigationMenuItem");
            return toolingFieldsAndTypes;
        }

        public static List<String> networkFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("allowedExtensions");
            toolingFieldsAndTypes.Add("allowInternalUserLogin");
            toolingFieldsAndTypes.Add("allowMembersToFlag");
            toolingFieldsAndTypes.Add("branding");
            toolingFieldsAndTypes.Add("caseCommentEmailTemplate");
            toolingFieldsAndTypes.Add("changePasswordTemplate");
            toolingFieldsAndTypes.Add("communityRoles");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("disableReputationRecordConversations");
            toolingFieldsAndTypes.Add("emailFooterLogo");
            toolingFieldsAndTypes.Add("emailFooterText");
            toolingFieldsAndTypes.Add("emailSenderAddress");
            toolingFieldsAndTypes.Add("emailSenderName");
            toolingFieldsAndTypes.Add("enableCustomVFError​PageOverrides");
            toolingFieldsAndTypes.Add("enableDirectMessages");
            toolingFieldsAndTypes.Add("enableGuestChatter");
            toolingFieldsAndTypes.Add("enableGuestFileAccess");
            toolingFieldsAndTypes.Add("enableGuestMemberVisibility");
            toolingFieldsAndTypes.Add("enableInvitation");
            toolingFieldsAndTypes.Add("enableKnowledgeable");
            toolingFieldsAndTypes.Add("enableNicknameDisplay");
            toolingFieldsAndTypes.Add("enablePrivateMessages");
            toolingFieldsAndTypes.Add("enableReputation");
            toolingFieldsAndTypes.Add("enableShowAllNetworkSettings");
            toolingFieldsAndTypes.Add("enableSiteAsContainer");
            toolingFieldsAndTypes.Add("enableTalkingAboutStats");
            toolingFieldsAndTypes.Add("enableTopicAssignmentRules");
            toolingFieldsAndTypes.Add("enableTopicSuggestions");
            toolingFieldsAndTypes.Add("enableUpDownVote");
            toolingFieldsAndTypes.Add("feedChannel");
            toolingFieldsAndTypes.Add("forgotPasswordTemplate");
            toolingFieldsAndTypes.Add("gatherCustomerSentimentData");
            toolingFieldsAndTypes.Add("lockoutTemplate");
            toolingFieldsAndTypes.Add("logoutUrl");
            toolingFieldsAndTypes.Add("maxFileSizeKb");
            toolingFieldsAndTypes.Add("navigationLinkSet");
            toolingFieldsAndTypes.Add("networkMemberGroups");
            toolingFieldsAndTypes.Add("networkPageOverrides");
            toolingFieldsAndTypes.Add("newSenderAddress");
            toolingFieldsAndTypes.Add("enableMemberVisibility");
            toolingFieldsAndTypes.Add("picassoSite");
            toolingFieldsAndTypes.Add("recommendationAudience");
            toolingFieldsAndTypes.Add("recommendationDefinition");
            toolingFieldsAndTypes.Add("reputationLevels");
            toolingFieldsAndTypes.Add("reputationPointsRules");
            toolingFieldsAndTypes.Add("selfRegProfile");
            toolingFieldsAndTypes.Add("selfRegistration");
            toolingFieldsAndTypes.Add("sendWelcomeEmail");
            toolingFieldsAndTypes.Add("site");
            toolingFieldsAndTypes.Add("status");
            toolingFieldsAndTypes.Add("tabs");
            toolingFieldsAndTypes.Add("urlPathPrefix");
            toolingFieldsAndTypes.Add("verificationTemplate");
            toolingFieldsAndTypes.Add("welcomeTemplate");

            return toolingFieldsAndTypes;
        }

        public static List<String> networkBrandingFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("loginLogo");
            toolingFieldsAndTypes.Add("loginLogoName");
            toolingFieldsAndTypes.Add("loginLogoStaticImageUrl");
            toolingFieldsAndTypes.Add("loginQuaternaryColor");
            toolingFieldsAndTypes.Add("loginRightFrameUrl");
            toolingFieldsAndTypes.Add("network");
            toolingFieldsAndTypes.Add("pageFooter");
            toolingFieldsAndTypes.Add("pageHeader");
            toolingFieldsAndTypes.Add("primaryColor");
            toolingFieldsAndTypes.Add("primaryComplementColor");
            toolingFieldsAndTypes.Add("quaternaryColor");
            toolingFieldsAndTypes.Add("quaternaryComplementColor");
            toolingFieldsAndTypes.Add("secondaryColor");
            toolingFieldsAndTypes.Add("tertiaryColor");
            toolingFieldsAndTypes.Add("tertiaryComplementColor");
            toolingFieldsAndTypes.Add("zeronaryColor");
            toolingFieldsAndTypes.Add("zeronaryComplementColor");
            return toolingFieldsAndTypes;
        }

        public static List<String> notificationTypeConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("notificationType");
            toolingFieldsAndTypes.Add("appSettings");
            toolingFieldsAndTypes.Add("notificationChannels");
            return toolingFieldsAndTypes;
        }

        public static List<String> oauthCustomScopeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("developerName");
            toolingFieldsAndTypes.Add("isProtected");
            toolingFieldsAndTypes.Add("isPublic");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> packageFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("apiAccessLevel");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("namespacePrefix");
            toolingFieldsAndTypes.Add("objectPermissions");
            toolingFieldsAndTypes.Add("packageType");
            toolingFieldsAndTypes.Add("postInstallClass");
            toolingFieldsAndTypes.Add("setupWeblink");
            toolingFieldsAndTypes.Add("types");
            toolingFieldsAndTypes.Add("uninstallClass");
            toolingFieldsAndTypes.Add("version");
            return toolingFieldsAndTypes;
        }

        public static List<String> pathAssistantFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("entityName");
            toolingFieldsAndTypes.Add("fieldName");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("pathAssistantSteps");
            toolingFieldsAndTypes.Add("recordTypeName");
            return toolingFieldsAndTypes;
        }

        public static List<String> paymentGatewayProviderFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("apexAdapter");
            toolingFieldsAndTypes.Add("comments");
            toolingFieldsAndTypes.Add("idempotencySupported");
            toolingFieldsAndTypes.Add("isProtected");
            toolingFieldsAndTypes.Add("masterLabel");
            return toolingFieldsAndTypes;
        }

        public static List<String> permissionSetGroupFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("mutingPermissionSets");
            toolingFieldsAndTypes.Add("permissionSets");
            toolingFieldsAndTypes.Add("status");
            return toolingFieldsAndTypes;
        }

        public static List<String> platformCachePartitionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("isDefaultPartition");
            toolingFieldsAndTypes.Add("platformCachePartitionTypes");
            return toolingFieldsAndTypes;
        }

        public static List<String> platformEventChannelFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("channelMembers");
            toolingFieldsAndTypes.Add("channelType");
            toolingFieldsAndTypes.Add("label");
            return toolingFieldsAndTypes;
        }

        public static List<String> platformEventChannelMemberFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("eventChannel");
            toolingFieldsAndTypes.Add("selectedEntity");
            return toolingFieldsAndTypes;
        }

        public static List<String> portalFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("admin");
            toolingFieldsAndTypes.Add("defaultLanguage");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("emailSenderAddress");
            toolingFieldsAndTypes.Add("emailSenderName");
            toolingFieldsAndTypes.Add("enableSelfCloseCase");
            toolingFieldsAndTypes.Add("footerDocument");
            toolingFieldsAndTypes.Add("forgotPassTemplate");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("headerDocument");
            toolingFieldsAndTypes.Add("isSelfRegistrationActivated");
            toolingFieldsAndTypes.Add("loginHeaderDocument");
            toolingFieldsAndTypes.Add("logoDocument");
            toolingFieldsAndTypes.Add("logoutUrl");
            toolingFieldsAndTypes.Add("newCommentTemplate");
            toolingFieldsAndTypes.Add("newPassTemplate");
            toolingFieldsAndTypes.Add("newUserTemplate");
            toolingFieldsAndTypes.Add("ownerNotifyTemplate");
            toolingFieldsAndTypes.Add("selfRegNewUserUrl");
            toolingFieldsAndTypes.Add("selfRegUserDefaultProfile");
            toolingFieldsAndTypes.Add("selfRegUserDefaultRole");
            toolingFieldsAndTypes.Add("selfRegUserTemplate");
            toolingFieldsAndTypes.Add("showActionConfirmation");
            toolingFieldsAndTypes.Add("stylesheetDocument");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> postTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("default");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fields");
            toolingFieldsAndTypes.Add("label");
            return toolingFieldsAndTypes;
        }

        public static List<String> presenceDeclineReasonFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            return toolingFieldsAndTypes;
        }

        public static List<String> presenceUserConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("assignments");
            toolingFieldsAndTypes.Add("capacity");
            toolingFieldsAndTypes.Add("declineReasons");
            toolingFieldsAndTypes.Add("enableAutoAccept");
            toolingFieldsAndTypes.Add("enableDecline");
            toolingFieldsAndTypes.Add("enableDeclineReason");
            toolingFieldsAndTypes.Add("enableDisconnectSound");
            toolingFieldsAndTypes.Add("enableRequestSound");
            toolingFieldsAndTypes.Add("presenceStatusOnDecline");
            toolingFieldsAndTypes.Add("presenceStatusOnPushTimeout");

            return toolingFieldsAndTypes;
        }

        public static List<String> profilePermissionSetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("userLicense");
            toolingFieldsAndTypes.Add("license");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("custom");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("hasActivationRequired");
            toolingFieldsAndTypes.Add("loginHours");
            toolingFieldsAndTypes.Add("loginIpRanges");
            toolingFieldsAndTypes.Add("applicationVisibilities");
            toolingFieldsAndTypes.Add("tabVisibilities");
            toolingFieldsAndTypes.Add("layoutAssignments");
            toolingFieldsAndTypes.Add("objectPermissions");
            toolingFieldsAndTypes.Add("customMetadataTypeAccesses");
            toolingFieldsAndTypes.Add("recordTypeVisibilities");
            toolingFieldsAndTypes.Add("fieldPermissions");      // API version 23.0 and later
            toolingFieldsAndTypes.Add("fieldLevelSecurities");  // API version 22.0 and earlier
            toolingFieldsAndTypes.Add("pageAccesses");
            toolingFieldsAndTypes.Add("classAccesses");
            toolingFieldsAndTypes.Add("flowAccesses");
            toolingFieldsAndTypes.Add("categoryGroupVisibilities");
            toolingFieldsAndTypes.Add("userPermissions");
            toolingFieldsAndTypes.Add("customPermissions");
            toolingFieldsAndTypes.Add("customSettingAccesses");
            toolingFieldsAndTypes.Add("externalDataSourceAccesses");
            toolingFieldsAndTypes.Add("profileActionOverrides");

            return toolingFieldsAndTypes;
        }

        public static List<String> profileActionOverrideFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("actionName");
            toolingFieldsAndTypes.Add("content");
            toolingFieldsAndTypes.Add("formFactor");
            toolingFieldsAndTypes.Add("pageOrSobjectType");
            toolingFieldsAndTypes.Add("recordType");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> profilePasswordPolicyFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("forgotPasswordRedirect");
            toolingFieldsAndTypes.Add("lockoutInterval");
            toolingFieldsAndTypes.Add("maxLoginAttempts");
            toolingFieldsAndTypes.Add("minimumPasswordLength");
            toolingFieldsAndTypes.Add("minimumPasswordLifetime");
            toolingFieldsAndTypes.Add("obscure");
            toolingFieldsAndTypes.Add("passwordComplexity");
            toolingFieldsAndTypes.Add("passwordExpiration");
            toolingFieldsAndTypes.Add("passwordHistory");
            toolingFieldsAndTypes.Add("passwordQuestion");
            toolingFieldsAndTypes.Add("profile");

            return toolingFieldsAndTypes;
        }

        public static List<String> profileSessionSettingFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("forceLogout");
            toolingFieldsAndTypes.Add("profile");
            toolingFieldsAndTypes.Add("requiredSessionLevel");
            toolingFieldsAndTypes.Add("sessionPersistence");
            toolingFieldsAndTypes.Add("sessionTimeout");
            toolingFieldsAndTypes.Add("sessionTimeoutWarning");
            return toolingFieldsAndTypes;
        }

        public static List<String> promptFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("promptVersions");
            return toolingFieldsAndTypes;
        }

        public static List<String> queueFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("doesSendEmailToMembers");
            toolingFieldsAndTypes.Add("email");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("queueMembers");
            toolingFieldsAndTypes.Add("queueRoutingConfig");
            toolingFieldsAndTypes.Add("queueSobject");

            return toolingFieldsAndTypes;
        }

        public static List<String> queueRoutingConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("capacityPercentage");
            toolingFieldsAndTypes.Add("capacityWeight");
            toolingFieldsAndTypes.Add("dropAdditionalSkillsTimeout");
            toolingFieldsAndTypes.Add("isAttributeBased");
            toolingFieldsAndTypes.Add("pushTimeout");
            toolingFieldsAndTypes.Add("queueOverflowAssignee");
            toolingFieldsAndTypes.Add("routingModel");
            toolingFieldsAndTypes.Add("routingPriority");
            toolingFieldsAndTypes.Add("userOverflowAssignee");

            return toolingFieldsAndTypes;
        }

        public static List<String> quickActionFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("canvas");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fieldOverrides");
            toolingFieldsAndTypes.Add("flowDefinition");
            toolingFieldsAndTypes.Add("height");
            toolingFieldsAndTypes.Add("icon");
            toolingFieldsAndTypes.Add("isProtected");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("lightningComponent");
            toolingFieldsAndTypes.Add("optionsCreateFeedItem");
            toolingFieldsAndTypes.Add("page");
            toolingFieldsAndTypes.Add("quickActionLayout");
            toolingFieldsAndTypes.Add("standardLabel");
            toolingFieldsAndTypes.Add("targetObject");
            toolingFieldsAndTypes.Add("targetParentField");
            toolingFieldsAndTypes.Add("targetRecordType");
            toolingFieldsAndTypes.Add("type");
            toolingFieldsAndTypes.Add("width");

            return toolingFieldsAndTypes;
        }

        public static List<String> redirectWhitelistUrlFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("Url");
            return toolingFieldsAndTypes;
        }

        public static List<String> recommendationStrategyFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("actionContext");
            toolingFieldsAndTypes.Add("contextRecordType");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("filter");
            toolingFieldsAndTypes.Add("if");
            toolingFieldsAndTypes.Add("invocableAction");
            toolingFieldsAndTypes.Add("isTemplate");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("map");
            toolingFieldsAndTypes.Add("mutuallyExclusive");
            toolingFieldsAndTypes.Add("onBehalfOfExpression");
            toolingFieldsAndTypes.Add("recommendationLimit");
            toolingFieldsAndTypes.Add("recommendationLoad");
            toolingFieldsAndTypes.Add("sort");
            toolingFieldsAndTypes.Add("union");
            return toolingFieldsAndTypes;
        }

        public static List<String> recordActionDeploymentFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("channelConfigurations");
            toolingFieldsAndTypes.Add("deploymentContexts");
            toolingFieldsAndTypes.Add("hasGuidedActions");
            toolingFieldsAndTypes.Add("hasRecommendations");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("recommendation");
            toolingFieldsAndTypes.Add("selectableItems");
            toolingFieldsAndTypes.Add("shouldLaunchActionOnReject");
            return toolingFieldsAndTypes;
        }

        public static List<String> remoteSiteSettingFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("url");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("disableProtocolSecurity");

            return toolingFieldsAndTypes;
        }

        public static List<String> reportFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("aggregates");
            toolingFieldsAndTypes.Add("block");
            toolingFieldsAndTypes.Add("blockInfo");
            toolingFieldsAndTypes.Add("buckets");
            toolingFieldsAndTypes.Add("chart");
            toolingFieldsAndTypes.Add("colorRanges");
            toolingFieldsAndTypes.Add("columns");
            toolingFieldsAndTypes.Add("crossFilters");
            toolingFieldsAndTypes.Add("currency");
            toolingFieldsAndTypes.Add("dataCategoryFilters");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("division");
            toolingFieldsAndTypes.Add("filter");
            toolingFieldsAndTypes.Add("folderName");
            toolingFieldsAndTypes.Add("format");
            toolingFieldsAndTypes.Add("formattingRules");
            toolingFieldsAndTypes.Add("groupingsAcross");
            toolingFieldsAndTypes.Add("groupingsDown");
            toolingFieldsAndTypes.Add("historicalSelector");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("numSubscriptions");
            toolingFieldsAndTypes.Add("params");
            toolingFieldsAndTypes.Add("reportCustomDetailFormula");
            toolingFieldsAndTypes.Add("reportType");
            toolingFieldsAndTypes.Add("roleHierarchyFilter");
            toolingFieldsAndTypes.Add("rowLimit");
            toolingFieldsAndTypes.Add("scope");
            toolingFieldsAndTypes.Add("showCurrentDate");
            toolingFieldsAndTypes.Add("showDetails");
            toolingFieldsAndTypes.Add("showGrandTotal");
            toolingFieldsAndTypes.Add("showSubTotals");
            toolingFieldsAndTypes.Add("sortColumn");
            toolingFieldsAndTypes.Add("sortOrder");
            toolingFieldsAndTypes.Add("territoryHierarchyFilter");
            toolingFieldsAndTypes.Add("timeFrameFilter");
            toolingFieldsAndTypes.Add("userFilter");
            return toolingFieldsAndTypes;
        }

        public static List<String> reportTypeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("autogenerated");
            toolingFieldsAndTypes.Add("baseObject");
            toolingFieldsAndTypes.Add("category");
            toolingFieldsAndTypes.Add("deployed");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("join");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("sections");
            return toolingFieldsAndTypes;
        }

        public static List<String> roleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("parentRole");
            return toolingFieldsAndTypes;
        }

        public static List<String> roleOrTerritoryFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("caseAccessLevel");
            toolingFieldsAndTypes.Add("contactAccessLevel");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("mayForecastManagerShare");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("opportunityAccessLevel");
            return toolingFieldsAndTypes;
        }

        public static List<String> samlSsoConfigFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("attributeNameIdFormat");
            toolingFieldsAndTypes.Add("attributeName");
            toolingFieldsAndTypes.Add("decryptionCertificate");
            toolingFieldsAndTypes.Add("errorUrl");
            toolingFieldsAndTypes.Add("executionUserId");
            toolingFieldsAndTypes.Add("identityLocation");
            toolingFieldsAndTypes.Add("identityMapping");
            toolingFieldsAndTypes.Add("issuer");
            toolingFieldsAndTypes.Add("loginUrl");
            toolingFieldsAndTypes.Add("logoutUrl");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("oauthTokenEndpoint");
            toolingFieldsAndTypes.Add("redirectBinding");
            toolingFieldsAndTypes.Add("requestSignatureMethod");
            toolingFieldsAndTypes.Add("requestSigningCertId");
            toolingFieldsAndTypes.Add("salesforceLoginUrl");
            toolingFieldsAndTypes.Add("samlEntityId");
            toolingFieldsAndTypes.Add("samlJitHandlerId");
            toolingFieldsAndTypes.Add("samlVersion");
            toolingFieldsAndTypes.Add("singleLogoutBinding");
            toolingFieldsAndTypes.Add("singleLogoutUrl");
            toolingFieldsAndTypes.Add("useConfigRequestMethod");
            toolingFieldsAndTypes.Add("userProvisioning");
            toolingFieldsAndTypes.Add("validationCert");
            return toolingFieldsAndTypes;
        }

        public static List<String> scontrolFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("content");
            toolingFieldsAndTypes.Add("contentSource");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("encodingKey");
            toolingFieldsAndTypes.Add("fileContent");
            toolingFieldsAndTypes.Add("fileName");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("supportsCaching");
            return toolingFieldsAndTypes;
        }

        public static List<String> serviceChannelFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("doesMinimizeWidgetOnAccept");
            toolingFieldsAndTypes.Add("interactionComponent");
            toolingFieldsAndTypes.Add("relatedEntityType");
            toolingFieldsAndTypes.Add("secondaryRoutingPriorityField");
            toolingFieldsAndTypes.Add("serviceChannelFieldPriorities");

            return toolingFieldsAndTypes;
        }

        public static List<String> servicePresenceStatusFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("channels");
            return toolingFieldsAndTypes;
        }

        //public static List<String> settingsFieldNames()
        //{
        //    List<String> toolingFieldsAndTypes = new List<String>();
        //    toolingFieldsAndTypes.Add("");
        //    return toolingFieldsAndTypes;
        //}

        public static List<String> sharedToFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("allCustomerPortalUsers");
            toolingFieldsAndTypes.Add("allInternalUsers");
            toolingFieldsAndTypes.Add("allPartnerUsers");
            toolingFieldsAndTypes.Add("channelProgramGroup");
            toolingFieldsAndTypes.Add("channelProgramGroups");
            toolingFieldsAndTypes.Add("group");
            toolingFieldsAndTypes.Add("guestUser");
            toolingFieldsAndTypes.Add("groups");
            toolingFieldsAndTypes.Add("managerSubordinates");
            toolingFieldsAndTypes.Add("managers");
            toolingFieldsAndTypes.Add("portalRole");
            toolingFieldsAndTypes.Add("portalRoleandSubordinates");
            toolingFieldsAndTypes.Add("role");
            toolingFieldsAndTypes.Add("roleAndSubordinates");
            toolingFieldsAndTypes.Add("roleAndSubordinatesInternal");
            toolingFieldsAndTypes.Add("roles");
            toolingFieldsAndTypes.Add("rolesAndSubordinates");
            toolingFieldsAndTypes.Add("territories");
            toolingFieldsAndTypes.Add("territoriesAndSubordinates");
            toolingFieldsAndTypes.Add("territory");
            toolingFieldsAndTypes.Add("territoryAndSubordinates");
            toolingFieldsAndTypes.Add("queue");
            return toolingFieldsAndTypes;
        }

        public static List<String> sharingBaseRuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("accessLevel");
            toolingFieldsAndTypes.Add("accountSettings");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("sharedTo");
            return toolingFieldsAndTypes;
        }

        public static List<String> sharingRulesFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("sharingCriteriaRules");
            toolingFieldsAndTypes.Add("sharingGuestRules");
            toolingFieldsAndTypes.Add("sharingOwnerRules");
            toolingFieldsAndTypes.Add("sharingTerritoryRules");
            return toolingFieldsAndTypes;
        }

        public static List<String> sharingSetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("accessMappings");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("profiles");
            return toolingFieldsAndTypes;
        }

        public static List<String> siteDotComFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("label");
            toolingFieldsAndTypes.Add("siteType");
            return toolingFieldsAndTypes;
        }

        public static List<String> skillFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("assignments");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("label");

            return toolingFieldsAndTypes;
        }

        public static List<String> standardValueSetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("groupingStringEnum");
            toolingFieldsAndTypes.Add("sorted");
            toolingFieldsAndTypes.Add("standardValue");
            return toolingFieldsAndTypes;
        }

        public static List<String> standardValueSetTranslationFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("valueTranslation");
            return toolingFieldsAndTypes;
        }

        public static List<String> staticResourceFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("cacheControl");
            toolingFieldsAndTypes.Add("contentType");

            return toolingFieldsAndTypes;
        }

        public static List<String> synonymDictionaryFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("groups");
            toolingFieldsAndTypes.Add("isProtected");
            toolingFieldsAndTypes.Add("label");
            return toolingFieldsAndTypes;
        }

        public static List<String> territoryFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("accountAccessLevel");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("parentTerritory");
            return toolingFieldsAndTypes;
        }

        public static List<String> territory2FieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("accountAccessLevel");
            toolingFieldsAndTypes.Add("caseAccessLevel");
            toolingFieldsAndTypes.Add("contactAccessLevel");
            toolingFieldsAndTypes.Add("customFields");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("opportunityAccessLevel");
            toolingFieldsAndTypes.Add("parentTerritory");
            toolingFieldsAndTypes.Add("ruleAssociations");
            toolingFieldsAndTypes.Add("territory2Type");
            return toolingFieldsAndTypes;
        }

        public static List<String> territory2ModelFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("customFields");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("name");
            return toolingFieldsAndTypes;
        }

        public static List<String> territory2RuleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("booleanFilter");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("objectType");
            toolingFieldsAndTypes.Add("ruleItems");
            return toolingFieldsAndTypes;
        }

        public static List<String> territory2TypeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("name");
            toolingFieldsAndTypes.Add("priority");
            return toolingFieldsAndTypes;
        }

        public static List<String> testSuiteFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("testClassName");
            return toolingFieldsAndTypes;
        }

        public static List<String> timeSheetTemplateFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("frequency");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("startDate");
            toolingFieldsAndTypes.Add("timeSheetTemplateAssignments");
            toolingFieldsAndTypes.Add("workWeekEndDay");
            toolingFieldsAndTypes.Add("workWeekStartDay");
            return toolingFieldsAndTypes;
        }

        public static List<String> topicsForObjectsFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("enableTopics");
            toolingFieldsAndTypes.Add("entityApiName");
            return toolingFieldsAndTypes;
        }

        public static List<String> transactionSecurityPolicyFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("action");
            toolingFieldsAndTypes.Add("active");
            toolingFieldsAndTypes.Add("apexClass");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("developerName");
            toolingFieldsAndTypes.Add("eventName");
            toolingFieldsAndTypes.Add("eventType");
            toolingFieldsAndTypes.Add("executionUser");
            toolingFieldsAndTypes.Add("flow");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("resourceName");
            toolingFieldsAndTypes.Add("type");
            return toolingFieldsAndTypes;
        }

        public static List<String> translationsFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("customApplications");
            toolingFieldsAndTypes.Add("customLabels");
            toolingFieldsAndTypes.Add("customPageWebLinks");
            toolingFieldsAndTypes.Add("customTabs");
            toolingFieldsAndTypes.Add("flowDefinitions");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("globalPicklists");
            toolingFieldsAndTypes.Add("prompts");
            toolingFieldsAndTypes.Add("quickActions");
            toolingFieldsAndTypes.Add("reportTypes");
            toolingFieldsAndTypes.Add("scontrols");
            return toolingFieldsAndTypes;
        }

        public static List<String> userCriteriaFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("creationAgeInSeconds");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("lastChatterActivityAgeInSeconds");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("userTypes");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveApplicationFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("assetIcon");
            toolingFieldsAndTypes.Add("description");
            toolingFieldsAndTypes.Add("folder");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("shares");
            toolingFieldsAndTypes.Add("templateOrigin");
            toolingFieldsAndTypes.Add("templateVersion");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveDataflowFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveDashboardFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveDatasetFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveLensFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveRecipeFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveTemplateBundleFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> waveXmdFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("");
            return toolingFieldsAndTypes;
        }

        public static List<String> workflowFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();

            toolingFieldsAndTypes.Add("alerts");
            toolingFieldsAndTypes.Add("fieldUpdates");
            toolingFieldsAndTypes.Add("flowActions");
            toolingFieldsAndTypes.Add("fullName");
            toolingFieldsAndTypes.Add("knowledgePublishes");
            toolingFieldsAndTypes.Add("outboundMessages");
            toolingFieldsAndTypes.Add("rules");
            toolingFieldsAndTypes.Add("tasks");

            return toolingFieldsAndTypes;
        }

        public static List<String> workSkillRoutingFieldNames()
        {
            List<String> toolingFieldsAndTypes = new List<String>();
            toolingFieldsAndTypes.Add("isActive");
            toolingFieldsAndTypes.Add("masterLabel");
            toolingFieldsAndTypes.Add("relatedEntity");
            toolingFieldsAndTypes.Add("workSkillRoutingAttributes");
            return toolingFieldsAndTypes;
        }


        public static void getApexClasses(SalesforceCredentials sc, String queryString, UtilityClass.REQUESTINGORG reqOrg, Dictionary<String, String> classIdToClassName)
        {
            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(query);
            }

            if (toolingQr.records == null) return;

            Boolean done = false;

            while (!done)
            {
                toolingRecords = toolingQr.records;

                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ApexClass1 apexClass = (SalesforceMetadata.ToolingWSDL.ApexClass1)toolingRecord;
                    if (!classIdToClassName.ContainsKey(apexClass.Id))
                    {
                        classIdToClassName.Add(apexClass.Id, apexClass.Name);
                    }
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void apexClassToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                            SalesforceCredentials sc,
                                            String queryString, 
                                            UtilityClass.REQUESTINGORG reqOrg, 
                                            Dictionary<String, String> classIdToClassName, 
                                            Boolean retrieveAggregateCoverage)
        {
            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            SalesforceMetadata.ToolingWSDL.QueryResult apexTestClassesAndMethods;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(query);
            }

            if (toolingQr.records == null) return;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "ApexClasses";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "ApiVersion";
            xlWorksheet.Cells[1, 5].Value = "IsValid";
            xlWorksheet.Cells[1, 6].Value = "Status";
            xlWorksheet.Cells[1, 7].Value = "IsTestClass";
            xlWorksheet.Cells[1, 8].Value = "LengthWithoutComments";
            xlWorksheet.Cells[1, 9].Value = "NumberOfLines";
            xlWorksheet.Cells[1, 10].Value = "NumLinesCovered";
            xlWorksheet.Cells[1, 11].Value = "NumLinesUncovered";
            xlWorksheet.Cells[1, 12].Value = "PercentageCovered";
            xlWorksheet.Cells[1, 13].Value = "ManageableState";
            xlWorksheet.Cells[1, 14].Value = "CreatedById";
            xlWorksheet.Cells[1, 15].Value = "CreatedByName";
            xlWorksheet.Cells[1, 16].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 17].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 18].Value = "CreatedDate";
            xlWorksheet.Cells[1, 19].Value = "LastModifiedDate";
            xlWorksheet.Cells[1, 20].Value = "TestClasses";
            xlWorksheet.Cells[1, 21].Value = "TestClassesAndMethods";
            xlWorksheet.Cells[1, 22].Value = "Interfaces";
            xlWorksheet.Cells[1, 23].Value = "ClassContainsTestMethods";


            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                toolingRecords = toolingQr.records;

                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ApexClass1 apexClass = (SalesforceMetadata.ToolingWSDL.ApexClass1)toolingRecord;

                    Boolean isTestClass = false;

                    // Note the skip to accommodate the population of values in the skipped column numbers
                    xlWorksheet.Cells[rowNumber, 1].Value = apexClass.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = apexClass.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = apexClass.NamespacePrefix;

                    xlWorksheet.Cells[rowNumber, 4].Value = apexClass.ApiVersion;
                    xlWorksheet.Cells[rowNumber, 5].Value = apexClass.IsValid;
                    xlWorksheet.Cells[rowNumber, 6].Value = apexClass.Status;

                    xlWorksheet.Cells[rowNumber, 8].Value = apexClass.LengthWithoutComments;

                    xlWorksheet.Cells[rowNumber, 13].Value = apexClass.ManageableState;
                    xlWorksheet.Cells[rowNumber, 14].Value = apexClass.CreatedById;

                    if (apexClass.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 15].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 15].Value = apexClass.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 16].Value = apexClass.LastModifiedById;

                    if (apexClass.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 17].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 17].Value = apexClass.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 18].Value = apexClass.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 19].Value = apexClass.LastModifiedDate;

                    SalesforceMetadata.ToolingWSDL.SymbolTable clsSymbols = new SalesforceMetadata.ToolingWSDL.SymbolTable();

                    if (apexClass.SymbolTable != null)
                    {
                        clsSymbols = (SalesforceMetadata.ToolingWSDL.SymbolTable)apexClass.SymbolTable;
                    }

                    // Determine if it is Test Class or not. We don't need to retrieve code coverage for test classes
                    if (clsSymbols.interfaces != null)
                    {
                        String interfaces = "";

                        foreach (String ifc in clsSymbols.interfaces)
                        {
                            interfaces = interfaces + ifc + ", ";
                        }

                        interfaces = interfaces.Substring(0, interfaces.Length - 2);
                        xlWorksheet.Cells[rowNumber, 22].Value = interfaces;
                    }
                    
                    // Determine if a Class is a test class
                    if (clsSymbols.tableDeclaration != null)
                    {
                        if (clsSymbols.tableDeclaration.annotations != null
                            && clsSymbols.tableDeclaration.annotations.Length > 0)
                        {
                            foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in clsSymbols.tableDeclaration.annotations)
                            {
                                if (annot.name == "IsTest")
                                {
                                    isTestClass = true;
                                    xlWorksheet.Cells[rowNumber, 7].Value = true;
                                }
                                else
                                {
                                    xlWorksheet.Cells[rowNumber, 7].Value = false;
                                }
                            }
                        }
                        else
                        {
                            xlWorksheet.Cells[rowNumber, 7].Value = false;
                        }
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = false;
                    }

                    // Determine if Class contains test methods
                    xlWorksheet.Cells[rowNumber, 23].Value = false;
                    if (clsSymbols.methods != null)
                    {
                        foreach (SalesforceMetadata.ToolingWSDL.Method method in clsSymbols.methods)
                        {
                            if (method.annotations != null)
                            {
                                foreach (SalesforceMetadata.ToolingWSDL.Annotation annot in method.annotations)
                                {
                                    if (annot.name == "IsTest")
                                    {
                                        isTestClass = true;
                                        xlWorksheet.Cells[rowNumber, 23].Value = true;
                                    }
                                }
                            }
                        }
                    }

                    // Number of Lines, Number of Lines Covered, Uncovered, and Percentage Covered
                    if (retrieveAggregateCoverage
                        && isTestClass == false
                        && (apexClass.NamespacePrefix == null || apexClass.NamespacePrefix == ""))
                    {
                        SalesforceMetadata.ToolingWSDL.QueryResult toolingAggregateCoverageQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
                        toolingAggregateCoverageQr = sc.fromOrgToolingSvc.query(ApexCodeCoverageAggregateQuery(apexClass.Id));

                        // Skip Test Classes
                        if (toolingAggregateCoverageQr.records != null)
                        {
                            SalesforceMetadata.ToolingWSDL.sObject[] toolingQrRecords = toolingAggregateCoverageQr.records;
                            foreach (SalesforceMetadata.ToolingWSDL.sObject toolingSobj in toolingQrRecords)
                            {
                                ApexCodeCoverageAggregate accAggregate = (ApexCodeCoverageAggregate)toolingSobj;
                                Double denominator = ((Double)accAggregate.NumLinesCovered + (Double)accAggregate.NumLinesUncovered);

                                xlWorksheet.Cells[rowNumber, 9].Value = denominator;
                                xlWorksheet.Cells[rowNumber, 10].Value = accAggregate.NumLinesCovered;
                                xlWorksheet.Cells[rowNumber, 11].Value = accAggregate.NumLinesUncovered;

                                if (denominator != 0)
                                {
                                    Double percentCovered = (Double)accAggregate.NumLinesCovered / ((Double)accAggregate.NumLinesCovered + (Double)accAggregate.NumLinesUncovered);
                                    percentCovered = percentCovered * 100;
                                    xlWorksheet.Cells[rowNumber, 12].Value = percentCovered;
                                }
                            }

                            // Get Test Classes and Test Methods
                            apexTestClassesAndMethods = new SalesforceMetadata.ToolingWSDL.QueryResult();
                            apexTestClassesAndMethods = sc.fromOrgToolingSvc.query(ApexCodeCoverageQuery(apexClass.Id));

                            SalesforceMetadata.ToolingWSDL.sObject[] apexTestClassesAndMethodsRecords = apexTestClassesAndMethods.records;

                            String testClasses = "";
                            String apexTestClassAndMethod = "";

                            if (apexTestClassesAndMethodsRecords != null)
                            {
                                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingClass in apexTestClassesAndMethodsRecords)
                                {
                                    ApexCodeCoverage acm = (ApexCodeCoverage)toolingClass;

                                    if (!testClasses.Contains(classIdToClassName[acm.ApexTestClassId]))
                                    {
                                        testClasses = testClasses + classIdToClassName[acm.ApexTestClassId] + "; ";
                                    }

                                    testClasses = testClasses.Substring(0, testClasses.Length - 2);

                                    apexTestClassAndMethod = apexTestClassAndMethod + classIdToClassName[acm.ApexTestClassId] + " - " + acm.TestMethodName + "; ";
                                }

                                xlWorksheet.Cells[rowNumber, 20].Value = testClasses;
                                xlWorksheet.Cells[rowNumber, 21].Value = apexTestClassAndMethod;
                            }
                        }
                    }

                    rowNumber++;
                }
                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void apexComponentToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                SalesforceCredentials sc,
                                                String queryString, 
                                                UtilityClass.REQUESTINGORG reqOrg, 
                                                Dictionary<String, String> classIdToClassName) 
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);
            xlWorksheet.Name = "ApexComponent";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "MasterLabel";
            xlWorksheet.Cells[1, 4].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 5].Value = "ApiVersion";
            xlWorksheet.Cells[1, 6].Value = "ControllerKey";
            xlWorksheet.Cells[1, 7].Value = "ControllerType";
            xlWorksheet.Cells[1, 8].Value = "ManageableState";
            xlWorksheet.Cells[1, 9].Value = "CreatedById";
            xlWorksheet.Cells[1, 10].Value = "CreatedByName";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 13].Value = "CreatedDate";
            xlWorksheet.Cells[1, 14].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ApexComponent1 apexComponent = (SalesforceMetadata.ToolingWSDL.ApexComponent1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = apexComponent.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = apexComponent.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = apexComponent.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 4].Value = apexComponent.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 5].Value = apexComponent.ApiVersion;
                    xlWorksheet.Cells[rowNumber, 6].Value = apexComponent.ControllerKey;
                    xlWorksheet.Cells[rowNumber, 7].Value = apexComponent.ControllerType;
                    xlWorksheet.Cells[rowNumber, 8].Value = apexComponent.ManageableState;
                    xlWorksheet.Cells[rowNumber, 9].Value = apexComponent.CreatedById;

                    if (apexComponent.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = apexComponent.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = apexComponent.LastModifiedById;

                    if (apexComponent.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = apexComponent.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 13].Value = apexComponent.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 14].Value = apexComponent.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void apexPageToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                           SalesforceCredentials sc,
                                           String queryString, 
                                           UtilityClass.REQUESTINGORG reqOrg, 
                                           Dictionary<String, String> classIdToClassName) 
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);
            xlWorksheet.Name = "ApexPage";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "MasterLabel";
            xlWorksheet.Cells[1, 4].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 5].Value = "ApiVersion";
            xlWorksheet.Cells[1, 6].Value = "ControllerKey";
            xlWorksheet.Cells[1, 7].Value = "ControllerType";
            xlWorksheet.Cells[1, 8].Value = "ManageableState";
            xlWorksheet.Cells[1, 9].Value = "CreatedById";
            xlWorksheet.Cells[1, 10].Value = "CreatedByName";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 13].Value = "CreatedDate";
            xlWorksheet.Cells[1, 14].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ApexPage1 apexPage = (SalesforceMetadata.ToolingWSDL.ApexPage1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = apexPage.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = apexPage.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = apexPage.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 4].Value = apexPage.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 5].Value = apexPage.ApiVersion;
                    xlWorksheet.Cells[rowNumber, 6].Value = apexPage.ControllerKey;
                    xlWorksheet.Cells[rowNumber, 7].Value = apexPage.ControllerType;
                    xlWorksheet.Cells[rowNumber, 8].Value = apexPage.ManageableState;
                    xlWorksheet.Cells[rowNumber, 9].Value = apexPage.CreatedById;
                    if (apexPage.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = apexPage.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = apexPage.LastModifiedById;
                    
                    if (apexPage.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = apexPage.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 13].Value = apexPage.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 14].Value = apexPage.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void apexTriggerToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                              SalesforceCredentials sc,
                                              String queryString,
                                              UtilityClass.REQUESTINGORG reqOrg,
                                              Dictionary<String, String> classIdToClassName,
                                              Dictionary<String, String> customObjIdToName,
                                              Boolean retrieveAggregateCoverage)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            SalesforceMetadata.ToolingWSDL.QueryResult apexTestClassesAndMethods;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "ApexTriggers";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "ApiVersion";
            xlWorksheet.Cells[1, 5].Value = "EntityDefinition";
            xlWorksheet.Cells[1, 6].Value = "IsValid";
            xlWorksheet.Cells[1, 7].Value = "Status";
            xlWorksheet.Cells[1, 8].Value = "LengthWithoutComments";
            xlWorksheet.Cells[1, 9].Value = "NumberOfLines";
            xlWorksheet.Cells[1, 10].Value = "NumLinesCovered";
            xlWorksheet.Cells[1, 11].Value = "NumLinesUncovered";
            xlWorksheet.Cells[1, 12].Value = "PercentageCovered";
            xlWorksheet.Cells[1, 13].Value = "ManageableState";
            xlWorksheet.Cells[1, 14].Value = "UsageBeforeInsert";
            xlWorksheet.Cells[1, 15].Value = "UsageBeforeUpdate";
            xlWorksheet.Cells[1, 16].Value = "UsageBeforeDelete";
            xlWorksheet.Cells[1, 17].Value = "UsageAfterInsert";
            xlWorksheet.Cells[1, 18].Value = "UsageAfterUpdate";
            xlWorksheet.Cells[1, 19].Value = "UsageAfterDelete";
            xlWorksheet.Cells[1, 20].Value = "UsageAfterUndelete";
            xlWorksheet.Cells[1, 21].Value = "UsageIsBulk";
            xlWorksheet.Cells[1, 22].Value = "CreatedById";
            xlWorksheet.Cells[1, 23].Value = "CreatedByName";
            xlWorksheet.Cells[1, 24].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 25].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 26].Value = "CreatedDate";
            xlWorksheet.Cells[1, 27].Value = "LastModifiedDate";
            xlWorksheet.Cells[1, 28].Value = "TestClasses";
            xlWorksheet.Cells[1, 29].Value = "TestClassesAndMethods";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ApexTrigger1 apexTrigger = (SalesforceMetadata.ToolingWSDL.ApexTrigger1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = apexTrigger.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = apexTrigger.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = apexTrigger.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 4].Value = apexTrigger.ApiVersion;

                    if (apexTrigger.EntityDefinitionId != null && customObjIdToName.ContainsKey(apexTrigger.EntityDefinitionId))
                    {
                        xlWorksheet.Cells[rowNumber, 5].Value = customObjIdToName[apexTrigger.EntityDefinitionId];
                    }
                    //else if (apexTrigger.EntityDefinitionId != null && customObjIdToName15.ContainsKey(apexTrigger.EntityDefinitionId))
                    //{
                    //    xlWorksheet.Cells[rowNumber, 5].Value = customObjIdToName15[apexTrigger.EntityDefinitionId];
                    //}
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 5].Value = apexTrigger.EntityDefinitionId;
                    }

                    xlWorksheet.Cells[rowNumber, 6].Value = apexTrigger.IsValid;
                    xlWorksheet.Cells[rowNumber, 7].Value = apexTrigger.Status;
                    xlWorksheet.Cells[rowNumber, 8].Value = apexTrigger.LengthWithoutComments;

                    xlWorksheet.Cells[rowNumber, 13].Value = apexTrigger.ManageableState;
                    xlWorksheet.Cells[rowNumber, 14].Value = apexTrigger.UsageBeforeInsert;
                    xlWorksheet.Cells[rowNumber, 15].Value = apexTrigger.UsageBeforeUpdate;
                    xlWorksheet.Cells[rowNumber, 16].Value = apexTrigger.UsageBeforeDelete;
                    xlWorksheet.Cells[rowNumber, 17].Value = apexTrigger.UsageAfterInsert;
                    xlWorksheet.Cells[rowNumber, 18].Value = apexTrigger.UsageAfterUpdate;
                    xlWorksheet.Cells[rowNumber, 19].Value = apexTrigger.UsageAfterDelete;
                    xlWorksheet.Cells[rowNumber, 20].Value = apexTrigger.UsageAfterUndelete;
                    xlWorksheet.Cells[rowNumber, 21].Value = apexTrigger.UsageIsBulk;
                    xlWorksheet.Cells[rowNumber, 22].Value = apexTrigger.CreatedById;

                    if (apexTrigger.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 23].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 23].Value = apexTrigger.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 24].Value = apexTrigger.LastModifiedById;

                    if (apexTrigger.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 25].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 25].Value = apexTrigger.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 26].Value = apexTrigger.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 27].Value = apexTrigger.LastModifiedDate;

                    if (retrieveAggregateCoverage
                        && (apexTrigger.NamespacePrefix == null || apexTrigger.NamespacePrefix == ""))
                    {
                        SalesforceMetadata.ToolingWSDL.QueryResult toolingAggregateCoverageQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
                        toolingAggregateCoverageQr = sc.fromOrgToolingSvc.query(ApexCodeCoverageAggregateQuery(apexTrigger.Id));

                        if (toolingAggregateCoverageQr.records != null)
                        {
                            SalesforceMetadata.ToolingWSDL.sObject[] toolingQrRecords = toolingAggregateCoverageQr.records;
                            foreach (SalesforceMetadata.ToolingWSDL.sObject toolingSobj in toolingQrRecords)
                            {
                                ApexCodeCoverageAggregate accAggregate = (ApexCodeCoverageAggregate)toolingSobj;
                                Double denominator = ((Double)accAggregate.NumLinesCovered + (Double)accAggregate.NumLinesUncovered);

                                xlWorksheet.Cells[rowNumber, 9].Value = denominator;
                                xlWorksheet.Cells[rowNumber, 10].Value = accAggregate.NumLinesCovered;
                                xlWorksheet.Cells[rowNumber, 11].Value = accAggregate.NumLinesUncovered;

                                if (denominator != 0)
                                {
                                    Double percentCovered = (Double)accAggregate.NumLinesCovered / ((Double)accAggregate.NumLinesCovered + (Double)accAggregate.NumLinesUncovered);
                                    percentCovered = percentCovered * 100;
                                    xlWorksheet.Cells[rowNumber, 12].Value = percentCovered;
                                }
                            }

                            // Get Test Classes and Test Methods
                            apexTestClassesAndMethods = new SalesforceMetadata.ToolingWSDL.QueryResult();
                            apexTestClassesAndMethods = sc.fromOrgToolingSvc.query(ApexCodeCoverageQuery(apexTrigger.Id));

                            SalesforceMetadata.ToolingWSDL.sObject[] apexTestClassesAndMethodsRecords = apexTestClassesAndMethods.records;

                            String testClasses = "";
                            String apexTestClassAndMethod = "";

                            if (apexTestClassesAndMethodsRecords != null)
                            {
                                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingClass in apexTestClassesAndMethodsRecords)
                                {
                                    ApexCodeCoverage acm = (ApexCodeCoverage)toolingClass;

                                    if (classIdToClassName.Count > 0)
                                    {
                                        if (!testClasses.Contains(classIdToClassName[acm.ApexTestClassId]))
                                        {
                                            testClasses = testClasses + classIdToClassName[acm.ApexTestClassId] + "; ";
                                        }

                                        apexTestClassAndMethod = apexTestClassAndMethod + classIdToClassName[acm.ApexTestClassId] + " - " + acm.TestMethodName + "; ";
                                    }
                                }

                                xlWorksheet.Cells[rowNumber, 28].Value = testClasses;
                                xlWorksheet.Cells[rowNumber, 29].Value = apexTestClassAndMethod;
                            }
                        }
                    }

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void auraDefinitionBundleToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                      SalesforceCredentials sc,
                                                      String queryString, 
                                                      UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "AuraDefinitionBundles";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "ApiVersion";
            xlWorksheet.Cells[1, 5].Value = "CreatedById";
            xlWorksheet.Cells[1, 6].Value = "CreatedByName";
            xlWorksheet.Cells[1, 7].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 9].Value = "CreatedDate";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.AuraDefinitionBundle1 auraDefnBundle = (SalesforceMetadata.ToolingWSDL.AuraDefinitionBundle1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = auraDefnBundle.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = auraDefnBundle.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 3].Value = auraDefnBundle.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 4].Value = auraDefnBundle.ApiVersion;
                    xlWorksheet.Cells[rowNumber, 5].Value = auraDefnBundle.CreatedById;

                    if (auraDefnBundle.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = auraDefnBundle.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 7].Value = auraDefnBundle.LastModifiedById;

                    if (auraDefnBundle.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = auraDefnBundle.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = auraDefnBundle.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 10].Value = auraDefnBundle.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void compactLayoutToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                SalesforceCredentials sc,
                                                String queryString, 
                                                UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "CompactLayouts";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "ManageableState";
            xlWorksheet.Cells[1, 4].Value = "MasterLabel";
            xlWorksheet.Cells[1, 5].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 6].Value = "SobjectType";
            xlWorksheet.Cells[1, 7].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 9].Value = "CreatedDate";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.CompactLayout1 cmpctLayout = (SalesforceMetadata.ToolingWSDL.CompactLayout1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = cmpctLayout.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = cmpctLayout.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 3].Value = cmpctLayout.ManageableState;
                    xlWorksheet.Cells[rowNumber, 4].Value = cmpctLayout.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 5].Value = cmpctLayout.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 6].Value = cmpctLayout.SobjectType;
                    xlWorksheet.Cells[rowNumber, 7].Value = cmpctLayout.LastModifiedById;

                    if (cmpctLayout.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = cmpctLayout.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = cmpctLayout.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 10].Value = cmpctLayout.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void customApplicationToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                    SalesforceCredentials sc,
                                                    String queryString, 
                                                    UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "CustomApplication";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Label";
            xlWorksheet.Cells[1, 3].Value = "DeveloperName";
            xlWorksheet.Cells[1, 4].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 5].Value = "NavType";
            xlWorksheet.Cells[1, 6].Value = "CreatedById";
            xlWorksheet.Cells[1, 7].Value = "CreatedByName";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 9].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 10].Value = "CreatedDate";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.CustomApplication1 custAppl = (SalesforceMetadata.ToolingWSDL.CustomApplication1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = custAppl.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = custAppl.Label;
                    xlWorksheet.Cells[rowNumber, 3].Value = custAppl.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 4].Value = custAppl.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 5].Value = custAppl.NavType;
                    xlWorksheet.Cells[rowNumber, 6].Value = custAppl.CreatedById;

                    if (custAppl.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = custAppl.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 8].Value = custAppl.LastModifiedById;

                    if (custAppl.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = custAppl.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 10].Value = custAppl.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 11].Value = custAppl.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void fieldSetToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                           SalesforceCredentials sc,
                                           String queryString, 
                                           UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "FieldSet";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "MasterLabel";
            xlWorksheet.Cells[1, 3].Value = "DeveloperName";
            xlWorksheet.Cells[1, 4].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 5].Value = "CreatedById";
            xlWorksheet.Cells[1, 6].Value = "CreatedByName";
            xlWorksheet.Cells[1, 7].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 9].Value = "CreatedDate";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.FieldSet1 fieldSet = (SalesforceMetadata.ToolingWSDL.FieldSet1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = fieldSet.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = fieldSet.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 3].Value = fieldSet.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 4].Value = fieldSet.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 5].Value = fieldSet.CreatedById;

                    if (fieldSet.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = fieldSet.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 7].Value = fieldSet.LastModifiedById;

                    if (fieldSet.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = fieldSet.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = fieldSet.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 10].Value = fieldSet.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void customFieldToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                              SalesforceCredentials sc,
                                              String queryString, 
                                              UtilityClass.REQUESTINGORG reqOrg, 
                                              Dictionary<String, String> customObjIdToName, 
                                              Dictionary<String, List<String>> objectFieldNameToLabel)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;


            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "CustomFields";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "CustomObjectName";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "DeveloperName";
            xlWorksheet.Cells[1, 5].Value = "Label";
            xlWorksheet.Cells[1, 6].Value = "FieldApiName";
            xlWorksheet.Cells[1, 7].Value = "Type";
            xlWorksheet.Cells[1, 8].Value = "RelationshipLabel";
            xlWorksheet.Cells[1, 9].Value = "ManageableState";
            xlWorksheet.Cells[1, 10].Value = "CreatedById";
            xlWorksheet.Cells[1, 11].Value = "CreatedByName";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 13].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 14].Value = "CreatedDate";
            xlWorksheet.Cells[1, 15].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                toolingRecords = toolingQr.records;

                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    String customObjName = "";
                    String objectFieldNameToLabelKey = "";

                    SalesforceMetadata.ToolingWSDL.CustomField1 customFld = (SalesforceMetadata.ToolingWSDL.CustomField1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = customFld.Id;

                    if (customFld.TableEnumOrId != null && customObjIdToName.ContainsKey(customFld.TableEnumOrId))
                    {
                        customObjName = customObjIdToName[customFld.TableEnumOrId];

                        if (customFld.NamespacePrefix == null)
                        {
                            xlWorksheet.Cells[rowNumber, 2].Value = customObjName;
                            objectFieldNameToLabelKey = customObjName + "." + customFld.DeveloperName;
                        }
                        else
                        {
                            xlWorksheet.Cells[rowNumber, 2].Value = customObjName;
                            objectFieldNameToLabelKey = customObjName + "." + customFld.NamespacePrefix + "__" + customFld.DeveloperName;
                        }
                    }
                    //else if (customFld.TableEnumOrId != null && customObjIdToName15.ContainsKey(customFld.TableEnumOrId))
                    //{
                    //    customObjName = customObjIdToName15[customFld.TableEnumOrId];

                    //    if (customFld.NamespacePrefix == null)
                    //    {
                    //        xlWorksheet.Cells[rowNumber, 2].Value = customObjName;
                    //        objectFieldNameToLabelKey = customObjName + "." + customFld.DeveloperName;
                    //    }
                    //    else
                    //    {
                    //        xlWorksheet.Cells[rowNumber, 2].Value = customObjName;
                    //        objectFieldNameToLabelKey = customObjName + "." + customFld.NamespacePrefix + "__" + customFld.DeveloperName;
                    //    }
                    //}
                    else
                    {
                        customObjName = customFld.TableEnumOrId;

                        if (customFld.NamespacePrefix == null)
                        {
                            xlWorksheet.Cells[rowNumber, 2].Value = customObjName;
                            objectFieldNameToLabelKey = customObjName + "." + customFld.DeveloperName;
                        }
                        else
                        {
                            xlWorksheet.Cells[rowNumber, 2].Value = customObjName;
                            objectFieldNameToLabelKey = customObjName + "." + customFld.NamespacePrefix + "__" + customFld.DeveloperName;
                        }
                    }

                    xlWorksheet.Cells[rowNumber, 3].Value = customFld.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 4].Value = customFld.DeveloperName;

                    if (objectFieldNameToLabel.ContainsKey(objectFieldNameToLabelKey))
                    {
                        xlWorksheet.Cells[rowNumber, 5].Value = objectFieldNameToLabel[objectFieldNameToLabelKey][0];
                        xlWorksheet.Cells[rowNumber, 6].Value = objectFieldNameToLabel[objectFieldNameToLabelKey][1];
                        xlWorksheet.Cells[rowNumber, 7].Value = objectFieldNameToLabel[objectFieldNameToLabelKey][2];
                    }

                    if (customFld.RelationshipLabel == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = customFld.RelationshipLabel;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = customFld.ManageableState.ToString();
                    xlWorksheet.Cells[rowNumber, 10].Value = customFld.CreatedById;

                    if (customFld.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = customFld.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 12].Value = customFld.LastModifiedById;

                    if (customFld.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 13].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 13].Value = customFld.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 14].Value = customFld.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 15].Value = customFld.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void customObjectToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                               SalesforceCredentials sc,
                                               String queryString,
                                               UtilityClass.REQUESTINGORG reqOrg,
                                               Dictionary<String, String> customObjIdToName)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "CustomObjects";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "ExternalName";
            xlWorksheet.Cells[1, 5].Value = "ExternalRepository";
            xlWorksheet.Cells[1, 6].Value = "ManageableState";
            xlWorksheet.Cells[1, 7].Value = "SharingModel";
            xlWorksheet.Cells[1, 8].Value = "CreatedById";
            xlWorksheet.Cells[1, 9].Value = "CreatedByName";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 12].Value = "CreatedDate";
            xlWorksheet.Cells[1, 13].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.CustomObject1 customObj = (SalesforceMetadata.ToolingWSDL.CustomObject1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = customObj.Id;

                    if (customObj.NamespacePrefix == null)
                    {
                        xlWorksheet.Cells[rowNumber, 2].Value = customObj.DeveloperName;
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 2].Value = customObj.NamespacePrefix + "__" + customObj.DeveloperName;
                    }

                    xlWorksheet.Cells[rowNumber, 3].Value = customObj.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 4].Value = customObj.ExternalName;
                    xlWorksheet.Cells[rowNumber, 5].Value = customObj.ExternalRepository;
                    xlWorksheet.Cells[rowNumber, 6].Value = customObj.ManageableState.ToString();
                    xlWorksheet.Cells[rowNumber, 7].Value = customObj.SharingModel;

                    xlWorksheet.Cells[rowNumber, 8].Value = customObj.CreatedById;
                    if (customObj.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = customObj.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 10].Value = customObj.LastModifiedById;

                    if (customObj.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber,11].Value = customObj.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 12].Value = customObj.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 13].Value = customObj.LastModifiedDate;

                    if (customObjIdToName.ContainsKey(customObj.Id) == false)
                    {
                        if (customObj.NamespacePrefix == null)
                        {
                            customObjIdToName.Add(customObj.Id, customObj.DeveloperName);
                            customObjIdToName.Add(customObj.Id.Substring(0, customObj.Id.Length - 3), customObj.DeveloperName);
                        }
                        else
                        {
                            customObjIdToName.Add(customObj.Id, customObj.NamespacePrefix + "__" + customObj.DeveloperName);
                            customObjIdToName.Add(customObj.Id.Substring(0, customObj.Id.Length - 3), customObj.NamespacePrefix + "__" + customObj.DeveloperName);
                        }
                    }

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void customTabToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                            SalesforceCredentials sc,
                                            String queryString,
                                            UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;



            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "CustomTabs";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "MasterLabel";
            xlWorksheet.Cells[1, 5].Value = "MotifName";
            xlWorksheet.Cells[1, 6].Value = "Type";
            xlWorksheet.Cells[1, 7].Value = "CreatedById";
            xlWorksheet.Cells[1, 8].Value = "CreatedByName";
            xlWorksheet.Cells[1, 9].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 11].Value = "CreatedDate";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                toolingRecords = toolingQr.records;

                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.CustomTab1 customTab = (SalesforceMetadata.ToolingWSDL.CustomTab1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = customTab.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = customTab.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 3].Value = customTab.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 4].Value = customTab.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 5].Value = customTab.MotifName;
                    xlWorksheet.Cells[rowNumber, 6].Value = customTab.Type;
                    xlWorksheet.Cells[rowNumber, 7].Value = customTab.CreatedById;

                    if (customTab.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = customTab.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = customTab.LastModifiedById;

                    if (customTab.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = customTab.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = customTab.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 12].Value = customTab.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void emailTemplateToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                SalesforceCredentials sc,
                                                String queryString, 
                                                UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "EmailTemplates";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "Subject";
            xlWorksheet.Cells[1, 4].Value = "ApiVersion";
            xlWorksheet.Cells[1, 5].Value = "CreatedById";
            xlWorksheet.Cells[1, 6].Value = "CreatedByName";
            xlWorksheet.Cells[1, 7].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 9].Value = "CreatedDate";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.EmailTemplate1 emailTemplate = (SalesforceMetadata.ToolingWSDL.EmailTemplate1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = emailTemplate.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = emailTemplate.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = emailTemplate.Subject;
                    //xlWorksheet.Cells[rowNumber, 4].Value = emailTemplate.ApiVersion;
                    xlWorksheet.Cells[rowNumber, 5].Value = emailTemplate.CreatedById;

                    if (emailTemplate.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = emailTemplate.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 7].Value = emailTemplate.LastModifiedById;

                    if (emailTemplate.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = emailTemplate.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = emailTemplate.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 10].Value = emailTemplate.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void flexiPageToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                            SalesforceCredentials sc,
                                            String queryString, 
                                            UtilityClass.REQUESTINGORG reqOrg,
                                            Dictionary<String, String> customObjIdToName)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = sc.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Flexipages";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "EntityDefinitionId";
            xlWorksheet.Cells[1, 4].Value = "ManageableState";
            xlWorksheet.Cells[1, 5].Value = "MasterLabel";
            xlWorksheet.Cells[1, 6].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 7].Value = "ParentFlexiPage";
            xlWorksheet.Cells[1, 8].Value = "Type";
            xlWorksheet.Cells[1, 9].Value = "CreatedDate";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.FlexiPage1 flexiPage = (SalesforceMetadata.ToolingWSDL.FlexiPage1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = flexiPage.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = flexiPage.DeveloperName;

                    if (flexiPage.EntityDefinitionId != null && customObjIdToName.ContainsKey(flexiPage.EntityDefinitionId))
                    {
                        xlWorksheet.Cells[rowNumber, 3].Value = customObjIdToName[flexiPage.EntityDefinitionId];
                    }
                    //else if (flexiPage.EntityDefinitionId != null && customObjIdToName15.ContainsKey(flexiPage.EntityDefinitionId))
                    //{
                    //    xlWorksheet.Cells[rowNumber, 3].Value = customObjIdToName15[flexiPage.EntityDefinitionId];
                    //}
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 3].Value = flexiPage.EntityDefinitionId;
                    }

                    xlWorksheet.Cells[rowNumber, 4].Value = flexiPage.ManageableState;
                    xlWorksheet.Cells[rowNumber, 5].Value = flexiPage.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 6].Value = flexiPage.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 7].Value = flexiPage.ParentFlexiPage;
                    xlWorksheet.Cells[rowNumber, 8].Value = flexiPage.Type;
                    xlWorksheet.Cells[rowNumber, 9].Value = flexiPage.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 10].Value = flexiPage.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void flowToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                       SalesforceCredentials sc,
                                       String queryString, 
                                       UtilityClass.REQUESTINGORG reqOrg)
        {
            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Flows";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "MasterLabel";
            xlWorksheet.Cells[1, 3].Value = "DefinitionId";
            xlWorksheet.Cells[1, 4].Value = "VersionNumber";
            xlWorksheet.Cells[1, 5].Value = "ApiVersion";
            xlWorksheet.Cells[1, 6].Value = "ProcessType";
            xlWorksheet.Cells[1, 7].Value = "RunInMode";
            xlWorksheet.Cells[1, 8].Value = "Status";
            xlWorksheet.Cells[1, 9].Value = "IsTemplate";
            xlWorksheet.Cells[1, 10].Value = "ManageableState";
            xlWorksheet.Cells[1, 11].Value = "CreatedById";
            xlWorksheet.Cells[1, 12].Value = "CreatedByName";
            xlWorksheet.Cells[1, 13].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 14].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 15].Value = "CreatedDate";
            xlWorksheet.Cells[1, 16].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.Flow1 flow = (SalesforceMetadata.ToolingWSDL.Flow1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = flow.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = flow.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 3].Value = flow.DefinitionId;
                    xlWorksheet.Cells[rowNumber, 4].Value = flow.VersionNumber;

                    if (flow.ApiVersionSpecified == true)
                    {
                        xlWorksheet.Cells[rowNumber, 5].Value = flow.ApiVersion;
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 5].Value = "";
                    }

                    xlWorksheet.Cells[rowNumber, 6].Value = flow.ProcessType;

                    if (flow.RunInMode == null)
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = flow.RunInMode;
                    }

                    xlWorksheet.Cells[rowNumber, 8].Value = flow.Status;
                    xlWorksheet.Cells[rowNumber, 9].Value = flow.IsTemplate;
                    xlWorksheet.Cells[rowNumber, 10].Value = flow.ManageableState;
                    xlWorksheet.Cells[rowNumber, 11].Value = flow.CreatedById;

                    if (flow.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = flow.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 13].Value = flow.LastModifiedById;

                    if (flow.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 14].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 14].Value = flow.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 15].Value = flow.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 16].Value = flow.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void globalValueSetToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                 SalesforceCredentials sc,
                                                 String queryString, 
                                                 UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "GlobalValueSet";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "MasterLabel";
            xlWorksheet.Cells[1, 3].Value = "CreatedById";
            xlWorksheet.Cells[1, 4].Value = "CreatedByName";
            xlWorksheet.Cells[1, 5].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 6].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 7].Value = "CreatedDate";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.GlobalValueSet1 globalValSet = (SalesforceMetadata.ToolingWSDL.GlobalValueSet1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = globalValSet.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = globalValSet.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 3].Value = globalValSet.CreatedById;

                    if (globalValSet.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = globalValSet.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 5].Value = globalValSet.LastModifiedById;

                    if (globalValSet.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = globalValSet.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 7].Value = globalValSet.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 8].Value = globalValSet.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }

        }

        public static void groupToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                        SalesforceCredentials sc,
                                        String queryString, 
                                        UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Group";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "DeveloperName";
            xlWorksheet.Cells[1, 4].Value = "Type";
            xlWorksheet.Cells[1, 5].Value = "DoesIncludeBosses";
            xlWorksheet.Cells[1, 6].Value = "OwnerId";
            xlWorksheet.Cells[1, 7].Value = "RelatedId";
            xlWorksheet.Cells[1, 8].Value = "CreatedById";
            xlWorksheet.Cells[1, 9].Value = "CreatedByName";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 12].Value = "CreatedDate";
            xlWorksheet.Cells[1, 13].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.Group1 groupRes = (SalesforceMetadata.ToolingWSDL.Group1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = groupRes.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = groupRes.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = groupRes.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 4].Value = groupRes.Type;
                    xlWorksheet.Cells[rowNumber, 5].Value = groupRes.DoesIncludeBosses;
                    xlWorksheet.Cells[rowNumber, 6].Value = groupRes.OwnerId;
                    xlWorksheet.Cells[rowNumber, 7].Value = groupRes.RelatedId;
                    xlWorksheet.Cells[rowNumber, 8].Value = groupRes.CreatedById;

                    if (groupRes.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = groupRes.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 10].Value = groupRes.LastModifiedById;

                    if (groupRes.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = groupRes.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 12].Value = groupRes.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 13].Value = groupRes.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void layoutToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                         SalesforceCredentials sc,
                                         String queryString, 
                                         UtilityClass.REQUESTINGORG reqOrg,
                                         Dictionary<String, String> customObjIdToName)
        {
            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Layouts";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "EntityDefinitionId";
            xlWorksheet.Cells[1, 3].Value = "LayoutType";
            xlWorksheet.Cells[1, 4].Value = "ManageableState";
            xlWorksheet.Cells[1, 5].Value = "Name";
            xlWorksheet.Cells[1, 6].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 7].Value = "ShowSubmitAndAttachButton";
            xlWorksheet.Cells[1, 8].Value = "TableEnumOrId";
            xlWorksheet.Cells[1, 9].Value = "CreatedById";
            xlWorksheet.Cells[1, 10].Value = "CreatedByName";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 13].Value = "CreatedDate";
            xlWorksheet.Cells[1, 14].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.Layout1 lyOut = (SalesforceMetadata.ToolingWSDL.Layout1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = lyOut.Id;

                    //xlWorksheet.Cells[rowNumber, 2].Value = lyOut.EntityDefinitionId;
                    if (lyOut.EntityDefinitionId != null && customObjIdToName.ContainsKey(lyOut.EntityDefinitionId))
                    {
                        xlWorksheet.Cells[rowNumber, 2].Value = customObjIdToName[lyOut.EntityDefinitionId];
                    }
                    //else if (lyOut.EntityDefinitionId != null && customObjIdToName15.ContainsKey(lyOut.EntityDefinitionId))
                    //{
                    //    xlWorksheet.Cells[rowNumber, 2].Value = customObjIdToName15[lyOut.EntityDefinitionId];
                    //}
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 2].Value = lyOut.EntityDefinitionId;
                    }

                    xlWorksheet.Cells[rowNumber, 3].Value = lyOut.LayoutType;
                    xlWorksheet.Cells[rowNumber, 4].Value = lyOut.ManageableState;
                    xlWorksheet.Cells[rowNumber, 5].Value = lyOut.Name;
                    xlWorksheet.Cells[rowNumber, 6].Value = lyOut.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 7].Value = lyOut.ShowSubmitAndAttachButton;
                    xlWorksheet.Cells[rowNumber, 8].Value = lyOut.TableEnumOrId;
                    xlWorksheet.Cells[rowNumber, 9].Value = lyOut.CreatedById;

                    if (lyOut.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = lyOut.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = lyOut.LastModifiedById;

                    if (lyOut.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 17].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 12].Value = lyOut.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 13].Value = lyOut.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 14].Value = lyOut.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }


        public static void lwcToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                      SalesforceCredentials sc,
                                      String queryString,
                                      UtilityClass.REQUESTINGORG reqOrg)
        {
            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "LightningComponentBundle";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "ApiVersion";
            xlWorksheet.Cells[1, 4].Value = "IsExposed";
            xlWorksheet.Cells[1, 5].Value = "TargetConfigs";
            xlWorksheet.Cells[1, 6].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 7].Value = "CreatedById";
            xlWorksheet.Cells[1, 8].Value = "CreatedByName";
            xlWorksheet.Cells[1, 9].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 11].Value = "CreatedDate";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.LightningComponentBundle1 lwc = (SalesforceMetadata.ToolingWSDL.LightningComponentBundle1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = lwc.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = lwc.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 3].Value = lwc.ApiVersion;
                    xlWorksheet.Cells[rowNumber, 4].Value = lwc.IsExposed;
                    xlWorksheet.Cells[rowNumber, 5].Value = lwc.TargetConfigs;
                    xlWorksheet.Cells[rowNumber, 6].Value = lwc.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 7].Value = lwc.CreatedById;

                    if (lwc.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = lwc.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = lwc.LastModifiedById;

                    if (lwc.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = lwc.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = lwc.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 12].Value = lwc.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }


        public static void permissionSetToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                SalesforceCredentials sc,
                                                String queryString, 
                                                UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "PermissionSet";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Label";
            xlWorksheet.Cells[1, 3].Value = "Name";
            xlWorksheet.Cells[1, 4].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 5].Value = "PermissionSetGroupId";
            xlWorksheet.Cells[1, 6].Value = "ProfileId";
            xlWorksheet.Cells[1, 7].Value = "Type";
            xlWorksheet.Cells[1, 8].Value = "CreatedById";
            xlWorksheet.Cells[1, 9].Value = "CreatedByName";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 12].Value = "CreatedDate";
            xlWorksheet.Cells[1, 13].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.PermissionSet1 permSet = (SalesforceMetadata.ToolingWSDL.PermissionSet1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = permSet.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = permSet.Label;
                    xlWorksheet.Cells[rowNumber, 3].Value = permSet.Name;
                    xlWorksheet.Cells[rowNumber, 4].Value = permSet.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 5].Value = permSet.PermissionSetGroupId;
                    xlWorksheet.Cells[rowNumber, 6].Value = permSet.ProfileId;
                    xlWorksheet.Cells[rowNumber, 7].Value = permSet.Type;
                    xlWorksheet.Cells[rowNumber, 8].Value = permSet.CreatedById;

                    if (permSet.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = permSet.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 10].Value = permSet.LastModifiedById;

                    if (permSet.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = permSet.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 12].Value = permSet.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 13].Value = permSet.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void profileToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                          SalesforceCredentials sc,
                                          String queryString,
                                          UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Profile";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "CreatedById";
            xlWorksheet.Cells[1, 4].Value = "CreatedByName";
            xlWorksheet.Cells[1, 5].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 6].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 7].Value = "CreatedDate";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.Profile1 prof = (SalesforceMetadata.ToolingWSDL.Profile1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = prof.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = prof.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = prof.CreatedById;

                    if (prof.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = prof.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 5].Value = prof.LastModifiedById;

                    if (prof.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = prof.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 7].Value = prof.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 8].Value = prof.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void quickActionDefinitionToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                        SalesforceCredentials sc,
                                                        String queryString, 
                                                        UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "QuickActions";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Label";
            xlWorksheet.Cells[1, 3].Value = "MasterLabel";
            xlWorksheet.Cells[1, 4].Value = "DeveloperName";
            xlWorksheet.Cells[1, 5].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 6].Value = "ManageableState";
            xlWorksheet.Cells[1, 7].Value = "StandardLabel";
            xlWorksheet.Cells[1, 8].Value = "Type";
            xlWorksheet.Cells[1, 9].Value = "Height";
            xlWorksheet.Cells[1, 10].Value = "Width";
            xlWorksheet.Cells[1, 11].Value = "IconId";
            xlWorksheet.Cells[1, 12].Value = "Language";
            xlWorksheet.Cells[1, 13].Value = "OptionsCreateFeedItem";
            xlWorksheet.Cells[1, 14].Value = "SobjectType";
            xlWorksheet.Cells[1, 15].Value = "TargetSobjectType";
            xlWorksheet.Cells[1, 16].Value = "TargetField";
            xlWorksheet.Cells[1, 17].Value = "TargetRecordTypeId";
            xlWorksheet.Cells[1, 18].Value = "SuccessMessage";
            xlWorksheet.Cells[1, 19].Value = "Description";
            xlWorksheet.Cells[1, 20].Value = "CreatedById";
            xlWorksheet.Cells[1, 21].Value = "CreatedByName";
            xlWorksheet.Cells[1, 22].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 23].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 24].Value = "CreatedDate";
            xlWorksheet.Cells[1, 25].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.QuickActionDefinition quickAction = (SalesforceMetadata.ToolingWSDL.QuickActionDefinition)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = quickAction.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = quickAction.Label;
                    xlWorksheet.Cells[rowNumber, 3].Value = quickAction.MasterLabel;
                    xlWorksheet.Cells[rowNumber, 4].Value = quickAction.DeveloperName;
                    xlWorksheet.Cells[rowNumber, 5].Value = quickAction.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 6].Value = quickAction.ManageableState;
                    xlWorksheet.Cells[rowNumber, 7].Value = quickAction.StandardLabel;

                    xlWorksheet.Cells[rowNumber, 8].Value = quickAction.Type;
                    if (quickAction.Height == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = quickAction.Height;
                    }
                    if (quickAction.Height == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = quickAction.Width;
                    }
                    xlWorksheet.Cells[rowNumber, 11].Value = quickAction.IconId;
                    xlWorksheet.Cells[rowNumber, 12].Value = quickAction.Language;
                    xlWorksheet.Cells[rowNumber, 13].Value = quickAction.OptionsCreateFeedItem;
                    xlWorksheet.Cells[rowNumber, 14].Value = quickAction.SobjectType;
                    xlWorksheet.Cells[rowNumber, 15].Value = quickAction.TargetSobjectType;
                    xlWorksheet.Cells[rowNumber, 16].Value = quickAction.TargetField;
                    xlWorksheet.Cells[rowNumber, 17].Value = quickAction.TargetRecordTypeId;
                    xlWorksheet.Cells[rowNumber, 18].Value = quickAction.SuccessMessage;
                    xlWorksheet.Cells[rowNumber, 19].Value = quickAction.Description;

                    xlWorksheet.Cells[rowNumber, 20].Value = quickAction.CreatedById;

                    if (quickAction.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 21].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 21].Value = quickAction.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 22].Value = quickAction.LastModifiedById;

                    if (quickAction.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 23].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 23].Value = quickAction.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 24].Value = quickAction.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 35].Value = quickAction.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void recordTypesToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                              SalesforceCredentials sc,
                                              String queryString,
                                              UtilityClass.REQUESTINGORG reqOrg)
        {
            // Make a call to the Tooling API to retrieve the ApexClassMember passing in the ApexClass IDs
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);
            xlWorksheet.Name = "RecordTypes";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "SobjectType";
            xlWorksheet.Cells[1, 5].Value = "IsActive";
            xlWorksheet.Cells[1, 6].Value = "ManageableState";
            xlWorksheet.Cells[1, 7].Value = "CreatedById";
            xlWorksheet.Cells[1, 8].Value = "CreatedByName";
            xlWorksheet.Cells[1, 9].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 11].Value = "CreatedDate";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.RecordType1 rt = (SalesforceMetadata.ToolingWSDL.RecordType1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = rt.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = rt.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = rt.NamespacePrefix;
                    xlWorksheet.Cells[rowNumber, 4].Value = rt.SobjectType;
                    xlWorksheet.Cells[rowNumber, 5].Value = rt.IsActive.ToString();
                    xlWorksheet.Cells[rowNumber, 6].Value = rt.ManageableState;
                    xlWorksheet.Cells[rowNumber, 7].Value = rt.CreatedById;

                    if (rt.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = rt.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = rt.LastModifiedById;

                    if (rt.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = rt.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = rt.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 12].Value = rt.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }

            }
        }


        public static void tabDefinitionToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                SalesforceCredentials sc,
                                                String queryString, 
                                                UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "Tabs";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "Label";
            xlWorksheet.Cells[1, 4].Value = "IsAvailableInDesktop";
            xlWorksheet.Cells[1, 5].Value = "IsAvailableInLightning";
            xlWorksheet.Cells[1, 6].Value = "IsAvailableInMobile";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.TabDefinition tabDefn = (SalesforceMetadata.ToolingWSDL.TabDefinition)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = tabDefn.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = tabDefn.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = tabDefn.Label;
                    xlWorksheet.Cells[rowNumber, 4].Value = tabDefn.IsAvailableInDesktop;
                    xlWorksheet.Cells[rowNumber, 5].Value = tabDefn.IsAvailableInLightning;
                    xlWorksheet.Cells[rowNumber, 6].Value = tabDefn.IsAvailableInMobile;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void validationRuleToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                 SalesforceCredentials sc,
                                                 String queryString, 
                                                 UtilityClass.REQUESTINGORG reqOrg, 
                                                 Dictionary<String, String> customObjIdToName)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "ValidationRules";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "ValidationName";
            xlWorksheet.Cells[1, 3].Value = "Active";
            xlWorksheet.Cells[1, 4].Value = "EntityDefinitionId";
            xlWorksheet.Cells[1, 5].Value = "ErrorDisplayField";
            xlWorksheet.Cells[1, 6].Value = "CreatedById";
            xlWorksheet.Cells[1, 7].Value = "CreatedByName";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 9].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 10].Value = "CreatedDate";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedDate";
            xlWorksheet.Cells[1, 12].Value = "ErrorFormula";
            xlWorksheet.Cells[1, 13].Value = "ErrorDisplayField";
            xlWorksheet.Cells[1, 14].Value = "ErrorMessage";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ValidationRule1 validationRule = (SalesforceMetadata.ToolingWSDL.ValidationRule1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = validationRule.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = validationRule.ValidationName;
                    xlWorksheet.Cells[rowNumber, 3].Value = validationRule.Active;

                    if (validationRule.EntityDefinitionId != null && customObjIdToName.ContainsKey(validationRule.EntityDefinitionId))
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = customObjIdToName[validationRule.EntityDefinitionId];
                    }
                    //else if (validationRule.EntityDefinitionId != null && customObjIdToName15.ContainsKey(validationRule.EntityDefinitionId))
                    //{
                    //    xlWorksheet.Cells[rowNumber, 4].Value = customObjIdToName15[validationRule.EntityDefinitionId];
                    //}
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = validationRule.EntityDefinitionId;
                    }

                    xlWorksheet.Cells[rowNumber, 5].Value = validationRule.ErrorDisplayField;
                    xlWorksheet.Cells[rowNumber, 6].Value = validationRule.CreatedById;

                    if (validationRule.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 7].Value = validationRule.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 8].Value = validationRule.LastModifiedById;

                    if (validationRule.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = validationRule.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 10].Value = validationRule.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 11].Value = validationRule.LastModifiedDate;

                    // Get the Validation Rule metadata
                    String queryByName = ValidationRuleQuery(validationRule.ValidationName, validationRule.EntityDefinitionId);
                    SalesforceMetadata.ToolingWSDL.QueryResult toolingQrByName = new SalesforceMetadata.ToolingWSDL.QueryResult();
                    SalesforceMetadata.ToolingWSDL.sObject[] toolingRecordsByName;
                    toolingQrByName = sc.fromOrgToolingSvc.query(queryByName);

                    if (toolingQr.records == null) return;
                    toolingRecordsByName = toolingQrByName.records;

                    foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecordByName in toolingRecordsByName)
                    {
                        SalesforceMetadata.ToolingWSDL.ValidationRule1 validationRuleByName = (SalesforceMetadata.ToolingWSDL.ValidationRule1)toolingRecordByName;

                        SalesforceMetadata.ToolingWSDL.ValidationRule validationRuleMetadata = (SalesforceMetadata.ToolingWSDL.ValidationRule)validationRuleByName.Metadata;
                        xlWorksheet.Cells[rowNumber, 12].Value = validationRuleMetadata.errorConditionFormula;
                        xlWorksheet.Cells[rowNumber, 13].Value = validationRuleMetadata.errorDisplayField;
                        xlWorksheet.Cells[rowNumber, 14].Value = validationRuleMetadata.errorMessage;
                    }

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void workflowRuleToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                               SalesforceCredentials sc,
                                               String queryString, 
                                               UtilityClass.REQUESTINGORG reqOrg, 
                                               Dictionary<String, String> customObjIdToName)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "WorkflowRules";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "NamespacePrefix";
            xlWorksheet.Cells[1, 4].Value = "TableEnumOrId";
            xlWorksheet.Cells[1, 5].Value = "CreatedById";
            xlWorksheet.Cells[1, 6].Value = "CreatedByName";
            xlWorksheet.Cells[1, 7].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 8].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 9].Value = "CreatedDate";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.WorkflowRule1 wrkFlowRule = (SalesforceMetadata.ToolingWSDL.WorkflowRule1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = wrkFlowRule.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = wrkFlowRule.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = wrkFlowRule.NamespacePrefix;

                    if (wrkFlowRule.TableEnumOrId != null && customObjIdToName.ContainsKey(wrkFlowRule.TableEnumOrId))
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = customObjIdToName[wrkFlowRule.TableEnumOrId];
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 4].Value = wrkFlowRule.TableEnumOrId;
                    }
                    
                    xlWorksheet.Cells[rowNumber, 5].Value = wrkFlowRule.CreatedById;

                    if (wrkFlowRule.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 6].Value = wrkFlowRule.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 7].Value = wrkFlowRule.LastModifiedById;

                    if (wrkFlowRule.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = wrkFlowRule.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = wrkFlowRule.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 10].Value = wrkFlowRule.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void workflowAlertToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                SalesforceCredentials sc,
                                                String queryString, 
                                                UtilityClass.REQUESTINGORG reqOrg,
                                                Dictionary<String, WorkflowRule> workflowRules)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "WorkflowAlerts";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "DeveloperName";
            xlWorksheet.Cells[1, 3].Value = "WorkflowRuleNames";
            xlWorksheet.Cells[1, 4].Value = "SenderType";
            xlWorksheet.Cells[1, 5].Value = "CcEmails";
            xlWorksheet.Cells[1, 6].Value = "TemplateId";
            xlWorksheet.Cells[1, 7].Value = "CreatedById";
            xlWorksheet.Cells[1, 8].Value = "CreatedByName";
            xlWorksheet.Cells[1, 9].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 11].Value = "CreatedDate";
            xlWorksheet.Cells[1, 12].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.WorkflowAlert1 workflowAlert = (SalesforceMetadata.ToolingWSDL.WorkflowAlert1)toolingRecord;

                    xlWorksheet.Cells[rowNumber, 1].Value = workflowAlert.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = workflowAlert.DeveloperName;

                    if (workflowAlert.EntityDefinitionId != null && workflowRules.ContainsKey(workflowAlert.EntityDefinitionId))
                    {
                        xlWorksheet.Cells[rowNumber, 3].Value = workflowRules[workflowAlert.EntityDefinitionId];
                    }

                    xlWorksheet.Cells[rowNumber, 4].Value = workflowAlert.SenderType;
                    xlWorksheet.Cells[rowNumber, 5].Value = workflowAlert.CcEmails;
                    xlWorksheet.Cells[rowNumber, 6].Value = workflowAlert.TemplateId;
                    xlWorksheet.Cells[rowNumber, 7].Value = workflowAlert.CreatedById;

                    if (workflowAlert.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = workflowAlert.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = workflowAlert.LastModifiedById;

                    if (workflowAlert.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = workflowAlert.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = workflowAlert.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 12].Value = workflowAlert.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public static void workflowFieldUpdateToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                      SalesforceCredentials sc,
                                                      String queryString, 
                                                      UtilityClass.REQUESTINGORG reqOrg, 
                                                      Dictionary<String, String> customObjIdToName,
                                                      Dictionary<String, WorkflowFieldUpdate> workflowFieldUpdatesByName)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                                                                    (System.Reflection.Missing.Value,
                                                                        xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                                                                        System.Reflection.Missing.Value,
                                                                        System.Reflection.Missing.Value);

            xlWorksheet.Name = "WorkflowFieldUpdates";
            xlWorksheet.Cells[1, 1].Value = "Id";
            xlWorksheet.Cells[1, 2].Value = "Name";
            xlWorksheet.Cells[1, 3].Value = "WorkflowRuleNames";
            xlWorksheet.Cells[1, 4].Value = "WorkflowObjectName";
            xlWorksheet.Cells[1, 5].Value = "SourceTableEnumOrId";
            xlWorksheet.Cells[1, 6].Value = "Operation";
            xlWorksheet.Cells[1, 7].Value = "ReevaluateOnChange";
            xlWorksheet.Cells[1, 8].Value = "CreatedById";
            xlWorksheet.Cells[1, 9].Value = "CreatedByName";
            xlWorksheet.Cells[1, 10].Value = "LastModifiedById";
            xlWorksheet.Cells[1, 11].Value = "LastModifiedByName";
            xlWorksheet.Cells[1, 12].Value = "CreatedDate";
            xlWorksheet.Cells[1, 13].Value = "LastModifiedDate";

            Int32 rowNumber = 2;
            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.WorkflowFieldUpdate1 wfFieldUpdate = (SalesforceMetadata.ToolingWSDL.WorkflowFieldUpdate1)toolingRecord;

                    String workFlowObjectName = "";
                    String workFlowRuleNames = "";
                    String operation = "";
                    String reevaluateOnChange = "";
                    String sourceTable = "";

                    if (wfFieldUpdate.SourceTableEnumOrId != null && customObjIdToName.ContainsKey(wfFieldUpdate.SourceTableEnumOrId))
                    {
                        sourceTable = customObjIdToName[wfFieldUpdate.SourceTableEnumOrId];
                    }
                    //else if (wfFieldUpdate.SourceTableEnumOrId != null && customObjIdToName15.ContainsKey(wfFieldUpdate.SourceTableEnumOrId))
                    //{
                    //    sourceTable = customObjIdToName15[wfFieldUpdate.SourceTableEnumOrId];
                    //}
                    else
                    {
                        sourceTable = wfFieldUpdate.SourceTableEnumOrId;
                    }


                    if (wfFieldUpdate.EntityDefinitionId != null && customObjIdToName.ContainsKey(wfFieldUpdate.EntityDefinitionId))
                    {
                        workFlowObjectName = customObjIdToName[wfFieldUpdate.EntityDefinitionId];
                    }
                    //else if (wfFieldUpdate.EntityDefinitionId != null && customObjIdToName15.ContainsKey(wfFieldUpdate.EntityDefinitionId))
                    //{
                    //    workFlowObjectName = customObjIdToName15[wfFieldUpdate.EntityDefinitionId];
                    //}
                    else
                    {
                        workFlowObjectName = wfFieldUpdate.EntityDefinitionId;
                    }


                    foreach (String wrkFlowFieldUpdateName in workflowFieldUpdatesByName.Keys)
                    {
                        if (wrkFlowFieldUpdateName == sourceTable + "|" + wfFieldUpdate.Name)
                        {
                            //workFlowObjectName = workflowFieldUpdatesByName[wfFieldUpdate.EntityDefinitionId + "|" + wfFieldUpdate.Name].objectName;

                            operation = workflowFieldUpdatesByName[sourceTable + "|" + wfFieldUpdate.Name].operation;
                            reevaluateOnChange = workflowFieldUpdatesByName[sourceTable + "|" + wfFieldUpdate.Name].reevaluateOnChange;

                            foreach (String wrkFlowRule in workflowFieldUpdatesByName[sourceTable + "|" + wfFieldUpdate.Name].workflowRules)
                            {
                                workFlowRuleNames = workFlowRuleNames + wrkFlowRule + "; ";
                            }
                        }
                    }

                    if (workFlowRuleNames.Length > 2)
                    {
                        workFlowRuleNames = workFlowRuleNames.Substring(0, workFlowRuleNames.Length - 2);
                    }

                    xlWorksheet.Cells[rowNumber, 1].Value = wfFieldUpdate.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = wfFieldUpdate.Name;
                    xlWorksheet.Cells[rowNumber, 3].Value = workFlowRuleNames;
                    xlWorksheet.Cells[rowNumber, 4].Value = workFlowObjectName;
                    xlWorksheet.Cells[rowNumber, 5].Value = sourceTable;
                    xlWorksheet.Cells[rowNumber, 6].Value = operation;
                    xlWorksheet.Cells[rowNumber, 7].Value = reevaluateOnChange;
                    xlWorksheet.Cells[rowNumber, 8].Value = wfFieldUpdate.CreatedById;

                    if (wfFieldUpdate.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 9].Value = wfFieldUpdate.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 10].Value = wfFieldUpdate.LastModifiedById;

                    if (wfFieldUpdate.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 11].Value = wfFieldUpdate.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 12].Value = wfFieldUpdate.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 13].Value = wfFieldUpdate.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }


        public static void apexClassMemberToExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
                                                  SalesforceCredentials sc,
                                                  String queryString, 
                                                  UtilityClass.REQUESTINGORG reqOrg)
        {
            SalesforceMetadata.ToolingWSDL.QueryResult toolingQr = new SalesforceMetadata.ToolingWSDL.QueryResult();
            SalesforceMetadata.ToolingWSDL.sObject[] toolingRecords;

            if (reqOrg == UtilityClass.REQUESTINGORG.FROMORG)
            {
                toolingQr = sc.fromOrgToolingSvc.query(queryString);
            }
            else
            {
                //toolingQr = SalesforceCredentials.toOrgToolingSvc.query(queryString);
            }

            if (toolingQr.records == null) return;

            toolingRecords = toolingQr.records;

            Boolean worksheetExists = false;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = new Microsoft.Office.Interop.Excel.Worksheet();

            Int32 rowNumber = 2;

            foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in xlWorkbook.Worksheets)
            {
                if (sheet.Name == "ApexClassMembers")
                {
                    worksheetExists = true;
                    xlWorksheet = sheet;

                    // Get the number of rows
                    Int32 lastUsedRow = xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value,
                                                   System.Reflection.Missing.Value, 
                                                   System.Reflection.Missing.Value,
                                                   Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, 
                                                   Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                                   false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                    rowNumber = lastUsedRow++;

                    break;
                }
            }


            if (worksheetExists == false)
            {
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.Add
                              (System.Reflection.Missing.Value,
                               xlWorkbook.Worksheets[xlWorkbook.Worksheets.Count],
                               System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value);

                xlWorksheet.Name = "ApexClassMembers";
                xlWorksheet.Cells[1, 1].Value = "Id";
                xlWorksheet.Cells[1, 2].Value = "FullName";
                xlWorksheet.Cells[1, 3].Value = "Content";
                xlWorksheet.Cells[1, 4].Value = "ContentEntity";
                xlWorksheet.Cells[1, 5].Value = "ContentEntityId";
                xlWorksheet.Cells[1, 6].Value = "Metadata";
                xlWorksheet.Cells[1, 7].Value = "CreatedById";
                xlWorksheet.Cells[1, 8].Value = "CreatedByName";
                xlWorksheet.Cells[1, 9].Value = "LastModifiedById";
                xlWorksheet.Cells[1, 10].Value = "LastModifiedByName";
                xlWorksheet.Cells[1, 11].Value = "CreatedDate";
                xlWorksheet.Cells[1, 12].Value = "LastModifiedDate";
            }

            Boolean done = false;

            while (done == false)
            {
                foreach (SalesforceMetadata.ToolingWSDL.sObject toolingRecord in toolingRecords)
                {
                    SalesforceMetadata.ToolingWSDL.ApexClassMember apexClsMbr = (SalesforceMetadata.ToolingWSDL.ApexClassMember)toolingRecord;

                    SalesforceMetadata.ToolingWSDL.SymbolTable symbTbl = apexClsMbr.SymbolTable;

                    if (symbTbl != null)
                    {
                        SalesforceMetadata.ToolingWSDL.Constructor[] constrs = symbTbl.constructors;
                        SalesforceMetadata.ToolingWSDL.ExternalReference[] extRefs = symbTbl.externalReferences;
                        SalesforceMetadata.ToolingWSDL.SymbolTable[] innerCls = symbTbl.innerClasses;
                        String[] interfcs = symbTbl.interfaces;
                        SalesforceMetadata.ToolingWSDL.Method[] methds = symbTbl.methods;
                        String parentCls = symbTbl.parentClass;
                        SalesforceMetadata.ToolingWSDL.VisibilitySymbol[] proptys = symbTbl.properties;
                        SalesforceMetadata.ToolingWSDL.Symbol tblDecl = symbTbl.tableDeclaration;
                        SalesforceMetadata.ToolingWSDL.Symbol[] vars = symbTbl.variables;
                    }


                    xlWorksheet.Cells[rowNumber, 1].Value = apexClsMbr.Id;
                    xlWorksheet.Cells[rowNumber, 2].Value = apexClsMbr.FullName;
                    xlWorksheet.Cells[rowNumber, 3].Value = apexClsMbr.Content;
                    xlWorksheet.Cells[rowNumber, 4].Value = apexClsMbr.ContentEntity;
                    xlWorksheet.Cells[rowNumber, 5].Value = apexClsMbr.ContentEntityId;
                    xlWorksheet.Cells[rowNumber, 6].Value = apexClsMbr.Metadata;
                    xlWorksheet.Cells[rowNumber, 7].Value = apexClsMbr.CreatedById;

                    if (apexClsMbr.CreatedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 8].Value = apexClsMbr.CreatedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 9].Value = apexClsMbr.LastModifiedById;

                    if (apexClsMbr.LastModifiedBy == null)
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = "";
                    }
                    else
                    {
                        xlWorksheet.Cells[rowNumber, 10].Value = apexClsMbr.LastModifiedBy.Name;
                    }

                    xlWorksheet.Cells[rowNumber, 11].Value = apexClsMbr.CreatedDate;
                    xlWorksheet.Cells[rowNumber, 12].Value = apexClsMbr.LastModifiedDate;

                    rowNumber++;
                }

                if (toolingQr.done)
                {
                    done = true;
                }
                else
                {
                    toolingQr = sc.fromOrgToolingSvc.queryMore(toolingQr.queryLocator);
                }
            }
        }

        public class WorkflowRule
        {
            public String fullName;
            public String objectName;
            public String active;
            public String triggerType;
            public Dictionary<String, WorkflowAlert> wrkFlowAlerts;
            public Dictionary<String, WorkflowFieldUpdate> wrkFlowFieldupdates;
            public Dictionary<String, WorkflowOutboundMessage> wrkFlowOutboundMsgs;
            public Dictionary<String, WorkflowTask> wrkFlowTasks;

            public WorkflowRule()
            {
                wrkFlowAlerts = new Dictionary<String, WorkflowAlert>();
                wrkFlowFieldupdates = new Dictionary<String, WorkflowFieldUpdate>();
                wrkFlowOutboundMsgs = new Dictionary<String, WorkflowOutboundMessage>();
                wrkFlowTasks = new Dictionary<String, WorkflowTask>();
            }
        }

        public class WorkflowAlert
        {
            public String fullName;
            public String isProtected;
            public String senderType;
            public String template;
        }

        public class WorkflowFieldUpdate
        {
            public String fullName;
            public String name;
            public String objectName;
            public String field;
            public String formula;
            public String notifyAssignee;
            public String operation;
            public String isProtected;
            public String reevaluateOnChange;
            public List<String> workflowRules;
        }

        public class WorkflowOutboundMessage
        {
            public String fullName;
            public String apiVersion;
            public String endpointUrl;
            public String includeSessionId;
            public String integrationUser;
            public String isProtected;
            public String useDeadLetterQueue;
        }

        public class WorkflowTask
        {
            public String fullName;
            public String assignedTo;
            public String assignedToType;
            public String dueDateOffset;
            public String notifyAssignee;
            public String offsetFromField;
            public String priority;
            public String isProtected;
            public String status;
            public String subject;
        }
    }
}

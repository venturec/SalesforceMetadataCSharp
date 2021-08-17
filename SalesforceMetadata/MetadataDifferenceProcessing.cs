using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    /*  Notes: Many of the metadata fields return arrays of objects and can be nested in multiple layers
     *         This application uses the XML GetElementsByTagName which can return all nodes, regardless of parent, with that element name
     *         To process only the nodes related to the actual Parent node you are after (i.e. CustomObject Description vs. CustomObject fieldSets Description vs. CustomObject Fields Description, etc.
     *         I'm passing in the Parent Node to make sure I'm only processing the node values I want
     *         
     *         GetElementsByTagName("Description") will return all nodes within the XML document with the tag "Description" but if I'm looping through Fields, I want to only look at 
     *         nodes where the parent is Fields and not fieldSets
     *         
     */

    class MetadataDifferenceProcessing
    {
        /****************************************************************************************************************************************/
        public static List<MetadataFieldTypes> accountRelationshipShareRuleFieldNames()
        {
            List<MetadataFieldTypes> mdtFieldsAndTypes = new List<MetadataFieldTypes>();

            MetadataFieldTypes mft = new MetadataFieldTypes("accessLevel", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            mft = new MetadataFieldTypes("accountToCriteriaField", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            mft = new MetadataFieldTypes("description", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            mft = new MetadataFieldTypes("entityType", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            mft = new MetadataFieldTypes("masterLabel", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            mft = new MetadataFieldTypes("staticFormulaCriteria", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            mft = new MetadataFieldTypes("type", "String", false, false);
            mdtFieldsAndTypes.Add(mft);

            return mdtFieldsAndTypes;
        }

        public static List<String> actionLinkGroupTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionLinkTemplates");
            mdtFieldsAndTypes.Add("category");
            mdtFieldsAndTypes.Add("executionsAllowed");
            mdtFieldsAndTypes.Add("hoursUntilExpiration");
            mdtFieldsAndTypes.Add("isPublished");
            mdtFieldsAndTypes.Add("name");

            return mdtFieldsAndTypes;
        }

        public static List<String> actionLinkTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionUrl");
            mdtFieldsAndTypes.Add("headers");
            mdtFieldsAndTypes.Add("isConfirmationRequired");
            mdtFieldsAndTypes.Add("isGroupDefault");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("labelKey");
            mdtFieldsAndTypes.Add("linkType");
            mdtFieldsAndTypes.Add("method");
            mdtFieldsAndTypes.Add("position");
            mdtFieldsAndTypes.Add("requestBody");
            mdtFieldsAndTypes.Add("userAlias");
            mdtFieldsAndTypes.Add("userVisibility");

            return mdtFieldsAndTypes;
        }

        public static List<String> actionPlanTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionPlanTemplateItem");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("targetEntityType");
            mdtFieldsAndTypes.Add("uniqueName");

            return mdtFieldsAndTypes;
        }

        public static List<String> actionPlanTemplateItemFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionPlanTemplateItemValue");
            mdtFieldsAndTypes.Add("displayOrder");
            mdtFieldsAndTypes.Add("isRequired");
            mdtFieldsAndTypes.Add("itemEntityType");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("uniqueName");

            return mdtFieldsAndTypes;
        }

        public static List<String> actionPlanTemplateItemValueFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("itemEntityType");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("valueFormula");
            mdtFieldsAndTypes.Add("valueLiteral");

            return mdtFieldsAndTypes;
        }

        public static List<String> analyticSnapshotFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("groupColumn");
            mdtFieldsAndTypes.Add("mappings");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("runningUser");
            mdtFieldsAndTypes.Add("sourceReport");
            mdtFieldsAndTypes.Add("targetObject");

            return mdtFieldsAndTypes;
        }

        public static List<String> analyticSnapshotMappingFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("aggregateType");
            mdtFieldsAndTypes.Add("sourceField");
            mdtFieldsAndTypes.Add("sourceType");
            mdtFieldsAndTypes.Add("targetField");

            return mdtFieldsAndTypes;
        }

        public static List<String> animationRuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("animationFrequency");
            mdtFieldsAndTypes.Add("developerName");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("recordTypeContext");
            mdtFieldsAndTypes.Add("recordTypeName");
            mdtFieldsAndTypes.Add("sobjectType");
            mdtFieldsAndTypes.Add("targetField");
            mdtFieldsAndTypes.Add("targetFieldChangeToValues");

            return mdtFieldsAndTypes;
        }

        public static List<String> apexClassFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("apiVersion");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("packageVersions");
            mdtFieldsAndTypes.Add("status");

            return mdtFieldsAndTypes;
        }

        public static List<String> apexComponentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("apiVersion");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("packageVersions");

            return mdtFieldsAndTypes;
        }

        public static List<String> apexPageFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("apiVersion");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("availableInTouch");
            mdtFieldsAndTypes.Add("confirmationTokenRequired");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("packageVersions");

            return mdtFieldsAndTypes;
        }

        public static List<String> apexTriggerFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("apiVersion");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("packageVersions");
            mdtFieldsAndTypes.Add("status");

            return mdtFieldsAndTypes;
        }

        public static List<String> appMenuFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("appMenuItems");
            return mdtFieldsAndTypes;
        }

        public static List<String> appMenuItemFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("type");

            return mdtFieldsAndTypes;
        }

        public static List<String> appointmentSchedulingPolicyFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("appointmentStartTimeInterval");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("shouldConsiderCalendarEvents");
            mdtFieldsAndTypes.Add("shouldEnforceExcludedResource");
            mdtFieldsAndTypes.Add("shouldEnforceRequiredResource");
            mdtFieldsAndTypes.Add("shouldMatchSkill");
            mdtFieldsAndTypes.Add("shouldMatchSkillLevel");
            mdtFieldsAndTypes.Add("shouldRespectVisitingHours");
            mdtFieldsAndTypes.Add("shouldUsePrimaryMembers");
            mdtFieldsAndTypes.Add("shouldUseSecondaryMembers");
            return mdtFieldsAndTypes;
        }

        public static List<String> approvalProcessFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("allowRecall");
            mdtFieldsAndTypes.Add("allowedSubmitters");
            mdtFieldsAndTypes.Add("approvalPageFields");
            mdtFieldsAndTypes.Add("approvalStep");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("emailTemplate");
            mdtFieldsAndTypes.Add("entryCriteria");
            mdtFieldsAndTypes.Add("finalApprovalActions");
            mdtFieldsAndTypes.Add("finalApprovalRecordLock");
            mdtFieldsAndTypes.Add("finalRejectionActions");
            mdtFieldsAndTypes.Add("finalRejectionRecordLock");
            mdtFieldsAndTypes.Add("initialSubmissionActions");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("nextAutomatedApprover");
            mdtFieldsAndTypes.Add("postTemplate");
            mdtFieldsAndTypes.Add("recallActions");
            mdtFieldsAndTypes.Add("recordEditability");
            mdtFieldsAndTypes.Add("showApprovalHistory");

            return mdtFieldsAndTypes;
        }

        public static List<String> articleTypeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("articleTypeChannel​Display");
            mdtFieldsAndTypes.Add("deploymentStatus");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fields");
            mdtFieldsAndTypes.Add("gender");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("pluralLabel");
            mdtFieldsAndTypes.Add("startsWith");

            return mdtFieldsAndTypes;
        }

        public static List<String> assignmentRulesFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("assignmentRule");
            return mdtFieldsAndTypes;
        }

        public static List<String> audienceFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("audienceName");
            mdtFieldsAndTypes.Add("container");
            mdtFieldsAndTypes.Add("criteria");
            mdtFieldsAndTypes.Add("criterion");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("formula");
            mdtFieldsAndTypes.Add("formulaFilterType");
            mdtFieldsAndTypes.Add("isDefaultAudience");
            mdtFieldsAndTypes.Add("targets");
            return mdtFieldsAndTypes;
        }

        public static List<String> auraDefinitionBundleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("");

            return mdtFieldsAndTypes;
        }

        public static List<String> authProviderFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("authorizeUrl");
            mdtFieldsAndTypes.Add("consumerKey");
            mdtFieldsAndTypes.Add("consumerSecret");
            mdtFieldsAndTypes.Add("customMetadataTypeRecord");
            mdtFieldsAndTypes.Add("defaultScopes");
            mdtFieldsAndTypes.Add("errorUrl");
            mdtFieldsAndTypes.Add("executionUser");
            mdtFieldsAndTypes.Add("friendlyName");
            mdtFieldsAndTypes.Add("iconUrl");
            mdtFieldsAndTypes.Add("idTokenIssuer");
            mdtFieldsAndTypes.Add("includeOrgIdInIdentifier");
            mdtFieldsAndTypes.Add("LinkKickoffUrl");
            mdtFieldsAndTypes.Add("logoutUrl");
            mdtFieldsAndTypes.Add("oauthKickoffUrl");
            mdtFieldsAndTypes.Add("plugin");
            mdtFieldsAndTypes.Add("portal");
            mdtFieldsAndTypes.Add("providerType");
            mdtFieldsAndTypes.Add("registrationHandler");
            mdtFieldsAndTypes.Add("sendAccessTokenInHeader");
            mdtFieldsAndTypes.Add("sendClientCredentialsInHeader");
            mdtFieldsAndTypes.Add("sendSecretInApis");
            mdtFieldsAndTypes.Add("SsoKickoffUrl");
            mdtFieldsAndTypes.Add("tokenUrl");
            mdtFieldsAndTypes.Add("userInfoUrl");
            return mdtFieldsAndTypes;
        }

        public static List<String> autoResponseRulesFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("autoresponseRule");

            return mdtFieldsAndTypes;
        }

        public static List<String> blacklistedComsumersFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("blockedByApiWhitelisting");
            mdtFieldsAndTypes.Add("consumerKey");
            mdtFieldsAndTypes.Add("consumerName");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> botFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("botMlDomain");
            mdtFieldsAndTypes.Add("botUser");
            mdtFieldsAndTypes.Add("botVersions");
            mdtFieldsAndTypes.Add("contextVariables");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            return mdtFieldsAndTypes;
        }

        public static List<String> botVersionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("botDialogGroups");
            mdtFieldsAndTypes.Add("botDialogs");
            mdtFieldsAndTypes.Add("conversationVariables");
            mdtFieldsAndTypes.Add("entryDialog");
            mdtFieldsAndTypes.Add("mainMenuDialog");
            mdtFieldsAndTypes.Add("responseDelayMilliseconds");
            return mdtFieldsAndTypes;
        }

        public static List<String> brandingSetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("brandingSetProperty");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> callCenterFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("adapterUrl");
            mdtFieldsAndTypes.Add("displayName");
            mdtFieldsAndTypes.Add("displayNameLabel");
            mdtFieldsAndTypes.Add("internalNameLabel");
            mdtFieldsAndTypes.Add("version");
            mdtFieldsAndTypes.Add("sections");
            return mdtFieldsAndTypes;
        }

        public static List<String> campaignInfluenceModelFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("isDefaultModel");
            mdtFieldsAndTypes.Add("isModelLocked");
            mdtFieldsAndTypes.Add("modelDescription");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("recordPreference");
            return mdtFieldsAndTypes;
        }

        public static List<String> caseSubjectParticleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("index");
            mdtFieldsAndTypes.Add("textField");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> certificateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("caSigned");
            mdtFieldsAndTypes.Add("encryptedWithPlatformEncryption");
            mdtFieldsAndTypes.Add("expirationDate");
            mdtFieldsAndTypes.Add("keySize");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("privateKeyExportable");
            return mdtFieldsAndTypes;
        }

        public static List<String> chatterExtensionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("extensionName");
            mdtFieldsAndTypes.Add("compositionComponent");
            mdtFieldsAndTypes.Add("headerText");
            mdtFieldsAndTypes.Add("hoverText");
            mdtFieldsAndTypes.Add("icon");
            mdtFieldsAndTypes.Add("isProtected");
            mdtFieldsAndTypes.Add("renderComponent");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> cleanDataServiceFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("cleanRules");
            mdtFieldsAndTypes.Add("matchEngine");
            return mdtFieldsAndTypes;
        }

        public static List<String> CMSConnectSourceFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("cmsConnectAsset");
            mdtFieldsAndTypes.Add("cmsConnectLanguage");
            mdtFieldsAndTypes.Add("cmsConnectPersonalization");
            mdtFieldsAndTypes.Add("cmsConnectResourceType");
            mdtFieldsAndTypes.Add("connectionType");
            mdtFieldsAndTypes.Add("cssScope");
            mdtFieldsAndTypes.Add("developerName");
            mdtFieldsAndTypes.Add("languageEnabled");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("namedCredential");
            mdtFieldsAndTypes.Add("personalizationEnabled");
            mdtFieldsAndTypes.Add("rootPath");
            mdtFieldsAndTypes.Add("sortOrder");
            mdtFieldsAndTypes.Add("status");
            mdtFieldsAndTypes.Add("type");
            mdtFieldsAndTypes.Add("websiteUrl");
            return mdtFieldsAndTypes;
        }

        public static List<String> communityFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("chatterAnswersFacebookSsoUrl");
            mdtFieldsAndTypes.Add("communityFeedPage");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("emailFooterDocument");
            mdtFieldsAndTypes.Add("emailHeaderDocument");
            mdtFieldsAndTypes.Add("emailNotificationUrl");
            mdtFieldsAndTypes.Add("enableChatterAnswers");
            mdtFieldsAndTypes.Add("enablePrivateQuestions");
            mdtFieldsAndTypes.Add("expertsGroup");
            mdtFieldsAndTypes.Add("portal");
            mdtFieldsAndTypes.Add("portalEmailNotificationUrl");
            mdtFieldsAndTypes.Add("reputationLevels");
            mdtFieldsAndTypes.Add("showInPortal");
            mdtFieldsAndTypes.Add("site");
            return mdtFieldsAndTypes;
        }

        public static List<String> communityTemplateDefinitionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("chatterAnswersFacebookSsoUrl");
            mdtFieldsAndTypes.Add("communityFeedPage");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("emailFooterDocument");
            mdtFieldsAndTypes.Add("emailHeaderDocument");
            mdtFieldsAndTypes.Add("emailNotificationUrl");
            mdtFieldsAndTypes.Add("enableChatterAnswers");
            mdtFieldsAndTypes.Add("enablePrivateQuestions");
            mdtFieldsAndTypes.Add("expertsGroup");
            mdtFieldsAndTypes.Add("portal");
            mdtFieldsAndTypes.Add("portalEmailNotificationUrl");
            mdtFieldsAndTypes.Add("reputationLevels");
            mdtFieldsAndTypes.Add("showInPortal");
            mdtFieldsAndTypes.Add("site");
            return mdtFieldsAndTypes;
        }

        public static List<String> communityThemeDefinitionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("bundlesInfo");
            mdtFieldsAndTypes.Add("customThemeLayoutType");
            mdtFieldsAndTypes.Add("defaultBrandingSet");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("enableExtendedCleanUp");
            mdtFieldsAndTypes.Add("OnDelete");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("publisher");
            mdtFieldsAndTypes.Add("themeRouteOverride");
            mdtFieldsAndTypes.Add("themeSetting");
            return mdtFieldsAndTypes;
        }

        public static List<String> connectedAppFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("canvasConfig");
            mdtFieldsAndTypes.Add("contactEmail");
            mdtFieldsAndTypes.Add("contactPhone");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("profileName");
            mdtFieldsAndTypes.Add("attributes");
            mdtFieldsAndTypes.Add("startUrl");
            mdtFieldsAndTypes.Add("infoUrl");
            mdtFieldsAndTypes.Add("logoUrl");
            mdtFieldsAndTypes.Add("iconUrl");
            mdtFieldsAndTypes.Add("mobileStartUrl");
            mdtFieldsAndTypes.Add("ipRanges");
            mdtFieldsAndTypes.Add("oauthConfig");
            mdtFieldsAndTypes.Add("permissionSetName");
            mdtFieldsAndTypes.Add("plugin");
            mdtFieldsAndTypes.Add("pluginExecutionUser");
            mdtFieldsAndTypes.Add("samlConfig");
            return mdtFieldsAndTypes;
        }

        public static List<String> contentAssetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("format");
            mdtFieldsAndTypes.Add("isVisibleByExternalUsers");
            mdtFieldsAndTypes.Add("language");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("originNetwork");
            mdtFieldsAndTypes.Add("relationships");
            mdtFieldsAndTypes.Add("versions");
            return mdtFieldsAndTypes;
        }

        public static List<String> corsWhitelistOriginFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("urlPattern");

            return mdtFieldsAndTypes;
        }

        public static List<String> cspTrustedSiteFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("context");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("endpointUrl");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("isApplicableToConnectSrc");
            mdtFieldsAndTypes.Add("isApplicableToFontSrc");
            mdtFieldsAndTypes.Add("isApplicableToFrameSrc");
            mdtFieldsAndTypes.Add("isApplicableToImgSrc");
            mdtFieldsAndTypes.Add("isApplicableToMediaSrc");
            mdtFieldsAndTypes.Add("isApplicableToStyleSrc");
            mdtFieldsAndTypes.Add("mobileExtension");

            return mdtFieldsAndTypes;
        }

        public static List<String> customApplicationFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionOverrides");
            mdtFieldsAndTypes.Add("brand");
            mdtFieldsAndTypes.Add("consoleConfig");
            mdtFieldsAndTypes.Add("defaultLandingTab");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("formFactors");
            mdtFieldsAndTypes.Add("isNavAutoTempTabsDisabled");
            mdtFieldsAndTypes.Add("isNavPersonalizationDisabled");
            mdtFieldsAndTypes.Add("isServiceCloudConsole");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("navType");
            mdtFieldsAndTypes.Add("preferences");
            mdtFieldsAndTypes.Add("profileActionOverrides");
            mdtFieldsAndTypes.Add("setupExperience");
            mdtFieldsAndTypes.Add("subscriberTabs");
            mdtFieldsAndTypes.Add("tabs");
            mdtFieldsAndTypes.Add("uiType");
            mdtFieldsAndTypes.Add("utilityBar");
            mdtFieldsAndTypes.Add("workspaceConfig");

            return mdtFieldsAndTypes;
        }

        public static List<String> customApplicationComponentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("buttonIconUrl");
            mdtFieldsAndTypes.Add("buttonStyle");
            mdtFieldsAndTypes.Add("buttonText");
            mdtFieldsAndTypes.Add("buttonWidth");
            mdtFieldsAndTypes.Add("height");
            mdtFieldsAndTypes.Add("isHeightFixed");
            mdtFieldsAndTypes.Add("isHidden");
            mdtFieldsAndTypes.Add("isWidthFixed");
            mdtFieldsAndTypes.Add("visualforcePage");
            mdtFieldsAndTypes.Add("width");
            return mdtFieldsAndTypes;
        }

        public static List<String> customFeedFilterFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("criteria");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("isProtected");
            return mdtFieldsAndTypes;
        }

        public static List<String> customHelpMenuSectionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("customHelpMenuItems");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> customLabelsFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("labels");

            return mdtFieldsAndTypes;
        }

        public static List<String> customMetadataFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("protected");
            mdtFieldsAndTypes.Add("values");

            return mdtFieldsAndTypes;
        }

        public static List<String> customNotificationTypeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("customNotifTypeName");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("desktop");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("mobile");
            return mdtFieldsAndTypes;
        }

        public static List<String> customObjectFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionOverrides");
            mdtFieldsAndTypes.Add("allowInChatterGroups");
            mdtFieldsAndTypes.Add("businessProcesses");
            mdtFieldsAndTypes.Add("compactLayoutAssignment");
            mdtFieldsAndTypes.Add("compactLayouts");
            mdtFieldsAndTypes.Add("customHelp");
            mdtFieldsAndTypes.Add("customHelpPage");
            mdtFieldsAndTypes.Add("customSettingsType");
            mdtFieldsAndTypes.Add("customSettingsVisibility");
            mdtFieldsAndTypes.Add("dataStewardGroup");
            mdtFieldsAndTypes.Add("dataStewardUser");
            mdtFieldsAndTypes.Add("deploymentStatus");
            mdtFieldsAndTypes.Add("deprecated");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("enableActivities");
            mdtFieldsAndTypes.Add("enableBulkApi");
            mdtFieldsAndTypes.Add("enableDivisions");
            mdtFieldsAndTypes.Add("enableEnhancedLookup");
            mdtFieldsAndTypes.Add("enableFeeds");
            mdtFieldsAndTypes.Add("enableHistory");
            mdtFieldsAndTypes.Add("enableReports");
            mdtFieldsAndTypes.Add("enableSearch");
            mdtFieldsAndTypes.Add("enableSharing");
            mdtFieldsAndTypes.Add("enableStreamingApi");
            mdtFieldsAndTypes.Add("eventType");
            mdtFieldsAndTypes.Add("externalDataSource");
            mdtFieldsAndTypes.Add("externalName");
            mdtFieldsAndTypes.Add("externalRepository");
            mdtFieldsAndTypes.Add("externalSharingModel");
            mdtFieldsAndTypes.Add("fields");
            mdtFieldsAndTypes.Add("fieldSets");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("gender");
            mdtFieldsAndTypes.Add("household");
            mdtFieldsAndTypes.Add("historyRetentionPolicy");
            mdtFieldsAndTypes.Add("indexes");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("listViews");
            mdtFieldsAndTypes.Add("namedFilter");
            mdtFieldsAndTypes.Add("nameField");
            mdtFieldsAndTypes.Add("pluralLabel");
            mdtFieldsAndTypes.Add("profileSearchLayouts");
            mdtFieldsAndTypes.Add("publishBehavior");
            mdtFieldsAndTypes.Add("recordTypes");
            mdtFieldsAndTypes.Add("recordTypeTrackFeedHistory");
            mdtFieldsAndTypes.Add("recordTypeTrackHistory");
            mdtFieldsAndTypes.Add("searchLayouts");
            mdtFieldsAndTypes.Add("sharingModel");
            mdtFieldsAndTypes.Add("sharingReasons");
            mdtFieldsAndTypes.Add("sharingRecalculations");
            mdtFieldsAndTypes.Add("startsWith");
            mdtFieldsAndTypes.Add("validationRules");
            mdtFieldsAndTypes.Add("visibility");
            mdtFieldsAndTypes.Add("webLinks");

            return mdtFieldsAndTypes;
        }

        public static List<String> customObjectTranslationFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("caseValues");
            mdtFieldsAndTypes.Add("fields");
            mdtFieldsAndTypes.Add("fieldSets");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("gender");
            mdtFieldsAndTypes.Add("layouts");
            mdtFieldsAndTypes.Add("nameFieldLabel");
            mdtFieldsAndTypes.Add("namedFilters");
            mdtFieldsAndTypes.Add("quickActions");
            mdtFieldsAndTypes.Add("recordTypes");
            mdtFieldsAndTypes.Add("sharingReasons");
            mdtFieldsAndTypes.Add("startsWith");
            mdtFieldsAndTypes.Add("validationRules");
            mdtFieldsAndTypes.Add("webLinks");
            mdtFieldsAndTypes.Add("workflowTasks");
            return mdtFieldsAndTypes;
        }

        public static List<String> customPageWebLinkFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("availability");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("displayType");
            mdtFieldsAndTypes.Add("encodingKey");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("hasMenubar");
            mdtFieldsAndTypes.Add("hasScrollbars");
            mdtFieldsAndTypes.Add("hasToolbar");
            mdtFieldsAndTypes.Add("height");
            mdtFieldsAndTypes.Add("isResizable");
            mdtFieldsAndTypes.Add("linkType");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("openType");
            mdtFieldsAndTypes.Add("page");
            mdtFieldsAndTypes.Add("position");
            mdtFieldsAndTypes.Add("protected");
            mdtFieldsAndTypes.Add("requireRowSelection");
            mdtFieldsAndTypes.Add("scontrol");
            mdtFieldsAndTypes.Add("showsLocation");
            mdtFieldsAndTypes.Add("showsStatus");
            mdtFieldsAndTypes.Add("url");
            mdtFieldsAndTypes.Add("width");

            return mdtFieldsAndTypes;
        }

        public static List<String> customPermissionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("connectedApp");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("requiredPermission");
            return mdtFieldsAndTypes;
        }

        public static List<String> customSiteFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("allowHomePage");
            mdtFieldsAndTypes.Add("allowStandardAnswersPages");
            mdtFieldsAndTypes.Add("allowStandardIdeasPages");
            mdtFieldsAndTypes.Add("allowStandardLookups");
            mdtFieldsAndTypes.Add("allowStandardPortalPages");
            mdtFieldsAndTypes.Add("allowStandardSearch");
            mdtFieldsAndTypes.Add("analyticsTrackingCode");
            mdtFieldsAndTypes.Add("authorizationRequiredPage");
            mdtFieldsAndTypes.Add("bandwidthExceededPage");
            mdtFieldsAndTypes.Add("browserXssProtection");
            mdtFieldsAndTypes.Add("changePasswordPage");
            mdtFieldsAndTypes.Add("chatterAnswersForgotPasswordConfirmPage");
            mdtFieldsAndTypes.Add("chatterAnswersForgotPasswordPage");
            mdtFieldsAndTypes.Add("chatterAnswersHelpPage");
            mdtFieldsAndTypes.Add("chatterAnswersLoginPage");
            mdtFieldsAndTypes.Add("chatterAnswersRegistrationPage");
            mdtFieldsAndTypes.Add("clickjackProtectionLevel");
            mdtFieldsAndTypes.Add("contentSniffingProtection");
            mdtFieldsAndTypes.Add("cspUpgradeInsecureRequests");
            mdtFieldsAndTypes.Add("customWebAddresses");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("enableAuraRequests");
            mdtFieldsAndTypes.Add("favoriteIcon");
            mdtFieldsAndTypes.Add("fileNotFoundPage");
            mdtFieldsAndTypes.Add("forgotPasswordPage");
            mdtFieldsAndTypes.Add("genericErrorPage");
            mdtFieldsAndTypes.Add("guestProfile");
            mdtFieldsAndTypes.Add("inMaintenancePage");
            mdtFieldsAndTypes.Add("inactiveIndexPage");
            mdtFieldsAndTypes.Add("indexPage");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("portal");
            mdtFieldsAndTypes.Add("referrerPolicyOriginWhenCrossOrigin");
            mdtFieldsAndTypes.Add("requireHttps");
            mdtFieldsAndTypes.Add("requireInsecurePortalAccess");
            mdtFieldsAndTypes.Add("robotsTxtPage");
            mdtFieldsAndTypes.Add("serverIsDown");
            mdtFieldsAndTypes.Add("siteAdmin");
            mdtFieldsAndTypes.Add("siteRedirectMappings");
            mdtFieldsAndTypes.Add("siteTemplate");
            mdtFieldsAndTypes.Add("siteType");
            mdtFieldsAndTypes.Add("subdomain");
            mdtFieldsAndTypes.Add("urlPathPrefix");

            return mdtFieldsAndTypes;
        }

        public static List<String> customTabFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionOverrides");
            mdtFieldsAndTypes.Add("auraComponent");
            mdtFieldsAndTypes.Add("customObject");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("flexiPage");
            mdtFieldsAndTypes.Add("frameHeight");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("hasSidebar");
            mdtFieldsAndTypes.Add("icon");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("lwcComponent");
            mdtFieldsAndTypes.Add("motif");
            mdtFieldsAndTypes.Add("page");
            mdtFieldsAndTypes.Add("scontrol");
            mdtFieldsAndTypes.Add("splashPageLink");
            mdtFieldsAndTypes.Add("url");
            mdtFieldsAndTypes.Add("urlEncodingKey");

            return mdtFieldsAndTypes;
        }

        public static List<String> customValueFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("color");
            mdtFieldsAndTypes.Add("default");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("label");
            return mdtFieldsAndTypes;
        }

        public static List<String> dashboardFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("backgroundEndColor");
            mdtFieldsAndTypes.Add("backgroundFadeDirection");
            mdtFieldsAndTypes.Add("backgroundStartColor");
            mdtFieldsAndTypes.Add("chartTheme");
            mdtFieldsAndTypes.Add("colorPalette");
            mdtFieldsAndTypes.Add("dashboardChartTheme");
            mdtFieldsAndTypes.Add("dashboardColorPalette");
            mdtFieldsAndTypes.Add("dashboardFilters");
            mdtFieldsAndTypes.Add("dashboardGridLayout");
            mdtFieldsAndTypes.Add("dashboardType");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("folderName");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("isGridLayout");
            mdtFieldsAndTypes.Add("dashboardResultRefreshedDate");
            mdtFieldsAndTypes.Add("dashboardResultRunningUser");
            mdtFieldsAndTypes.Add("leftSection");
            mdtFieldsAndTypes.Add("middleSection");
            mdtFieldsAndTypes.Add("numSubscriptions");
            mdtFieldsAndTypes.Add("rightSection");
            mdtFieldsAndTypes.Add("runningUser");
            mdtFieldsAndTypes.Add("textColor");
            mdtFieldsAndTypes.Add("title");
            mdtFieldsAndTypes.Add("titleColor");
            mdtFieldsAndTypes.Add("titleSize");
            return mdtFieldsAndTypes;
        }

        public static List<String> dataCategoryGroupFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("dataCategory");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("objectUsage");
            return mdtFieldsAndTypes;
        }

        public static List<String> delegateGroupFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("customObjects");
            mdtFieldsAndTypes.Add("groups");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("loginAccess");
            mdtFieldsAndTypes.Add("permissionSets");
            mdtFieldsAndTypes.Add("profiles");
            mdtFieldsAndTypes.Add("roles");
            return mdtFieldsAndTypes;
        }

        public static List<String> documentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("content");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("internalUseOnly");
            mdtFieldsAndTypes.Add("keywords");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("public");
            return mdtFieldsAndTypes;
        }

        public static List<String> duplicateRuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("actionOnInsert");
            mdtFieldsAndTypes.Add("actionOnUpdate");
            mdtFieldsAndTypes.Add("alertText");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("duplicateRuleFilter");
            mdtFieldsAndTypes.Add("duplicateRuleMatchRules");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("operationsOnInsert");
            mdtFieldsAndTypes.Add("operationsOnUpdate");
            mdtFieldsAndTypes.Add("securityOption");
            mdtFieldsAndTypes.Add("sortOrder");
            return mdtFieldsAndTypes;
        }

        public static List<String> eclairGeoDataFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("maps");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> emailServicesFunctionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("apexClass");
            mdtFieldsAndTypes.Add("attachmentOption");
            mdtFieldsAndTypes.Add("authenticationFailureAction");
            mdtFieldsAndTypes.Add("authorizationFailureAction");
            mdtFieldsAndTypes.Add("authorizedSenders");
            mdtFieldsAndTypes.Add("emailServicesAddresses");
            mdtFieldsAndTypes.Add("errorRoutingAddress");
            mdtFieldsAndTypes.Add("functionInactiveAction");
            mdtFieldsAndTypes.Add("functionName");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("isAuthenticationRequired");
            mdtFieldsAndTypes.Add("isErrorRoutingEnabled");
            mdtFieldsAndTypes.Add("isTextAttachmentsAsBinary");
            mdtFieldsAndTypes.Add("isTlsRequired");
            mdtFieldsAndTypes.Add("overLimitAction");

            return mdtFieldsAndTypes;
        }

        public static List<String> emailTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("apiVersion");
            mdtFieldsAndTypes.Add("attachedDocuments");
            mdtFieldsAndTypes.Add("attachments");
            mdtFieldsAndTypes.Add("available");
            mdtFieldsAndTypes.Add("content");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("encodingKey");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("letterhead");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("packageVersions");
            mdtFieldsAndTypes.Add("relatedEntityType");
            mdtFieldsAndTypes.Add("style");
            mdtFieldsAndTypes.Add("subject");
            mdtFieldsAndTypes.Add("textOnly");
            mdtFieldsAndTypes.Add("type");
            mdtFieldsAndTypes.Add("UiType");

            return mdtFieldsAndTypes;
        }

        public static List<String> embeddedServiceBrandingFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("contrastInvertedColor");
            mdtFieldsAndTypes.Add("contrastPrimaryColor");
            mdtFieldsAndTypes.Add("embeddedServiceConfig");
            mdtFieldsAndTypes.Add("font");
            mdtFieldsAndTypes.Add("height");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("navBarColor");
            mdtFieldsAndTypes.Add("primaryColor");
            mdtFieldsAndTypes.Add("secondaryColor");
            mdtFieldsAndTypes.Add("width");

            return mdtFieldsAndTypes;
        }

        public static List<String> embeddedServiceConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("areGuestUsersAllowed");
            mdtFieldsAndTypes.Add("authMethod");
            mdtFieldsAndTypes.Add("customMinimizedComponent");
            mdtFieldsAndTypes.Add("embeddedServiceCustomComponents");
            mdtFieldsAndTypes.Add("embeddedServiceCustomLabels");
            mdtFieldsAndTypes.Add("embeddedServiceFlowConfig");
            mdtFieldsAndTypes.Add("embeddedServiceFlows");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("shouldHideAuthDialog");
            mdtFieldsAndTypes.Add("site");

            return mdtFieldsAndTypes;
        }

        public static List<String> embeddedServiceFieldServiceFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("appointmentBookingFlowName");
            mdtFieldsAndTypes.Add("cancelApptBookingFlowName");
            mdtFieldsAndTypes.Add("embeddedServiceConfig");
            mdtFieldsAndTypes.Add("enabled");
            mdtFieldsAndTypes.Add("fieldServiceConfirmCardImg");
            mdtFieldsAndTypes.Add("fieldServiceHomeImg");
            mdtFieldsAndTypes.Add("fieldServiceLogoImg");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("modifyApptBookingFlowName");
            mdtFieldsAndTypes.Add("shouldShowExistingAppointment");
            mdtFieldsAndTypes.Add("shouldShowNewAppointment");

            return mdtFieldsAndTypes;
        }

        public static List<String> embeddedServiceFlowConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("enabled");

            return mdtFieldsAndTypes;
        }

        public static List<String> embeddedServiceLiveAgentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("avatarImg");
            mdtFieldsAndTypes.Add("customPrechatComponent");
            mdtFieldsAndTypes.Add("embeddedServiceConfig");
            mdtFieldsAndTypes.Add("embeddedServiceQuickActions");
            mdtFieldsAndTypes.Add("enabled");
            mdtFieldsAndTypes.Add("fontSize");
            mdtFieldsAndTypes.Add("headerBackgroundImg");
            mdtFieldsAndTypes.Add("isOfflineCaseEnabled");
            mdtFieldsAndTypes.Add("isQueuePositionEnabled");
            mdtFieldsAndTypes.Add("liveAgentChatUrl");
            mdtFieldsAndTypes.Add("liveAgentContentUrl");
            mdtFieldsAndTypes.Add("liveChatButton");
            mdtFieldsAndTypes.Add("liveChatDeployment");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("offlineCaseBackgroundImg");
            mdtFieldsAndTypes.Add("prechatBackgroundImg");
            mdtFieldsAndTypes.Add("prechatEnabled");
            mdtFieldsAndTypes.Add("prechatJson");
            mdtFieldsAndTypes.Add("scenario");
            mdtFieldsAndTypes.Add("smallCompanyLogoImg");
            mdtFieldsAndTypes.Add("waitingStateBackgroundImg");

            return mdtFieldsAndTypes;
        }

        public static List<String> embeddedServiceMenuSettingsFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("branding");
            mdtFieldsAndTypes.Add("embeddedServiceCustomLabels");
            mdtFieldsAndTypes.Add("embeddedServiceMenuItems");
            mdtFieldsAndTypes.Add("isEnabled");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("site");

            return mdtFieldsAndTypes;
        }

        public static List<String> entitlementProcessFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("businessHours");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("entryStartDateField");
            mdtFieldsAndTypes.Add("exitCriteriaBooleanFilter");
            mdtFieldsAndTypes.Add("exitCriteriaFilterItems");
            mdtFieldsAndTypes.Add("exitCriteriaFormula");
            mdtFieldsAndTypes.Add("isVersionDefault");
            mdtFieldsAndTypes.Add("milestones");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("SObjectType");
            mdtFieldsAndTypes.Add("versionMaster");
            mdtFieldsAndTypes.Add("versionNotes");
            mdtFieldsAndTypes.Add("versionNumber");
            return mdtFieldsAndTypes;
        }

        public static List<String> entitlementTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("businessHours");
            mdtFieldsAndTypes.Add("casesPerEntitlement");
            mdtFieldsAndTypes.Add("entitlementProcess");
            mdtFieldsAndTypes.Add("isPerIncident");
            mdtFieldsAndTypes.Add("term");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> escalationRulesFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("escalationRule");

            return mdtFieldsAndTypes;
        }

        public static List<String> eventDeliveryFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("eventParameters");
            mdtFieldsAndTypes.Add("eventSubscription");
            mdtFieldsAndTypes.Add("referenceData");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> eventSubscriptionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("eventParameters");
            mdtFieldsAndTypes.Add("eventType");
            mdtFieldsAndTypes.Add("referenceData");
            return mdtFieldsAndTypes;
        }

        public static List<String> experienceBundleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("experienceResources");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> externalDataSourceFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("authProvider");
            mdtFieldsAndTypes.Add("certificate");
            mdtFieldsAndTypes.Add("customConfiguration");
            mdtFieldsAndTypes.Add("customHttpHeader");
            mdtFieldsAndTypes.Add("endpoint");
            mdtFieldsAndTypes.Add("isWritable");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("oauthRefreshToken");
            mdtFieldsAndTypes.Add("oauthScope");
            mdtFieldsAndTypes.Add("oauthToken");
            mdtFieldsAndTypes.Add("password");
            mdtFieldsAndTypes.Add("principalType");
            mdtFieldsAndTypes.Add("protocol");
            mdtFieldsAndTypes.Add("repository");
            mdtFieldsAndTypes.Add("type");
            mdtFieldsAndTypes.Add("username");
            mdtFieldsAndTypes.Add("version");
            return mdtFieldsAndTypes;
        }

        public static List<String> externalServiceRegistrationFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("namedCredential");
            mdtFieldsAndTypes.Add("schema");
            mdtFieldsAndTypes.Add("schemaType");
            mdtFieldsAndTypes.Add("schemaUrl");
            mdtFieldsAndTypes.Add("status");
            return mdtFieldsAndTypes;
        }

        public static List<String> featureParameterFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("dataFlowDirection");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("value");
            return mdtFieldsAndTypes;
        }

        /*
        public static List<String> featureParameterBooleanFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("dataFlowDirection");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("value");
            return mdtFieldsAndTypes;
        }

        public static List<String> featureParameterDateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("dataFlowDirection");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("value");
            return mdtFieldsAndTypes;
        }

        public static List<String> featureParameterIntegerFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("dataFlowDirection");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("value");
            return mdtFieldsAndTypes;
        }
        */

        public static List<String> flexiPageFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("flexiPageRegions");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("pageTemplate");
            mdtFieldsAndTypes.Add("parentFlexiPage");
            mdtFieldsAndTypes.Add("platformActionList");
            mdtFieldsAndTypes.Add("quickActionList");
            mdtFieldsAndTypes.Add("sobjectType");
            mdtFieldsAndTypes.Add("template");
            mdtFieldsAndTypes.Add("type");

            return mdtFieldsAndTypes;
        }

        public static List<String> flowFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionCalls");
            mdtFieldsAndTypes.Add("apexPluginCalls");
            mdtFieldsAndTypes.Add("assignments");
            mdtFieldsAndTypes.Add("choices");
            mdtFieldsAndTypes.Add("constants");
            mdtFieldsAndTypes.Add("decisions");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("dynamicChoiceSets");
            mdtFieldsAndTypes.Add("formulas");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("interviewLabel");
            mdtFieldsAndTypes.Add("isAdditionalPermissionRequiredToRun");
            mdtFieldsAndTypes.Add("isTemplate");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("loops");
            mdtFieldsAndTypes.Add("processMetadataValues");
            mdtFieldsAndTypes.Add("processType");
            mdtFieldsAndTypes.Add("recordCreates");
            mdtFieldsAndTypes.Add("recordDeletes");
            mdtFieldsAndTypes.Add("recordLookups");
            mdtFieldsAndTypes.Add("recordUpdates");
            mdtFieldsAndTypes.Add("runInMode");
            mdtFieldsAndTypes.Add("screens");
            mdtFieldsAndTypes.Add("stages");
            mdtFieldsAndTypes.Add("start");
            mdtFieldsAndTypes.Add("startElementReference");
            mdtFieldsAndTypes.Add("status");
            mdtFieldsAndTypes.Add("steps");
            mdtFieldsAndTypes.Add("subflows");
            mdtFieldsAndTypes.Add("textTemplates");
            mdtFieldsAndTypes.Add("variables");
            mdtFieldsAndTypes.Add("waits");

            return mdtFieldsAndTypes;
        }

        public static List<String> flowCategoryFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("flowCategoryItems");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> flowDefinitionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("activeVersionNumber");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("masterLabel");

            return mdtFieldsAndTypes;
        }

        public static List<String> folderFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("accessType");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("publicFolderAccess");
            mdtFieldsAndTypes.Add("sharedTo");
            return mdtFieldsAndTypes;
        }

        public static List<String> globalPicklistFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("globalPicklistValues");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("sorted");

            return mdtFieldsAndTypes;
        }

        public static List<String> globalPicklistValueFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("color");
            mdtFieldsAndTypes.Add("default");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("isActive");
            return mdtFieldsAndTypes;
        }

        public static List<String> globalValueSetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("sorted");
            mdtFieldsAndTypes.Add("customValue");
            return mdtFieldsAndTypes;
        }

        public static List<String> globalValueSetTranslationFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("translation");
            return mdtFieldsAndTypes;
        }

        public static List<String> groupFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("doesIncludeBosses");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("name");
            return mdtFieldsAndTypes;
        }

        public static List<String> homePageComponentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("body");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("height");
            mdtFieldsAndTypes.Add("links");
            mdtFieldsAndTypes.Add("page");
            mdtFieldsAndTypes.Add("pageComponentType");
            mdtFieldsAndTypes.Add("showLabel");
            mdtFieldsAndTypes.Add("showScrollbars");
            mdtFieldsAndTypes.Add("width");
            return mdtFieldsAndTypes;
        }

        public static List<String> homePageLayoutFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("narrowComponents");
            mdtFieldsAndTypes.Add("wideComponents");
            return mdtFieldsAndTypes;
        }

        public static List<String> installedPackageFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("activateRSS");
            mdtFieldsAndTypes.Add("password");
            mdtFieldsAndTypes.Add("versionNumber");

            return mdtFieldsAndTypes;
        }

        public static List<String> keywordListFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("keywords");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> layoutFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("customButtons");
            mdtFieldsAndTypes.Add("customConsoleComponents");
            mdtFieldsAndTypes.Add("emailDefault");
            mdtFieldsAndTypes.Add("excludeButtons");
            mdtFieldsAndTypes.Add("feedLayout");
            mdtFieldsAndTypes.Add("headers");
            mdtFieldsAndTypes.Add("layoutSections");
            mdtFieldsAndTypes.Add("miniLayout");
            mdtFieldsAndTypes.Add("multilineLayoutFields");
            mdtFieldsAndTypes.Add("platformActionList");
            mdtFieldsAndTypes.Add("quickActionList");
            mdtFieldsAndTypes.Add("relatedContent");
            mdtFieldsAndTypes.Add("relatedLists");
            mdtFieldsAndTypes.Add("relatedObjects");
            mdtFieldsAndTypes.Add("runAssignmentRulesDefault");
            mdtFieldsAndTypes.Add("showEmailCheckbox");
            mdtFieldsAndTypes.Add("showHighlightsPanel");
            mdtFieldsAndTypes.Add("showInteractionLogPanel");
            mdtFieldsAndTypes.Add("showKnowledgeComponent");
            mdtFieldsAndTypes.Add("showRunAssignmentRulesCheckbox");
            mdtFieldsAndTypes.Add("showSolutionSection");
            mdtFieldsAndTypes.Add("showSubmitAndAttachButton");
            mdtFieldsAndTypes.Add("summaryLayout");

            return mdtFieldsAndTypes;
        }

        public static List<String> letterheadFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("available");
            mdtFieldsAndTypes.Add("backgroundColor");
            mdtFieldsAndTypes.Add("bodyColor");
            mdtFieldsAndTypes.Add("bottomLine");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("footer");
            mdtFieldsAndTypes.Add("header");
            mdtFieldsAndTypes.Add("middleLine");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("topLine");
            return mdtFieldsAndTypes;
        }

        public static List<String> lightningBoltFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("category");
            mdtFieldsAndTypes.Add("lightningBoltFeatures");
            mdtFieldsAndTypes.Add("lightningBoltImages");
            mdtFieldsAndTypes.Add("lightningBoltItems");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("publisher");
            mdtFieldsAndTypes.Add("summary");
            return mdtFieldsAndTypes;
        }

        public static List<String> lightningComponentBundleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("apiVersion");
            mdtFieldsAndTypes.Add("capabilities");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("isExplicitImport");
            mdtFieldsAndTypes.Add("isExposed");
            mdtFieldsAndTypes.Add("lwcResources");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("targetConfigs");
            mdtFieldsAndTypes.Add("targets");
            return mdtFieldsAndTypes;
        }

        public static List<String> lightningExperienceThemeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("defaultBrandingSet");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("shouldOverrideLoadingImage");
            return mdtFieldsAndTypes;
        }

        public static List<String> lightningMessageChannelFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("isExposed");
            mdtFieldsAndTypes.Add("lightningMessageFields");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> liveChatAgentConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("assignments");
            mdtFieldsAndTypes.Add("autoGreeting");
            mdtFieldsAndTypes.Add("capacity");
            mdtFieldsAndTypes.Add("criticalWaitTime");
            mdtFieldsAndTypes.Add("customAgentName");
            mdtFieldsAndTypes.Add("enableAgentFileTransfer");
            mdtFieldsAndTypes.Add("enableAgentSneakPeek");
            mdtFieldsAndTypes.Add("enableAssistanceFlag");
            mdtFieldsAndTypes.Add("enableAutoAwayOnDecline");
            mdtFieldsAndTypes.Add("enableAutoAwayOnPushTimeout");
            mdtFieldsAndTypes.Add("enableChatConferencing");
            mdtFieldsAndTypes.Add("enableChatMonitoring");
            mdtFieldsAndTypes.Add("enableChatTransferToAgent");
            mdtFieldsAndTypes.Add("enableChatTransferToButton");
            mdtFieldsAndTypes.Add("enableChatTransferToSkill");
            mdtFieldsAndTypes.Add("enableLogoutSound");
            mdtFieldsAndTypes.Add("enableNotifications");
            mdtFieldsAndTypes.Add("enableRequestSound");
            mdtFieldsAndTypes.Add("enableSneakPeek");
            mdtFieldsAndTypes.Add("enableVisitorBlocking");
            mdtFieldsAndTypes.Add("enableWhisperMessage");
            mdtFieldsAndTypes.Add("supervisorDefaultAgentStatusFilter");
            mdtFieldsAndTypes.Add("supervisorDefaultButtonFilter");
            mdtFieldsAndTypes.Add("supervisorDefaultSkillFilter");
            mdtFieldsAndTypes.Add("supervisorSkills");
            mdtFieldsAndTypes.Add("transferableButtons");
            mdtFieldsAndTypes.Add("transferableSkills");

            return mdtFieldsAndTypes;
        }

        public static List<String> liveChatButtonFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("animation");
            mdtFieldsAndTypes.Add("autoGreeting");
            mdtFieldsAndTypes.Add("chasitorIdleTimeout");
            mdtFieldsAndTypes.Add("chasitorIdleTimeoutWarning");
            mdtFieldsAndTypes.Add("chatPage");
            mdtFieldsAndTypes.Add("customAgentName");
            mdtFieldsAndTypes.Add("deployments");
            mdtFieldsAndTypes.Add("enableQueue");
            mdtFieldsAndTypes.Add("inviteEndPosition");
            mdtFieldsAndTypes.Add("inviteImage");
            mdtFieldsAndTypes.Add("inviteStartPosition");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("numberOfReroutingAttempts");
            mdtFieldsAndTypes.Add("offlineImage");
            mdtFieldsAndTypes.Add("onlineImage");
            mdtFieldsAndTypes.Add("optionsCustomRoutingIsEnabled");
            mdtFieldsAndTypes.Add("optionsHasChasitorIdleTimeout");
            mdtFieldsAndTypes.Add("optionsHasInviteAfterAccept");
            mdtFieldsAndTypes.Add("optionsHasInviteAfterReject");
            mdtFieldsAndTypes.Add("optionsHasRerouteDeclinedRequest");
            mdtFieldsAndTypes.Add("optionsIsAutoAccept");
            mdtFieldsAndTypes.Add("optionsIsInviteAutoRemove");
            mdtFieldsAndTypes.Add("overallQueueLength");
            mdtFieldsAndTypes.Add("perAgentQueueLength");
            mdtFieldsAndTypes.Add("postChatPage");
            mdtFieldsAndTypes.Add("postChatUrl");
            mdtFieldsAndTypes.Add("preChatFormPage");
            mdtFieldsAndTypes.Add("preChatFormUrl");
            mdtFieldsAndTypes.Add("pushTimeOut");
            mdtFieldsAndTypes.Add("routingType");
            mdtFieldsAndTypes.Add("site");
            mdtFieldsAndTypes.Add("skills");
            mdtFieldsAndTypes.Add("timeToRemoveInvite");
            mdtFieldsAndTypes.Add("type");
            mdtFieldsAndTypes.Add("windowLanguage");

            return mdtFieldsAndTypes;
        }

        public static List<String> liveChatDeploymentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("brandingImage");
            mdtFieldsAndTypes.Add("connectionTimeoutDuration");
            mdtFieldsAndTypes.Add("ConnectionWarningDuration");
            mdtFieldsAndTypes.Add("displayQueuePosition");
            mdtFieldsAndTypes.Add("domainWhiteList");
            mdtFieldsAndTypes.Add("enablePrechatApi");
            mdtFieldsAndTypes.Add("enableTranscriptSave");
            mdtFieldsAndTypes.Add("mobileBrandingImage");
            mdtFieldsAndTypes.Add("site");
            mdtFieldsAndTypes.Add("windowTitle");

            return mdtFieldsAndTypes;
        }

        public static List<String> liveChatSensitiveDataRuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("actionType");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("enforceOn");
            mdtFieldsAndTypes.Add("isEnabled");
            mdtFieldsAndTypes.Add("pattern");
            mdtFieldsAndTypes.Add("replacement");

            return mdtFieldsAndTypes;
        }

        public static List<String> managedContentTypeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("developerName");
            mdtFieldsAndTypes.Add("managedContentNodeTypes");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> managedTopicsFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("managedTopicType");
            mdtFieldsAndTypes.Add("topicDescription");
            mdtFieldsAndTypes.Add("parentName");
            mdtFieldsAndTypes.Add("position");
            return mdtFieldsAndTypes;
        }

        public static List<String> matchingRuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("booleanFilter");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("matchingRuleItems");
            mdtFieldsAndTypes.Add("ruleStatus");
            return mdtFieldsAndTypes;
        }

        public static List<String> metadataFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("fullName");
            return mdtFieldsAndTypes;
        }

        public static List<String> metadataWithContentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("content");
            mdtFieldsAndTypes.Add("fullName");
            return mdtFieldsAndTypes;
        }

        public static List<String> milestoneTypeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("RecurrenceType");
            return mdtFieldsAndTypes;
        }

        public static List<String> mlDomainFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("mlIntents");
            mdtFieldsAndTypes.Add("mlSlotClasses");
            return mdtFieldsAndTypes;
        }

        public static List<String> mobileApplicationDetailFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("applicationBinaryFile");
            mdtFieldsAndTypes.Add("applicationBinaryFileName");
            mdtFieldsAndTypes.Add("applicationBundleIdentifier");
            mdtFieldsAndTypes.Add("applicationFileLength");
            mdtFieldsAndTypes.Add("applicationIconFile");
            mdtFieldsAndTypes.Add("applicationIconFileName");
            mdtFieldsAndTypes.Add("applicationInstallUrl");
            mdtFieldsAndTypes.Add("devicePlatform");
            mdtFieldsAndTypes.Add("deviceType");
            mdtFieldsAndTypes.Add("minimumOsVersion");
            mdtFieldsAndTypes.Add("privateApp");
            mdtFieldsAndTypes.Add("version");
            return mdtFieldsAndTypes;
        }

        public static List<String> moderationRuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("action");
            mdtFieldsAndTypes.Add("actionLimit");
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("entitiesAndFields");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("notifyLimit");
            mdtFieldsAndTypes.Add("userCriteria");
            mdtFieldsAndTypes.Add("userMessage");
            return mdtFieldsAndTypes;
        }

        public static List<String> myDomainDiscoverableLoginFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("apexHandler");
            mdtFieldsAndTypes.Add("executeApexHandlerAs");
            mdtFieldsAndTypes.Add("usernameLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> namedCredentialFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("awsAccessKey");
            mdtFieldsAndTypes.Add("awsAccessSecret");
            mdtFieldsAndTypes.Add("awsRegion");
            mdtFieldsAndTypes.Add("awsService");
            mdtFieldsAndTypes.Add("allowMergeFieldsInBody");
            mdtFieldsAndTypes.Add("allowMergeFieldsInHeader");
            mdtFieldsAndTypes.Add("authProvider");
            mdtFieldsAndTypes.Add("authTokenEndpointUrl");
            mdtFieldsAndTypes.Add("certificate");
            mdtFieldsAndTypes.Add("endpoint");
            mdtFieldsAndTypes.Add("generateAuthorizationHeader");
            mdtFieldsAndTypes.Add("jwtAudience");
            mdtFieldsAndTypes.Add("jwtFormulaSubject");
            mdtFieldsAndTypes.Add("jwtIssuer");
            mdtFieldsAndTypes.Add("jwtSigningCertificate");
            mdtFieldsAndTypes.Add("jwtTextSubject");
            mdtFieldsAndTypes.Add("jwtValidityPeriodSeconds");
            mdtFieldsAndTypes.Add("oauthRefreshToken");
            mdtFieldsAndTypes.Add("oauthScope");
            mdtFieldsAndTypes.Add("oauthToken");
            mdtFieldsAndTypes.Add("password");
            mdtFieldsAndTypes.Add("principalType");
            mdtFieldsAndTypes.Add("protocol");
            mdtFieldsAndTypes.Add("username");

            return mdtFieldsAndTypes;
        }

        public static List<String> navigationMenuFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("container");
            mdtFieldsAndTypes.Add("containerType");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("navigationMenuItem");
            return mdtFieldsAndTypes;
        }

        public static List<String> networkFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("allowedExtensions");
            mdtFieldsAndTypes.Add("allowInternalUserLogin");
            mdtFieldsAndTypes.Add("allowMembersToFlag");
            mdtFieldsAndTypes.Add("branding");
            mdtFieldsAndTypes.Add("caseCommentEmailTemplate");
            mdtFieldsAndTypes.Add("changePasswordTemplate");
            mdtFieldsAndTypes.Add("communityRoles");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("disableReputationRecordConversations");
            mdtFieldsAndTypes.Add("emailFooterLogo");
            mdtFieldsAndTypes.Add("emailFooterText");
            mdtFieldsAndTypes.Add("emailSenderAddress");
            mdtFieldsAndTypes.Add("emailSenderName");
            mdtFieldsAndTypes.Add("enableCustomVFError​PageOverrides");
            mdtFieldsAndTypes.Add("enableDirectMessages");
            mdtFieldsAndTypes.Add("enableGuestChatter");
            mdtFieldsAndTypes.Add("enableGuestFileAccess");
            mdtFieldsAndTypes.Add("enableGuestMemberVisibility");
            mdtFieldsAndTypes.Add("enableInvitation");
            mdtFieldsAndTypes.Add("enableKnowledgeable");
            mdtFieldsAndTypes.Add("enableNicknameDisplay");
            mdtFieldsAndTypes.Add("enablePrivateMessages");
            mdtFieldsAndTypes.Add("enableReputation");
            mdtFieldsAndTypes.Add("enableShowAllNetworkSettings");
            mdtFieldsAndTypes.Add("enableSiteAsContainer");
            mdtFieldsAndTypes.Add("enableTalkingAboutStats");
            mdtFieldsAndTypes.Add("enableTopicAssignmentRules");
            mdtFieldsAndTypes.Add("enableTopicSuggestions");
            mdtFieldsAndTypes.Add("enableUpDownVote");
            mdtFieldsAndTypes.Add("feedChannel");
            mdtFieldsAndTypes.Add("forgotPasswordTemplate");
            mdtFieldsAndTypes.Add("gatherCustomerSentimentData");
            mdtFieldsAndTypes.Add("lockoutTemplate");
            mdtFieldsAndTypes.Add("logoutUrl");
            mdtFieldsAndTypes.Add("maxFileSizeKb");
            mdtFieldsAndTypes.Add("navigationLinkSet");
            mdtFieldsAndTypes.Add("networkMemberGroups");
            mdtFieldsAndTypes.Add("networkPageOverrides");
            mdtFieldsAndTypes.Add("newSenderAddress");
            mdtFieldsAndTypes.Add("enableMemberVisibility");
            mdtFieldsAndTypes.Add("picassoSite");
            mdtFieldsAndTypes.Add("recommendationAudience");
            mdtFieldsAndTypes.Add("recommendationDefinition");
            mdtFieldsAndTypes.Add("reputationLevels");
            mdtFieldsAndTypes.Add("reputationPointsRules");
            mdtFieldsAndTypes.Add("selfRegProfile");
            mdtFieldsAndTypes.Add("selfRegistration");
            mdtFieldsAndTypes.Add("sendWelcomeEmail");
            mdtFieldsAndTypes.Add("site");
            mdtFieldsAndTypes.Add("status");
            mdtFieldsAndTypes.Add("tabs");
            mdtFieldsAndTypes.Add("urlPathPrefix");
            mdtFieldsAndTypes.Add("verificationTemplate");
            mdtFieldsAndTypes.Add("welcomeTemplate");

            return mdtFieldsAndTypes;
        }

        public static List<String> networkBrandingFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("loginLogo");
            mdtFieldsAndTypes.Add("loginLogoName");
            mdtFieldsAndTypes.Add("loginLogoStaticImageUrl");
            mdtFieldsAndTypes.Add("loginQuaternaryColor");
            mdtFieldsAndTypes.Add("loginRightFrameUrl");
            mdtFieldsAndTypes.Add("network");
            mdtFieldsAndTypes.Add("pageFooter");
            mdtFieldsAndTypes.Add("pageHeader");
            mdtFieldsAndTypes.Add("primaryColor");
            mdtFieldsAndTypes.Add("primaryComplementColor");
            mdtFieldsAndTypes.Add("quaternaryColor");
            mdtFieldsAndTypes.Add("quaternaryComplementColor");
            mdtFieldsAndTypes.Add("secondaryColor");
            mdtFieldsAndTypes.Add("tertiaryColor");
            mdtFieldsAndTypes.Add("tertiaryComplementColor");
            mdtFieldsAndTypes.Add("zeronaryColor");
            mdtFieldsAndTypes.Add("zeronaryComplementColor");
            return mdtFieldsAndTypes;
        }

        public static List<String> notificationTypeConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("notificationType");
            mdtFieldsAndTypes.Add("appSettings");
            mdtFieldsAndTypes.Add("notificationChannels");
            return mdtFieldsAndTypes;
        }

        public static List<String> oauthCustomScopeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("developerName");
            mdtFieldsAndTypes.Add("isProtected");
            mdtFieldsAndTypes.Add("isPublic");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> packageFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("apiAccessLevel");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("namespacePrefix");
            mdtFieldsAndTypes.Add("objectPermissions");
            mdtFieldsAndTypes.Add("packageType");
            mdtFieldsAndTypes.Add("postInstallClass");
            mdtFieldsAndTypes.Add("setupWeblink");
            mdtFieldsAndTypes.Add("types");
            mdtFieldsAndTypes.Add("uninstallClass");
            mdtFieldsAndTypes.Add("version");
            return mdtFieldsAndTypes;
        }

        public static List<String> pathAssistantFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("entityName");
            mdtFieldsAndTypes.Add("fieldName");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("pathAssistantSteps");
            mdtFieldsAndTypes.Add("recordTypeName");
            return mdtFieldsAndTypes;
        }

        public static List<String> paymentGatewayProviderFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("apexAdapter");
            mdtFieldsAndTypes.Add("comments");
            mdtFieldsAndTypes.Add("idempotencySupported");
            mdtFieldsAndTypes.Add("isProtected");
            mdtFieldsAndTypes.Add("masterLabel");
            return mdtFieldsAndTypes;
        }

        public static List<String> permissionSetGroupFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("mutingPermissionSets");
            mdtFieldsAndTypes.Add("permissionSets");
            mdtFieldsAndTypes.Add("status");
            return mdtFieldsAndTypes;
        }

        public static List<String> platformCachePartitionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("isDefaultPartition");
            mdtFieldsAndTypes.Add("platformCachePartitionTypes");
            return mdtFieldsAndTypes;
        }

        public static List<String> platformEventChannelFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("channelMembers");
            mdtFieldsAndTypes.Add("channelType");
            mdtFieldsAndTypes.Add("label");
            return mdtFieldsAndTypes;
        }

        public static List<String> platformEventChannelMemberFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("eventChannel");
            mdtFieldsAndTypes.Add("selectedEntity");
            return mdtFieldsAndTypes;
        }

        public static List<String> portalFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("admin");
            mdtFieldsAndTypes.Add("defaultLanguage");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("emailSenderAddress");
            mdtFieldsAndTypes.Add("emailSenderName");
            mdtFieldsAndTypes.Add("enableSelfCloseCase");
            mdtFieldsAndTypes.Add("footerDocument");
            mdtFieldsAndTypes.Add("forgotPassTemplate");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("headerDocument");
            mdtFieldsAndTypes.Add("isSelfRegistrationActivated");
            mdtFieldsAndTypes.Add("loginHeaderDocument");
            mdtFieldsAndTypes.Add("logoDocument");
            mdtFieldsAndTypes.Add("logoutUrl");
            mdtFieldsAndTypes.Add("newCommentTemplate");
            mdtFieldsAndTypes.Add("newPassTemplate");
            mdtFieldsAndTypes.Add("newUserTemplate");
            mdtFieldsAndTypes.Add("ownerNotifyTemplate");
            mdtFieldsAndTypes.Add("selfRegNewUserUrl");
            mdtFieldsAndTypes.Add("selfRegUserDefaultProfile");
            mdtFieldsAndTypes.Add("selfRegUserDefaultRole");
            mdtFieldsAndTypes.Add("selfRegUserTemplate");
            mdtFieldsAndTypes.Add("showActionConfirmation");
            mdtFieldsAndTypes.Add("stylesheetDocument");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> postTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("default");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fields");
            mdtFieldsAndTypes.Add("label");
            return mdtFieldsAndTypes;
        }

        public static List<String> presenceDeclineReasonFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            return mdtFieldsAndTypes;
        }

        public static List<String> presenceUserConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("assignments");
            mdtFieldsAndTypes.Add("capacity");
            mdtFieldsAndTypes.Add("declineReasons");
            mdtFieldsAndTypes.Add("enableAutoAccept");
            mdtFieldsAndTypes.Add("enableDecline");
            mdtFieldsAndTypes.Add("enableDeclineReason");
            mdtFieldsAndTypes.Add("enableDisconnectSound");
            mdtFieldsAndTypes.Add("enableRequestSound");
            mdtFieldsAndTypes.Add("presenceStatusOnDecline");
            mdtFieldsAndTypes.Add("presenceStatusOnPushTimeout");

            return mdtFieldsAndTypes;
        }

        public static List<String> profilePermissionSetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("userLicense");
            mdtFieldsAndTypes.Add("license");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("custom");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("hasActivationRequired");
            mdtFieldsAndTypes.Add("loginHours");
            mdtFieldsAndTypes.Add("loginIpRanges");
            mdtFieldsAndTypes.Add("applicationVisibilities");
            mdtFieldsAndTypes.Add("tabVisibilities");
            mdtFieldsAndTypes.Add("layoutAssignments");
            mdtFieldsAndTypes.Add("objectPermissions");
            mdtFieldsAndTypes.Add("customMetadataTypeAccesses");
            mdtFieldsAndTypes.Add("recordTypeVisibilities");
            mdtFieldsAndTypes.Add("fieldPermissions");      // API version 23.0 and later
            mdtFieldsAndTypes.Add("fieldLevelSecurities");  // API version 22.0 and earlier
            mdtFieldsAndTypes.Add("pageAccesses");
            mdtFieldsAndTypes.Add("classAccesses");
            mdtFieldsAndTypes.Add("flowAccesses");
            mdtFieldsAndTypes.Add("categoryGroupVisibilities");
            mdtFieldsAndTypes.Add("userPermissions");
            mdtFieldsAndTypes.Add("customPermissions");
            mdtFieldsAndTypes.Add("customSettingAccesses");
            mdtFieldsAndTypes.Add("externalDataSourceAccesses");
            mdtFieldsAndTypes.Add("profileActionOverrides");

            return mdtFieldsAndTypes;
        }

        public static List<String> profileActionOverrideFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("actionName");
            mdtFieldsAndTypes.Add("content");
            mdtFieldsAndTypes.Add("formFactor");
            mdtFieldsAndTypes.Add("pageOrSobjectType");
            mdtFieldsAndTypes.Add("recordType");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> profilePasswordPolicyFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("forgotPasswordRedirect");
            mdtFieldsAndTypes.Add("lockoutInterval");
            mdtFieldsAndTypes.Add("maxLoginAttempts");
            mdtFieldsAndTypes.Add("minimumPasswordLength");
            mdtFieldsAndTypes.Add("minimumPasswordLifetime");
            mdtFieldsAndTypes.Add("obscure");
            mdtFieldsAndTypes.Add("passwordComplexity");
            mdtFieldsAndTypes.Add("passwordExpiration");
            mdtFieldsAndTypes.Add("passwordHistory");
            mdtFieldsAndTypes.Add("passwordQuestion");
            mdtFieldsAndTypes.Add("profile");

            return mdtFieldsAndTypes;
        }

        public static List<String> profileSessionSettingFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("forceLogout");
            mdtFieldsAndTypes.Add("profile");
            mdtFieldsAndTypes.Add("requiredSessionLevel");
            mdtFieldsAndTypes.Add("sessionPersistence");
            mdtFieldsAndTypes.Add("sessionTimeout");
            mdtFieldsAndTypes.Add("sessionTimeoutWarning");
            return mdtFieldsAndTypes;
        }

        public static List<String> promptFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("promptVersions");
            return mdtFieldsAndTypes;
        }

        public static List<String> queueFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("doesSendEmailToMembers");
            mdtFieldsAndTypes.Add("email");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("queueMembers");
            mdtFieldsAndTypes.Add("queueRoutingConfig");
            mdtFieldsAndTypes.Add("queueSobject");

            return mdtFieldsAndTypes;
        }

        public static List<String> queueRoutingConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("capacityPercentage");
            mdtFieldsAndTypes.Add("capacityWeight");
            mdtFieldsAndTypes.Add("dropAdditionalSkillsTimeout");
            mdtFieldsAndTypes.Add("isAttributeBased");
            mdtFieldsAndTypes.Add("pushTimeout");
            mdtFieldsAndTypes.Add("queueOverflowAssignee");
            mdtFieldsAndTypes.Add("routingModel");
            mdtFieldsAndTypes.Add("routingPriority");
            mdtFieldsAndTypes.Add("userOverflowAssignee");

            return mdtFieldsAndTypes;
        }

        public static List<String> quickActionFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("canvas");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fieldOverrides");
            mdtFieldsAndTypes.Add("flowDefinition");
            mdtFieldsAndTypes.Add("height");
            mdtFieldsAndTypes.Add("icon");
            mdtFieldsAndTypes.Add("isProtected");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("lightningComponent");
            mdtFieldsAndTypes.Add("optionsCreateFeedItem");
            mdtFieldsAndTypes.Add("page");
            mdtFieldsAndTypes.Add("quickActionLayout");
            mdtFieldsAndTypes.Add("standardLabel");
            mdtFieldsAndTypes.Add("targetObject");
            mdtFieldsAndTypes.Add("targetParentField");
            mdtFieldsAndTypes.Add("targetRecordType");
            mdtFieldsAndTypes.Add("type");
            mdtFieldsAndTypes.Add("width");

            return mdtFieldsAndTypes;
        }

        public static List<String> redirectWhitelistUrlFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("Url");
            return mdtFieldsAndTypes;
        }

        public static List<String> recommendationStrategyFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("actionContext");
            mdtFieldsAndTypes.Add("contextRecordType");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("filter");
            mdtFieldsAndTypes.Add("if");
            mdtFieldsAndTypes.Add("invocableAction");
            mdtFieldsAndTypes.Add("isTemplate");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("map");
            mdtFieldsAndTypes.Add("mutuallyExclusive");
            mdtFieldsAndTypes.Add("onBehalfOfExpression");
            mdtFieldsAndTypes.Add("recommendationLimit");
            mdtFieldsAndTypes.Add("recommendationLoad");
            mdtFieldsAndTypes.Add("sort");
            mdtFieldsAndTypes.Add("union");
            return mdtFieldsAndTypes;
        }

        public static List<String> recordActionDeploymentFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("channelConfigurations");
            mdtFieldsAndTypes.Add("deploymentContexts");
            mdtFieldsAndTypes.Add("hasGuidedActions");
            mdtFieldsAndTypes.Add("hasRecommendations");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("recommendation");
            mdtFieldsAndTypes.Add("selectableItems");
            mdtFieldsAndTypes.Add("shouldLaunchActionOnReject");
            return mdtFieldsAndTypes;
        }

        public static List<String> remoteSiteSettingFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("url");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("disableProtocolSecurity");

            return mdtFieldsAndTypes;
        }

        public static List<String> reportFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("aggregates");
            mdtFieldsAndTypes.Add("block");
            mdtFieldsAndTypes.Add("blockInfo");
            mdtFieldsAndTypes.Add("buckets");
            mdtFieldsAndTypes.Add("chart");
            mdtFieldsAndTypes.Add("colorRanges");
            mdtFieldsAndTypes.Add("columns");
            mdtFieldsAndTypes.Add("crossFilters");
            mdtFieldsAndTypes.Add("currency");
            mdtFieldsAndTypes.Add("dataCategoryFilters");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("division");
            mdtFieldsAndTypes.Add("filter");
            mdtFieldsAndTypes.Add("folderName");
            mdtFieldsAndTypes.Add("format");
            mdtFieldsAndTypes.Add("formattingRules");
            mdtFieldsAndTypes.Add("groupingsAcross");
            mdtFieldsAndTypes.Add("groupingsDown");
            mdtFieldsAndTypes.Add("historicalSelector");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("numSubscriptions");
            mdtFieldsAndTypes.Add("params");
            mdtFieldsAndTypes.Add("reportCustomDetailFormula");
            mdtFieldsAndTypes.Add("reportType");
            mdtFieldsAndTypes.Add("roleHierarchyFilter");
            mdtFieldsAndTypes.Add("rowLimit");
            mdtFieldsAndTypes.Add("scope");
            mdtFieldsAndTypes.Add("showCurrentDate");
            mdtFieldsAndTypes.Add("showDetails");
            mdtFieldsAndTypes.Add("showGrandTotal");
            mdtFieldsAndTypes.Add("showSubTotals");
            mdtFieldsAndTypes.Add("sortColumn");
            mdtFieldsAndTypes.Add("sortOrder");
            mdtFieldsAndTypes.Add("territoryHierarchyFilter");
            mdtFieldsAndTypes.Add("timeFrameFilter");
            mdtFieldsAndTypes.Add("userFilter");
            return mdtFieldsAndTypes;
        }

        public static List<String> reportTypeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("autogenerated");
            mdtFieldsAndTypes.Add("baseObject");
            mdtFieldsAndTypes.Add("category");
            mdtFieldsAndTypes.Add("deployed");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("join");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("sections");
            return mdtFieldsAndTypes;
        }

        public static List<String> roleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("parentRole");
            return mdtFieldsAndTypes;
        }

        public static List<String> roleOrTerritoryFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("caseAccessLevel");
            mdtFieldsAndTypes.Add("contactAccessLevel");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("mayForecastManagerShare");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("opportunityAccessLevel");
            return mdtFieldsAndTypes;
        }

        public static List<String> samlSsoConfigFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("attributeNameIdFormat");
            mdtFieldsAndTypes.Add("attributeName");
            mdtFieldsAndTypes.Add("decryptionCertificate");
            mdtFieldsAndTypes.Add("errorUrl");
            mdtFieldsAndTypes.Add("executionUserId");
            mdtFieldsAndTypes.Add("identityLocation");
            mdtFieldsAndTypes.Add("identityMapping");
            mdtFieldsAndTypes.Add("issuer");
            mdtFieldsAndTypes.Add("loginUrl");
            mdtFieldsAndTypes.Add("logoutUrl");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("oauthTokenEndpoint");
            mdtFieldsAndTypes.Add("redirectBinding");
            mdtFieldsAndTypes.Add("requestSignatureMethod");
            mdtFieldsAndTypes.Add("requestSigningCertId");
            mdtFieldsAndTypes.Add("salesforceLoginUrl");
            mdtFieldsAndTypes.Add("samlEntityId");
            mdtFieldsAndTypes.Add("samlJitHandlerId");
            mdtFieldsAndTypes.Add("samlVersion");
            mdtFieldsAndTypes.Add("singleLogoutBinding");
            mdtFieldsAndTypes.Add("singleLogoutUrl");
            mdtFieldsAndTypes.Add("useConfigRequestMethod");
            mdtFieldsAndTypes.Add("userProvisioning");
            mdtFieldsAndTypes.Add("validationCert");
            return mdtFieldsAndTypes;
        }

        public static List<String> scontrolFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("content");
            mdtFieldsAndTypes.Add("contentSource");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("encodingKey");
            mdtFieldsAndTypes.Add("fileContent");
            mdtFieldsAndTypes.Add("fileName");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("supportsCaching");
            return mdtFieldsAndTypes;
        }

        public static List<String> serviceChannelFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("doesMinimizeWidgetOnAccept");
            mdtFieldsAndTypes.Add("interactionComponent");
            mdtFieldsAndTypes.Add("relatedEntityType");
            mdtFieldsAndTypes.Add("secondaryRoutingPriorityField");
            mdtFieldsAndTypes.Add("serviceChannelFieldPriorities");

            return mdtFieldsAndTypes;
        }

        public static List<String> servicePresenceStatusFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("channels");
            return mdtFieldsAndTypes;
        }

        //public static List<String> settingsFieldNames()
        //{
        //    List<String> mdtFieldsAndTypes = new List<String>();
        //    mdtFieldsAndTypes.Add("");
        //    return mdtFieldsAndTypes;
        //}

        public static List<String> sharedToFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("allCustomerPortalUsers");
            mdtFieldsAndTypes.Add("allInternalUsers");
            mdtFieldsAndTypes.Add("allPartnerUsers");
            mdtFieldsAndTypes.Add("channelProgramGroup");
            mdtFieldsAndTypes.Add("channelProgramGroups");
            mdtFieldsAndTypes.Add("group");
            mdtFieldsAndTypes.Add("guestUser");
            mdtFieldsAndTypes.Add("groups");
            mdtFieldsAndTypes.Add("managerSubordinates");
            mdtFieldsAndTypes.Add("managers");
            mdtFieldsAndTypes.Add("portalRole");
            mdtFieldsAndTypes.Add("portalRoleandSubordinates");
            mdtFieldsAndTypes.Add("role");
            mdtFieldsAndTypes.Add("roleAndSubordinates");
            mdtFieldsAndTypes.Add("roleAndSubordinatesInternal");
            mdtFieldsAndTypes.Add("roles");
            mdtFieldsAndTypes.Add("rolesAndSubordinates");
            mdtFieldsAndTypes.Add("territories");
            mdtFieldsAndTypes.Add("territoriesAndSubordinates");
            mdtFieldsAndTypes.Add("territory");
            mdtFieldsAndTypes.Add("territoryAndSubordinates");
            mdtFieldsAndTypes.Add("queue");
            return mdtFieldsAndTypes;
        }

        public static List<String> sharingBaseRuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("accessLevel");
            mdtFieldsAndTypes.Add("accountSettings");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("sharedTo");
            return mdtFieldsAndTypes;
        }

        public static List<String> sharingRulesFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("sharingCriteriaRules");
            mdtFieldsAndTypes.Add("sharingGuestRules");
            mdtFieldsAndTypes.Add("sharingOwnerRules");
            mdtFieldsAndTypes.Add("sharingTerritoryRules");
            return mdtFieldsAndTypes;
        }

        public static List<String> sharingSetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("accessMappings");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("profiles");
            return mdtFieldsAndTypes;
        }

        public static List<String> siteDotComFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("label");
            mdtFieldsAndTypes.Add("siteType");
            return mdtFieldsAndTypes;
        }

        public static List<String> skillFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("assignments");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("label");

            return mdtFieldsAndTypes;
        }

        public static List<String> standardValueSetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("groupingStringEnum");
            mdtFieldsAndTypes.Add("sorted");
            mdtFieldsAndTypes.Add("standardValue");
            return mdtFieldsAndTypes;
        }

        public static List<String> standardValueSetTranslationFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("valueTranslation");
            return mdtFieldsAndTypes;
        }

        public static List<String> staticResourceFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("cacheControl");
            mdtFieldsAndTypes.Add("contentType");

            return mdtFieldsAndTypes;
        }

        public static List<String> synonymDictionaryFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("groups");
            mdtFieldsAndTypes.Add("isProtected");
            mdtFieldsAndTypes.Add("label");
            return mdtFieldsAndTypes;
        }

        public static List<String> territoryFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("accountAccessLevel");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("parentTerritory");
            return mdtFieldsAndTypes;
        }

        public static List<String> territory2FieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("accountAccessLevel");
            mdtFieldsAndTypes.Add("caseAccessLevel");
            mdtFieldsAndTypes.Add("contactAccessLevel");
            mdtFieldsAndTypes.Add("customFields");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("opportunityAccessLevel");
            mdtFieldsAndTypes.Add("parentTerritory");
            mdtFieldsAndTypes.Add("ruleAssociations");
            mdtFieldsAndTypes.Add("territory2Type");
            return mdtFieldsAndTypes;
        }

        public static List<String> territory2ModelFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("customFields");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("name");
            return mdtFieldsAndTypes;
        }

        public static List<String> territory2RuleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("booleanFilter");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("objectType");
            mdtFieldsAndTypes.Add("ruleItems");
            return mdtFieldsAndTypes;
        }

        public static List<String> territory2TypeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("name");
            mdtFieldsAndTypes.Add("priority");
            return mdtFieldsAndTypes;
        }

        public static List<String> testSuiteFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("testClassName");
            return mdtFieldsAndTypes;
        }

        public static List<String> timeSheetTemplateFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("frequency");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("startDate");
            mdtFieldsAndTypes.Add("timeSheetTemplateAssignments");
            mdtFieldsAndTypes.Add("workWeekEndDay");
            mdtFieldsAndTypes.Add("workWeekStartDay");
            return mdtFieldsAndTypes;
        }

        public static List<String> topicsForObjectsFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("enableTopics");
            mdtFieldsAndTypes.Add("entityApiName");
            return mdtFieldsAndTypes;
        }

        public static List<String> transactionSecurityPolicyFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("action");
            mdtFieldsAndTypes.Add("active");
            mdtFieldsAndTypes.Add("apexClass");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("developerName");
            mdtFieldsAndTypes.Add("eventName");
            mdtFieldsAndTypes.Add("eventType");
            mdtFieldsAndTypes.Add("executionUser");
            mdtFieldsAndTypes.Add("flow");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("resourceName");
            mdtFieldsAndTypes.Add("type");
            return mdtFieldsAndTypes;
        }

        public static List<String> translationsFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("customApplications");
            mdtFieldsAndTypes.Add("customLabels");
            mdtFieldsAndTypes.Add("customPageWebLinks");
            mdtFieldsAndTypes.Add("customTabs");
            mdtFieldsAndTypes.Add("flowDefinitions");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("globalPicklists");
            mdtFieldsAndTypes.Add("prompts");
            mdtFieldsAndTypes.Add("quickActions");
            mdtFieldsAndTypes.Add("reportTypes");
            mdtFieldsAndTypes.Add("scontrols");
            return mdtFieldsAndTypes;
        }

        public static List<String> userCriteriaFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("creationAgeInSeconds");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("lastChatterActivityAgeInSeconds");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("userTypes");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveApplicationFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("assetIcon");
            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("folder");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("shares");
            mdtFieldsAndTypes.Add("templateOrigin");
            mdtFieldsAndTypes.Add("templateVersion");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveDataflowFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveDashboardFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveDatasetFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveLensFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveRecipeFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveTemplateBundleFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> waveXmdFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("");
            return mdtFieldsAndTypes;
        }

        public static List<String> workflowFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("alerts");
            mdtFieldsAndTypes.Add("fieldUpdates");
            mdtFieldsAndTypes.Add("flowActions");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("knowledgePublishes");
            mdtFieldsAndTypes.Add("outboundMessages");
            mdtFieldsAndTypes.Add("rules");
            mdtFieldsAndTypes.Add("tasks");

            return mdtFieldsAndTypes;
        }

        public static List<String> workSkillRoutingFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("masterLabel");
            mdtFieldsAndTypes.Add("relatedEntity");
            mdtFieldsAndTypes.Add("workSkillRoutingAttributes");
            return mdtFieldsAndTypes;
        }


        /*****************************************************************************************************/
        // Sub Types
        /*****************************************************************************************************/
        public static List<String> actionOverrideFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("actionName");
            mdtFieldsAndTypes.Add("comment");
            mdtFieldsAndTypes.Add("content");
            mdtFieldsAndTypes.Add("formFactor");
            mdtFieldsAndTypes.Add("skipRecordTypeSelect");
            mdtFieldsAndTypes.Add("type");

            return mdtFieldsAndTypes;
        }

        public static List<String> businessProcessFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("description");
            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("isActive");
            mdtFieldsAndTypes.Add("formFactor");
            mdtFieldsAndTypes.Add("namespacePrefix");
            mdtFieldsAndTypes.Add("values");

            return mdtFieldsAndTypes;
        }

        public static List<String> compactLayoutFieldNames()
        {
            List<String> mdtFieldsAndTypes = new List<String>();

            mdtFieldsAndTypes.Add("fullName");
            mdtFieldsAndTypes.Add("fields");
            mdtFieldsAndTypes.Add("label");

            return mdtFieldsAndTypes;
        }


        /*****************************************************************************************************/
        public static String folderToType(String folderName, String fileExtension)
        {
            String typeName = "";

            if (folderName == "accountRelationshipShareRules")
            {
                typeName = "AccountRelationshipShareRule";
            }
            else if (folderName == "actionLinkGroupTemplates")
            {
                typeName = "ActionLinkGroupTemplate";
            }
            else if (folderName == "actionPlanTemplates")
            {
                typeName = "ActionPlanTemplate";
            }
            else if (folderName == "analyticSnapshots")
            {
                typeName = "AnalyticSnapshot";
            }
            else if (folderName == "animationRules")
            {
                typeName = "AnimationRule";
            }
            else if (folderName == "objects" && fileExtension == "__kav")
            {
                typeName = "ArticleType";
            }
            else if (folderName == "classes")
            {
                typeName = "ApexClass";
            }
            else if (folderName == "components")
            {
                typeName = "ApexComponent";
            }
            else if (folderName == "pages")
            {
                typeName = "ApexPage";
            }
            else if (folderName == "testSuites")
            {
                typeName = "ApexTestSuite";
            }
            else if (folderName == "triggers")
            {
                typeName = "ApexTrigger";
            }
            else if (folderName == "apexEmailNotifications")
            {
                typeName = "ApexEmailNotifications";
            }
            else if (folderName == "appMenus")
            {
                typeName = "AppMenu";
            }
            else if (folderName == "appointmentSchedulingPolicies")
            {
                typeName = "AppointmentSchedulingPolicy";
            }
            else if (folderName == "approvalProcesses")
            {
                typeName = "ApprovalProcess";
            }
            else if (folderName == "assignmentRules")
            {
                typeName = "AssignmentRules";
            }
            else if (folderName == "audience")
            {
                typeName = "Audience";
            }
            //else if (folderName == "")
            //{
            //    typeName = "AuraDefinitionBundle";
            //}
            else if (folderName == "authproviders")
            {
                typeName = "AuthProvider";
            }
            else if (folderName == "autoResponseRules")
            {
                typeName = "AutoResponseRules";
            }
            // TODO: Update the bot handling
            //else if (folderName == "bot")
            //{
            //    typeName = "Bot";
            //}
            //else if (folderName == "bot")
            //{
            //    typeName = "BotVersion";
            //}
            else if (folderName == "brandingSets")
            {
                typeName = "BrandingSet";
            }
            else if (folderName == "callCenters")
            {
                typeName = "CallCenter";
            }
            else if (folderName == "campaignInfluenceModels")
            {
                typeName = "CampaignInfluenceModel";
            }
            else if (folderName == "CaseSubjectParticles")
            {
                typeName = "CaseSubjectParticle";
            }
            else if (folderName == "certs")
            {
                typeName = "Certificate";
            }
            //else if (folderName == "")
            //{
            //    typeName = "ChatterExtension";
            //}
            else if (folderName == "cleanDataServices")
            {
                typeName = "CleanDataService";
            }
            else if (folderName == "cmsConnectSource")
            {
                typeName = "CMSConnectSource";
            }
            else if (folderName == "communities")
            {
                typeName = "Community";
            }
            else if (folderName == "communityTemplateDefinitions")
            {
                typeName = "CommunityTemplateDefinition";
            }
            else if (folderName == "communityThemeDefinitions")
            {
                typeName = "CommunityThemeDefinition";
            }
            else if (folderName == "connectedApps")
            {
                typeName = "ConnectedApp";
            }
            else if (folderName == "contentassets")
            {
                typeName = "ContentAsset";
            }
            else if (folderName == "corsWhitelistOrigins")
            {
                typeName = "CorsWhitelistOrigin";
            }
            else if (folderName == "cspTrustedSites")
            {
                typeName = "CspTrustedSite";
            }
            else if (folderName == "applications")
            {
                typeName = "CustomApplication";
            }
            else if (folderName == "customApplicationComponents")
            {
                typeName = "CustomApplicationComponent";
            }
            else if (folderName == "feedFilters")
            {
                typeName = "CustomFeedFilter";
            }
            else if (folderName == "customHelpMenuSections")
            {
                typeName = "CustomHelpMenuSection";
            }
            else if (folderName == "labels")
            {
                typeName = "CustomLabels";
            }
            else if (folderName == "customMetadata")
            {
                typeName = "CustomMetadata";
            }
            else if (folderName == "notificationtypes")
            {
                typeName = "CustomNotificationType";
            }
            else if (folderName == "objects")
            {
                typeName = "CustomObject";
            }
            else if (folderName == "objectTranslations")
            {
                typeName = "CustomObjectTranslation";
            }
            else if (folderName == "weblinks")
            {
                typeName = "CustomPageWebLink";
            }
            else if (folderName == "customPermissions")
            {
                typeName = "CustomPermission";
            }
            else if (folderName == "sites")
            {
                typeName = "CustomSite";
            }
            else if (folderName == "tabs")
            {
                typeName = "CustomTab";
            }
            //else if (folderName == "")
            //{
            //    typeName = "CustomValue";
            //}
            else if (folderName == "dashboards")
            {
                typeName = "Dashboard";
            }
            else if (folderName == "datacategorygroups")
            {
                typeName = "DataCategoryGroup";
            }
            else if (folderName == "delegateGroups")
            {
                typeName = "DelegateGroup";
            }
            //else if (folderName == "")
            //{
            //    typeName = "Document";
            //}
            else if (folderName == "duplicateRules")
            {
                typeName = "DuplicateRule";
            }
            else if (folderName == "eclair")
            {
                typeName = "EclairGeoData";
            }
            else if (folderName == "emailservices")
            {
                typeName = "EmailServicesFunction";
            }
            //else if (folderName == "")
            //{
            //    typeName = "EmailTemplate";
            //}
            else if (folderName == "EmbeddedServiceBranding")
            {
                typeName = "EmbeddedServiceBranding";
            }
            else if (folderName == "EmbeddedServiceConfig")
            {
                typeName = "EmbeddedServiceConfig";
            }
            else if (folderName == "EmbeddedServiceFieldService")
            {
                typeName = "EmbeddedServiceFieldService";
            }
            else if (folderName == "EmbeddedServiceFlowConfig")
            {
                typeName = "EmbeddedServiceFlowConfig";
            }
            else if (folderName == "EmbeddedServiceLiveAgent")
            {
                typeName = "EmbeddedServiceLiveAgent";
            }
            //else if (folderName == "")
            //{
            //    typeName = "EmbeddedServiceMenuSettings";
            //}
            else if (folderName == "entitlementProcesses")
            {
                typeName = "EntitlementProcess";
            }
            else if (folderName == "entitlementTemplates")
            {
                typeName = "EntitlementTemplate";
            }
            else if (folderName == "escalationRules")
            {
                typeName = "EscalationRules";
            }
            else if (folderName == "eventDeliveries")
            {
                typeName = "EventDelivery";
            }
            else if (folderName == "eventSubscriptions")
            {
                typeName = "EventSubscription";
            }
            else if (folderName == "experiences")
            {
                typeName = "ExperienceBundle";
            }
            else if (folderName == "dataSources")
            {
                typeName = "ExternalDataSource";
            }
            else if (folderName == "externalServiceRegistrations")
            {
                typeName = "ExternalServiceRegistration";
            }
            else if (folderName == "featureParameters" && fileExtension == "featureParameterBoolean")
            {
                typeName = "FeatureParameterBoolean";
            }
            else if (folderName == "featureParameters" && fileExtension == "featureParameterDate")
            {
                typeName = "FeatureParameterDate";
            }
            else if (folderName == "featureParameters" && fileExtension == "featureParameterInteger")
            {
                typeName = "FeatureParameterInteger";
            }
            else if (folderName == "flexipages")
            {
                typeName = "FlexiPage";
            }
            else if (folderName == "flows")
            {
                typeName = "Flow";
            }
            else if (folderName == "flowCategories")
            {
                typeName = "FlowCategory";
            }
            else if (folderName == "flowDefinitions")
            {
                typeName = "FlowDefinition";
            }
            //else if (folderName == "")
            //{
            //    typeName = "Folder";
            //}
            else if (folderName == "globalPicklist")
            {
                typeName = "GlobalPicklist";
            }
            //else if (folderName == "")
            //{
            //    typeName = "GlobalPicklistValue";
            //}
            else if (folderName == "globalValueSets")
            {
                typeName = "GlobalValueSet";
            }
            else if (folderName == "globalValueSetTranslations")
            {
                typeName = "GlobalValueSetTranslation";
            }
            else if (folderName == "groups")
            {
                typeName = "Group";
            }
            else if (folderName == "homepagecomponents")
            {
                typeName = "HomePageComponent";
            }
            else if (folderName == "homePageLayouts")
            {
                typeName = "HomePageLayout";
            }
            else if (folderName == "installedPackages")
            {
                typeName = "InstalledPackage";
            }
            else if (folderName == "moderation")
            {
                typeName = "KeywordList";
            }
            else if (folderName == "layouts")
            {
                typeName = "Layout";
            }
            else if (folderName == "LeadConvertSettings")
            {
                typeName = "LeadConvertSettings";
            }
            //else if (folderName == "")
            //{
            //    typeName = "Letterhead";
            //}
            else if (folderName == "lightningBolts")
            {
                typeName = "LightningBolt";
            }
            //else if (folderName == "")
            //{
            //    typeName = "LightningComponentBundle";
            //}
            else if (folderName == "lightningExperienceThemes")
            {
                typeName = "LightningExperienceTheme";
            }
            else if (folderName == "messageChannels")
            {
                typeName = "LightningMessageChannel";
            }
            else if (folderName == "liveChatAgentConfigs")
            {
                typeName = "LiveChatAgentConfig";
            }
            else if (folderName == "liveChatButtons")
            {
                typeName = "LiveChatButton";
            }
            else if (folderName == "liveChatDeployments")
            {
                typeName = "LiveChatDeployment";
            }
            else if (folderName == "")
            {
                typeName = "LiveChatSensitiveDataRule";
            }
            else if (folderName == "managedContentTypes")
            {
                typeName = "ManagedContentType";
            }
            else if (folderName == "managedTopics")
            {
                typeName = "ManagedTopics";
            }
            else if (folderName == "matchingRules")
            {
                typeName = "MatchingRule";
            }
            //else if (folderName == "")
            //{
            //    typeName = "Metadata";
            //}
            //else if (folderName == "")
            //{
            //    typeName = "MetadataWithContent";
            //}
            else if (folderName == "milestoneTypes")
            {
                typeName = "MilestoneType";
            }
            else if (folderName == "mlDomains")
            {
                typeName = "MlDomain";
            }
            else if (folderName == "MobileApplicationDetails")
            {
                typeName = "MobileApplicationDetail";
            }
            else if (folderName == "moderation")
            {
                typeName = "ModerationRule";
            }
            else if (folderName == "myDomainDiscoverableLogins")
            {
                typeName = "MyDomainDiscoverableLogin";
            }
            else if (folderName == "namedCredentials")
            {
                typeName = "NamedCredential";
            }
            else if (folderName == "navigationMenus")
            {
                typeName = "NavigationMenu";
            }
            else if (folderName == "networks")
            {
                typeName = "Network";
            }
            else if (folderName == "networkBranding")
            {
                typeName = "NetworkBranding";
            }
            else if (folderName == "notificationTypeConfig")
            {
                typeName = "NotificationTypeConfig";
            }
            else if (folderName == "oauthcustomscopes")
            {
                typeName = "OauthCustomScope";
            }
            //else if (folderName == "")
            //{
            //    typeName = "Package";
            //}
            else if (folderName == "pathAssistants")
            {
                typeName = "PathAssistant";
            }
            else if (folderName == "paymentGatewayProvider")
            {
                typeName = "PaymentGatewayProvider";
            }
            else if (folderName == "permissionsets")
            {
                typeName = "PermissionSet";
            }
            else if (folderName == "permissionsetgroups")
            {
                typeName = "PermissionSetGroup";
            }
            else if (folderName == "cachePartitions")
            {
                typeName = "PlatformCachePartition";
            }
            else if (folderName == "platformEventChannels")
            {
                typeName = "PlatformEventChannel";
            }
            else if (folderName == "platformEventChannelMembers")
            {
                typeName = "PlatformEventChannelMember";
            }
            else if (folderName == "portals")
            {
                typeName = "Portal";
            }
            else if (folderName == "postTemplates")
            {
                typeName = "PostTemplate";
            }
            else if (folderName == "presenceDeclineReasons")
            {
                typeName = "PresenceDeclineReason";
            }
            else if (folderName == "presenceUserConfigs")
            {
                typeName = "PresenceUserConfig";
            }
            else if (folderName == "profiles")
            {
                typeName = "Profile";
            }
            //else if (folderName == "")
            //{
            //    typeName = "ProfileActionOverride";
            //}
            else if (folderName == "profilePasswordPolicies")
            {
                typeName = "ProfilePasswordPolicy";
            }
            else if (folderName == "profileSessionSettings")
            {
                typeName = "ProfileSessionSetting";
            }
            else if (folderName == "prompts")
            {
                typeName = "Prompt";
            }
            else if (folderName == "queues")
            {
                typeName = "Queue";
            }
            else if (folderName == "queueRoutingConfigs")
            {
                typeName = "QueueRoutingConfig";
            }
            else if (folderName == "quickActions")
            {
                typeName = "QuickAction";
            }
            else if (folderName == "redirectWhitelistUrls")
            {
                typeName = "RedirectWhitelistUrl";
            }
            else if (folderName == "recommendationStrategies")
            {
                typeName = "RecommendationStrategy";
            }
            else if (folderName == "recordActionDeployments")
            {
                typeName = "RecordActionDeployment";
            }
            else if (folderName == "remoteSiteSettings")
            {
                typeName = "RemoteSiteSetting";
            }
            //else if (folderName == "reports")
            //{
            //    typeName = "Report";
            //}
            else if (folderName == "reportTypes")
            {
                typeName = "ReportType";
            }
            else if (folderName == "roles")
            {
                typeName = "Role";
            }
            //else if (folderName == "")
            //{
            //    typeName = "RoleOrTerritory";
            //}
            else if (folderName == "samlssoconfigs")
            {
                typeName = "SamlSsoConfig";
            }
            else if (folderName == "scontrols")
            {
                typeName = "Scontrol";
            }
            else if (folderName == "serviceChannels")
            {
                typeName = "ServiceChannel";
            }
            else if (folderName == "servicePresenceStatuses")
            {
                typeName = "ServicePresenceStatus";
            }
            else if (folderName == "settings")
            {
                typeName = "Settings";
            }
            //else if (folderName == "")
            //{
            //    typeName = "SharedTo";
            //}
            //else if (folderName == "sharingRules")
            //{
            //    typeName = "SharingBaseRule";
            //}
            else if (folderName == "sharingRules")
            {
                typeName = "SharingRules";
            }
            else if (folderName == "sharingSets")
            {
                typeName = "SharingSet";
            }
            //else if (folderName == "siteDotComSites")
            //{
            //    typeName = "SiteDotCom";
            //}
            else if (folderName == "skills")
            {
                typeName = "Skill";
            }
            else if (folderName == "standardValueSets")
            {
                typeName = "StandardValueSet";
            }
            else if (folderName == "standardValueSetTranslations")
            {
                typeName = "StandardValueSetTranslation";
            }
            else if (folderName == "staticresources")
            {
                typeName = "StaticResource";
            }
            else if (folderName == "synonymDictionaries")
            {
                typeName = "SynonymDictionary";
            }
            else if (folderName == "territories")
            {
                typeName = "Territory";
            }
            else if (folderName == "territories")
            {
                typeName = "Territory2";
            }
            else if (folderName == "territory2Models")
            {
                typeName = "Territory2Model";
            }
            else if (folderName == "rules")
            {
                typeName = "Territory2Rule";
            }
            else if (folderName == "territory2Types")
            {
                typeName = "Territory2Type";
            }
            else if (folderName == "timeSheetTemplates")
            {
                typeName = "TimeSheetTemplate";
            }
            else if (folderName == "topicsForObjects")
            {
                typeName = "TopicsForObjects";
            }
            else if (folderName == "transactionSecurityPolicies")
            {
                typeName = "TransactionSecurityPolicy";
            }
            else if (folderName == "translations")
            {
                typeName = "Translations";
            }
            else if (folderName == "userCriteria")
            {
                typeName = "UserCriteria";
            }
            else if (folderName == "")
            {
                typeName = "WaveApplication";
            }
            else if (folderName == "")
            {
                typeName = "WaveDataflow";
            }
            else if (folderName == "")
            {
                typeName = "WaveDashboard";
            }
            else if (folderName == "")
            {
                typeName = "WaveDataset";
            }
            else if (folderName == "")
            {
                typeName = "WaveLens";
            }
            else if (folderName == "")
            {
                typeName = "WaveRecipe";
            }
            else if (folderName == "")
            {
                typeName = "WaveTemplateBundle";
            }
            else if (folderName == "")
            {
                typeName = "WaveXmd";
            }
            else if (folderName == "workflows")
            {
                typeName = "Workflow";
            }
            else if (folderName == "workSkillRoutings")
            {
                typeName = "WorkSkillRouting";
            }

            return typeName;
        }



        public static String typeToFolder(String typeName)
        {
            String folderName = "";

            if (typeName == "CustomObject")
            {
                folderName = "objects";
            }
            else if (typeName == "PermissionSet")
            {
                folderName = "permissionsets";
            }
            else if (typeName == "Profile")
            {
                folderName = "profiles";
            }

            return folderName;
        }
        /*****************************************************************************************************/



        /*****************************************************************************************************/
        // This readjusts the column names to allow for a better report format
        // The reason for the two blanks at the beginning are for the purpose of finding the Index which leads to setting 
        // the proper Excel column. For example, when getting the IndexOf("field"), the return value will be 2.
        // Excel column numbers cannot be set to 0.
        public static List<String> getSubHeaderNames(String folderName, String parentNode)
        {
            List<String> subHeaderNames = new List<string>();
            if (folderName == "permissionsets")
            {
                if (parentNode == "fieldPermissions")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("field");
                    subHeaderNames.Add("readable");
                    subHeaderNames.Add("editable");
                }
                else if (parentNode == "objectPermissions")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("object");
                    subHeaderNames.Add("allowCreate");
                    subHeaderNames.Add("allowRead");
                    subHeaderNames.Add("allowEdit");
                    subHeaderNames.Add("allowDelete");
                    subHeaderNames.Add("viewAllRecords");
                    subHeaderNames.Add("modifyAllRecords");
                }
                else if (parentNode == "recordTypeVisibilities")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("recordType");
                    subHeaderNames.Add("visible");
                    subHeaderNames.Add("default");
                    subHeaderNames.Add("personAccountDefault");
                }
                else if (parentNode == "userPermissions")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("name");
                    subHeaderNames.Add("enabled");
                }
            }
            else if (folderName == "profiles")
            {
                if (parentNode == "fieldPermissions")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("field");
                    subHeaderNames.Add("readable");
                    subHeaderNames.Add("editable");
                }
                else if (parentNode == "objectPermissions")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("object");
                    subHeaderNames.Add("allowCreate");
                    subHeaderNames.Add("allowRead");
                    subHeaderNames.Add("allowEdit");
                    subHeaderNames.Add("allowDelete");
                    subHeaderNames.Add("viewAllRecords");
                    subHeaderNames.Add("modifyAllRecords");
                }
                else if (parentNode == "recordTypeVisibilities")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("recordType");
                    subHeaderNames.Add("visible");
                    subHeaderNames.Add("default");
                    subHeaderNames.Add("personAccountDefault");
                }
                else if (parentNode == "userPermissions")
                {
                    subHeaderNames.Add("");
                    subHeaderNames.Add("");
                    subHeaderNames.Add("name");
                    subHeaderNames.Add("enabled");
                }
            }

            return subHeaderNames;
        }

        public static String getNameField(String node1Name, String node2Name, String xmlNode)
        {
            String nameFieldValue = "";

            if (node1Name == "AppMenu"
                && node2Name == "appMenuItems")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "AssignmentRules"
                     && node2Name == "assignmentRule")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "ApprovalProcess"
                     && node2Name == "approvalStep")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "ApprovalProcess"
                     && node2Name == "finalApprovalActions")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "ApprovalProcess"
                     && node2Name == "finalRejectionActions")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "AssignmentRules"
                     && node2Name == "assignmentRule")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CleanDataService"
                    && node2Name == "cleanRules")
            {
                nameFieldValue = getNameValue(node2Name, "masterLabel", xmlNode);
            }
            else if (node1Name == "CustomApplication"
                    && node2Name == "actionOverrides")
            {
                nameFieldValue = getNameValue(node2Name, "actionName", "formFactor", xmlNode);
            }
            else if (node1Name == "CustomApplication"
                    && node2Name == "profileActionOverrides")
            {
                nameFieldValue = getNameValue(node2Name, "actionName", "formFactor", xmlNode);
            }
            else if (node1Name == "CustomLabels"
                    && node2Name == "labels")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomMetadata"
                    && node2Name == "values")
            {
                nameFieldValue = getNameValue(node2Name, "field", xmlNode);
            }
            else if (node1Name == "CustomObject"
                   && node2Name == "actionOverrides")
            {
                nameFieldValue = getNameValue(node2Name, "actionName", "formFactor", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "businessProcesses")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "compactLayouts")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "fields")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "fieldSets")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "listViews")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "recordTypes")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "validationRules")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObject"
                && node2Name == "webLinks")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "CustomObjectTranslation"
                && node2Name == "fields")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "CustomObjectTranslation"
                && node2Name == "layouts")
            {
                nameFieldValue = getNameValue(node2Name, "layout", xmlNode);
            }
            else if (node1Name == "CustomObjectTranslation"
                && node2Name == "quickActions")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "CustomObjectTranslation"
                && node2Name == "validationRules")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "CustomObjectTranslation"
                && node2Name == "webLinks")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "DataCategoryGroup"
                && node2Name == "dataCategory")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "EmailServicesFunction"
                && node2Name == "emailServicesAddresses")
            {
                nameFieldValue = getNameValue(node2Name, "developerName", xmlNode);
            }
            else if (node1Name == "FlexiPage"
                && node2Name == "flexiPageRegions")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "actionCalls")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "assignments")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "choices")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "decisions")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "dynamicChoiceSets")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "loops")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "processMetadataValues")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "recordCreates")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "recordLookups")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "recordUpdates")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "screens")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "subflows")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Flow"
                && node2Name == "variables")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Forecasting"
                && node2Name == "forecastingTypeSettings")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "GlobalValueSet"
                && node2Name == "customValue")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Layout"
                && node2Name == "layoutSections")
            {
                nameFieldValue = getNameValue(node2Name, "label", xmlNode);
            }
            else if (node1Name == "Layout"
                && node2Name == "relatedLists")
            {
                nameFieldValue = getNameValue(node2Name, "relatedList", xmlNode);
            }
            else if (node1Name == "ManagedContentType"
                && node2Name == "managedContentNodeTypes")
            {
                nameFieldValue = getNameValue(node2Name, "nodeName", xmlNode);
            }
            else if (node1Name == "ManagedTopics"
                && node2Name == "managedTopic")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "NavigationMenu"
                && node2Name == "navigationMenuItem")
            {
                nameFieldValue = getNameValue(node2Name, "label", xmlNode);
            }
            else if (node1Name == "PathAssistant"
                && node2Name == "pathAssistantSteps")
            {
                nameFieldValue = getNameValue(node2Name, "picklistValueName", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "applicationVisibilities")
            {
                nameFieldValue = getNameValue(node2Name, "application", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "classAccesses")
            {
                nameFieldValue = getNameValue(node2Name, "apexClass", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "fieldPermissions")
            {
                nameFieldValue = getNameValue(node2Name, "field", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "objectPermissions")
            {
                nameFieldValue = getNameValue(node2Name, "object", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "pageAccesses")
            {
                nameFieldValue = getNameValue(node2Name, "apexPage", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "recordTypeVisibilities")
            {
                nameFieldValue = getNameValue(node2Name, "recordType", xmlNode);
            }
            else if (node1Name == "PermissionSet"
                && node2Name == "tabSettings")
            {
                nameFieldValue = getNameValue(node2Name, "tab", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "applicationVisibilities")
            {
                nameFieldValue = getNameValue(node2Name, "application", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "classAccesses")
            {
                nameFieldValue = getNameValue(node2Name, "apexClass", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "customSettingAccesses")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "fieldPermissions")
            {
                nameFieldValue = getNameValue(node2Name, "field", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "fieldPermissions")
            {
                nameFieldValue = getNameValue(node2Name, "field", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "flowAccesses")
            {
                nameFieldValue = getNameValue(node2Name, "flow", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "layoutAssignments")
            {
                nameFieldValue = getNameValue(node2Name, "layout", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "loginFlows")
            {
                nameFieldValue = getNameValue(node2Name, "flow", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "objectPermissions")
            {
                nameFieldValue = getNameValue(node2Name, "object", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "pageAccesses")
            {
                nameFieldValue = getNameValue(node2Name, "apexPage", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "recordTypeVisibilities")
            {
                nameFieldValue = getNameValue(node2Name, "recordType", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "tabVisibilities")
            {
                nameFieldValue = getNameValue(node2Name, "tab", xmlNode);
            }
            else if (node1Name == "Profile"
                && node2Name == "userPermissions")
            {
                nameFieldValue = getNameValue(node2Name, "name", xmlNode);
            }
            else if (node1Name == "Prompt"
                && node2Name == "promptVersions")
            {
                nameFieldValue = getNameValue(node2Name, "masterLabel", xmlNode);
            }
            else if (node1Name == "SharingRules"
                && node2Name == "sharingOwnerRules")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "Workflow"
                && node2Name == "alerts")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "Workflow"
                && node2Name == "fieldUpdates")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "Workflow"
                && node2Name == "outboundMessages")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "Workflow"
                && node2Name == "rules")
            {
                nameFieldValue = getNameValue(node2Name, "fullName", xmlNode);
            }
            else if (node1Name == "WorkSkillRouting"
                && node2Name == "workSkillRoutingAttributes")
            {
                nameFieldValue = getNameValue(node2Name, "skill", xmlNode);
            }

            return nameFieldValue;
        }

        public static String getNameValue(String parentNodeName, String nameTag, String xmlNode)
        {
            String nameFieldValue = "";

            String xmlString = "<document>" + xmlNode + "</document>";

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xmlString);

            XmlNodeList nodeList = xd.GetElementsByTagName(nameTag);

            foreach (XmlNode xn in nodeList)
            {
                if (xn.Name == nameTag
                    && xn.ParentNode.Name == parentNodeName)
                {
                    nameFieldValue = xn.InnerText;
                }
            }

            return nameFieldValue;
        }

        public static String getNameValue(String parentNodeName, String nameTag1, String nameTag2, String xmlNode)
        {
            String nameFieldValue = "";
            String nameFieldValue1 = "";
            String nameFieldValue2 = "";

            String xmlString = "<document>" + xmlNode + "</document>";

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xmlString);

            XmlNodeList nodeList1 = xd.GetElementsByTagName(nameTag1);
            XmlNodeList nodeList2 = xd.GetElementsByTagName(nameTag2);

            foreach (XmlNode xn in nodeList1)
            {
                if (xn.Name == nameTag1
                    && xn.ParentNode.Name == parentNodeName)
                {
                    nameFieldValue1 = xn.InnerText;
                }
            }

            foreach (XmlNode xn in nodeList2)
            {
                if (xn.Name == nameTag2
                    && xn.ParentNode.Name == parentNodeName)
                {
                    nameFieldValue2 = xn.InnerText;
                }
            }


            if (nameFieldValue2 == "")
            {
                nameFieldValue = nameFieldValue1;
            }
            else
            {
                nameFieldValue = nameFieldValue1 + "_" + nameFieldValue2;
            }

            return nameFieldValue;
        }


        public class MetadataFieldTypes
        {
            public String fieldName;
            public String fieldType;
            public Boolean isRequired;
            public Boolean isUnique;

            //public MetadataFieldTypes() { }

            public MetadataFieldTypes(String fn, String ft, Boolean req, Boolean unique)
            {
                this.fieldName = fn;
                this.fieldType = ft;
                this.isRequired = req;
                this.isUnique = unique;
            }
        }

    }
}

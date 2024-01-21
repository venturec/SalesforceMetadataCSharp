NOTES:
I have built this on the fly as needed and have not implemented a lot of asynchronous processes except for in the Retrieve Metadata. 

The current application is built in Visual Studio using .NET 4.8 meaning you can only run this on a Windows machine with .NET 4.8 runtime currently. 

It is advisable, with the different reports available, to have Microsoft Excel installed. Most of the exports will allow for CSV which you can import into a spreadsheet or database table of your choosing, but the reports are geared towards using Excel as the most effective tool.

This software tool also uses the following:
* Salesforce Partner WSDL API 59.0
* Salesforce Metadata WSDL API 59.0
* Salesforce Tooling WSDL API 59.0

The main ares for enhancement, which will be my focus in 2024, are the following:
* Build more asynchronous processes
* Move this tool into .NET 7 or possibly .NET 8 (depending on stability) and test. Since .NET 8 is still fairly new, will need to make sure any immediate bugs are worked out. 
* Will be generating a How-To document along with a Youtube video which walks users through the steps from initial setup to processes you can follow for handling the metadata and running the tooling reports.

If you want to use the PDF Parser, make sure to have the itextsharp.dll located in the same location as the executable. This library allows you to parse a PDF file as a flat text file and there is an option to include additional PDF text type values. These text type values allow you to import the flat file as a tab-delimited CSV file into a database table and query out the titles, sub-titles, etc. This helps when parsing through the Salesforce PDF Developer Docs to funnel down to the core pieces needed to build a code library, or at least that is what I've done with it.

The itextsharp.dll is located in the References folder.


/************ Setting up the security structure, selecting your default Metadata folders to be selected and encrypting the credentials to be stored on your machine ************/

The SalesforceMetadata application uses AES-2048 encryption to encrypt the user settings file and requires:
    A master password
    A salt

Create the first initial encrypted text file
    Create an XML file called SFLogins.xml but leave it empty.
    
    Create a text file in another location either on the current machine or a network drive called SharedSecret.txt and type in a password of your choosing.
    The value here will be used as the master password.
    
    Generate a Salt and hold onto this. You will need it to encrypt / decrypt the XML file. If you lose it, you will need to regenerate your XML file. You can use a random password generator for this or one of your choosing.
    

When you first run the tool, it will open up the User Settings form.
    In the first box, double click to select the path to the SFLogins.xml file
    In the second box, double click to select the path to the master password file
    In the third box, use a random password generator to generate the Salt
    
Click the Encrypt / Decrypt button

Copy and paste the text in the ExampleUserSettings.xml file into the Decrypted text field.

You will need update the username, password and, depending on the IP address, whether it is trusted or in the trusted IP range, possibly a security token.

The other items are related to what the default Metadata types selected will be when downloading the package.xml. Add/Remove any you don't necessary need as defaults.

After you are done, click the Encrypt button. You will see the encrypted text in the Encrypted Text box.

Click the Save Encrypted To File button to save the encrypted text to the SFLogins.xml file.

Click Save.

To confirm whether the encrypted text is decrypting successfully, on the Landing Page form, click Metadata Form. You should see your username(s) in the drop down. Click the Get Metadata Types button to see if a list of available metadata types is returned.



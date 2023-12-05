NOTES:
I have built this on the fly as needed and have not implemented a lot of asynchronous processes (I know). This is something I'm going to be working on and will enhance the forms to utilize asynchronous processes during long retrievals allowing the use of the app while tasks are running in the background.


/************ Setting up the security structure, selecting your default Metadata folders to be selected and encrypting the credentials to be stored on your machine ************/

The SalesforceMetadata application uses AES-2048 encryption to encrypt the user settings file and requires:
    A master password
    A salt

Create the first initial encrypted text file
    Create an XML file called SFLogins.xml but leave it empty.
    
    Create a text file in another location either on the current machine or a network drive called SharedSecret.txt and type in a password of your choosing.
    The value here will be used as the master password
    
    Generate a Salt and hold onto this. You will need it to encrypt / decrypt the XML file. If you lose it, you will need to regenerate your XML file. You can use a random           password generator for this or one of your choosing.
    

When you first run the tool, it will open up the User Settings form.
    In the first box, double click to select the path to the SFLogins.xml file
    In the second box, double click to select the path to the master password file
    In the third box, use a random password generator to generate the Salt
    
Click the Encrypt / Decrypt button

Copy and paste the text in the ExampleUserSettings.xml file into the Decrypted text field.

You will need update the username and possibly add a security token.

The other items are related to what the default Metadata types selected will be when downloading the package.xml. Add/Remove any you don't necessary need as defaults.

After you are done, click the Encrypt button. You will see the encrypted text in the Encrypted Text box.

Click the Save Encrypted To File button to save the encrypted text to the SFLogins.xml file.

Click Save.

To confirm whether the encrypted text is decrypting successfully, on the Landing Page form, click Metadata Form. You should see your username(s) in the drop down. Click the Get Metadata Types button to see if a list of available metadata types is returned.



/************ Items to complete ************/
There are many items which still need to be completed or updated, but the below list provides a few items which could definitely be enhanced to make this tool more user friendly.

The primary one is for long running tasks such as downloading the metadata package, genereating the Excel reports, etc, spinning up a new thread for asynchronous processing needs to be implemented. Since I built this myself, I did not put the time into creating threads. However this would be nice.

For the GenerateDeploymentPackage, after running the Metadata comparison, then clicking the Generate Deployment Package button, bringing over the differences, whether all or selected into the GenerateDeploymentPackage tree view would be more efficient. There are some caveats to this though which need to be considered, especially since there are required metadata items which need to be selected when another item is brought over from the difference.

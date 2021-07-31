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

To confirm whether the encrypted text is decrypting successfully, on the Landing Page form, click Metadata Form. You should see your username(s) in the drop down and when you select one, the Security Token field should also be populated if you filled that value in the security token xml element.


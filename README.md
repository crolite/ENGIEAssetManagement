# ENGIE Asset Management

## Team 11 CSC2033 Coursework

### App with QR scanning functionality that returns corresponding asset forms from a firebase store. 

##### Getting Started:
To boot the app on an android device, open the solution in Visual Studio and connect the device to your computer and ensure USB debugging is enabled. See how to this here: https://developer.android.com/studio/debug/dev-options
Once connected find the green play button at the top of the window and select your device from the drop down. Then click the button to launch the app on your device. It will now remain on the device and can be launched as normal.
For the functionality of this application it is essential the target device has an internet connection.

##### Logging In:
If you already have access to account details, enter these into the login page and press login and you will be allowed access to the app.
If you don't have login details you can create an account br clicking the text at the bottom of the login page.
Accounts must have a unique username and a password containing at least on lower case, upper case letter and a symbol and must be at least 8 characters in length.
Once the account is created it will automatically log you in. In the next session the account details you created can be used to login.

##### Admin Functionality:
If an account is made an admin account, logging into it will take the user to the admin page where all registered users can be listed.
In order to change an account to have the admin role a change must be made in the firebase database.
In order to make these changes you must be added as an editor of the project. To sort this get in touch with one of the developers.
To change an account to admin simply edit the admin value in the accounts record to read true instead of false.
For testing purposes there is an admin account currently available with username "RegTest" and password "P@ssword1".

##### QR Scanning:
Upon clicking the QR scanner option you may be asked to give the app permission to use the camera, click accept to allow this.
Once the camera is on, point it at the QR code you wish to scan. If the QR code is read the value it stores will appear in a box beneath the ENGIE logo.
If the value is a valid file name you can press the get file button and a download link will be generated and opeed in the devices default browser, beggining the download of the requested file.
This file can then be accessed on the device to open or attach to an email and send on.
If the value isn't a stored file then an invalid QR code message will be shown to you in place of the QR value.

##### QR Codes:
In the project file there are 5 QR codes that are valid for the 5 currently stored files.
To make more valid so other files are available, the file must first be added to the firebase storage system. 
Once the file is stored, the filename with it's extension can be used to generate a QR code, eg "text.txt" would be the value of the QR code.
Then scanning this new QR code should begin downloading the stored file.

##### Firebase:
The back end of the application is hosted on firebase. To get access to this to add documents or eddit database values, get in touch with developers, either on the dedicated teams channel or at academic emails.
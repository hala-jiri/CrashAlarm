# CrashAlarm

Project for subject **Introduction to Mobile Technologies**.

**Jiří Hála**, hala.jiri@gmail.com, 2020



## About Application

* Reason = help people when they are „lost“.
* By click on one button – send all your prefilled contacts a message with precise location.

![Demo view](https://github.com/hala-jiri/CrashAlarm/blob/master/CrashAlarmDemoView.png)

This application should help people get localized when they are lost somewhere, for example in the forest.
This can happen to seniors or even to our kids.
In this time almost everyone has some kind of a smart phone. Using it to vocally explain to someone where their position is can be very hard.

This application serves as a step between someone who is lost and someone who should find them and help them.

The user can send all necessary information to their friends, family or even emergency numbers by clicking on one button.
This message contains user‘s predefined information plus exact gps position and a direct link on the map where exactly that person is right now.



## Technical part

* Xamarin forms (UWP, Android, iOS)
* Prism framework (MVVM, Commands, etc.)
* Tabbed pages UI
* Localization (CZ, EN)
* Rest API for mailing
* GPS, GSM, SQLite
* Plugins (Mailjet, Acr.UserDialogs,...)

The application is based on Xamarin forms (so it is implemented for platforms such as UWP, Android and iOS).
It is also supported by the Prism framework which helps us writing code in correct design pattern as (MVVM, Commands, and etc.)
Application user interface is achieved by Xamarin tabbed pages for a clear orientation in the application.

In this project localization (resources) for Czech and English language are used.
Sensors or communicators which the application is using are GPS for location, GSM for sending "lost" messages and SQLite database for all settings.

The project cointains few nuget plugins such as UserDialogs for Toast messages, MailJet for REST API for emails which can be also used for sending GSM messages and few others.


## Future improvements
* With location data in the App, even a map can be displayed
* Integrated phone contact list into App

There is always a room for improvment and in our application this can be easily achieved by solving for example the issue of not showing the map in the application together with location.

Or you can also integrate a phone contact list into the application so that the user doesnt need to add contacts manually.


## Issues

### Issue 1: 
During first instalation on Android and grand permission for SMS sent, application freeze. During second run is application running already well. Issue is described on https://github.com/xamarin/XamarinComponents/issues/801 

### Issue 2: 
On platform UWP doesnt work sending SMS. In that case we can use service from MailJet which provide even SMS service over REST API. This service is paid, so in our case is not used. https://docs.microsoft.com/cs-cz/xamarin/essentials/permissions?tabs=android#available-permissions 

### Issue 3: 
I am not owner of iOS. I am not able to debug this project for platform iOS.

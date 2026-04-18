# Device Management System

A modern Device Management dashboard built with **Angular** and **.NET**. This system allows users to track hardware inventory, manage their device assignments, and perform CRUD operations.

##  Features
- **Assignment:** Users can assign devices to themselves or manage inventory.
- **Searchable Inventory:** Ranked free-form text search, showing responses in order of search ranking ( certain criteria are rated higher like Name )
- **Device Details View:** Able to view device's details
- **AI-integrated Device Description Generation:** Generation of device description automatically on creation (leave description blank to trigger)
- **Responsive Layout:** Optimized for desktop and tablet usage with grid-based layouts.

## Tech Stack
- **Frontend:** Angular 19.
- **Backend:** .NET 8.0

## Prerequisites to run locally
Ensure you have the following installed on your machine:
- .NET 8.0
- Angular 19
## Running locally
- clone repo
- set up MSSql database
- run the SQL scripts ( all three of them )
- create .env files inside both frontend and backend projects, respecting the .env templates provided (named TEMPLATEenv or TEMPLATEenvironment.ts)
- cd inside frontend project, run npm install
- cd inside backend project, run dotnet build (or dotnet restore)
- run backend and frontend

## Screenshots / Walkthrough

After registering / logging in, the user accesses the main page, featuring the list of all devices, from where the user can access a multitude of features
<img width="1901" height="589" alt="image" src="https://github.com/user-attachments/assets/9af975d4-630c-4813-9ee0-67d37a9aef05" />


The user can view device details and edit the device

<img width="1165" height="819" alt="image" src="https://github.com/user-attachments/assets/b04496f2-53d4-4b8b-bb18-db8153c5339a" />
<br>
<img width="1124" height="876" alt="image" src="https://github.com/user-attachments/assets/59a73633-ac7c-48da-99e6-1fbef40a4867" />


Add a new device from the main page

<img width="1168" height="882" alt="image" src="https://github.com/user-attachments/assets/6c234c6d-2c85-4629-a058-301956a9e9d6" />

Delete devices
<img width="1849" height="80" alt="image" src="https://github.com/user-attachments/assets/36a442e3-50f5-4310-83c3-9b2d52978d51" />
<br>
<img width="1832" height="334" alt="image" src="https://github.com/user-attachments/assets/113a458f-3e29-4700-b1bd-1a4b5c3ed1c2" />

Or Assign devices to themselves
<img width="1835" height="104" alt="image" src="https://github.com/user-attachments/assets/86255923-2fe9-4d0f-921b-b4c093252efd" />
<br>
<img width="1800" height="94" alt="image" src="https://github.com/user-attachments/assets/a3e0e63b-a968-4f89-9689-4c55dbfbf80e" />

To automatically generate a description utilizing AI, simply leave the description of the device blank:

<img width="1152" height="712" alt="image" src="https://github.com/user-attachments/assets/148775ab-780f-42ee-be93-3c94132d6222" />
<br>

<img width="1101" height="824" alt="image" src="https://github.com/user-attachments/assets/d95c954c-de92-4d14-96c8-7059a78217ad" />






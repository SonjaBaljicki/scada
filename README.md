# SCADA System Project

## Overview
This project is a Supervisory Control and Data Acquisition (SCADA) system implemented in C#. It provides functionalities for monitoring and control, including tag management, user authentication, and various client applications for data visualization and reporting.

## Features
Database Manager: UI for managing tags, controlling scans, writing tag values, and user management.

Trending App: Displays live data of input tags.

SCADA Core: Core system component for server-client communication, includes Simulation Driver and Tag Processing.

Real-Time Driver: Handles RTU data integration.

Alarm Display: Console interface for real-time alarm monitoring.

Report Manager: Generates system reports.

## Installation and Setup
Prerequisites: Ensure .NET framework and a compatible IDE (e.g., Visual Studio) are installed.

Clone the Repository: git clone <repository-url>

Build the Project: Open the solution in your IDE and build the project.

Configuration: Edit scadaConfig.xml for initial settings.

Run the Applications: Start SCADA Core, DatabaseManager, Trending App, Alarm Display, and Report Manager.

## Images
Adding analog input tag
![Adding analog input tag](https://github.com/VanjaJe/scada/assets/104532211/078dd977-2a6e-42ba-8d5b-0ad0c91f8d9f)

Adding digital output tag
![Adding digital output tag](https://github.com/VanjaJe/scada/assets/104532211/a5ac5804-cb44-4b64-8be7-e2a520837a7a)

Database manager, alarm display and trending
![Database manager, alarm display and trending](https://github.com/VanjaJe/scada/assets/104532211/8604ad17-1a57-4cbf-87a2-ce29473db5ba)

Report manager - Latest values of all analog input tags
![Latest values of all analog input tags](https://github.com/VanjaJe/scada/assets/104532211/80e0d8fd-553f-45ea-b488-e163d2b047f6)


## Authors
Valentina Jevtić (SV11-2021)

Dunja Matejić (SV21-2021)

Mila Milović (SV22-2021)

Sonja Baljicki (SV59-2021)

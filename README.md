# Ruedas Felices – Technical Inspection Management System
Ruedas Felices is a console-based management system built in C# and .NET 8.
The application handles clients, vehicles, inspectors, inspection appointments, and sends automated confirmation emails using MailKit.

## Features

* Customer registration and editing
* Vehicle registration linked to customers
* Inspector management and filtering by specialization
* Appointment scheduling, cancellation, and completion
* Automatic email confirmation for scheduled appointments
* Email log history stored locally
* Environment variable support using DotNetEnv

### 🛠️ Technologies Used
* C# / .NET 8
* MailKit – SMTP email sending
* DotNetEnv – Environment variable handling
* Object-oriented architecture (repositories + services + controllers)

### 📁 Project Structure

```
RuedasFelices/
├─ Controllers/
├─ Models/
├─ Repositories/
├─ Services/
├─ Program.cs
├─ .env
└─ RuedasFelices.csproj
```

### 📝 Prerequisites

Before running the Ruedas Felices project, make sure you have the following installed and configured:

##### 1. C# / .NET 8 SDK

Download and install from https://dotnet.microsoft.com/download/dotnet/8.0
Verify installation with: `dotnet --version`

#### 2. Git (optional, for cloning the repository)

#### 3. Gmail account with App Password

* Gmail accounts require an App Password to allow SMTP access.

* Create one at https://myaccount.google.com/apppasswords

* This password will be used in your .env file as SMTP_PASSWORD.

#### 4. Environment file (.env)

Create a .env file in the root of the project with the following variables:

```
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_EMAIL=your_email@gmail.com
SMTP_PASSWORD=your_app_password
```

#### 5. IDE

- JetBrains Rider


### ▶️ How to Run the Project

To run the Ruedas Felices project in Rider, follow these steps:

1. Open the project in Rider by selecting the folder containing RuedasFelices.csproj.

2. Ensure the .env file is in the project root with your SMTP credentials.

3. Build the project via Build → Build Solution.

4. Run the application by clicking the green Run button or pressing Shift+F10.

5. Use the console menu to manage customers, vehicles, inspectors, appointments, and email confirmations.



## Table of Contents
1. [Overview](#overview)
2. [Features](#features)
   - [Doctor Availability](#1-doctor-availability)
   - [Appointment Booking](#2-appointment-booking)
   - [Appointment Confirmation](#3-appointment-confirmation)
   - [Doctor Appointment Management](#4-doctor-appointment-management)
3. [Architecture Overview](#architecture-overview)
4. [How to Run the Project](#how-to-run-the-project)
   - [Prerequisites](#prerequisites)
   - [Steps to Run](#steps-to-run)
6. [Testing](#testing)
   - [Running Tests](#running-tests)
   - [Coverage](#coverage)
7. [Technologies and Tools](#technologies-and-tools)

---

## Overview
This repository contains a backend system for managing doctor appointment bookings. The system is designed for a single doctor and adheres to modular monolithic architecture principles, implementing four key modules:

1. **Doctor Availability** (Traditional Layered Architecture)
2. **Appointment Booking** (Clean Architecture)
3. **Appointment Confirmation** (Simple Architecture)
4. **Doctor Appointment Management** (Hexagonal Architecture)

---

## Features

### 1. Doctor Availability
- **Add Slots**: Add new time slots for the doctor, including slot details like time, and cost.
- **List Slots**: View all available and reserved slots for the doctor.

### 2. Appointment Booking
- **View Slots**: Patients can view all available slots.
- **Book Appointment**: Patients can book an appointment for a specific slot.

### 3. Appointment Confirmation
- **Send Notification**: Send a confirmation notification to the patient and doctor upon successful booking.

### 4. Doctor Appointment Management
- **View Appointments**: Doctors can view their upcoming appointments.
- **Manage Appointments**: Doctors can mark appointments as completed or cancel them.

---

## Architecture Overview

The system follows a **modular monolithic architecture**, with each module designed independently to ensure separation of concerns and maintainability.

1. **Doctor Availability Module**
   - Architecture: Traditional Layered
   - Layers: API → Business → Data

2. **Appointment Booking Module**
   - Architecture: Clean Architecture
   - Layers:
     - **API**: Controllers
     - **Application**: Use Cases
     - **Domain**: Business Rules
     - **Infrastructure**: Database and external dependencies

3. **Appointment Confirmation Module**
   - Architecture: Simple architecture
   - Implementation: Straightforward logging of notifications upon booking confirmation.

4. **Doctor Appointment Management Module**
   - Architecture: Hexagonal Architecture
   - Core: Domain logic for managing appointments
   - Shell: External interaction

![Clinicore Software Architecture Diagram](https://github.com/user-attachments/assets/c46f3bed-9aa8-4be3-8af8-31fa1ad05b21)

---

## How to Run the Project

### Prerequisites
- Install the [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or higher).
  
### Steps to Run
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/MostafaAE/clinicore-modulith
   ```
2. **Navigate to the Project Directory**:
   ```bash
   cd .\clinicore-modulith\src\Bootstrapper\CliniCore.Bootstrapper
   ```
3. **Run the Application**:
   ```bash
   dotnet run --project CliniCore.Bootstrapper.csproj
   ```
4. **Access the API**:
   ```bash
   http://localhost:5050/swagger/index.html
   ```
---

## Testing

The system is thoroughly tested to ensure reliability and accuracy. Both unit tests and integration tests are implemented, with a strong emphasis on covering all modules and edge cases.

### Testing Highlights
- **Comprehensive Coverage**: All four modules are fully tested, including business logic, API endpoints, and inter-module integration.
- **Edge Cases**: Focused extensively on testing edge scenarios to ensure the system handles unexpected or extreme inputs gracefully.
- **Mocking**: Dependencies are mocked using Moq to isolate and test components effectively.

### Running Tests
To execute all unit and integration tests:
```bash
dotnet test
```
### Coverage
![Coverage Summary](https://github.com/user-attachments/assets/cbc13342-7f94-4384-9880-df2a40756b17)
![Coverage Details](https://github.com/user-attachments/assets/2865c547-52b3-4dd3-a5bf-717dfc7b04c3)

---

## Technologies and Tools

- **.NET**: Core framework for the backend system
- **SQLite**: As the database engine
- **Entity Framework**: ORM for the database
- **xUnit**: Unit and integration testing
- **Fluent Assertions**: For expressive and readable assertions in tests
- **Moq**: For mocking dependencies in tests
- **AutoFixture**: For generating test data

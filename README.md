# Golf Club Manager

A member and booking management system for Strawberry Fields Golf Club built with ASP.NET 8 and Blazor WebAssembly.

## Features

### Member Management
- Create, view, update, and delete member profiles
- Each member has:
    - Membership Number (automatically assigned)
    - Name
    - Email
    - Gender
    - Handicap

### Tee Time Booking System
- Book tee times at 15-minute intervals (9:00, 9:15, 9:30, etc.)
- Up to four players per booking
- Restrict members to one booking per day
- View all bookings for a specific member
- View all bookings for a specific date

### Filtering and Sorting
- Filter members by:
    - Gender (Male/Female)
    - Handicap ranges (Below 10, Between 11-20, Above 20)
- Sort members by:
    - Name (ascending or descending)
    - Handicap (ascending or descending)

### Validation
- Client-side validation using data annotations
- Server-side validation through API controllers
- Proper error handling and user feedback

## System Architecture

This application uses a clean architecture with:
- Blazor WebAssembly frontend
- ASP.NET Core 8 Web API backend
- SQL Server database
- Entity Framework Core for data access

### Project Structure
- **GolfClubManagerAPI**: Backend API for data operations
- **GolfClubManagerWASM**: Blazor WebAssembly frontend

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or JetBrains Rider

### Database Setup
1. Update the connection string in `GolfClubManagerAPI/appsettings.json` to point to your SQL Server instance
2. Run the following commands from the API project directory:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### Running the Application
1. Open the solution in Visual Studio/Rider
2. Set both projects to start simultaneously
3. Run the application
4. API will run on http://localhost:5080
5. Blazor WASM app will run on http://localhost:5158

## Usage Guide

### Adding Members
1. Navigate to the Members page
2. Fill in the required fields in the "Add New Member" form
3. Click "Add Member"

### Making Bookings
1. Navigate to the Bookings page
2. Select a date
3. Choose an available tee time
4. Select up to four members for the booking
5. Click "Book Tee Time"

### Viewing and Filtering Members
1. Go to the Members page
2. Use the filter options to narrow down the member list
3. Click column headers to sort the results

### Member Bookings
1. Navigate to the Bookings page
2. Select a member from the dropdown
3. View all bookings for that member

## Future Enhancements
- Implement user authentication and role-based access
- Improve UI/UX on mobile devices
- Add email notifications for bookings
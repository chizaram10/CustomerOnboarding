# Project Setup and Configuration

## Prerequisites

- .NET 6 SDK
- SQL Server

## Setup Instructions

1. Clone the Repository

    ```
    git clone https://github.com/chizaram10/CustomerOnboarding.git
    ```

2. Update Connection String

    Update the connection string in `appsettings.json` to point to your SQL Server instance. The `appsettings.json` file is located in the root of the project.
    ```

3. Run Migrations

    Run an update database command to create the database and tables using already existing migrations.
    ```

4. Seed Test Data

    The application will automatically seed test data for states and LGAs on the first run. This data includes the two states with the fewest LGAs in Nigeria and their respective local governments.

5. Run the Application

    Run the application using the following command:

    ```
    dotnet run
    ```

    The application should now be running on `https://localhost:5001` or `http://localhost:5000`.

## Testing the API with Swagger

Swagger is integrated into the project for API testing.


### Note on Seeded Data

- The seed data for states and LGAs will have their IDs start from 1 and increment sequentially (Identity).
- You can verify the seeded data and their IDs by checking the database directly.

Example of the seeded data:

- **State IDs:**
  - Bayelsa: 1
  - Gombe: 2

- **LGA IDs:**
  - Bayelsa LGAs:
    - Brass: 1
    - Ekeremor: 2
    - Kolokuma/Opokuma: 3
    - Nembe: 4
    - Ogbia: 5
    - Sagbama: 6
    - Southern Ijaw: 7
    - Yenagoa: 8
  - Gombe LGAs:
    - Akko: 9
    - Balanga: 10
    - Billiri: 11
    - Dukku: 12
    - Funakaye: 13
    - Gombe: 14
    - Kaltungo: 15
    - Kwami: 16
    - Nafada: 17
    - Shongom: 18
    - Yamaltu/Deba: 19

You can use these IDs for testing the API endpoints with Swagger.

## Additional Notes

- Ensure your SQL Server is running and accessible.
- Make sure to use the correct SQL Server credentials in the connection string.

## Troubleshooting

If you encounter any issues, ensure that:

- Your connection string is correctly configured.
- SQL Server is running and accessible.
- Confirm the IDs from your database.
- You have run the `dotnet ef database update` command to apply migrations.
- You have the necessary .NET SDK installed.



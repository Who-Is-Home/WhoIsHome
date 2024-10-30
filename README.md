# WhoIsHome
Backend for [Mobile App](https://github.com/Darki002/WhoIsHome.App)

# API

This Project uses Swagger for the API Documentation. You can start up the project locally 
and the swagger you should open automatically or use "localhost:7165/swagger/index.html"

For information about Authentication & Authorized read the following Documentation: [AuthDocs](./docs/Auth.md)

# Environment Variables

| Environment Variable             | Description                                                                                                                                            | Default Value |
|----------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| `JWT_SECRET_KEY`                 | Key used for the JWT Authentication.                                                                                                                   | -             |
| `API_KEY`                        | API Key used by the middleware in every request to Authorized.                                                                                         | -             |
| `MYSQL__SERVER`                  | The SQL Server.                                                                                                                                        | -             |
| `MYSQL__PORT`                    | Port for the Database.                                                                                                                                 | 3306          |
| `MYSQL__DATABASE`                | Database that will be used by the Application.                                                                                                         | WhoIsHome     |
| `MYSQL__USER`                    | The User that is being used by the app to connect to the db.                                                                                           | root          |
| `MYSQL__PASSWORD`                | The Password for the user that is being used by the app to connect to the db.                                                                          | -             |
| `PROJECT_ID`                     | The Firebase `projectId` of your Firebase project. *(Not in use yet)*                                                                                  | -             |
| `GOOGLE_APPLICATION_CREDENTIALS` | Google Authentication for Firebase. [Read more here](https://cloud.google.com/docs/authentication/provide-credentials-adc#wlif-key) *(Not in use yet)* | -             |


*Note:* Some Environment Variables use two underscore, this is due to how dotnet maps those onto the App Configuration 

## Local Development

- MySql DB running locally
- Env Variables are already set in the `appsettings.Development.json` to the default values

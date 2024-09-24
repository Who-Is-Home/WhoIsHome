# WhoIsHome
Backend for [Android App](https://github.com/Darki002/WhoIsHome.Android)

# API Endpoints

This Project uses Swagger for the API Documentation. You can start up the project locally 
and the swagger you should open automatically or use "localhost:7165/swagger/index.html"

For informations about Authentication & Authorized read the following Documentation: [AuthDocs](./Docs/Auth.md)

# Environment Variables

- `JWT_SECRET_KEY` : Key used for the JWT Authentication.
- `API_KEY` : API Key used by the middleware in every request to Authorized.
- `PROJECT_ID` : the Firebase projectId of your Firebase project.
- `GOOGLE_APPLICATION_CREDENTIALS` : Google Authentication for Firebase. Read more [here](https://cloud.google.com/docs/authentication/provide-credentials-adc#wlif-key)

## Local Development

For running the project locally the project uses "DotNetEnv".

1. Create `.env` file in the `WhoIsHome.Host` project
2. Add the Environment Variables in the `.env` file. For example `API_KEY=dev1234`

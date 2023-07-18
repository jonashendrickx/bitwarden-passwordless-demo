
# Getting Started
1. Clone https://github.com/passwordless/passwordless-server
2. Clone me.
3. Setup Passwordless Server & Admin Console
4. To generate the Sqlite database & tables, open your terminal with 'Passwordless.YourBackend' being your working directory. And execute
   `dotnet ef database update --context YourBackendContext`
5. You will need the API Key and API Secret from #3 and enter it in appsettings.Development.json

# Environment
- Passwordless Server: http://localhost: 7001
- Passwordless Admin Console: http://localhost:8001
- YourBackend: http://localhost:5013
- React: http://localhost:3000

# RBAC
- /public: Should be accessible anonymously.
- /admin: Accessible when user has the 'Admin' role assigned. You can assign it manually.
- /user: Accessible when user has the 'User' role assigned. You can assign it manually.


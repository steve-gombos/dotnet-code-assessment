## Prerequisites

1. Install Visual Studio or Jetbrains Rider
2. Install the latest .NET 8 SDK

## Guidelines

- You can only modify the contents of the API project.
- You can view the tests and their inputs/outputs, but test modifications are not permitted.

## Tasks

1. There are numerous bugs in the code.  Use the tests provided to identify these issues and fix them.
2. Create an HTTP Client that implements the `ISomeSystemApiClient` interface.  This implementation should source it's data from https://gist.githubusercontent.com/steve-gombos/baf2739c0a6cdac77f781bfb2f70b6c1/raw/c1491a261ce45f010add8df3b43c9198c47e4408/mock-users.json. Below is the exact data returned from this URI.  This data is used by the `IUserService` to refresh data associated to a user in our data context.
    ```json
    {
      "users": [
        {
          "userId": 1,
          "isComplete": true,
          "isActive": true
        },
        {
          "userId": 3,
          "isComplete": false,
          "isActive": false
        }
      ]
    }
    ```
3. Create an implementation for the `IFizzBuzzService` interface.  The definition accepts an integer and will output using the following rules:
   - When the integer is a multiple of 3, return `Fizz`
   - When the integer is a multiple of 5, return `Buzz`
   - When the integer is a multiple of both 3 and 5, return `FizzBuzz`
   - Otherwise return the integer



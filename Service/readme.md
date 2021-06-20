## Execution
- Open in Visual Studio and Start
- Run command line
```
cd [path]\Iw.Services\Iw.Services.Api
dotnet run
```

## Notes
This API is an in memory API.  Nothing is stored on disk.  The data set will reset everytime it is run.

## Endpoints

#### Get All ToDo Items
Retrieves all the ToDo items
>Endpoint:
```
GET: /v1/todoitems
```
>Response:
```
[
    {
        "id": 1,
        "name": "First To Do Item",
        "description": "I really need to get this done",
        "createDate": "2018-08-01T00:00:00",
        "dueDate": "2018-10-01T00:00:00",
        "assignedTo": "Scott Neu",
        "completedDate": null,
        "completedBy": null,
        "completed": false,
        "isPastDue": false,
        "isLate": false
    },
    {
        "id": 2,
        "name": "Another Really Important Item",
        "description": "Something super important",
        "createDate": "2018-08-03T00:00:00",
        "dueDate": "2018-09-15T00:00:00",
        "assignedTo": "Ryan Jackson",
        "completedDate": null,
        "completedBy": null,
        "completed": false,
        "isPastDue": false,
        "isLate": false
	}
]
```

#### Get Filtered ToDo Items
Retrieves a filtered list of the ToDo items

>Endpoint:
```
GET: /v1/todoitems?name={nameFilter}&late={true|false}&pastdue={true|false}&completed={true|false}
```
>Response:
```
[
    {
        "id": 1,
        "name": "First To Do Item",
        "description": "I really need to get this done",
        "createDate": "2018-08-01T00:00:00",
        "dueDate": "2018-10-01T00:00:00",
        "assignedTo": "Scott Neu",
        "completedDate": null,
        "completedBy": null,
        "completed": false,
        "isPastDue": false,
        "isLate": false
    }
]
```

#### Get Specific ToDo Item
Retrieves a specific ToDo item

>Endpoint:
```
GET: /v1/todoitems/{id}
```
>Response:
```
{
    "id": 2,
    "name": "Another Really Important Item",
    "description": "Something super important",
    "createDate": "2018-08-03T00:00:00",
    "dueDate": "2018-09-15T00:00:00",
    "assignedTo": "Ryan Jackson",
    "completedDate": null,
    "completedBy": null,
    "completed": false,
    "isPastDue": false,
    "isLate": false
}
```
#### Create a New ToDo Item
Creates a new ToDo item

>Endpoint:
```
POST: /v1/todoitems
```
>Body:
```
{
    "name": "name", (required)
    "description": "description", (required)
    "dueDate": "2018-09-15T00:00:00", (optional)
    "assignedTo": "Ryan Jackson" (optional)
}
```
>Response:
```
{
    "id": 10,
    "name": "name",
    "description": "description",
    "createDate": "2018-08-03T00:00:00",
    "dueDate": "2018-09-15T00:00:00",
    "assignedTo": "Ryan Jackson",
    "completedDate": null,
    "completedBy": null,
    "completed": false,
    "isPastDue": false,
    "isLate": false
}
```

#### Update an Existing ToDo Item
Update an existing ToDo item

>Endpoint:
```
PUT: /v1/todoitems/{id}
```
>Body:
```
{
    "name": "new name", (required)
    "description": "new description", (required)
    "dueDate": "2018-09-15T00:00:00", (optional)
    "assignedTo": "new Assignnee" (optional)
}
```
>Response:
```
{
    "id": 10,
    "name": "new name",
    "description": "new description",
    "createDate": "2018-08-03T00:00:00",
    "dueDate": "2018-09-15T00:00:00",
    "assignedTo": "new Assignnee",
    "completedDate": null,
    "completedBy": null,
    "completed": false,
    "isPastDue": false,
    "isLate": false
}
```
#### Delete an Existing ToDo Item
Delete an existing ToDo item

>Endpoint:
```
DELETE: /v1/todoitems/{id}
```
>Response: standard HTTP status codes

#### Complete a ToDo Item
Completes an existing ToDo item.  The onDate field is optional, if it is not supplied, the server will create the date.

>Endpoint:
```
PATCH: /v1/todoitems/{id}/complete
```
>Body:
```
{
    "byUser": "First Last", (required)
    "onDate": "2018-09-15T00:00:00", (optional)
}
```
>Response:
```
{
    "id": 10,
    "name": "new name",
    "description": "new description",
    "createDate": "2018-08-03T00:00:00",
    "dueDate": "2018-09-15T00:00:00",
    "assignedTo": "new Assignnee",
    "completedDate": "2018-09-15T00:00:00",
    "completedBy": "First Last",
    "completed": false,
    "isPastDue": false,
    "isLate": false
}
```

## Version 2 API
Version 2 of the api has the exact same functions as the Version 1, but they require an Authorization: Bearer <token> in the request header.
## Additional Endpoints

#### Login
Processes a login request
>Endpoint:
```
POST: /v2/authentication/login
```
>Body:
```
{
	"username": "test",
	"password": "qwere"
}
```
>Response:
```
{
  "accessToken": "a jwt token",
  "expires": 576000
}
```
Fields in the JWT will include:
```
	aud: "Todo Services"
	email: "demo.user@email.com"
	exp: 1546498519
	family_name: "User"
	given_name: "Demo"
	iat: 1545922519
	iss: "Iw.Services"
	jti: "b3c9fada-0dfb-40af-9a54-42101c5494c5"
	nbf: 1545922519
	sub: "100"
	unique_name: "test"
	user_authorization: "CanView,CanAdd,CanEdit,CanComplete,CanDelete"
```
#### Refresh
Processes a token refresh
>Endpoint:
```
Get: /v2/authentication/token/refresh
```
>Body: (none)
>Response:
```
{
  "accessToken": "a jwt token",
  "expires": 576000
}
```

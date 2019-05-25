# GildedRose
## API -- The Gilded Rose Expands


As you may know, the Gilded Rose* is a small inn in a prominent city that buys and sells only the finest items. The shopkeeper, Allison, is looking to expand by providing merchants in other cities with access to the shop's inventory via a public HTTP-accessible API.

API requirements
- Retrieve the current inventory (i.e. list of items)
- Buy an item (user must be authenticated)


### Deliverables
1. A system that can process the two API requests via HTTP
2. Appropriate tests (unit, integration, etc)
3. A quick explanation of:
    1. choice of data format. Include one example of a request and response.
    2. what authentication mechanism was chosen, and why


### Technologies/frameworks used
* ASP.NET Web API 2.2
* .NET Framework 4.6.1
* ORM -- EntityFramework 6.1.0
* Membership -- ASP.NET Identity 2.2.2
* DI -- Ninject 3.3.3
* Object mapper -- AutoMapper 8.1.0
* Testing framework -- MSTest V2
* Mocking framework -- Moq 4.10.1
* Test assertions -- FluentAssertions 5.6.0
* [Separation of concerns in Web API 2 pipeline](https://www.asp.net/media/4071077/aspnet-web-api-poster.pdf)
* Basic authentication
* Repository pattern
* Unit of work pattern

The system was developed using Visual Studio 2017.


### Solution Projects
* GildedRose (main/API)
* GildedRose.Tests (unit test)
* GildedRose.IntegrationTests (integration tests)


### Models
* Item
```c#
	public class Item
	{
		public int Id { get; set; }
		public string Sku { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
```
* Order
```c#
 	public class Order
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public ApplicationUser Customer { get; set; }
		public string CustomerId { get; set; }
		public decimal TotalPrice { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
	}
```
* OrderItem
```c#
	public class OrderItem
	{
		public Order Order { get; set; }
		public Item Item { get; set; }
		public int OrderId { get; set; }
		public int ItemId { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal LinePrice { get; set; }
	}
```


### API Endpoints
* /items GET
* /orders POST


### Sample GET request/response
```json
Request:
/items GET

Response:
[
    {
        "id": 1,
        "name": "Copper spoon",
        "description": "The finest spoon in town.",
        "price": 49.97
    },
    {
        "id": 2,
        "name": "Silver Plate",
        "description": "The finest plate in town.",
        "price": 59.97
    },
    {
        "id": 3,
        "name": "Gold fork",
        "description": "The finest fork in town.",
        "price": 299.97
    },
    {
        "id": 4,
        "name": "Platinum glass",
        "description": "The finest glass in town.",
        "price": 1479.97
    }
]
```

### Sample POST request/response
```json
Request:
/orders POST
authorization: Basic dGVzdEBkb21haW4uY29tOlRlc3QxMjM0IQ==
{
	"orderItems": [{
		"itemId": 1,
		"quantity": 8
	},
	{
		"itemId": 4,
		"quantity": 1
	}]
}

Response:
{
    "id": 1,
    "orderDate": "2019-05-24T16:45:57.28",
    "customer": {
        "id": "cc6d6c35-f878-4251-a0b3-256d8463a161",
        "userName": "test@domain.com"
    },
    "totalPrice": 1879.73,
    "orderItems": [
        {
            "itemId": 1,
            "item": {
                "name": "Copper spoon",
                "description": "The finest spoon in town."
            },
            "price": 49.97,
            "quantity": 8,
            "linePrice": 399.76
        },
        {
            "itemId": 4,
            "item": {
                "name": "Platinum glass",
                "description": "The finest glass in town."
            },
            "price": 1479.97,
            "quantity": 1,
            "linePrice": 1479.97
        }
    ]
}
```


### Data Format
The system uses JSON for request payload and response. Also, "/items GET" endpoint forces clients to receive JSON, regardless of HttpRequestHeaders.Accept value. JSON is compact (RESTful APIs depend on fast data exchanges) and can be easily loaded in JavaScript.


### DTOs
The system have DTOs for request and reponse needs. We do not want unnecessary fields be requested from the client and that every information is sent back to the client.

In the POST response example above, it does not include the security details of the customer and the SKU of the products.

In the GET response example above, currently, it does not include the items quantities. This endpoint is accessible to the public and we do not want consumers to see the inventory. Later on, we can have a separate endpoint for the merchants that requires authentication and displays the items quantities.


### Authentication
Basic authentication is used (as opposed to what is available in ASP.Net Identity) to accomodate the outside world. Clients have different systems and, most probably, will not use the Gilded Rose user interface (if there's any available) for logging-in.

Later on, other authentications can be added, like bearer token or JWT authentication.


### Data Persistence
The system uses a special copy of SQL Server instance attached to a specific DB Filename for single user mode.


### Unit and Integration Tests
The system has unit tests for the two API endpoints and their corresponding repositories. Mocking is used for these tests.

The system also includes integration test which simulates an actual end-to-end process, from request to database read/write. For this, the system uses a local test DB that is installed in the local machine (as opposed to attached DB). This local test DB is always fresh by using transaction. The transaction is instantiated at TestInitialize and rolled back at TestCleanup.


### Other Design Concerns
**Routing:**
Template-style routing is disabled. Instead, attribute-style routing is used for more controllability, scalability, and understability.

**Authentication Filter:**
The system keeps single responsibility principle in the Web API pipeline. Authentication can be done inside the controller, but putting modules at the appropriate layer of the pipeline is better for usability and maintainability. For that reason, authentication is done in its respective filter. 

**Action Filter:**
Action filters should be for things to be done on a per route basis. For that reason, the system does model state and body parameter validation (because this is right after model binding), and changing request headers in this filter.

The framework was designed for good reasons. For more understanding of this opinionated view of the Web API, below is the pipeline, its layers, and their responsibilities.


![alt text](https://image.isu.pub/170128084338-783b569f2e63a265832757636129a032/jpg/page_1.jpg "ASP.NET Web API 2: HTTP Message Lifecycle")


### Questions and Answers
- Q: How do we know if a user is authenticated?

  A: The system uses basic authentication. The user puts its Base64-encoded encoded credentials to the request payload. The system then decodes the username and password and should match DB values.
- Q: Is it always possible to buy an item?

  A: No. User can buy only if all the following conditions are met:
  - the user is authenticated
  - all Items exist
  - all OrderItem Quantities are greater than zero
  - OrderItem Quantity is less than or equal to the Item Quantity (stock)
  - no duplicate Items

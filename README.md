# CarAuctionManagementSystem

## Design 
#### Decisions
### Unified Vehicle Table with VehicleType Enum
#### Decision:
* Store all vehicle types (Hatchback, Sedan, SUV, Truck) in a single Vehicle table with a VehicleType enum.

#### Reasoning:
* Reduces schema complexity (no need for multiple inheritance tables).
* Simplifies querying/searching across all vehicles (e.g., filtering by manufacturer or year regardless of type).
* Allows adding new vehicle types without schema changes (only extend enum and adjust validation).

### Layered Architecture (Separation of Concerns)
#### Manager API:
* Exposes endpoints for managing vehicles, auctions, and bidding. Handles admin actions like creating auctions and assigning vehicles to them.
#### Repositories:
* Encapsulate all database operations using EF Core. Makes it easy to change or mock data sources (e.g., for unit testing).
#### Services:
* Contain core business logic (auction lifecycle, bid validation, etc.). Decouple domain rules from API controllers.
#### DTOs and AutoMapper:
* API communicates via DTOs, avoiding exposure of entity internals. AutoMapper reduces boilerplate and keeps mapping centralized.

### Real-Time Updates with SignalR
#### Decision:
* Use SignalR hubs to broadcast auction and bidding events to connected clients.

#### Reasoning:

* Real-time feedback is critical in auction systems (e.g., bid updates, vehicle availability).
* Supports multiple connected clients simultaneously without polling.

### Background Auction Lifecycle
#### Decision:
Auction lifecycle (opening/closing vehicles) is handled in a background service method.

#### Reasoning:

* Auctions automatically progress through their stages (e.g., 5-minute bidding window per vehicle).
* Eliminates manual intervention to manage auction phases.
* Updates are broadcast via SignalR to keep clients in sync.

### Authentication and Authorization with JWT
#### Decision:
* Secure bidding endpoints with JWT tokens; require login/registration for all bidding actions.
#### Reasoning:
* Prevents unauthorized users from participating in auctions.
* Lightweight and stateless authentication suitable for distributed systems.
* Claims (e.g., NameIdentifier) allow retrieving the logged-in user in bid operations.

### Validation with FluentValidation
#### Decision:
* Validate vehicle-specific properties (e.g., doors, seats, load capacity) using FluentValidation.
#### Reasoning:
* Keeps validation logic clean, reusable, and separate from controllers.
* Different rules per vehicle type enforced automatically via adapters.

### Extensible Vehicle Type Adapter
#### Decision:
* Use adapter pattern (IVehicleTypeAdapter) to encapsulate type-specific behaviors.

#### Reasoning:
* Allows each vehicle type to have its own validation and mapping logic.
* Easily extendable when adding new vehicle types without changing core logic.

### Unified Auction/Bid Flow
#### Decision:
* Tie bids directly to vehicles, auctions and users with relationships in the database.

#### Reasoning:

Ensures referential integrity (a bid always belongs to a valid vehicle, auction and user).
Simplifies winner determination (highest bid per vehicle at auction close).

### Client Console Application
#### Decision:
* Create a separate console app to consume SignalR events.
#### Reasoning:
* Simulates real auction participants receiving updates in real time.

10. Logging with Serilog
#### Decision:
* Use Serilog for structured logging across API and services.

#### Reasoning:
* Provides detailed logs for auction lifecycle and bids.
* Useful for debugging and operational monitoring.


### Assumptions
#### Single-Vehicle Bidding:

* Bidding in auctions is handled one vehicle at a time. Each vehicle is presented to participants sequentially.
* Time Limit Per Vehicle: each vehicle remains open for bidding for 5 minutes. After this time, the highest bid is evaluated, and the vehicle is either marked as Sold (if there is a winning bid) or remains Unsold.
* Sequential Auction Flow: Once bidding ends for one vehicle, the next vehicle in the auction is presented automatically until all vehicles have been processed.


## Test guide

1. Setup
* Run the Manager API Project.
* Run the Client API Project
* Ensure MySQL database is configured and migrations applied.

2. Create Vehicles (Manager API)

```json
{
  "year": 2022,
  "startingBid": 10000,
  "manufacturer": "Toyota",
  "model": "Corolla",
  "identificationNumber": "ABC123456789",
  "numberOfDoors": 5
}
```

3. Create an Auction (Manager API) - for each vehicle added to the auction, it will have 5 minutes to be licited

```json
{
  "name": "Auction1",
  "vehicleIdentificationNumbers": ["ABC123456789"]
}
```

3. Register and Login a User (Client API)

``` json
{
  "username": "john",
  "password": "123456"
}
```

4. Login (Client API) - get the token

``` json
{
  "username": "john",
  "password": "123456"
}
```

5. Place a Bid

* Authorize with JWT token in Swagger or Postman (if you having problems with Swagger).
    - On Postman:
        * Authorization: select bearer option and add the token
        * http://localhost:5175/api/User/bid

```json
{
  "auctionName": "Auction1",
  "amount": 15000
}
```

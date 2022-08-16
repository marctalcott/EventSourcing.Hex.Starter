# EventSourcing.Hex.Starter

This project is a starter template for EventSourcing projects. using a hexagonal (aka: Clean, Onion, 'Ports and Adaptors', pick your term) architecture.

I use the CQRS pattern as that works very well for EventSourcing. 

I usually use the term *'Hexagonal Architecture'* for this project however you may use the terms:
- Hexagonal Architecture
- Onion Architecture
- Ports and Adaptors Architecture
- Clean Architecture

IMO they are all so close that it doesn't really matter. They share the same core idea of dependencies of always pointing inward to the domain.

## Layers
- #### Ports: 
  - Projects that start with 'Port.' are the interfaces to external systems. This will be your API's, EventListeners, etc.
- #### Application:
  - This is where the CQRS Commands and Queries live.
- #### Domain:
  - Aggregates live here.
  - ValueObjects
  - Interfaces that should be implemented in the Infrastructe Layer to reach out to backend services
- ### Domain.ES:
  - You generally don't need to work in this project. It defines the base classes and interfaces needed by the EventSourcing code
- #### Infrastructure:
  - Repositories 
  - Services that handle cross cutting concerns (mail, logging, etc)
- #### Infrastructure.ES:
  - You generally don't need to work in this project. It defines core code for the EventSourcing logic.
- #### Tests
  - Unit tests.

---

### Example data for your AppSettings.Local.json file:

{
  "Auth0": {
    "Domain": "value from: Auth0 - Applications / Your Application / Settings / Domain textbox",
    "Audience": "value from: Auth0 - APIs / Your API / Settings / Identifier textbox"
  },
  "AppName": "EventSourcing.Hex.Starter",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

---

And then to get a token using POSTMan, you can setup a POST using the information from Auth0 / APIs / Your API page / Settings / Test

That will give you the url, header, and body data to POST in order to get your AccessToken, which you can then use to call secured controller api endpoints.

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
  - Exposes inputs to the application such as your API projects.
- #### Application:
  - This is where the CQRS Commands and Queries live.
- #### Domain:
  - Aggregates live here.
  - ValueObjects
  - Interfaces that should be implemented in the Infrastructe Layer to reach out to backend services
- #### Infrastructure:
  - Repositories 
  - Services that handle cross cutting concerns (mail, logging, etc)
- #### UnitTests

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
Footer

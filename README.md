# Orange.ApiTokenValidation
[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ReyStar/Orange.StatsD/blob/master/LICENSE)
[![Build Status](https://dev.azure.com/starandrey/starandrey/_apis/build/status/ReyStar.Orange.ApiTokenValidation?branchName=master)](https://dev.azure.com/starandrey/starandrey/_build/latest?definitionId=2&branchName=master)
## Project target
The main target of this simple project must show how implement hexagonal architecture in practice, use repository pattern with Dapper, .Net Core Web API with versioning and write metrics and logs.
This project should be considered as just for fun) if u can find some interesting for u I will very happy!

Additional description will be soon ...

## Architecture of the project

The [hexagonal architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software)), or ports and adapters architecture, is an architectural pattern used in software design. It aims at creating loosely coupled application components that can be easily connected to their software environment by means of ports and adapters. This makes components exchangeable at any level and facilitates test automation.
The hexagonal architecture directs all dependencies to the center of the external infrastructure adapters in the domain layer. 
<img src="https://raw.githubusercontent.com/ReyStar/Orange.ApiTokenValidation/master/doc/Hexagonal.png" alt="Hexagonal.png" width="450"/>
The following architectural layers are usually distinguished:
* Domain layer - completely independent of other levels. Contains data models and the business logic inherent in these models.
* Domain services layer - layer that contains pure logic for working with domain objects without using port interfaces. This layer is quite ephemeral and in practice many developers aren't separete it as a separate layer and combine it with the domain layer.
* Application layer - depends on the domain level and sometimes on the Domain service layer. Layer containing the business rules of the application and if u use a micro-service approach with DDD principles, this layer essentially implements the business logic of our bounded context also contains the definition of port interfaces and never refers to specific frameworks. This layer  invokes the infrastructure using the port interface.
* Infrastructure layer - it level depends on all levels located above it. It layer implements adapters for the interfaces of ports. This level can be considered as a plug-in to the application level, it can be easily replaced with another one in case of infrastructure changes, since the levels above don't have any strong coupling with it. I don't agree with some colleagues who separate the user interface layer from the infrastructure layer. I wouldn't do that.
* Actors - are participants that interact with the application. It can be real users who connect using ui or other applications/services. Usually exist two Actors types: Primary/Driving Actors (web ui, console, etc...) and Secondary Actors are db, message queue or other external systems that other Primary/Driving Actors use.

## TODO list
- [ ] Add [Vault](https://www.vaultproject.io/) manager for secrets and protect sensitive data by HashiCorp use [VaultSharp](https://github.com/rajanadar/VaultSharp) or other alternative client or service
- [ ] Add embedded NoSQL database like [LiteDB](https://www.litedb.org/). This for testing a very interesting approach where data is distributed across a group of services, rather than services being distributed across a group of databases.

## Infrastructure for testing
If u want to see how it work u can build project Orange.ApiTokenValidation.Shell.csproj from src directory then run docker-compose.yml file. For testing u can use next web endpoints:
* Orange.ApiTokenValidation application https://localhost:60006/swagger/ (this simple application)
* Prometheus http://localhost:9090/ ([site](https://prometheus.io/))
* Prometheus Alertmanager http://localhost:9093/ ([site](https://prometheus.io/docs/alerting/alertmanager/))
* Prometheus Pushgateway http://localhost:9091/ ([GitHub](https://github.com/prometheus/pushgateway/blob/master/README.md))
* VictoriaMetrics for Prometheus (http://localhost:8428/) ([GitHub](https://github.com/VictoriaMetrics/VictoriaMetrics))
* Grafana http://localhost:3000/ ([site](https://grafana.com/))
* PgAdmin http://localhost:5433 ([site](https://www.pgadmin.org/))


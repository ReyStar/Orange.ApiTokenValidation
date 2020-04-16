# Orange.ApiTokenValidation
[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ReyStar/Orange.StatsD/blob/master/LICENSE)
[![Build Status](https://dev.azure.com/starandrey/starandrey/_apis/build/status/ReyStar.Orange.ApiTokenValidation?branchName=master)](https://dev.azure.com/starandrey/starandrey/_build/latest?definitionId=2&branchName=master)
## Project target
The main target of this simple project must show how implement hexagonal architecture in practice, use repository pattern with Dapper, .Net Core Web API with versioning and write metrics and logs.
This project should be considered as just for fun) if u can find some interesting for u I will very happy!

Additional description will be soon ...

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


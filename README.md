# HomeSensorServerAPI

A smart-home-like application build using ASP.NET Core 2.1 WebAPI, MySQL, Entity Framework Core.
Authentication is done using Json Web Token (JWT Bearer).
This application is meant to be deployed on Raspberry PI 3B+ using Kestrel + NGINX servers.
It can be published to internet using port redirection or 3rd partys like dataimplicity.

Main features:
 *gather data from sensor nodes
 *control actuator nodes
 *handle user accounts
 *grant and revoke permissions (view, control, admin, add-data {sensors}) to users
 
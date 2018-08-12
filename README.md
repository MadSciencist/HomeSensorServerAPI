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
 
 Publishing to Raspberry PI (Only RPi 2+ versions work):
 1) On develompent machine:
	dotnet clean
	dotnet publish -r linux-arm
	
	Use WinSCP to transfer files via FTP to Rpi.
	Make main file HomeSensorServerAPI executable (755 chmod)
	
2) On Rpi:
	Go to location where project was copied
	Make sure that Apache with MySQL is running
	Make sure that Nginx is running
	If necessary, apply migration:
	dotnet ef database update
	Then run:
	./HomeSensorServerAPI


Nginx config:
todo

Apache config:
todo
	
	
 
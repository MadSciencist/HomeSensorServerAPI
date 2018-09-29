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
	dotnet publish -c release -r linux-arm
	
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


###########################################################
Nginx config
I'm using nginx as a reverse proxy for kestrel. Also nginx is hangling the HTTPs connection.
config: (you need SSL certificate from next steps, if you want to use only HTTP, comment out the second server definition)

server {
    listen 80;
    include /etc/nginx/proxy_params;

    proxy_http_version 1.1;
    proxy_cache_bypass $http_upgrade;
    proxy_set_header Connection $http_connection;
    proxy_set_header Upgrade $http_upgrade;

    location / {
        proxy_pass http://localhost:5000;
    }
}

server {
    listen 443;

    ssl_certificate           /etc/letsencrypt/live/kryszczak.ddns.net/fullchain.pem;
    ssl_certificate_key       /etc/letsencrypt/live/kryszczak.ddns.net/privkey.pem;

    ssl on;
    ssl_session_cache  builtin:1000  shared:SSL:10m;
    ssl_protocols  TLSv1 TLSv1.1 TLSv1.2;
    ssl_ciphers HIGH:!aNULL:!eNULL:!EXPORT:!CAMELLIA:!DES:!MD5:!PSK:!RC4;
    ssl_prefer_server_ciphers on;

    location / {

      proxy_set_header        Host $host;
      proxy_set_header        X-Real-IP $remote_addr;
      proxy_set_header        X-Forwarded-For $proxy_add_x_forwarded_for;
      proxy_set_header        X-Forwarded-Proto $scheme;

      # Fix the “It appears that your reverse proxy set up is broken" error.
      proxy_pass          http://localhost:5000;
      proxy_read_timeout  90;
    }
}

###########################################################

###########################################################
Obtaining DNS

use noip.com service or similair and DUC client

mkdir /home/pi/noip
cd /home/pi/noip
sudo wget http://www.no-ip.com/client/linux/noip-duc-linux.tar.gz
sudo tar vzxf noip-duc-linux.tar.gz
cd noip-2.1.9-1
sudo make
sudo make install

Use your credentials, and set interval to 15-30minutes.

launch the client:
/usr/local/bin/noip2

To show config:
sudo noip2 ­S
###########################################################

###########################################################
Obtaining SSL certificate (when you already have your domain)
I'm using letsencrypt service. 

sudo apt-get install python-certbot-nginx -t stretch-backports
sudo certbot certonly --authenticator standalone --pre-hook "nginx -s stop" --post-hook "nginx"

The certificate is valid for 90, so you need to create some cron-job for frefreshing it:
sudo certbot renew --dry-run
###########################################################


###########################################################
Configure firewall (UFW)

sudo apt-get install ufw
sudo ufw default allow outgoing
sudo ufw default deny incoming
sudo ufw deny 5000
sudo ufw allow ssh
sudo ufw allow ftp
sudo ufw allow sftp
sudo ufw allow 80
sudo ufw allow 443

then enable firewall:
sudo ufw enable

you can view current config by:
sudo ufw show added

and current status by:
sudo ufw status
###########################################################
For autostart use supervisor:

sudo apt-get install supervisor
cd /etc/supervisor/config.d
touch aspnetcore.conf 

edit this file (i.e sudo nano aspnetcore.conf) and put the config:

[program:aspnetcore]
command=dotnet /home/pi/HomeAPI/HomeSensorServerAPI.dll
directory=home/pi/HomeAPI/
autostart=true
autorestart=true
stdout_logfile=/var/log/aspnetcore.out.log
stderr_logfile=/var/log/aspnetcore.err.log
environment=ASPNETCORE_ENVIRONMENT="Production"

(The logs are commented out, becouse we are using NLog for custom logging, no need to polute the RPI twice (: ).

Then:
sudo supervisorctl reread
sudo supervisorctl update
sudo supervisor start aspnetcore
###########################################################
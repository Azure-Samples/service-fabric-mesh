FROM microsoft/iis:20181009-nanoserver-sac2016

RUN powershell -NoProfile -Command Remove-Item -Recurse C:\inetpub\wwwroot\*
WORKDIR /inetpub/wwwroot
COPY . .

EXPOSE 80

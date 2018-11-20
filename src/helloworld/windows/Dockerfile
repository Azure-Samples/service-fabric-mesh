FROM microsoft/iis:nanoserver

RUN powershell -NoProfile -Command Remove-Item -Recurse C:\inetpub\wwwroot\*
WORKDIR /inetpub/wwwroot
COPY . .

EXPOSE 80

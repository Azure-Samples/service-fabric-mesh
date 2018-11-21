FROM microsoft/dotnet:2.1-sdk-bionic AS build
WORKDIR /src

COPY VotingData/VotingData.csproj VotingData/
# # TODO - The following 2 lines are temporary until the nuget package is available from nuget.org
# # Also the Microsoft.VisualStudio.Azure.SeaBreeze.Targets.0.3.0.nupkg should be copied to solution folder as a manual step
COPY VotingData/Nuget_Linux.Config ./
COPY Microsoft.ServiceFabric.6.4.592.nupkg ./
COPY Microsoft.ServiceFabric.Data.Interfaces.3.3.592.nupkg ./
COPY Microsoft.ServiceFabric.Mesh.AspNetCore.Data.1.0.592-beta.nupkg ./
COPY Microsoft.ServiceFabric.Mesh.Data.Collections.1.0.592-beta.nupkg ./

RUN export DEBIAN_FRONTEND=noninteractive
RUN apt-get update
RUN apt-get install dialog apt-utils -y
RUN DEBIAN_FRONTEND=noninteractive apt-get install -y tzdata
RUN dpkg-reconfigure --frontend noninteractive tzdata
RUN apt-get install -y nuget

RUN dotnet restore --configfile ./Nuget_Linux.Config VotingData/VotingData.csproj
COPY . .

WORKDIR /src/VotingData
RUN dotnet build VotingData.csproj -c Release -r ubuntu.16.04-x64 -o /app

FROM build AS publish
RUN dotnet publish VotingData.csproj -c Release -r ubuntu.16.04-x64 -o /app

FROM ubuntu:16.04
EXPOSE 20009

RUN apt-get update
RUN apt install -y openssh-server curl libc++1 cifs-utils
RUN apt install -y libssh2-1 libunwind8 libib-util
RUN apt install -y lttng-tools lttng-modules-dkms liblttng-ust0 chrpath members sshpass nodejs nodejs-legacy npm locales
RUN apt install -y cgroup-bin acl net-tools apt-transport-https rssh vim atop libcurl3 openjdk-8-jre
RUN wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get install -y apt-transport-https
RUN apt-get update
RUN apt-get install -y dotnet-runtime-2.1
RUN locale-gen en_US.UTF-8
ENV LANG=en_US.UTF-8
ENV LANGUAGE=en_US:en
ENV LC_ALL=en_US.UTF-8

WORKDIR /app
COPY --from=publish /app .
COPY VotingData/launch.sh .

ENTRYPOINT ["./launch.sh"]

FROM ubuntu:jammy AS builder

WORKDIR /tmp
RUN apt-get update && apt-get install -y wget
RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt-get update && apt-get install -y \
    dotnet-sdk-7.0

WORKDIR /root/dyn/
COPY . /root/dyn/

WORKDIR /root/dyn/DynamicImageAPI
ENTRYPOINT dotnet run

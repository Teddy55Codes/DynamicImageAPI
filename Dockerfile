FROM ubuntu:jammy AS runner

EXPOSE 29321

WORKDIR /tmp
RUN apt-get update && apt-get update -y wget
RUN wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet7pi/master/install.sh | sudo bash

WORKDIR /root/dyn/
COPY . /root/dyn/

WORKDIR /root/dyn/DynamicImageAPI
ENTRYPOINT dotnet run

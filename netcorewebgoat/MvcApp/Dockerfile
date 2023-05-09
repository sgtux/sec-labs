FROM sgtux/dotnet-sdk-6 AS build
COPY files /source
WORKDIR /source/
RUN dotnet publish -c release -o /app

FROM sgtux/aspnetcore-runtime-6
WORKDIR /app
COPY --from=build /app ./
COPY files/create-user.py /root
COPY files/init.sh /root
COPY files/rockyou.txt /root
ARG DEBIAN_FRONTEND=noninteractive
RUN apt update && \
    apt install openssh-server -y && \
    chmod +x /root/init.sh && \
    chmod +x /root/create-user.py
ENTRYPOINT [ "/root/init.sh"]
CMD ["dotnet", "NetCoreWebGoat.dll", "--urls=http://*"]
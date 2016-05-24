FROM 		mono
COPY        . /src
WORKDIR     /src
RUN         xbuild src/Xena.Micro.ZipCodeService.sln
EXPOSE      8900
ENTRYPOINT  ["mono", "src/Xena.Micro.ZipCodeService/bin/Debug/Xena.Micro.ZipCodeService.exe"]

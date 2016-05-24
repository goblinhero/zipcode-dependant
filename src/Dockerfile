FROM 		xena_base
COPY        . /src
WORKDIR     /src
RUN         xbuild Xena.Micro.ZipCodeService.sln
EXPOSE      8900
ENTRYPOINT  ["mono", "Xena.Micro.ZipCodeService/bin/Debug/Xena.Micro.ZipCodeService.exe"]


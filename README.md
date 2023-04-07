# AC.SharedKernel
Technical functionality shared between bounded contexts in the AfrikanCoder domain

This project can be pulled from Nuget with the following details:

.NET CLI : ```dotnet add package afrikancoder-shared --version 1.0.0```

Package Manager Console : ```NuGet\Install-Package afrikancoder-shared -Version 1.0.0```

PackageReference : ```<PackageReference Include="afrikancoder-shared" Version="1.0.0" />```

PacketCLI : ```paket add afrikancoder-shared --version 1.0.0```

Script & Interactive : ```#r "nuget: afrikancoder-shared, 1.0.0"```


Cake:
```// Install afrikancoder-shared as a Cake Addin
#addin nuget:?package=afrikancoder-shared&version=1.0.0

// Install afrikancoder-shared as a Cake Tool
#tool nuget:?package=afrikancoder-shared&version=1.0.0 

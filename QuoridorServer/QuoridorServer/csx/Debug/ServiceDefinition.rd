<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="QuoridorServer" generation="1" functional="0" release="0" Id="168949e6-cda7-408d-863c-a252f82b1110" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="QuoridorServerGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="QuoridorService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/QuoridorServer/QuoridorServerGroup/LB:QuoridorService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="QuoridorService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/QuoridorServer/QuoridorServerGroup/MapQuoridorService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="QuoridorServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/QuoridorServer/QuoridorServerGroup/MapQuoridorServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:QuoridorService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapQuoridorService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapQuoridorServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="QuoridorService" generation="1" functional="0" release="0" software="C:\Users\Jakub\OneDrive\Projekt Grupowy\quoridor-projekt-grupowy\QuoridorServer\QuoridorServer\csx\Debug\roles\QuoridorService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;QuoridorService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;QuoridorService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="QuoridorServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="QuoridorServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="QuoridorServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="91337303-43e3-47e4-be76-1ae0f441bbe6" ref="Microsoft.RedDog.Contract\ServiceContract\QuoridorServerContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="cfc155c1-8320-438b-b68f-cc32dbe0e712" ref="Microsoft.RedDog.Contract\Interface\QuoridorService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/QuoridorServer/QuoridorServerGroup/QuoridorService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
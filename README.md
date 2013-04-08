Glimpse.Soap
============

A Glimpse plugin to monitor soap requests.

Reference the Glimpse.Soap.dll in your project.

Make sure your web.config contains the below:

    <webServices>
      <soapExtensionTypes>
        <add type="Glimpse.Soap.GlimpseSoapExtension, Glimpse.Soap" priority="0" group="0" />
      </soapExtensionTypes>
    </webServices>

Now you should see a "Soap" tab in Glimpse.

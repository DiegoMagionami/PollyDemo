# PollyDemo
Provide examples for Polly Library implementation

To start the application, set Multiple Startup Projects as the startup option.
PollyTestClient is a Blazor application for frontend management.
PollyTestServer simulates a server that exposes APIs.

GET / SlowServer simulates an endpoint that throws an exception on the first three calls, while on the fourth call we will get a success message.
The WaitAndRetryAsync policy takes care of managing the attempts to call the endpoint until a successful status is obtained.

In the PollyTestServer there is an example of implementation of Polly Registry.
In the Startup file are declared all the policies. They are added to the registry, the registry is added to the HttpClientFactory and the HttpClientFactory is added to the services collections.
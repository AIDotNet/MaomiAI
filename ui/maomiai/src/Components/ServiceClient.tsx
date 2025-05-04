import { MaomiClient, createMaomiClient } from "../ApiClient/maomiClient";
import {
  AnonymousAuthenticationProvider,
  BaseBearerTokenAuthenticationProvider,
  AllowedHostsValidator,
  RequestOption,
  ParseNodeFactoryRegistry,
  SerializationWriterFactoryRegistry,
} from "@microsoft/kiota-abstractions";
import {
  FetchRequestAdapter,
  KiotaClientFactory,
  Middleware,
  MiddlewareFactory,
} from "@microsoft/kiota-http-fetchlibrary";
import { redirect } from "react-router";

import { EnvOptions } from "../Env";
import {
  JsonParseNodeFactory,
  JsonSerializationWriterFactory,
} from "@microsoft/kiota-serialization-json";
import { message } from "antd";


class FilterRequestHandler implements Middleware {
  async execute(
    url: string,
    requestInit: RequestInit,
    requestOptions?: Record<string, RequestOption>
  ): Promise<Response> {

    if (!this.next) {
      throw new Error("Next middleware is not set");
    }

    let response = await this.next.execute(
      url,
      requestInit as RequestInit,
      requestOptions
    );
    // Add any additional response handling logic here if needed
    if (response.status == 401) {
      redirect("/login");
    } else if (response.status == 403) {
    }
    return response;
  }
  next: Middleware | undefined;
}

const parseNodeFactoryRegistry = new ParseNodeFactoryRegistry();
parseNodeFactoryRegistry.contentTypeAssociatedFactories.set(
  "application/json",
  new JsonParseNodeFactory()
);

const serializationRegistry = new SerializationWriterFactoryRegistry();
serializationRegistry.contentTypeAssociatedFactories.set(
  "application/json",
  new JsonSerializationWriterFactory()
);

const handlers = MiddlewareFactory.getDefaultMiddlewares();
handlers.unshift(new FilterRequestHandler());

const getApiClient = function (): MaomiClient {
  const token = localStorage.getItem("token");
  let authProvider;
  if (token) {
    const jwtToken = `Bearer ${token}`;
    authProvider = new BaseBearerTokenAuthenticationProvider({
      getAuthorizationToken: async () => jwtToken,
      getAllowedHostsValidator: () => new AllowedHostsValidator(),
    });
  } else {
    authProvider = new AnonymousAuthenticationProvider();
  }
  const httpClient = KiotaClientFactory.create(undefined, handlers);
  const adapter = new FetchRequestAdapter(
    authProvider,
    parseNodeFactoryRegistry,
    serializationRegistry,
    httpClient
  );
  adapter.baseUrl = EnvOptions.ServerUrl;
  return createMaomiClient(adapter);
};

export const ServiceClient = getApiClient();

export const GetServiceInfo = async () => {
  try {
    const response = await ServiceClient.api.public.serverinfo.get();
    return response;
  } catch (error) {
    console.error("Error fetching service info:", error);
    throw error;
  }
};

export const ServiceInfo = await GetServiceInfo();

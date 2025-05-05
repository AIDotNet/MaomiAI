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

import { EnvOptions } from "../Env";
import {
  JsonParseNodeFactory,
  JsonSerializationWriterFactory,
} from "@microsoft/kiota-serialization-json";
import { message } from "antd";

// 中间件请求
class FilterRequestHandler implements Middleware {
  async execute(
    url: string,
    requestInit: RequestInit,
    requestOptions?: Record<string, RequestOption>
  ): Promise<Response> {
    if (!this.next) {
      throw new Error("Next middleware is not set");
    }

    try {
      let response = await this.next.execute(
        url,
        requestInit as RequestInit,
        requestOptions
      );

      if (response.status === 401) {
        if (!url.includes("login")) {
          message.error("登录过期，请重新登录");
          window.location.href = "/login";
        }
      }

      return response;
    } catch (ex) {
      window.location.href = "/login";
      console.log(ex);
      throw ex;
    }
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

export const GetApiClient = async function (): Promise<MaomiClient> {
  const token = localStorage.getItem("userinfo.accessToken");
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

// 刷新 token
export const RefreshAccessToken = async function (refreshToken: string) {
  let client = await GetApiClient();
  return await client.api.user.refresh_token.post({
    refreshToken: refreshToken,
  });
};

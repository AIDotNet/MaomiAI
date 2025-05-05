import { RefreshAccessToken, GetApiClient } from "./Components/ServiceClient";
import { IsTokenExpired } from "./helper/TokenHelper";

export interface ServerInfoModel {
  /**
   * 公共存储地址，静态资源时可直接访问.
   */
  publicStoreUrl: string;
  /**
   * RSA 公钥，用于加密密码等信息传输到服务器.
   */
  rsaPublic: string;
  /**
   * 系统访问地址.
   */
  serviceUrl: string;
}

export interface UserInfoModel {
  /**
   * 访问令牌.
   */
  accessToken?: string | null;
  /**
   * 过期时间（秒）.
   */
  expiresIn?: string | null;
  /**
   * 刷新令牌.
   */
  refreshToken?: string | null;
  /**
   * 令牌类型.
   */
  tokenType?: string | null;
  /**
   * 用户ID.
   */
  userId?: string | null;
  /**
   * 用户名.
   */
  userName?: string | null;
}

// 加载服务器公共信息
export const InitServerInfo = async () => {
  try {
    const client = await GetApiClient();
    const response = await client.api.public.serverinfo.get();
    if (response) {
      localStorage.setItem("serverinfo.serviceUrl", response.serviceUrl!);
      localStorage.setItem(
        "serverinfo.publicStoreUrl",
        response.publicStoreUrl!
      );
      localStorage.setItem("serverinfo.rsaPublic", response.rsaPublic!);
    }
    return response;
  } catch (error) {
    console.error("Error fetching service info:", error);
    throw error;
  }
};

export async function GetServiceInfo(): Promise<ServerInfoModel> {
  let serviceUrl = localStorage.getItem("serverinfo.serviceUrl");
  if (!serviceUrl) {
    await InitServerInfo();
  }

  return {
    publicStoreUrl: localStorage.getItem("serverinfo.publicStoreUrl")!,
    rsaPublic: localStorage.getItem("serverinfo.rsaPublic")!,
    serviceUrl: localStorage.getItem("serverinfo.serviceUrl")!,
  };
}

// 登录后设置用户信息到缓存
export const SetUserInfo = (userInfo: UserInfoModel) => {
  localStorage.setItem("userinfo.accessToken", userInfo.accessToken!);
  localStorage.setItem("userinfo.expiresIn", userInfo.expiresIn!);
  localStorage.setItem("userinfo.refreshToken", userInfo.refreshToken!);
  localStorage.setItem("userinfo.tokenType", userInfo.tokenType!);
  localStorage.setItem("userinfo.userId", userInfo.userId!);
  localStorage.setItem("userinfo.userId", userInfo.userName!);
};

export const GetUserInfo = (): UserInfoModel => {
  return {
    accessToken: localStorage.getItem("userinfo.accessToken"),
    expiresIn: localStorage.getItem("userinfo.expiresIn"),
    refreshToken: localStorage.getItem("userinfo.refreshToken"),
    tokenType: localStorage.getItem("userinfo.tokenType"),
    userId: localStorage.getItem("userinfo.userId"),
    userName: localStorage.getItem("userinfo.userName"),
  };
};

// 检查 accesstoken 是否有效，过期则自动刷新
export const CheckToken = async () => {
  const userInfo = GetUserInfo();
  if (!userInfo.accessToken) {
    return false;
  }

  try {
    if (IsTokenExpired(userInfo.accessToken)) {
      // 使用 refresh token 刷新 access token
      if (userInfo.refreshToken && !IsTokenExpired(userInfo.refreshToken)) {
        var response = await RefreshAccessToken(userInfo.refreshToken);

        // 刷新失败
        if (!response) {
          return false;
        }

        SetUserInfo(response);

        return true;
      }
    }
  } catch (error) {
    console.error("Error checking token:", error);
    return false;
  }

  return false;
};

export const GetAccessToken = async (): Promise<string | null> => {
  const userInfo = GetUserInfo();
  if (!userInfo.accessToken) {
    return null;
  }

  try {
    if (IsTokenExpired(userInfo.accessToken)) {
      // 使用 refresh token 刷新 access token
      if (userInfo.refreshToken && !IsTokenExpired(userInfo.refreshToken)) {
        var response = await RefreshAccessToken(userInfo.refreshToken);

        // 刷新失败
        if (!response) {
          return null;
        }

        SetUserInfo(response);

        return response.accessToken!;
      }
    }
  } catch (error) {
    console.error("Error checking token:", error);
    return null;
  }

  return null;
};

// 服务器信息
export const ServerInfo: ServerInfoModel = await GetServiceInfo();

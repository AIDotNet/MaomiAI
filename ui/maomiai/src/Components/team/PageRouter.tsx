import { RouteObject } from "react-router";
import Team from "./Team";
import Dashboard from "./Dashboard";
import { AiModelPageRouter } from "./aimodel/PageRouter";
import { ApplicationPageRouter } from "./application/PageRouter";
import { WikiPageRouter } from "./wiki/PageRouter";
import { PluginPageRouter } from "./plugin/PageRouter";
import { SettingPageRouter } from "./setting/PageRouter";

export const TeamPageRouter: RouteObject = {
  path: "team/:teamId/*",
  Component: Team,
  children: [
    {
      path: "aimodel/*",
      ...AiModelPageRouter,
    },
    {
      path: "application/*",
      ...ApplicationPageRouter,
    },
    {
      path: "wiki/*",
      ...WikiPageRouter,
    },
    {
      path: "plugin/*",
      ...PluginPageRouter,
    },
    {
      path: "setting/*",
      ...SettingPageRouter,
    },
  ],
};

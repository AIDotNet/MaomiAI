import { RouteObject } from "react-router";
import Team from "./Team";
import { AiModelPageRouter } from "./aimodel/PageRouter";
import { ApplicationPageRouter } from "./application/PageRouter";
import { WikiListPageRouter, WikiPageRouter } from "./wiki/PageRouter";
import { PluginPageRouter } from "./plugin/PageRouter";
import { SettingPageRouter } from "./setting/PageRouter";
import TeamDashboard from "./TeamDashboard";
import { PromptPageRouter } from "./prompt/PageRouter";

export const TeamPageRouter: RouteObject = {
  path: "team/:teamId/*",
  Component: Team,
  children: [
    {
      path: "dashboard",
      Component: TeamDashboard,
    },
    {
      path: "aimodel/*",
      ...AiModelPageRouter,
    },
    {
      path: "application/*",
      ...ApplicationPageRouter,
    },
    {
      path: "wikilist",
      ...WikiListPageRouter,
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
    {
      path: "prompt/*",
      ...PromptPageRouter,
    },
  ],
};

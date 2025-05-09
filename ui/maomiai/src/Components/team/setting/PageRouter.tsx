
import {
    RouteObject
} from "react-router";
import Setting from "./Setting";
import TeamAdmin from "./TeamAdmin";
import TeamSetting from "./TeamSetting";
import TeamMember from "./TeamMember";

export const SettingPageRouter: RouteObject = {
    path: ':/team/setting',
    Component: Setting,
    children: [
        {
            path: 'admin',
            Component: TeamAdmin
        },
        {
            path: 'settings',
            Component: TeamSetting
        },
        {
            path: 'member',
            Component: TeamMember
        }
    ]
}
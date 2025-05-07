import {
    RouteObject
} from "react-router";
import Team from "./Team";
import Dashboard from "./Dashboard";

export const TeamPageRouter: RouteObject = {
    path: 'team/*',
    Component: Team,
    children: [
        {
            path: ':teamId/dashboard',
            Component: Dashboard
        },
        {
            path: 'dashboard',
            Component: Dashboard
        }
    ]
}